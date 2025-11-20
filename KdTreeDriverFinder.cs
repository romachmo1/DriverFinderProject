using System;
using System.Collections.Generic;
using System.Linq;

public class KdTreeDriverFinder : IDriverFinder
{
    private KdTree? _tree;
    
    public string AlgorithmName => "KD-Tree";
    
    public void BuildTree(IEnumerable<Driver> drivers)
    {
        _tree = new KdTree();
        foreach (var driver in drivers)
        {
            _tree.Insert(new double[] { driver.X, driver.Y }, driver);
        }
    }
    
    public IEnumerable<Driver> FindNearestDrivers(int targetX, int targetY, IEnumerable<Driver> drivers, int count = 5)
    {
        if (_tree == null)
            BuildTree(drivers);
            
        var nearest = _tree!.Nearest(new double[] { targetX, targetY }, count);
        return nearest.Select(node => node.Data);
    }
}

public class KdTree
{
    public class Node
    {
        public double[] Point { get; set; }
        public Driver Data { get; set; }
        public Node? Left { get; set; }
        public Node? Right { get; set; }
        
        public Node(double[] point, Driver data)
        {
            Point = point;
            Data = data;
            Left = null;
            Right = null;
        }
    }
    
    private Node? _root;
    
    public void Insert(double[] point, Driver data)
    {
        _root = InsertRec(_root, point, data, 0);
    }
    
    private Node InsertRec(Node? node, double[] point, Driver data, int depth)
    {
        if (node == null)
            return new Node(point, data);
            
        int dimension = depth % point.Length;
        
        if (point[dimension] < node.Point[dimension])
            node.Left = InsertRec(node.Left, point, data, depth + 1);
        else
            node.Right = InsertRec(node.Right, point, data, depth + 1);
            
        return node;
    }
    
    public List<Node> Nearest(double[] target, int k)
    {
        var best = new List<Node>();
        if (_root != null)
        {
            NearestRec(_root, target, best, k, 0);
        }
        return best;
    }
    
    private void NearestRec(Node node, double[] target, List<Node> best, int k, int depth)
    {
        // Add current node to best list and maintain size
        best.Add(node);
        best.Sort((a, b) => DistanceSquared(a.Point, target).CompareTo(DistanceSquared(b.Point, target)));
        if (best.Count > k)
            best.RemoveAt(best.Count - 1);
        
        int dimension = depth % target.Length;
        Node? first, second;
        
        if (target[dimension] < node.Point[dimension])
        {
            first = node.Left;
            second = node.Right;
        }
        else
        {
            first = node.Right;
            second = node.Left;
        }
        
        if (first != null)
        {
            NearestRec(first, target, best, k, depth + 1);
        }
        
        // Check if we need to search the other branch
        if (second != null)
        {
            double currentWorstDistance = best.Count == k ? DistanceSquared(best[^1].Point, target) : double.MaxValue;
            double axisDistance = Math.Abs(node.Point[dimension] - target[dimension]);
            
            if (axisDistance * axisDistance < currentWorstDistance)
            {
                NearestRec(second, target, best, k, depth + 1);
            }
        }
    }
    
    private double DistanceSquared(double[] a, double[] b)
    {
        double sum = 0;
        for (int i = 0; i < a.Length; i++)
        {
            sum += (a[i] - b[i]) * (a[i] - b[i]);
        }
        return sum;
    }
}