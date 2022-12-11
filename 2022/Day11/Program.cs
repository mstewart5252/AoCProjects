using System.Globalization;
using System.Text;

namespace Day11;

public static class Program
{
    public static void Main()
    {
        var file = File.ReadAllLines("input.txt");
        
        var monkeys = GetMonkeys(file);
        var mod = 1;
        foreach (var monkey in monkeys)
        {
            mod *= monkey.Test;
        }
        SimulateRounds(ref monkeys, 20, true, mod);
        PrintMonkeyBusiness(monkeys);
        
        monkeys = GetMonkeys(file);
        SimulateRounds(ref monkeys, 10000, false, mod);
        PrintMonkeyBusiness(monkeys);
    }

    private static void PrintMonkeyBusiness(List<Monkey> monkeys)
    {
        var topMonkeys = monkeys.OrderByDescending(m => m.ItemsInspected).Take(2).ToArray();
        var result = topMonkeys[0].ItemsInspected * topMonkeys[1].ItemsInspected;
        Console.WriteLine(result);
    }

    private static void SimulateRounds(ref List<Monkey> monkeys, int rounds, bool worryEffect, int mod)
    {
        for (var i = 0; i < rounds; i++)
        {
            monkeys = SimulateRound(monkeys, worryEffect, mod);
        }
    }

    private static List<Monkey> SimulateRound(List<Monkey> monkeys, bool worryEffect, int mod)
    {
        foreach (var monkey in monkeys)
        {
            while (monkey.Items.Any())
            {
                var item = monkey.Items.First();
                var worry = worryEffect ? monkey.Operation(item) / 3 : monkey.Operation(item);
                monkey.ItemsInspected++;
                var testPassed = TestWorry(monkey.Test, worry);
                if (testPassed)
                    monkeys[monkey.MonkeyPassOnTrue].Items.Add(worry);
                else
                    monkeys[monkey.MonkeyPassOnFalse].Items.Add(worry);
                
                monkey.Items.Remove(item);
            }
        }

        return monkeys;
    }

    private static bool TestWorry(int monkeyTest, long worry)
    {
        return worry % monkeyTest == 0;
    }
    
    private static int GetMod(string[] file)
    {
        var mod = 1;
        for (var i = 3; i < file.Length; i += 7)
        {
            mod *= int.Parse(file[i][21..]);
        }

        return mod;
    }

    private static List<Monkey> GetMonkeys(string[] file)
    {
        var monkeys = new List<Monkey>();
        var mod = GetMod(file);

        var pos = 0;
        while (!(pos >= file.Length))
        {
            monkeys.Add(new Monkey(file[new Range(pos, pos + 6)], mod));
            pos += 7;
        }

        return monkeys;
    }
}

public class Monkey
{
    public int Id { get; set; }
    public List<long> Items { get; set; }
    public Func<long, long> Operation { get; set; }
    public int Test { get; set; }
    public int MonkeyPassOnTrue { get; set; }
    public int MonkeyPassOnFalse { get; set; }
    public long ItemsInspected { get; set; }

    public Monkey(string[] input, int mod)
    {
        Id = int.Parse(input[0][7].ToString());
        Items = GetItems(input[1][17..]);
        Test = int.Parse(input[3][20..]);
        
        var operationString = input[2][19..];
        Operation = (x) =>
        {
            var ops = operationString.Split(' ');
            var a = int.TryParse(ops[0], out var val) ? val : x;
            var b = int.TryParse(ops[2], out val) ? val : x;
            return (ops[1][0] == '+' ? a + b : a * b) % mod;
        };
        MonkeyPassOnTrue = int.Parse(input[4].Last().ToString());
        MonkeyPassOnFalse = int.Parse(input[5].Last().ToString());
        ItemsInspected = 0;
    }

    private List<long> GetItems(string items)
    {
        var output = new List<long>();
        var splitItems = items.Split(',');
        foreach (var item in splitItems)
        {
            output.Add(long.Parse(item));
        }

        return output;
    }
}