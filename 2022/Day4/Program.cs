using System.Globalization;
using System.Text;

namespace Day4;

public static class Program
{
    public static void Main()
    {
        var file = File.OpenText("input.txt");

        var totalContained = 0;
        var totalOverlapped = 0;
        var line = file.ReadLine();
        while (!string.IsNullOrEmpty(line))
        {
            var pair = line.Split(',');
            
            var sections1 = pair[0].Split('-');
            var sections2 = pair[1].Split('-');

            var start1 = int.Parse(sections1[0]);
            var start2 = int.Parse(sections2[0]);
            var end1 = int.Parse(sections1[1]);
            var end2 = int.Parse(sections2[1]);

            if (Part1(start1, start2, end1, end2))
                totalContained++;
            if (Part2(start1, start2, end1, end2))
                totalOverlapped++;

            line = file.ReadLine();
        }
        
        Console.WriteLine(totalContained);
        Console.WriteLine(totalOverlapped);
    }

    private static bool Part1(int start1, int start2, int end1, int end2)
    {
        return (start1 >= start2 && end1 <= end2) || (start2 >= start1 && end2 <= end1);
    }

    private static bool Part2(int start1, int start2, int end1, int end2)
    {
        if (start1 < start2)
            return end1 >= start2;
        
        else if (start2 < start1)
            return end2 >= start1;
        
        else
            return true;
    }
}