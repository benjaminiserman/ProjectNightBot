namespace ProjectNightBot;
using Discord;
using Discord.WebSocket;
using ProjectNightBot.Utils;

public class Program
{
	public static DiscordSocketClient Client { get; private set; }
	public static SocketGuild Guild { get; private set; }

	public static readonly string SECRET_FILE = "token.txt";
	public static readonly string MODULE_PATH = "Modules";

	internal static CommandHandler CommandHandler { get; private set; } = new();
	internal static AssemblyHandler AssemblyHandler { get; private set; } = new();
	internal static PluginHandler PluginHandler { get; private set; } = new();

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
		var config = new DiscordSocketConfig() { AlwaysDownloadUsers = true, GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.GuildMembers };
		Client = new(config);

		// Subscribe to events
		Client.Ready += ClientReady;
		Client.Log += ClientLog;
		Client.GuildAvailable += async g => Guild = g;

		// Log in
		if (!File.Exists(SECRET_FILE))
		{
			Console.WriteLine($"ERROR: File {SECRET_FILE} not found. Create this file and place your bot's login token within it.");
		}
		
		var token = File.ReadAllText(SECRET_FILE);
		await Client.LoginAsync(TokenType.Bot, token.Trim());
		await Client.StartAsync();

		// Block this task until program is closed
		await Task.Delay(-1);
	}

	private async Task ClientLog(LogMessage log) => Console.WriteLine(log);

	private async Task ClientReady()
	{
		// Load external assemblies
		Console.WriteLine("Loading assemblies...");
		await AssemblyHandler.Start();

		// Load commands
		Console.WriteLine("Building commands...");
		await CommandHandler.Start(AssemblyHandler, Guild);

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