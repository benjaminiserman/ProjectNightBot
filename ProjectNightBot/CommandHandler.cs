namespace ProjectNightBot;

using System.Collections.Immutable;
using Discord.WebSocket;
using SharedInterfaces;

internal class CommandHandler
{
	private Dictionary<string, ICommand> _commands;
	private bool _started = false;

	private ImmutableList<ICommand> _immutableCommands = null;
	public ImmutableList<ICommand> Commands
	{
		get
		{
			if (!_started)
			{
				throw new Exception("Commands cannot be accessed before CommandHandler::Start is called!");
			}

			return _immutableCommands ??= _commands.Values.ToImmutableList();
		}
	}

	public async Task Start(AssemblyHandler assemblyHandler, SocketGuild guild)
	{
		Program.Client.SlashCommandExecuted += SlashCommandHandler;

		_commands = assemblyHandler.Assemblies
			.SelectMany(a => a
				.GetTypes()
				.Where(t => t.GetInterfaces().Contains(typeof(ICommand))))
				.Select(t => Activator.CreateInstance(t) as ICommand)
			.ToDictionary(c => c.Name);

		foreach (var command in _commands.Values)
		{
			try
			{
				Console.WriteLine("Loading command '{0}'", command.Name);
				await guild.CreateApplicationCommandAsync(command.Build());
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		}
	}

	public async Task SlashCommandHandler(SocketSlashCommand command)
	{
		try
		{
			await _commands[command.Data.Name].Execute(command, Program.Client);
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex);
		}
	}
}
