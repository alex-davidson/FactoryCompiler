using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace FactoryCompiler.Jobs;

public class WpfAppThread
{
    private readonly Action<Application> run;
    private readonly Thread thread;
    private readonly TaskCompletionSource<Dispatcher> startTcs;
    private readonly TaskCompletionSource endTcs;

    public WpfAppThread(Action<Application> run)
    {
        this.run = run;
        startTcs = new TaskCompletionSource<Dispatcher>(TaskCreationOptions.RunContinuationsAsynchronously);
        endTcs = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        thread = new Thread(RunWpfThread);
        thread.SetApartmentState(ApartmentState.STA);
    }

    public void Start()
    {
        if (thread.ThreadState != ThreadState.Unstarted) throw new InvalidOperationException("Application was already started.");
        thread.Start();
    }

    public Task Dispatch(Action action) => DispatchAction(action);
    public Task<T> Dispatch<T>(Func<T> func) => DispatchFunc(func);
    public async Task Dispatch<T>(Func<Task> action) => await DispatchFunc(action).Unwrap();
    public async Task<T> Dispatch<T>(Func<Task<T>> func) => await DispatchFunc(func).Unwrap();

    private async Task DispatchAction(Action action)
    {
        var dispatcher = await startTcs.Task;
        var op = dispatcher.BeginInvoke(action);
        await op.Task;
    }

    private async Task<T> DispatchFunc<T>(Func<T> func)
    {
        var tcs = new TaskCompletionSource<T>();
        await DispatchAction(() => CaptureResult(tcs, func));
        return await tcs.Task;
    }

    private void CaptureResult<T>(TaskCompletionSource<T> tcs, Func<T> func)
    {
        try
        {
            tcs.TrySetResult(func());
        }
        catch (Exception ex)
        {
            tcs.TrySetException(ex);
        }
    }

    private void RunWpfThread()
    {
        try
        {
            var app = new Application();
            startTcs.TrySetResult(app.Dispatcher);
            // Native.ShowWindow(Native.GetConsoleWindow(), 0 /*SW_HIDE*/);
            run(app);
            endTcs.TrySetResult();
        }
        catch (Exception ex)
        {
            startTcs.TrySetException(ex);
            endTcs.TrySetException(ex);
        }
    }

    public async Task WaitForShutdown()
    {
        if (thread.ThreadState == ThreadState.Unstarted) throw new InvalidOperationException("Application has not been started.");
        await endTcs.Task;
        thread.Join();
    }

    internal static class Native
    {
        [DllImport("kernel32.dll")]
        internal static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    }
}
