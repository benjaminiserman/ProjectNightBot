namespace ProjectNightBot.Commands;

using Discord;
using Discord.WebSocket;
using SharedInterfaces;

internal class GooseCommand : ICommand
{
	public string Name => "goose";

	public string[] Files;

	public SlashCommandProperties Build()
	{
		Files = Directory.GetFiles("Images/Geese");

		return new SlashCommandBuilder()
			.WithName(Name)
			.WithDescription("Get goosed")
			.Build();
	}

	public async Task Execute(SocketSlashCommand command, DiscordSocketClient client)
	{
		var i = Random.Shared.Next(0, Files.Length);
		var path = Files[i];
		var fs = File.OpenRead(path);
		await command.RespondWithFileAsync(fs, path);
	}
}
