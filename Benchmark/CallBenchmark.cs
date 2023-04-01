using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

namespace MyBenchmarks;

public class CallBenchmark
{
    private delegate string? DelegateSignature(int? value);

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static string? DoNothing(int? value)
    {
        return null;
    }

    [Benchmark]
    public void TryCatchCallDirect()
    {
        try
        {
            DoNothing(null);
        }
        catch (Exception e)
        {
            for (var j = 0; j < 100; j++)
            {
                Console.Write(e);
            }
        }
    }

    [Benchmark]
    public void CallDirect()
    {
        DoNothing(null);
    }

    private static DelegateSignature DoNothingDelegateSignature = DoNothing;
    [Benchmark]
    public void CallViaDelegateSignature()
    {
        DoNothingDelegateSignature.Invoke(null);
    }

    private static Func<int?, string?> DoNothingFunc = DoNothing;
    [Benchmark]
    public void CallViaFunc()
    {
        DoNothingFunc(null);
    }
}
