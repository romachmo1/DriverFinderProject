using BenchmarkDotNet.Running;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Running Driver Finder Benchmarks...");
        var summary = BenchmarkRunner.Run<DriverBenchmarks>();
        Console.WriteLine("Benchmarks completed!");
    }
}