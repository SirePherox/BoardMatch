using System;
using BoardMatch.Utilities;
using UnityEngine;

namespace BoardMatch.Core
{
    public sealed class BoardModel
    {
        [Header("Board Settings")]
        private readonly int _width;
        private readonly int _height;
        private readonly int _minMatchCount;
        private readonly int[] _availableGemTypes;
        private readonly int[,] _grid;
        
        private readonly IRandomGemProvider _randomGemProvider;

        [Header("Variables")] 
        private const int MaxAttemptPerCell = 200; //while populating
        public const int EmptyCell = -1;
        //EVENTS
        public event Action<Vector2Int, Vector2Int> OnGemSwapped; //From -> To

        public BoardModel(MatchConfig config, IRandomGemProvider randomGemProvider)
        {
            #region - Validity Check-
            if (!config || randomGemProvider == null)
            {
                MatchLog.Log($"Config or Random Gem provider can not be null. Ensure a valid value/reference");
            }
            
            if(config == null) throw new ArgumentNullException(nameof(config));
            if(randomGemProvider == null) throw new ArgumentNullException(nameof(randomGemProvider));
            
            if (config.availableGemTypes == null || config.availableGemTypes.Length < config.minMatchCount)
            {
                throw new ArgumentException(
                    $"MatchConfig needs at least {config.minMatchCount} gem types to satisfy minMatchCount, " +
                    $"but only has {(config.availableGemTypes?.Length ?? 0)}.", nameof(config));
            }
            
            foreach (int id in config.availableGemTypes)
            {
                if (id < 0)
                    throw new ArgumentException("Gem type ids must be non-negative (-1 is reserved for an empty cell).", nameof(config));
            }

            #endregion
            
            _width = config.width;
            _height = config.height;
            _grid = new int[_width, _height];
            _availableGemTypes = (int[])config.availableGemTypes.Clone();
            
            PopulateBoardUniquely();
        }

        public bool TrySwapCells(Vector2Int a, Vector2Int b)
        {
            if (!IsInsideBoard(a) || !IsInsideBoard(b) || !AreAdjacent(a, b))
            {
                MatchLog.Log("Ensure cells are inside board (valid pos) and are adjacent");
                return false;
            }
            SwapCells(a, b);
            
            var matches = MatchFinder.FindAllMatches(_grid, _width, _height, _minMatchCount);
            if (matches.Count == 0)
            {
                MatchLog.Log("No valid matches. Reverting back");
                SwapCells(a, b);
                return false;
            }
            
            OnGemSwapped?.Invoke(a, b);
            ResolveCascade(matches);
            return true;
        }
        
        /// <summary>
        /// Populate the board while avoiding complete matches
        /// </summary>
        private void PopulateBoardUniquely()
        {
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    int gemType = _randomGemProvider.GetRandomGem(_availableGemTypes);
                    int attempts = 1;

                    while (WouldCompleteAMatchAt(x, y, gemType) &&
                           attempts < MaxAttemptPerCell)
                    {
                        gemType = _randomGemProvider.GetRandomGem(_availableGemTypes);
                        attempts++;
                    }
                }
            }
        }

        
        private void SwapCells(Vector2Int from, Vector2Int to)
        {
            int tempVal = _grid[from.x, from.y];
            _grid[from.x, from.y] = _grid[to.x, to.y];
            _grid[to.x, to.y] = tempVal;
        }

        /// <summary>
        /// Check if adding this gem type to the position [x,y] would make a complete match.
        /// A complete match is , _minMatchCount of the same gemType
        /// </summary>
        private bool WouldCompleteAMatchAt(int x, int y, int gemType)
        {
            int horizontalRun = 1;
            for (int i = 1; i < _minMatchCount && x - i >= 0; i++)
            {
                if (_grid[x - i, y] == gemType) horizontalRun++;
                else break;
            }

            if (horizontalRun >= _minMatchCount) return true;
            
            int verticalRun = 1;
            for (int i = 1; i < _minMatchCount && y - i >= 0; i++)
            {
                if(_grid[x, y-i]== gemType) verticalRun++;
                else break;
            }
            
            return verticalRun >= _minMatchCount;
        }

        private bool AreAdjacent(Vector2Int a, Vector2Int b)
        {
            int xDiff = Math.Abs(a.x - b.x);
            int yDiff = Math.Abs(a.y - b.y);
            return xDiff + yDiff == 1;
        }
        private bool IsInsideBoard(Vector2Int pos)
        {
            return pos.x >= 0 && pos.x < _width && pos.y >= 0 && pos.y < _height;
        }
        public int GemType (Vector2Int pos) => _grid[pos.x, pos.y];
        public int Width => _width;
        public int Height => _height;
    }

}
