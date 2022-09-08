namespace ProjectNightBot;
using Discord;
using Discord.WebSocket;
using ProjectNightBot.Utils;

public class Program
{
	public static DiscordSocketClient Client { get; private set; }

	public static readonly string SECRET_FILE = "token.txt";
	public static readonly string MODULE_PATH = "Modules";

	internal static CommandHandler CommandHandler { get; private set; } = new();
	internal static AssemblyHandler AssemblyHandler { get; private set; } = new();
	internal static PluginHandler PluginHandler { get; private set; } = new();
	internal static ExitHandler ExitHandler { get; private set; } = new();

	public static void Main(string[] args)
	{
		try
		{
			new Program().MainAsync().GetAwaiter().GetResult();
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
			Console.ReadLine();
			throw;
		}
	}

	public async Task MainAsync()
	{
		var config = new DiscordSocketConfig() { AlwaysDownloadUsers = true };
		Client = new(config);

		// Subscribe to events
		Client.Ready += ClientReady;

		// Log in
		Console.WriteLine("Loading secret...");
		var token = File.ReadAllText(SECRET_FILE);
		Console.WriteLine("Logging in...");
		await Client.LoginAsync(TokenType.Bot, token);
		Console.WriteLine("Starting...");
		await Client.StartAsync();

		// Block this task until program is closed
		await Task.Delay(-1);
	}

	private async Task ClientReady()
	{
		// catch CTRL+C
		Console.WriteLine("Hooking termination events...");
		ExitHandler.Start();

		// Load external assemblies
		Console.WriteLine("Loading assemblies...");
		await AssemblyHandler.Start();

		// Load commands
		Console.WriteLine("Building commands...");
		await CommandHandler.Start(AssemblyHandler, GuildUtils.GetGuild().ValueOr(GuildUtils.NoGuild));

		// Load plugins
		Console.WriteLine("Starting plugins...");
		await PluginHandler.Start(AssemblyHandler);
	}

	private Task Log(LogMessage message)
	{
		Console.WriteLine(message.Message);

		return Task.CompletedTask;
	}
}