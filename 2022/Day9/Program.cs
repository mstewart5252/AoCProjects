using System.Globalization;
using System.Text;

namespace Day9;

public static class Program
{
    public static void Main()
    {
        var file = File.ReadAllLines("input.txt");

        var part1 = MoveRope(file, 1);
        var part2 = MoveRope(file, 9);

        Console.WriteLine(part1.Count);
        Console.WriteLine(part2.Count);
    }

    private static List<Coordinate> MoveRope(string[] file, int knotCount)
    {
        var head = new Coordinate(0, 0);
        var knots = new List<Coordinate>();
        for (var i = 0; i < knotCount; i++)
        {
            knots.Add(new Coordinate(0, 0));
        }

        var positions = new List<Coordinate> { head.Position };
        foreach (var line in file)
        {
            var positionsVisited = new List<Coordinate>();
            switch (line[0])
            {
                case 'U':
                    positionsVisited = MoveUp(int.Parse(line[2..]), ref head, ref knots);
                    break;
                case 'D':
                    positionsVisited = MoveDown(int.Parse(line[2..]), ref head, ref knots);
                    break;
                case 'L':
                    positionsVisited = MoveLeft(int.Parse(line[2..]), ref head, ref knots);
                    break;
                case 'R':
                    positionsVisited = MoveRight(int.Parse(line[2..]), ref head, ref knots);
                    break;
            }

            foreach (var position in positionsVisited)
            {
                if (!positions.Any(p => p.X == position.X && p.Y == position.Y))
                    positions.Add(position);
            }
        }

        return positions;
    }

    private static List<Coordinate> MoveUp(int steps, ref Coordinate head, ref List<Coordinate> knots)
    {
        var output = new List<Coordinate>();
        for (var i = 0; i < steps; i++)
        {
            head.Y++;
            var tempHead = head.Position;
            foreach (var knot in knots)
            {
                var newKnot = MoveTowardsHead(tempHead, knot);
                knot.X = newKnot.X;
                knot.Y = newKnot.Y;
                tempHead = knot.Position;
            }
            output.Add(knots.Last().Position);
        }

        return output;
    }

    private static List<Coordinate> MoveDown(int steps, ref Coordinate head, ref List<Coordinate> knots)
    {
        var output = new List<Coordinate>();
        for (var i = 0; i < steps; i++)
        {
            head.Y--;
            var tempHead = head.Position;
            foreach (var knot in knots)
            {
                var newKnot = MoveTowardsHead(tempHead, knot);
                knot.X = newKnot.X;
                knot.Y = newKnot.Y;
                tempHead = knot.Position;
            }
            output.Add(knots.Last().Position);
        }

        return output;
    }

    private static List<Coordinate> MoveLeft(int steps, ref Coordinate head, ref List<Coordinate> knots)
    {
        var output = new List<Coordinate>();
        for (var i = 0; i < steps; i++)
        {
            head.X--;
            var tempHead = head.Position;
            foreach (var knot in knots)
            {
                var newKnot = MoveTowardsHead(tempHead, knot);
                knot.X = newKnot.X;
                knot.Y = newKnot.Y;
                tempHead = knot.Position;
            }
            output.Add(knots.Last().Position);
        }

        return output;
    }

    private static List<Coordinate> MoveRight(int steps, ref Coordinate head, ref List<Coordinate> knots)
    {
        var output = new List<Coordinate>();
        for (var i = 0; i < steps; i++)
        {
            head.X++;
            var tempHead = head.Position;
            foreach (var knot in knots)
            {
                var newKnot = MoveTowardsHead(tempHead, knot);
                knot.X = newKnot.X;
                knot.Y = newKnot.Y;
                tempHead = knot.Position;
            }
            output.Add(knots.Last().Position);
        }

        return output;
    }

    private static Coordinate MoveTowardsHead(Coordinate head, Coordinate tail)
    {
        switch (head.X - tail.X)
        {
            case > 1:
                tail.X++;
                if (tail.Y != head.Y)
                    switch (head.Y - tail.Y)
                    {
                        case >= 1:
                            tail.Y++;
                            break;
                        case <= -1:
                            tail.Y--;
                            break;
                    }
                return tail.Position;
            case < -1:
                tail.X--;
                if (tail.Y != head.Y)
                    if (tail.Y != head.Y)
                        switch (head.Y - tail.Y)
                        {
                            case >= 1:
                                tail.Y++;
                                break;
                            case <= -1:
                                tail.Y--;
                                break;
                        }
                return tail.Position;
        }
        switch (head.Y - tail.Y)
        {
            case > 1:
                tail.Y++;
                if (tail.X != head.X)
                    switch (head.X - tail.X)
                    {
                        case >= 1:
                            tail.X++;
                            break;
                        case <= -1:
                            tail.X--;
                            break;
                    }
                return tail.Position;
            case < -1:
                tail.Y--;
                if (tail.X != head.X)
                    switch (head.X - tail.X)
                    {
                        case >= 1:
                            tail.X++;
                            break;
                        case <= -1:
                            tail.X--;
                            break;
                    }
                return tail.Position;
        }

        return tail.Position;
    }
}

public class Coordinate
{
    public int X { get; set; }
    public int Y { get; set; }

    public Coordinate Position => new (X, Y);

    public Coordinate(int x, int y)
    {
        X = x;
        Y = y;
    }
}