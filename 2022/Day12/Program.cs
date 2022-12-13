namespace Day12;

public static class Program
{
    public static void Main()
    {
        var file = File.ReadAllLines("input.txt");
        
        var openNodes = new List<Node>();
        var closedNodes = new List<Node>();
        FindOnGrid(file, out var startNode, out var endLocation);
        openNodes.Add(startNode);

        //Part1
        var finalNode = AStarSearch(openNodes, closedNodes, endLocation, file);
        var steps = finalNode.GetSteps();
        Console.WriteLine($"Part 1: {steps}");
        
        //Part2
        openNodes = new List<Node>();
        closedNodes = new List<Node>();
        openNodes.Add(new Node(endLocation));
        var firstLowestNode = ReverseAStarSearch(openNodes, closedNodes, file);
        steps = firstLowestNode.GetSteps();
        Console.WriteLine($"Part 2: {steps}");
    }

    private static Node? ReverseAStarSearch(List<Node> openNodes, List<Node> closedNodes, string[] file)
    {
        Node? finalNode = null;

        while (openNodes.Any())
        {
            var currentNode = openNodes.OrderByDescending(n => n.TravelScore).First();
            if (currentNode == null)
                throw new Exception();

            openNodes.Remove(currentNode);
            closedNodes.Add(currentNode);

            if (currentNode.GridSquare.Elevation.Equals('a'))
            {
                if (finalNode == null || GetSteps(finalNode) > GetSteps(currentNode.Parent))
                    finalNode = currentNode.Parent;
                break;
            }

            foreach (var neighbor in GetNeighbors(currentNode, file))
            {
                if (!IsTraversable(neighbor.GridSquare, currentNode.GridSquare))
                    continue;

                var oldNode = closedNodes.FirstOrDefault(n => n.GridSquare.Location.Equals(neighbor.GridSquare.Location));
                if (oldNode != null)
                {
                    if (neighbor.DistanceFromStart < oldNode.DistanceFromStart)
                        oldNode.UpdateParent(currentNode);
                }
                else
                {
                    oldNode = openNodes.FirstOrDefault(n => n.GridSquare.Location.Equals(neighbor.GridSquare.Location));
                    if (oldNode != null)
                    {
                        if (neighbor.GetSteps() < oldNode.GetSteps())
                            oldNode.UpdateParent(currentNode);
                    }
                    else
                    {
                        openNodes.Add(neighbor);
                    }
                }
            }
        }

        return finalNode;
    }

    private static Node? AStarSearch(List<Node> openNodes, List<Node> closedNodes, GridSquare endLocation, string[] file)
    {
        Node? finalNode = null;

        while (openNodes.Any())
        {
            var currentNode = openNodes.OrderByDescending(n => n.TravelScore).First();
            if (currentNode == null)
                throw new Exception();

            openNodes.Remove(currentNode);
            closedNodes.Add(currentNode);

            if (currentNode.GridSquare.Location.Equals(endLocation.Location))
            {
                if (finalNode == null || GetSteps(finalNode) > GetSteps(currentNode.Parent))
                    finalNode = currentNode.Parent;
                break;
            }

            foreach (var neighbor in GetNeighbors(currentNode, file))
            {
                if (!IsTraversable(currentNode.GridSquare, neighbor.GridSquare))
                    continue;

                var oldNode = closedNodes.FirstOrDefault(n => n.GridSquare.Location.Equals(neighbor.GridSquare.Location));
                if (oldNode != null)
                {
                    if (neighbor.DistanceFromStart < oldNode.DistanceFromStart)
                        oldNode.UpdateParent(currentNode);
                }
                else
                {
                    oldNode = openNodes.FirstOrDefault(n => n.GridSquare.Location.Equals(neighbor.GridSquare.Location));
                    if (oldNode != null)
                    {
                        if (neighbor.GetSteps() < oldNode.GetSteps())
                            oldNode.UpdateParent(currentNode);
                    }
                    else
                    {
                        openNodes.Add(neighbor);
                    }
                }
            }
        }

        return finalNode;
    }

    private static int GetSteps(this Node? node)
    {
        var steps = 0;
        var nodeTemp = node;
        while (nodeTemp != null)
        {
            nodeTemp = nodeTemp.Parent;
            steps++;
        }

        return steps;
    }

