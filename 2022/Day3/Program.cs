using System.Globalization;
using System.Text;

namespace Day3;

public static class Program
{
    public static void Main()
    {
        var file = File.OpenText("input.txt");

        var part1Priority = 0;
        var part2Priority = 0;
        var group = new List<string>();
        
        var rucksack = file.ReadLine();
        while (!string.IsNullOrEmpty(rucksack))
        {
            part1Priority = Part1(rucksack, ref part1Priority);
            
            group.Add(rucksack);
            if (group.Count == 3)
            {
                part2Priority = Part2(group, ref part2Priority);
                group = new List<string>();
            }
            
            rucksack = file.ReadLine();
        }
                
        Console.WriteLine($"Total Part 1 Priority: {part1Priority}");
        Console.WriteLine($"Total Part 2 Priority: {part2Priority}");
    }

    private static int Part2(List<string> group, ref int part2Priority)
    {
        var badge = FindCommonItem(group);
        var priority = FindItemPriority(badge);
        part2Priority += priority;

        Console.WriteLine($"Badge: {badge}");
        Console.WriteLine($"Priority: {priority}");
        
        return part2Priority;
    }

    private static int Part1(string rucksack, ref int totalPriority)
    {
        var compartments = new List<string>
        {
            rucksack[Range.EndAt(rucksack.Length / 2)],
            rucksack[Range.StartAt(rucksack.Length / 2)]
        };

        var commonItem = FindCommonItem(compartments);
        var priority = FindItemPriority(commonItem);
        totalPriority += priority;

        Console.WriteLine($"Common Item: {commonItem}");
        Console.WriteLine($"Priority: {priority}");
        return totalPriority;
    }

    private static int FindItemPriority(char commonItem)
    {
        if (commonItem > 64 && commonItem < 91)
            return commonItem - 38;
        else
            return commonItem - 96;
    }

    private static char FindCommonItem(List<string> inventories, string commonItems = "")
    {
        var inv1 = inventories.First();
        var inv2 = inventories.Last();

        if (commonItems != "")
        {
            foreach (var item in commonItems)
            {
                if (!inv1.Contains(item))
                    commonItems = commonItems.Replace(item.ToString(), "");
            }
        }
        if (commonItems.Length == 1) return commonItems[0];
        
        char? commonItem;
        do
        {
            commonItem = inv1.Select(c => c).FirstOrDefault(c => inv2.Contains(c));
            if (commonItem != '\0')
            {
                if (!commonItems.Contains((char)commonItem))
                    commonItems += commonItem;
                inv1 = inv1.Replace(((char)commonItem).ToString(), "");
            }
        } while (commonItem != '\0');
        
        if (commonItems.Length == 1) return commonItems[0];
        if (inventories.Count > 2)
        {
            inventories.RemoveAt(0);
            return FindCommonItem(inventories, commonItems);
        }
        else return 'a';
    }
}