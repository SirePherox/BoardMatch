using System;
using BoardMatch.Utilities;
using UnityEngine;

namespace BoardMatch.Core
{
    public class BoardModel : MonoBehaviour
    {
        [Header("Board Settings")]
        private readonly int _width;
        private readonly int _height;
        private readonly int _minMatchCount;
        private readonly int[] _availableGemTypes;
        private readonly int[,] _grid;
        
        private readonly IRandomGemProvider _randomGemProvider;

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
        }
        
        public int Width => _width;
        public int Height => _height;
    }

}
