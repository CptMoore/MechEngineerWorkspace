using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using HarmonyLib;

namespace MyBenchmarks;

public class HarmonyXBenchmark
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static int DoNothing(int value)
    {
        return value;
    }

    [Benchmark]
    public void CallDirect()
    {
        DoNothing(55);
    }

    [GlobalSetup(Target = nameof(CallDirectAfterPatching))]
    public void SetupCallDirectAfterPatching()
    {
        Harmony.CreateAndPatchAll(typeof(HarmonyXBenchmark));
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(HarmonyXBenchmark), nameof(DoNothing))]
    public static void DoNothingPatch(ref bool __runOriginal, ref int __result, int value)
    {
        if (__runOriginal)
        {
            __runOriginal = false;
        }

        __result = value;
    }

    [Benchmark]
    public void CallDirectAfterPatching()
    {
        DoNothing(55);
    }
}
