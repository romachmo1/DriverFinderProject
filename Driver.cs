using System;

public class Driver
{
    public string Id { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    
    public Driver(string id, int x, int y)
    {
        Id = id;
        X = x;
        Y = y;
    }
    
    public double DistanceTo(int targetX, int targetY)
    {
        return Math.Sqrt(Math.Pow(X - targetX, 2) + Math.Pow(Y - targetY, 2));
    }
}