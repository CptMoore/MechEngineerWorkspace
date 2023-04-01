﻿using BenchmarkDotNet.Running;
using MyBenchmarks;

namespace Benchmark;

public static class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run<HarmonyXBenchmark>();
        // BenchmarkRunner.Run<CallBenchmark>();
        // BenchmarkRunner.Run<LoggingBenchmark>();
    }
}