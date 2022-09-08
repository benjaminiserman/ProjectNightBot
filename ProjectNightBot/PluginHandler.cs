namespace ProjectNightBot;

using System.Collections.Immutable;
using SharedInterfaces;

internal class PluginHandler
{
	private bool _started = false;

	private List<IPlugin> _plugins;

	private ImmutableList<IPlugin> _immutablePlugins = null;
	public ImmutableList<IPlugin> Plugins
	{
		get
		{
			if (!_started)
			{
				throw new Exception("Assemblies cannot be accessed before AssemblyHandler::Start is called!");
			}

			return _immutablePlugins ??= _plugins.ToImmutableList();
		}
	}

	public async Task Start(AssemblyHandler assemblyHandler)
	{
		_plugins = assemblyHandler.Assemblies
			.SelectMany(a => a
				.GetTypes()
				.Where(t => t.GetInterfaces().Contains(typeof(IPlugin)))
				.Select(t => Activator.CreateInstance(t) as IPlugin))
			.ToList();

		foreach (var plugin in _plugins)
		{
			await plugin.Start(Program.Client, typeof(Program).Assembly);
		}

		_started = true;
	}

	public void Stop()
	{
		foreach (var plugin in _plugins)
		{
			plugin.Stop();
		}
	}
}
