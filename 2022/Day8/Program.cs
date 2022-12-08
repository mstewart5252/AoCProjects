using System.Globalization;
using System.Text;

namespace Day8;

public static class Program
{
    public static void Main()
    {
        var grid = File.ReadAllLines("input.txt");

        //this isn't quite what I need, but dict doesn't work because keys wouldn't be unique
        var rotatedGrid = new Tuple<char, bool>[grid[0].Length];
        
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
                    rotatedGrid[j].Add(tree, true);
                }
                
                //if is visible in line
                if (grid[i][new Range(0, j)].All(t => int.Parse(t.ToString()) < int.Parse(tree.ToString())) ||
                         grid[i][new Range(j + 1, Index.End)].All(t => int.Parse(t.ToString()) < int.Parse(tree.ToString())))
                {
                    visibleTrees++;
                    rotatedGrid[j].Add(tree, true);
                }
                
                //else add to rotated grid and set to false
                else
                    rotatedGrid[j].Add(tree, false);
            }            
        }

        for (var i = 0; i < grid.Length; i++)
        {
            for (var j = 0; j < grid[0].Length; j++)
            {
                var tree = rotatedGrid[i].ElementAt(j);
                if (tree.Value) continue;

                var test = rotatedGrid[i].Keys.ToString();
            }
        }
    }
}