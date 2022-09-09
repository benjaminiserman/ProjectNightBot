namespace ProjectNightBot;

using System.Collections.Immutable;
using System.Reflection;

internal class AssemblyHandler
{
	private readonly HashSet<Assembly> _assemblies = new();
	private bool _started = false;

	private ImmutableList<Assembly> _immutableAssemblies = null;
	public ImmutableList<Assembly> Assemblies
	{
		get
		{
			if (!_started)
			{
				throw new Exception("Assemblies cannot be accessed before AssemblyHandler::Start is called!");
			}

			return _immutableAssemblies ??= _assemblies.ToImmutableList();
		}
	}

	public async Task Start()
	{
		foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
		{
			_assemblies.Add(assembly);
		}
		Console.WriteLine(typeof(JuliaPlugins.JuliaGreetingCommand).Assembly);

		// load assemblies from MODULE_PATH
		if (Directory.Exists(Program.MODULE_PATH))
		{
			foreach (var file in Directory.EnumerateFiles(Program.MODULE_PATH))
			{
				try
				{
					_assemblies.Add(Assembly.LoadFrom(file));
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex);
				}
			}
		}

		// load assemblies from referenced projects
		foreach (var assemblyName in typeof(Program).Assembly.GetReferencedAssemblies())
		{
			_assemblies.Add(Assembly.Load(assemblyName)); // note: this is not being called recursively
		}

		_started = true;
	}

	public ImmutableList<Assembly> GetAssemblies() => _assemblies.ToImmutableList();
}
