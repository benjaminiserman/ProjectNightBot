namespace ProjectNightBot;

using Discord;
using Discord.WebSocket;

public interface ICommand
{
	string Name { get; }
	SlashCommandProperties Build();
	Task Execute(SocketSlashCommand command);
}