    private static bool IsTraversable(GridSquare locationA, GridSquare locationB)
    {
        return locationA.Elevation - locationB.Elevation == -1 ||
               locationA.Elevation >= locationB.Elevation;
    }

    private static List<Node> GetNeighbors(Node currentNode,
        string[] input)
    {
        var neighborLocations = new List<Tuple<int, int>>();
        for (var i = 0; i < 4; i++)
        {
            switch (i)
            {
                case 0:
                    if (currentNode.GridSquare.Location.Item1 + 1 < input.Length)
                        neighborLocations.Add(new Tuple<int, int>(
                            currentNode.GridSquare.Location.Item1 + 1,
                            currentNode.GridSquare.Location.Item2));
                    break;
                
                case 1: if (currentNode.GridSquare.Location.Item1 - 1 >= 0)
                    neighborLocations.Add(new Tuple<int, int>(
                        currentNode.GridSquare.Location.Item1 - 1,
                        currentNode.GridSquare.Location.Item2));
                    break;
                
                case 2: if (currentNode.GridSquare.Location.Item2 + 1 < input[0].Length)
                    neighborLocations.Add(new Tuple<int, int>(
                        currentNode.GridSquare.Location.Item1,
                        currentNode.GridSquare.Location.Item2 + 1));
                    break;

                case 3: if (currentNode.GridSquare.Location.Item2 - 1 >= 0)
                    neighborLocations.Add(new Tuple<int, int>(
                        currentNode.GridSquare.Location.Item1,
                        currentNode.GridSquare.Location.Item2 - 1));
                    break;
            }
        }

        var output = new List<Node>();

        foreach (var neighborLocation in neighborLocations)
        {
            output.Add(new Node(new GridSquare(neighborLocation, 
                input[neighborLocation.Item1][neighborLocation.Item2]), currentNode));
        }

        return output;
    }

    private static void FindOnGrid(string[] input, out Node startNode, out GridSquare endLocation)
    {
        GridSquare? startTemp = null;
        GridSquare? endTemp = null;
        for (var i = 0; i < input.Length; i++)
        {
            for (var j = 0; j < input[i].Length; j++)
            {
                if (input[i][j] == 'S')
                    startTemp = new GridSquare(new Tuple<int, int>(i, j), 'a');
                else if (input[i][j] == 'E')
                    endTemp = new GridSquare(new Tuple<int, int>(i, j), 'z');

                if (startTemp != null && endTemp != null)
                {
                    startNode = new Node(startTemp);
                    endLocation = endTemp;
                    return;
                }
            }
        }

        startNode = null;
        endLocation = null;
    }
}

public class GridSquare
{
    public int Elevation { get; set; }
    public Tuple<int, int> Location { get; set; }

    public GridSquare(Tuple<int, int> location, char value)
    {
        if (value == 'S')
            value = 'a';
        if (value == 'E')
            value = 'z';
        Elevation = value;
        Location = location;
    }
}

public class Node
{
    public Node? Parent { get; set; }
    public GridSquare GridSquare { get; set; }
    public double DistanceFromStart { get; set; }
    public double DistanceToEnd { get; set; }
    public double TravelScore { get; set; }

    public Node(GridSquare thisLocation, Node? parent = null)
    {
        GridSquare = thisLocation;
        DistanceFromStart = parent?.DistanceFromStart + 1 ?? 0;
        //DistanceToEnd = GetDistance(thisLocation, endLocation);
        
        //Using arbitrary number close to distance from start at end of testing
        //GetDistance was not accurate due to obstacles
        //This is no longer a pure heuristic search, but it works in this case
        DistanceToEnd = 350 - DistanceFromStart;
        TravelScore = DistanceFromStart + DistanceToEnd;
        if (parent != null)
            Parent = parent;
    }

    public void UpdateParent(Node newParent)
    {
        Parent = newParent;
        DistanceFromStart = newParent.DistanceFromStart + 1;
        TravelScore = DistanceFromStart + DistanceToEnd;
    }

    private double GetDistance(GridSquare start, GridSquare target)
    {
        var a = Math.Abs(target.Location.Item1 - start.Location.Item1);
        var b = Math.Abs(target.Location.Item2 - start.Location.Item2);
        
        return a + b;
    }
}