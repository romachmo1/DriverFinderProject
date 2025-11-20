using System.Collections.Generic;
using System.Linq;

public class BruteForceDriverFinder : IDriverFinder
{
    public string AlgorithmName => "Brute Force";
    
    public IEnumerable<Driver> FindNearestDrivers(int targetX, int targetY, IEnumerable<Driver> drivers, int count = 5)
    {
        if (drivers == null || !drivers.Any())
            return Enumerable.Empty<Driver>();
            
        var driversList = drivers.ToList();
        if (driversList.Count <= count)
            return driversList;
            
        return driversList
            .Select(driver => new { Driver = driver, Distance = driver.DistanceTo(targetX, targetY) })
            .OrderBy(x => x.Distance)
            .Take(count)
            .Select(x => x.Driver);
    }
}