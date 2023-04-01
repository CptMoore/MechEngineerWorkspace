using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

namespace MyBenchmarks;

public class TryCatchBenchmark
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    private void DoNothing()
    {

    }

    [Benchmark]
    public void TryCatchCall()
    {
        try
        {
            DoNothing();
        }
        catch (Exception e)
        {
            for (var j = 0; j < 100; j++)
            {
                Console.Write(j);
            }
        }
    }

    [Benchmark]
    public void CallDirect()
    {
        DoNothing();
    }
}
