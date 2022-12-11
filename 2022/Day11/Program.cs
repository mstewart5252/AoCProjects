using System.Globalization;
using System.Text;

namespace Day11;

public static class Program
{
    public static void Main()
    {
        var file = File.ReadAllLines("test.txt");
        
        var monkeys = GetMonkeys(file);
        SimulateRounds(ref monkeys, 20, true);
        PrintMonkeyBusiness(monkeys);
        
        monkeys = GetMonkeys(file);
        SimulateRounds(ref monkeys, 10000, false);
        PrintMonkeyBusiness(monkeys);
    }

    private static void PrintMonkeyBusiness(List<Monkey> monkeys)
    {
        var topTwoMonkeys = monkeys.OrderByDescending(m => m.ItemsInspected).Take(2).ToList();
        var result = topTwoMonkeys.First().ItemsInspected * topTwoMonkeys.Last().ItemsInspected;
        Console.WriteLine(result);
    }

    private static void SimulateRounds(ref List<Monkey> monkeys, int rounds, bool worryEffect)
    {
        for (var i = 0; i < rounds; i++)
        {
            monkeys = SimulateRound(monkeys, worryEffect);
        }
    }

    private static List<Monkey> SimulateRound(List<Monkey> monkeys, bool worryEffect)
    {
        foreach (var monkey in monkeys)
        {
            foreach (var item in monkey.Items)
            {
                var worry = GetWorry(monkey.Operation, item, worryEffect);
                var testPassed = TestWorry(monkey.Test, worry);
                if (testPassed)
                {
                    monkeys[monkey.MonkeyPassOnTrue].Items.Add(worry);
                }
                else
                {
                    monkeys[monkey.MonkeyPassOnFalse].Items.Add(worry);
                }

                monkey.ItemsInspected++;
            }

            monkey.Items = new List<int>();
        }

        return monkeys;
    }

    private static bool TestWorry(int monkeyTest, int worry)
    {
        return worry % monkeyTest == 0;
    }

    private static int GetWorry(string monkeyOperation, int item, bool worryEffect)
    {
        var a = item;
        int b;
        if (monkeyOperation[6..] == "old")
            b = item;
        else
            b = int.Parse(monkeyOperation[6..]);

        switch (monkeyOperation[4])
        {
            case '+':
                return worryEffect ? (a + b) / 3 : a + b;
            case '-':
                return worryEffect ? (a - b) / 3 : a - b;
            case '*':
                return worryEffect ? (a * b) / 3 : a * b;
            default:
                return worryEffect ? (a / b) / 3 : a / b;
        }
    }

    private static List<Monkey> GetMonkeys(string[] file)
    {
        var monkeys = new List<Monkey>();

        var pos = 0;
        while (!(pos >= file.Length))
        {
            monkeys.Add(new Monkey(file[new Range(pos, pos + 6)]));
            pos += 7;
        }

        return monkeys;
    }
}

public class Monkey
{
    public int Id { get; set; }
    public List<int> Items { get; set; }
    public string Operation { get; set; }
    public int Test { get; set; }
    public int MonkeyPassOnTrue { get; set; }
    public int MonkeyPassOnFalse { get; set; }
    public long ItemsInspected { get; set; }

    public Monkey(string[] input)
    {
        Id = int.Parse(input[0][7].ToString());
        Items = GetItems(input[1][17..]);
        Operation = input[2][19..];
        Test = int.Parse(input[3][20..]);
        MonkeyPassOnTrue = int.Parse(input[4].Last().ToString());
        MonkeyPassOnFalse = int.Parse(input[5].Last().ToString());
        ItemsInspected = 0;
    }

    private List<int> GetItems(string items)
    {
        var output = new List<int>();
        var splitItems = items.Split(',');
        foreach (var item in splitItems)
        {
            output.Add(int.Parse(item));
        }

        return output;
    }
}