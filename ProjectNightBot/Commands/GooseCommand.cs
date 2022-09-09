namespace ProjectNightBot.Commands;

using Discord;
using Discord.WebSocket;
using SharedInterfaces;

internal class GooseCommand : ICommand
{
	public string Name => "goose";

	public SlashCommandProperties Build()
	{
		return new SlashCommandBuilder()
			.WithName(Name)
			.WithDescription("Get goosed sucker")
			.Build();
	}

	public async Task Execute(SocketSlashCommand command)
	{
        Random num = new Random();
        string path = @$"Images/goose{num.Next(1, 6)}.jpg";
        Stream fs = File.OpenRead(path);
		await command.RespondWithFileAsync(fs, path);
	}
}
