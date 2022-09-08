namespace ProjectNightBot;

using System.Runtime.InteropServices;

internal class ExitHandler
{
    // https://docs.microsoft.com/en-us/windows/console/setconsolectrlhandler?WT.mc_id=DT-MVP-5003978
    [DllImport("Kernel32")]
    private static extern bool SetConsoleCtrlHandler(SetConsoleCtrlEventHandler handler, bool add);

    // https://docs.microsoft.com/en-us/windows/console/handlerroutine?WT.mc_id=DT-MVP-5003978
    private delegate bool SetConsoleCtrlEventHandler(CtrlType sig);

    private enum CtrlType
    {
        CTRL_C_EVENT = 0,
        CTRL_BREAK_EVENT = 1,
        CTRL_CLOSE_EVENT = 2,
        CTRL_LOGOFF_EVENT = 5,
        CTRL_SHUTDOWN_EVENT = 6
    }

    public void Start()
    {
        AppDomain.CurrentDomain.ProcessExit += (_, _) => Exit();
        SetConsoleCtrlHandler(Handler, true);
    }
    
    private bool Handler(CtrlType signal)
    {
        switch (signal)
        {
            case CtrlType.CTRL_BREAK_EVENT:
            case CtrlType.CTRL_C_EVENT:
            case CtrlType.CTRL_LOGOFF_EVENT:
            case CtrlType.CTRL_SHUTDOWN_EVENT:
            case CtrlType.CTRL_CLOSE_EVENT:
                Exit();
                return false;

            default:
                return false;
        }
    }

    public static void Exit()
	{
        Console.WriteLine("Closing");

        Program.PluginHandler.Stop();

        Console.ReadLine();
        Environment.Exit(0);
    }
}