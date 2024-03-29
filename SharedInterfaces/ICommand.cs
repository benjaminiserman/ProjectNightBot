﻿namespace SharedInterfaces;

using Discord;
using Discord.WebSocket;

public interface ICommand
{
	string Name { get; }
	SlashCommandProperties Build();
	Task Execute(SocketSlashCommand command, DiscordSocketClient client);
}
