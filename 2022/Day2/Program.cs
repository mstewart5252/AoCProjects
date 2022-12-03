namespace AoCDay2;

public static class Program
{
    public static void Main()
    {
        var file = File.OpenText("input.txt");
        var inputLine = file.ReadLine();
        var totalScore = 0;
        while (inputLine != null)
        {
            var input = (int)inputLine[0];
            var response = (int)inputLine[2];

            //var score = MatchScorePartOne(input, response);
            var score = MatchScorePartTwo(input, response);

            totalScore += score;
            inputLine = file.ReadLine();
        }
        Console.WriteLine($"Total score: {totalScore}");
    }

    private static int MatchScorePartOne(int input, int response)
    {
        return input switch
        {
            (int)Input.Rock => response switch
            {
                (int)ResponseOne.Rock => (int)Match.Draw + (int)Score.Rock,
                (int)ResponseOne.Paper => (int)Match.Win + (int)Score.Paper,
                (int)ResponseOne.Scissors => (int)Match.Lose + (int)Score.Scissors,
                _ => 0
            },
            (int)Input.Paper => response switch
            {
                (int)ResponseOne.Rock => (int)Match.Lose + (int)Score.Rock,
                (int)ResponseOne.Paper => (int)Match.Draw + (int)Score.Paper,
                (int)ResponseOne.Scissors => (int)Match.Win + (int)Score.Scissors,
                _ => 0
            },
            (int)Input.Scissors => response switch
            {
                (int)ResponseOne.Rock => (int)Match.Win + (int)Score.Rock,
                (int)ResponseOne.Paper => (int)Match.Lose + (int)Score.Paper,
                (int)ResponseOne.Scissors => (int)Match.Draw + (int)Score.Scissors,
                _ => 0
            },
            _ => 0
        };
    }

    private static int MatchScorePartTwo(int input, int response)
    {
        return input switch
        {
            (int)Input.Rock => response switch
            {
                (int)ResponseTwo.Draw => (int)Match.Draw + (int)Score.Rock,
                (int)ResponseTwo.Win => (int)Match.Win + (int)Score.Paper,
                (int)ResponseTwo.Lose => (int)Match.Lose + (int)Score.Scissors,
                _ => 0
            },
            (int)Input.Paper => response switch
            {
                (int)ResponseTwo.Lose => (int)Match.Lose + (int)Score.Rock,
                (int)ResponseTwo.Draw => (int)Match.Draw + (int)Score.Paper,
                (int)ResponseTwo.Win => (int)Match.Win + (int)Score.Scissors,
                _ => 0
            },
            (int)Input.Scissors => response switch
            {
                (int)ResponseTwo.Win => (int)Match.Win + (int)Score.Rock,
                (int)ResponseTwo.Lose => (int)Match.Lose + (int)Score.Paper,
                (int)ResponseTwo.Draw => (int)Match.Draw + (int)Score.Scissors,
                _ => 0
            },
            _ => 0
        };
    }
}