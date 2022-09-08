namespace ProjectNightBot.Utils;

using Discord.WebSocket;
using Optional;

internal static class GuildUtils
{
	public static Option<SocketGuild> GetGuild()
	{
		var first = Program.Client.Guilds.First();

		if (first is null)
		{
			return Option.None<SocketGuild>();
		}
		else
		{
			return Option.Some(first);
		}
	}

	public static Option<SocketGuild> GetGuild(ulong? id)
	{
		if (id is ulong idNotNull)
		{
			return Option.Some(Program.Client.GetGuild(idNotNull));		
		}
		else
		{
			return Option.None<SocketGuild>();
		}
	}

	public static SocketGuild NoGuild => throw new Exception("No guild was found.");
}
