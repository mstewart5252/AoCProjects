using System.Globalization;
using System.Text;

namespace Day10;

public static class Program
{
    public static void Main()
    {
        var file = File.ReadAllLines("input.txt");
        var part1 = Part1(file);
        Console.WriteLine(part1);

        Part2(file);
    }

    private static void Part2(string[] file)
    {
        var print = "";
        var pos = 0;
        var register = 1;
        foreach (var line in file)
        {
            if (line.StartsWith("noop"))
            {
                print = DrawPixel(ref pos, register, print);
            }
            else if (line.StartsWith("addx"))
            {
                print = DrawPixel(ref pos, register, print);
                print = DrawPixel(ref pos, register, print);
                register += int.Parse(line[5..]);
            }
        }
    }

    private static void PrintRow(ref int pos, ref string print)
    {
        if (pos >= 40)
        {
            pos = 0;
            Console.WriteLine(print);
            print = "";
        }
    }

    private static string DrawPixel(ref int pos, int register, string print)
    {
        if (pos <= register + 1 && pos >= register - 1)
            print += '#';
        else
            print += '.';
        pos++;
        
        PrintRow(ref pos, ref print);
        return print;
    }

    private static int Part1(string[] file)
    {
        var cyclesToCheck = new int[] { 20, 60, 100, 140, 180, 220 };
        var signalStrengths = new List<int>();
        var cycle = 0;
        var register = 1;

        foreach (var line in file)
        {
            if (line.StartsWith("noop"))
            {
                cycle = IncrementCycle(cycle, cyclesToCheck, signalStrengths, register);
            }
            else if (line.StartsWith("addx"))
            {
                cycle = IncrementCycle(cycle, cyclesToCheck, signalStrengths, register);
                cycle = IncrementCycle(cycle, cyclesToCheck, signalStrengths, register);
                register += int.Parse(line[5..]);
            }
        }

        var part1 = signalStrengths.Sum();
        return part1;
    }

    private static int IncrementCycle(int cycle, int[] cyclesToCheck, List<int> signalStrengths, int register)
    {
        cycle++;
        if (cyclesToCheck.Contains(cycle))
            signalStrengths.Add(cycle * register);
        return cycle;
    }
}