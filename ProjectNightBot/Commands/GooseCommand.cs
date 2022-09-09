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
			.WithDescription("Get goosed")
			.Build();
	}

	public async Task Execute(SocketSlashCommand command)
	{
        var path = @$"Images/goose{Random.Shared.Next(1,6)}.jpg";
        var fs = File.OpenRead(path);
		await command.RespondWithFileAsync(fs, path);
	}
}
