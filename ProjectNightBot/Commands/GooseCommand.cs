namespace ProjectNightBot.Commands;

using Discord;
using Discord.WebSocket;
using SharedInterfaces;

internal class GooseCommand : ICommand
{
	public string Name => "goose";

	private string[] files;

	public SlashCommandProperties Build()
	{
		files = Directory.GetFiles("Images/Geese");

		return new SlashCommandBuilder()
			.WithName(Name)
			.WithDescription("Get goosed")
			.Build();
	}

	public async Task Execute(SocketSlashCommand command, DiscordSocketClient client)
	{
		var i = Random.Shared.Next(0, files.Length);
		var path = files[i];
		var fs = File.OpenRead(path);
		await command.RespondWithFileAsync(fs, path);
	}
}
