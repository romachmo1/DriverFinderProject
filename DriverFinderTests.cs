using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

[TestFixture]
public class DriverFinderTests
{
    private List<Driver> _drivers = new List<Driver>();
    private readonly int _targetX = 50;
    private readonly int _targetY = 50;
    
    [SetUp]
    public void Setup()
    {
        _drivers = new List<Driver>
        {
            new Driver("1", 10, 10),
            new Driver("2", 20, 20),
            new Driver("3", 30, 30),
            new Driver("4", 40, 40),
            new Driver("5", 50, 50),
            new Driver("6", 60, 60),
            new Driver("7", 70, 70),
            new Driver("8", 80, 80),
            new Driver("9", 90, 90),
            new Driver("10", 100, 100)
        };
    }
    
    [Test]
    public void BruteForceFinder_ShouldReturnFiveNearestDrivers()
    {
        // Arrange
        var finder = new BruteForceDriverFinder();
        
        // Act
        var result = finder.FindNearestDrivers(_targetX, _targetY, _drivers, 5).ToList();
        
        // Assert
        Assert.AreEqual(5, result.Count);
        Assert.AreEqual("5", result[0].Id); // Closest
        Assert.AreEqual("4", result[1].Id);
        Assert.AreEqual("6", result[2].Id);
    }
    
    [Test]
    public void PriorityQueueFinder_ShouldReturnFiveNearestDrivers()
    {
        // Arrange
        var finder = new PriorityQueueDriverFinder();
        
        // Act
        var result = finder.FindNearestDrivers(_targetX, _targetY, _drivers, 5).ToList();
        
        // Assert
        Assert.AreEqual(5, result.Count);
        Assert.AreEqual("5", result[0].Id);
    }
    
    [Test]
    public void SpatialPartitionFinder_ShouldReturnFiveNearestDrivers()
    {
        // Arrange
        var finder = new SpatialPartitionDriverFinder(25); // Increased grid size for better distribution
        finder.UpdateDrivers(_drivers);
        
        // Act
        var result = finder.FindNearestDrivers(_targetX, _targetY, _drivers, 5).ToList();
        
        // Debug output
        Console.WriteLine($"Found {result.Count} drivers:");
        foreach (var driver in result)
        {
            Console.WriteLine($"Driver {driver.Id}: ({driver.X}, {driver.Y}) - Distance: {driver.DistanceTo(_targetX, _targetY)}");
        }
        
        // Assert
        Assert.AreEqual(5, result.Count, $"Expected 5 drivers but got {result.Count}");
    }
    
    [Test]
    public void KdTreeFinder_ShouldReturnFiveNearestDrivers()
    {
        // Arrange
        var finder = new KdTreeDriverFinder();
        finder.BuildTree(_drivers);
        
        // Act
        var result = finder.FindNearestDrivers(_targetX, _targetY, _drivers, 5).ToList();
        
        // Assert
        Assert.AreEqual(5, result.Count);
    }
    
    [Test]
    public void Finder_ShouldHandleEmptyDriverList()
    {
        // Arrange
        var finder = new BruteForceDriverFinder();
        var emptyDrivers = new List<Driver>();
        
        // Act
        var result = finder.FindNearestDrivers(_targetX, _targetY, emptyDrivers, 5);
        
        // Assert
        Assert.IsEmpty(result);
    }
    
    [Test]
    public void Finder_ShouldHandleLessDriversThanRequested()
    {
        // Arrange
        var finder = new BruteForceDriverFinder();
        var fewDrivers = _drivers.Take(3).ToList();
        
        // Act
        var result = finder.FindNearestDrivers(_targetX, _targetY, fewDrivers, 5).ToList();
        
        // Assert
        Assert.AreEqual(3, result.Count);
    }
}