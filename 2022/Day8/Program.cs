using System.Globalization;
using System.Text;

namespace Day8;

public static class Program
{
    public static void Main()
    {
        var grid = File.ReadAllLines("input.txt");

        var rotatedGrid = new string[grid[0].Length];
        
        var visibleTrees = VisibleTrees(grid, ref rotatedGrid);
        var highestScenicScore = ScenicScores(grid);

        Console.WriteLine($"Visible trees: {visibleTrees}, Highest scenic score: {highestScenicScore}");
    }

    private static object ScenicScores(string[] grid)
    {
        var highestScenicScore = 0;
        
        for (var i = 0; i < grid.Length; i++)
        {
            for (var j = 0; j < grid[0].Length; j++)
            {
                var tree = grid[i][j];
                // {Up, Down, Left, Right}
                var directionalScores = new []{0, 0, 0, 0};

                //if on the edge, at least one score will be 0, making total score 0 by multiplication
                if (i == 0 ||
                    j == 0 ||
                    i == grid.Length - 1 ||
                    j == grid.Length - 1)
                    continue;
                
                //Get Up Score
                for (var k = i - 1; k >= 0; k--)
                {
                    directionalScores[0]++;
                    if (int.Parse(grid[k][j].ToString()) >= int.Parse(tree.ToString()))
                        break;
                }
                
                //Get Down Score
                for (var k = i + 1; k < grid[i].Length; k++)
                {
                    directionalScores[1]++;
                    if (int.Parse(grid[k][j].ToString()) >= int.Parse(tree.ToString()))
                        break;
                }
                
                //Get Left Score
                for (var k = j - 1; k >= 0; k--)
                {
                    directionalScores[2]++;
                    if (int.Parse(grid[i][k].ToString()) >= int.Parse(tree.ToString()))
                        break;
                }
                
                //Get Right Score
                for (var k = j + 1; k < grid[i].Length; k++)
                {
                    directionalScores[3]++;
                    if (int.Parse(grid[i][k].ToString()) >= int.Parse(tree.ToString()))
                        break;
                }

                var totalScore = directionalScores[0] * directionalScores[1] * directionalScores[2] *
                                 directionalScores[3];

                if (highestScenicScore < totalScore)
                    highestScenicScore = totalScore;
            }
        }

        return highestScenicScore;
    }

    private static int VisibleTrees(string[] grid, ref string[] rotatedGrid)
    {
        var processedGrid = new bool[grid[0].Length, grid.Length];

        var visibleTrees = 0;
        for (var i = 0; i < grid.Length; i++)
        {
            for (var j = 0; j < grid[0].Length; j++)
            {
                var tree = grid[i][j];

                //if is on the edge
                if (i == 0 ||
                    j == 0 ||
                    i == grid.Length - 1 ||
                    j == grid.Length - 1)
                {
                    visibleTrees++;
                    rotatedGrid[j] += tree;
                    processedGrid[i, j] = true;
                }

                //if is visible in line
                else if (grid[i][new Range(0, j)].All(t => int.Parse(t.ToString()) < int.Parse(tree.ToString())) ||
                         grid[i][new Range(j + 1, Index.End)]
                             .All(t => int.Parse(t.ToString()) < int.Parse(tree.ToString())))
                {
                    visibleTrees++;
                    rotatedGrid[j] += tree;
                    processedGrid[i, j] = true;
                }

                //else add to rotated grid and set to false
                else
                {
                    rotatedGrid[j] += tree;
                    processedGrid[i, j] = false;
                }
            }
        }

        for (var i = 0; i < grid.Length; i++)
        {
            for (var j = 0; j < grid[0].Length; j++)
            {
                var tree = rotatedGrid[i][j];
                if (processedGrid[j, i]) continue;

                if (rotatedGrid[i][new Range(0, j)].All(t => int.Parse(t.ToString()) < int.Parse(tree.ToString())) ||
                    rotatedGrid[i][new Range(j + 1, Index.End)]
                        .All(t => int.Parse(t.ToString()) < int.Parse(tree.ToString())))
                {
                    visibleTrees++;
                }
            }
        }

        return visibleTrees;
    }
}