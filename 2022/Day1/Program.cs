var file = File.OpenText("aoc-input.txt");

var elves = new List<int>();
var currentElf = 0;

var currentLine = "";
while (currentLine != null)
{
    currentLine = file.ReadLine();
    
    if (currentLine == "")
    {
        elves.Add(currentElf);
        currentElf = 0;
    }
    else
    {
        int.TryParse(currentLine, out var calories);
        currentElf += calories;
    }
}

var top3Elves = new List<int>();

for (int i = 0; i < 3; i++)
{
    var maxElf = elves.Max();
    top3Elves.Add(maxElf);
    elves.Remove(maxElf);
}

var grandSum = top3Elves.Sum();
Console.WriteLine(grandSum);