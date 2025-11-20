using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;

[MemoryDiagnoser]
[RankColumn]
[Config(typeof(BenchmarkConfig))]
public class DriverFinderBenchmarks
{
    private List<Driver> _drivers = new List<Driver>();
    private readonly int _targetX = 500;
    private readonly int _targetY = 500;
    private readonly int _driverCount = 10000;
    
    private BruteForceDriverFinder _bruteForceFinder = new BruteForceDriverFinder();
    private PriorityQueueDriverFinder _priorityQueueFinder = new PriorityQueueDriverFinder();
    private SpatialPartitionDriverFinder _spatialPartitionFinder = new SpatialPartitionDriverFinder(50);
    private KdTreeDriverFinder _kdTreeFinder = new KdTreeDriverFinder();
    
    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(42);
        _drivers = new List<Driver>();
        
        for (int i = 0; i < _driverCount; i++)
        {
            _drivers.Add(new Driver(
                i.ToString(),
                random.Next(0, 1000),
                random.Next(0, 1000)
            ));
        }
        
        _spatialPartitionFinder.UpdateDrivers(_drivers);
        _kdTreeFinder.BuildTree(_drivers);
    }
    
    [Benchmark(Baseline = true)]
    public void BruteForceFinder() 
        => _bruteForceFinder.FindNearestDrivers(_targetX, _targetY, _drivers, 5).ToList();
    
    [Benchmark]
    public void PriorityQueueFinder() 
        => _priorityQueueFinder.FindNearestDrivers(_targetX, _targetY, _drivers, 5).ToList();
    
    [Benchmark]
    public void SpatialPartitionFinder() 
        => _spatialPartitionFinder.FindNearestDrivers(_targetX, _targetY, _drivers, 5).ToList();
    
    [Benchmark]
    public void KdTreeFinder() 
        => _kdTreeFinder.FindNearestDrivers(_targetX, _targetY, _drivers, 5).ToList();
}