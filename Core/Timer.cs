using System.Diagnostics;

namespace Core;

public static class Timer
{
    public static async Task RunWithTimer<T>(this Task<T> task, string prefix)
    {
        var sw = new Stopwatch();
        sw.Start();
        var part1 = await task;
        sw.Stop();
        Console.WriteLine($"{prefix}{part1} [{sw.Elapsed.Seconds}s {sw.Elapsed.Milliseconds}ms]");
    }

    public static async Task RunWithTimer(this Task task, string prefix)
    {
        var sw = new Stopwatch();
        sw.Start();
        await task;
        sw.Stop();
        Console.WriteLine($"{prefix} [{sw.Elapsed.Seconds}s {sw.Elapsed.Milliseconds}ms]");
    }
}
