using System.Globalization;
using System.Text;

namespace Day7;

public static class Program
{
    public static void Main()
    {
        var file = File.ReadAllLines("input.txt");
        
        var directories = new Dictionary<string, int>();
        var currentPath = new List<string>();
        var currentDirectory = "";
        foreach (var line in file)
        {
            GetDirectorySizes(line, ref directories, ref currentPath, ref currentDirectory);
        }

        var totalOutput = directories.Sum(d => d.Value <= 100000 ? d.Value : 0);
        Console.WriteLine($"Total size of directories less than 100001: {totalOutput}");

        var totalStorage = 70000000;
        var unused = totalStorage - directories["/"];
        var needed = 30000000 - unused;
        var directoryToDelete = directories.Where(d => d.Value >= needed).Min(d => d.Value);
        Console.WriteLine($"Directory size to be deleted: {directoryToDelete}");
    }

    private static void GetDirectorySizes(string line, ref Dictionary<string, int> directories, ref List<string> currentPath,
        ref string currentDirectory)
    {
        var currentPathString = "";
        foreach (var dir in currentPath)
        {
            currentPathString += $"{dir}/";
        }
        if (line[0] == '$')
        {
            if (line[new Range(2, 4)] == "cd")
            {
                var directoryName = line.GetDirectoryName();
                if (directoryName == "/")
                {
                    directories.Add(directoryName, 0);
                    currentPath.Add(directoryName);
                    currentDirectory = directoryName;
                }
                else if (directoryName == "..")
                {
                    currentPath.Remove(currentDirectory);
                    currentDirectory = currentPath.Last();
                }
                else
                {
                    var fullName = $"{currentPathString}{directoryName}";
                    currentPath.Add(fullName);
                    currentDirectory = fullName;
                }
            }
        }
        else
        {
            if (line.IsDirectory())
            {
                var directoryName = line.GetDirectoryName();
                if (!directories.Keys.Contains(directoryName))
                {
                    directoryName = $"{currentPathString}{directoryName}";
                    directories.Add(directoryName, 0);
                }
            }
            else
            {
                var fileSize = line.GetFileSize();
                foreach (var directory in currentPath)
                {
                    directories[directory] += fileSize;
                }
            }
        }
    }

    private static int GetFileSize(this string line)
    {
        var fileNameStart = int.MaxValue;
        for (var i = 0; i < line.Length; i++)
        {
            if (!int.TryParse(line[i].ToString(), out _))
            {
                fileNameStart = i;
                break;
            }
        }

        return int.Parse(line[new Range(0, fileNameStart)]);
    }

    private static bool IsDirectory(this string line)
    {
        return line[new Range(0, 3)] == "dir";
    }

    private static string GetDirectoryName(this string line)
    {
        if (line[0] == '$')
            return line[5..];
        else 
            return line[4..];
    }
}