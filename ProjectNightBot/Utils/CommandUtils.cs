namespace ProjectNightBot.Utils;

using Discord.WebSocket;
using Optional;

internal static class CommandUtils
{
	public static Option<SocketGuild> GetGuild(SocketSlashCommand command) => GuildUtils.GetGuild(command.GuildId);
}
