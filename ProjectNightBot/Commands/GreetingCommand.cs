namespace ProjectNightBot.Commands;

using Discord;
using Discord.WebSocket;
using SharedInterfaces;

internal class GreetingCommand : ICommand
{
	private string[] greetings = { "Hello!", "Hi!", "Howdy!", "Sup?", "Bonjour!", "Bongiorno!", "Saluton!", "Привет!", "Здравствуйте!", "你好！" };

	public string Name => "greeting";

	public SlashCommandProperties Build()
	{
		return new SlashCommandBuilder()
			.WithName(Name)
			.WithDescription("Say hi!")
			.Build();
	}

	public async Task Execute(SocketSlashCommand command)
	{
		await command.RespondAsync(greetings[Random.Shared.Next(greetings.Length)]);
	}
}
