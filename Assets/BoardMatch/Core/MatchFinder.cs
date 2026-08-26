using System.Collections.Generic;
using UnityEngine;

namespace BoardMatch.Core
{
    public class MatchFinder : MonoBehaviour
    {
        public static List<Match> FindAllMatches(int[,] grid, int width, int height, int minMatchCount)
        {
            var matches = new List<Match>();
            FindHorizontalMatches(grid, width, height, minMatchCount, matches);
            FindVerticalMatches(grid, width, height, minMatchCount, matches);
            return matches;
        }

        private static void FindHorizontalMatches(int[,] grid, int width, int height,
            int minMatchCount, List<Match> matches)
        {
            for (int y = 0; y < height; y++)
            {
                int runStart = 0;
                for (int x = 1; x <= width; x++)
                {
                    bool continueRun = x < width && grid[x, y] == grid[x - 1, y] && grid[x, y] != BoardModel.EmptyCell;
                    if(continueRun) continue;

                    int runLength = x - runStart;
                    if (runLength >= minMatchCount &&
                        grid[runStart, y] != BoardModel.EmptyCell)
                    {
                        var cells = new List<Vector2Int>(runLength);
                        for (int cx = runStart; cx < x; cx++)
                        {
                            cells.Add(new Vector2Int(cx, y));
                        }
                        matches.Add(new Match(cells, grid[runStart, y], MatchOrientation.Horizontal));
                    }

                    runStart = x;
                }
            }
        }

        private static void FindVerticalMatches(int[,] grid, int width, int height,
            int minMatchCount, List<Match> matches)
        {
            for (int x = 0; x < width; x++)
            {
                int runStart = 0;
                for (int y = 1; y <= height; y++)
                {
                    bool continueRun = y < height && grid[x, y] == grid[x, y-1] && grid[x, y] != BoardModel.EmptyCell;
                    if (continueRun) continue;
                    
                    int runLength = y - runStart;
                    if (runLength >= minMatchCount &&
                        grid[x, runStart] != BoardModel.EmptyCell)
                    {
                        var cells = new List<Vector2Int>(runLength);
                        for (int cy = runStart; cy < y; cy++)
                        {
                            cells.Add(new Vector2Int(x, cy));
                        }
                        matches.Add(new Match(cells, grid[x, runStart], MatchOrientation.Vertical));
                    }
                    runStart = y;
                }
            }
        }
    }

}
