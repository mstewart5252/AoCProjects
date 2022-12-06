using System.Globalization;
using System.Text;

namespace Day5;

public static class Program
{
    public static void Main()
    {
        var lines = File.ReadLines("input.txt").ToArray();
        var crates = new string[9];

        var commandsBegin = int.MaxValue;
        
        for (var i = 0; i < lines.Count(); i++)
        {
            if (!string.IsNullOrEmpty(lines[i]) && lines[i][0] == '[')
            {
                BuildCrates(lines, i, ref crates);
            }

            if (lines[i] == " 1   2   3   4   5   6   7   8   9 ")
                commandsBegin = i + 2;

            if (i >= commandsBegin)
            {
                var commandLine = lines[i];
                var commands = FindCommands(commandLine);
                //MoveCratesPart1(commands, crates);
                MoveCratesPart2(commands, crates);
            }
        }

        var result = "";
        foreach (var line in crates)
        {
            result += line[0];
        }
        Console.WriteLine(result);
    }

    private static void MoveCratesPart2(int[] commands, string[] crates)
    {
        var cratesToMove = "";
        for (var j = 0; j < commands[0]; j++)
        {
            var crate = crates[commands[1] - 1][0];
            crates[commands[1] - 1] = crates[commands[1] - 1].Remove(0, 1);
            cratesToMove += crate;
        }

        crates[commands[2] - 1] = crates[commands[2] - 1].Insert(0, cratesToMove);
    }

    private static void MoveCratesPart1(int[] commands, string[] crates)
    {
        for (var j = 0; j < commands[0]; j++)
        {
            var crate = crates[commands[1] - 1][0];
            crates[commands[1] - 1] = crates[commands[1] - 1].Remove(0, 1);
            crates[commands[2] - 1] = crates[commands[2] - 1].Insert(0, crate.ToString());
        }
    }

    private static void BuildCrates(string[] lines, int i, ref string[] crates)
    {
        var crateStrings = lines[i].Chunk(4).ToArray();
        var crateString = "";
        for (var j = 0; j < crateStrings.Count(); j++)
        {
            crateString += crateStrings[j][1];
        }

        for (var j = 0; j < 9; j++)
        {
            if (crateString[j] != ' ')
                crates[j] += crateString[j];
        }
    }

    private static int[] FindCommands(string commandLine)
    {
        var commands = new int[3];
        var commandsFound = 0;
        for (var j = 0; j < commandLine.Length; j++)
        {
            var commandString = "";
            while (j < commandLine.Length && int.TryParse(commandLine[j].ToString(), out _))
            {
                commandString += commandLine[j];
                j++;
            }

            if (commandString != "")
            {
                commands[commandsFound] = int.Parse(commandString);
                commandsFound++;
            }
        }

        return commands;
    }
}