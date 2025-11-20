// IDriverFinder.cs
public interface IDriverFinder
{
    IEnumerable<Driver> FindNearestDrivers(int targetX, int targetY, IEnumerable<Driver> drivers, int count = 5);
    string AlgorithmName { get; }
}