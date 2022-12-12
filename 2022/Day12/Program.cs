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

        Node? finalNode = null;
        do
        {
            var currentNode = openNodes.OrderByDescending(n => n.TravelScore).First();
            if (currentNode == null)
                throw new Exception();
            openNodes.Remove(currentNode);
            closedNodes.Add(currentNode);

            if (!currentNode.GridSquare.Location.Equals(endLocation.Location))
            {
                var neighbors = GetNeighbors(currentNode, endLocation, file);
                foreach (var neighbor in neighbors)
                {
                    //I think it's something here
                    if (!closedNodes.Any(n => n.GridSquare.Location.Equals(neighbor.GridSquare.Location)) && 
                        IsTraversable(currentNode.GridSquare, neighbor.GridSquare))
                    {
                        if (!openNodes.Any(n => n.GridSquare.Location.Equals(neighbor.GridSquare.Location)))
                            openNodes.Add(neighbor);
                        else
                        {
                            var oldNode = openNodes.First(n => n.GridSquare.Location.Equals(neighbor.GridSquare.Location));
                            if (neighbor.TravelScore < oldNode.TravelScore)
                                oldNode.UpdateParent(currentNode);
                        }
                    }
                }
            }
            else
                finalNode = currentNode.Parent;
        } while (finalNode == null);

        var steps = 0;
        do
        {
            finalNode = finalNode.Parent;
            steps++;
        } while (finalNode != null);
        Console.WriteLine(steps);
    }

    private static bool IsTraversable(GridSquare locationA, GridSquare locationB)
    {
        return locationA.Elevation - locationB.Elevation == -1 ||
               locationA.Elevation >= locationB.Elevation;
    }

    private static List<Node> GetNeighbors(Node currentNode,
        GridSquare endLocation,
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
                input[neighborLocation.Item1][neighborLocation.Item2]), endLocation, currentNode));
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
                    startNode = new Node(startTemp, endTemp);
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
    public int DistanceFromStart { get; set; }
    public int DistanceToEnd { get; set; }
    public int TravelScore { get; set; }

    public Node(GridSquare thisLocation, GridSquare endLocation, Node? parent = null)
    {
        GridSquare = thisLocation;
        DistanceFromStart = parent?.DistanceFromStart + 1 ?? 0;
        DistanceToEnd = GetDistance(thisLocation, endLocation);
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

    private int GetDistance(GridSquare start, GridSquare target)
    {
        var a = target.Location.Item1 - start.Location.Item1;
        var b = target.Location.Item2 - start.Location.Item2;
        return a + b;
    }
}