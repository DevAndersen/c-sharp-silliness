using System.Diagnostics;
using System.Runtime.InteropServices;

// The number of attempts to hit close enough to a 1-second delay.
int attempt = 1;

// The maximum number of attempts we'll do. We don't want to be here all day.
int maxAttempts = 25;

// The number of times we'll pass the struct per attempt. Let's start with 100, that's a nice round number.
int loops = 100;

// The number of seconds it took to pass our struct {loop} number of times.
double measuredTime = 0;

// The error margin we're aiming for. Anything closer than this is assumed to be "close enough" to a 1-second delay.
double errorMargin = 0.003;

do
{
    measuredTime = StructSleeper.Sleep(loops);
    loops = (int)(1 / (measuredTime / loops));
    Console.WriteLine($"Attempt {attempt}: {loops} loops took {measuredTime} seconds");
}
while (double.Abs(measuredTime - 1) > errorMargin && ++attempt < maxAttempts);

Console.WriteLine($"Settled on {loops} after {attempt} attempts");

double finalMeasure = StructSleeper.Sleep(loops);
Console.WriteLine($"Sleep took {finalMeasure} seconds, which is reasonably close to a second");

/// <summary>
/// Contains logic for putting the current thread to sleep by spending time passing a very large struct around.
/// </summary>
public static class StructSleeper
{
    /// <summary>
    /// Sleep for <paramref name="loops"/> loops.
    /// </summary>
    /// <param name="loops"></param>
    /// <returns>The number of seconds it took to sleep.</returns>
    public static double Sleep(int loops)
    {
        Sleeper sleeper = new Sleeper();
        Stopwatch sw = Stopwatch.StartNew();

        for (int i = 0; i < loops; i++)
        {
            SleepInner(sleeper);
        }

        sw.Stop();
        return sw.Elapsed.TotalSeconds;

        static void SleepInner(Sleeper sleeper)
        {
        }
    }

    /// <summary>
    /// A very big struct.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 100_000)]
    public ref struct Sleeper
    {
    }
}
