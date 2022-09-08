namespace ProjectNightBot;

using System.Reflection;
using Discord.WebSocket;

public interface IPlugin
{
	Task Start(DiscordSocketClient client, Assembly callingAssembly);
	void Stop();
}