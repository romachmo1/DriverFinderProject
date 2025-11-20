using System.Collections.Generic;
using System.Linq;

public class PriorityQueueDriverFinder : IDriverFinder
{
    public string AlgorithmName => "Priority Queue";
    
    public IEnumerable<Driver> FindNearestDrivers(int targetX, int targetY, IEnumerable<Driver> drivers, int count = 5)
    {
        if (drivers == null || !drivers.Any())
            return Enumerable.Empty<Driver>();
            
        var driversList = drivers.ToList();
        if (driversList.Count <= count)
            return driversList;
        
        var priorityQueue = new PriorityQueue<Driver, double>();
        
        foreach (var driver in driversList)
        {
            var distance = driver.DistanceTo(targetX, targetY);
            priorityQueue.Enqueue(driver, distance);
        }
        
        var result = new List<Driver>();
        for (int i = 0; i < count && priorityQueue.Count > 0; i++)
        {
            result.Add(priorityQueue.Dequeue());
        }
        
        return result;
    }
}