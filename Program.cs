using BenchmarkDotNet.Running;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Driver Finder System");
        Console.WriteLine("====================");
        
        // Run benchmarks
        Console.WriteLine("Running benchmarks...");
        var summary = BenchmarkRunner.Run<DriverFinderBenchmarks>();
        
        Console.WriteLine("Benchmarks completed!");
    }
}