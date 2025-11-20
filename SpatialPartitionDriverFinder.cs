using System.Collections.Generic;
using System.Linq;

public class SpatialPartitionDriverFinder : IDriverFinder
{
    private Dictionary<(int, int), List<Driver>> _grid;
    private readonly int _gridSize;
    
    public string AlgorithmName => "Spatial Partition";
    
    public SpatialPartitionDriverFinder(int gridSize = 10)
    {
        _gridSize = gridSize;
        _grid = new Dictionary<(int, int), List<Driver>>();
    }
    
    public void UpdateDrivers(IEnumerable<Driver> drivers)
    {
        _grid.Clear();
        foreach (var driver in drivers)
        {
            var gridKey = (driver.X / _gridSize, driver.Y / _gridSize);
            if (!_grid.ContainsKey(gridKey))
            {
                _grid[gridKey] = new List<Driver>();
            }
            _grid[gridKey].Add(driver);
        }
    }
    
    public IEnumerable<Driver> FindNearestDrivers(int targetX, int targetY, IEnumerable<Driver> drivers, int count = 5)
    {
        if (!_grid.Any())
            UpdateDrivers(drivers);
            
        var targetGridKey = (targetX / _gridSize, targetY / _gridSize);
        var candidates = new List<Driver>();
        
        // Search in increasingly larger areas until we have enough candidates
        for (int radius = 0; radius <= 5; radius++)
        {
            for (int dx = -radius; dx <= radius; dx++)
            {
                for (int dy = -radius; dy <= radius; dy++)
                {
                    var searchKey = (targetGridKey.Item1 + dx, targetGridKey.Item2 + dy);
                    if (_grid.ContainsKey(searchKey))
                    {
                        candidates.AddRange(_grid[searchKey]);
                    }
                }
            }
            
            // If we have enough candidates, break early
            if (candidates.Count >= count * 2) // Collect some extra for sorting
                break;
        }
        
        // Fallback: if we still don't have enough, use all drivers
        if (candidates.Count < count)
        {
            candidates = _grid.Values.SelectMany(x => x).ToList();
        }
        
        return candidates
            .Select(driver => new { Driver = driver, Distance = driver.DistanceTo(targetX, targetY) })
            .OrderBy(x => x.Distance)
            .Take(count)
            .Select(x => x.Driver);
    }
}