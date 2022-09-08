namespace ProjectNightBot;

using Discord;
using Discord.WebSocket;

public interface ICommand
{
	SlashCommandProperties Build();
	Task Execute(SocketSlashCommand command);
}
