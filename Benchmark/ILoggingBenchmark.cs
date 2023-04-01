using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

namespace MyBenchmarks;

public interface ILoggingBenchmark
{
    public bool Enabled { set; }
    public void Raw();
    public void StringFormat();
    public void Interpolate();
}

public class LoggingBenchmark
{
    [Params(true, false)]
    public bool UseTraditional;

    [Params(true, false)]
    public bool Enabled;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _benchmark = UseTraditional
            ? new TraditionalLoggingBenchmark()
            : new NullableLoggingBenchmark();
        _benchmark.Enabled = Enabled;
    }
    private ILoggingBenchmark _benchmark = null!;

    [Benchmark]
    public void Raw() => _benchmark.Raw();

    [Benchmark]
    public void StringFormat() => _benchmark.StringFormat();

    [Benchmark]
    public void Interpolate() => _benchmark.Interpolate();
}

public class NullableLoggingBenchmark : ILoggingBenchmark
{
    public bool Enabled
    {
        set => _logger.Enabled = value;
    }

    public NullableLogger _logger = new();

    public void Raw() => _logger.Info?.Log("This works great");
    public void StringFormat() => _logger.Info?.Log(string.Format("This works great {0}", Helper.RandomStr()));
    public void Interpolate() => _logger.Info?.Log($"This works great {Helper.RandomStr()}");
}

public class NullableLogger
{
    public bool Enabled
    {
        set => Info = value ? _info : null;
    }

    private Logger _info = new ();
    public class Logger
    {
        public void Log(string message)
        {
            Helper.WriteLine(message);
        }
    }

    public Logger? Info { get; internal set; }
}

public static class Helper
{
    static Helper()
    {
        sw = new(Stream.Null);
        sw.AutoFlush = true;
    }
    private static readonly StreamWriter sw;

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void WriteLine(string message)
    {
        var line = $"{DateTime.Now} {message}";
        sw.WriteLine(line);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string RandomStr() => "YEEEEA";
}

public class TraditionalLoggingBenchmark : ILoggingBenchmark
{
    public bool Enabled
    {
        set => _logger.Enabled = value;
    }

    public TraditionalLogger _logger = new();

    public void Raw() => _logger.Log("This works great");

    public void StringFormat() => _logger.Log("This works great {0}", Helper.RandomStr());

    public void Interpolate() => _logger.Log($"This works great {Helper.RandomStr()}");
}

public class TraditionalLogger
{
    public bool Enabled
    {
        set => Level = value ? 1 : 0;
    }
    public int? Level { get; set; } = 1;

    public void Log(string message)
    {
        if (Level == null || Level > 0)
        {
            Helper.WriteLine(message);
        }
    }

    // [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
    public void Log(string format, params object?[] args)
    {
        if (Level == null || Level > 0)
        {
            var message = string.Format(format, args);
            Helper.WriteLine(message);
        }
    }
}