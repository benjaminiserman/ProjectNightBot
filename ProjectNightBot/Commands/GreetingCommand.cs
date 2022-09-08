namespace ProjectNightBot.Commands;

using Discord;
using Discord.WebSocket;

internal class GreetingCommand : ICommand
{
	private string[] greetings = { "Hello!", "Hi!", "Howdy!", "Sup?", "Bonjour!", "Bongiorno!", "Saluton!", "Привет!", "Здравствуйте!", "你好！" };

	public SlashCommandProperties Build()
	{
		return new SlashCommandBuilder()
			.WithName("greeting")
			.WithDescription("Say hi!")
			.Build();
	}

	public async Task Execute(SocketSlashCommand command)
	{
		await command.Channel.SendMessageAsync(greetings[Random.Shared.Next(greetings.Length)]);
	}
}
