using System.Globalization;
using System.Text;

namespace Day6;

public static class Program
{
    public static void Main()
    {
        var file = File.OpenText("input.txt");
        var input = file.ReadLine();
        
        if (string.IsNullOrEmpty(input)) return;

        var part1 = FindMarker(input, 4);
        Console.WriteLine($"Start of packet position: {part1}");

        var part2 = FindMarker(input, 14);
        Console.WriteLine($"Start of message position: {part2}");
    }

    private static int FindMarker(string input, int bufferLength)
    {
        for (var i = 0; i < input.Length - bufferLength; i++)
        {
            var isStartOfPacket = true;
            var buffer = input[new Range(i, i + bufferLength)];
            for (var j = 0; j < bufferLength; j++)
            {
                var temp = buffer[j];
                buffer = buffer.Remove(j, 1);

                if (buffer.Contains(temp))
                {
                    isStartOfPacket = false;
                    break;
                }

                buffer = buffer.Insert(j, temp.ToString());
            }

            if (isStartOfPacket)
                return i + bufferLength;
        }

        return 0;
    }
}