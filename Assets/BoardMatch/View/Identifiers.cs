using System;
using BoardMatch.Core;
using UnityEngine;

namespace BoardMatch.View
{
    public class Identifiers 
    {
        
    }
    
    [Serializable]
    public struct GemVisualEntry
    {
        public int gemTypeId;
        public Color color;
    }

    /// <summary>
    /// The minimum surface the rest of the game needs from a board
    /// renderer: converting between grid cells and world space, and which
    /// Board it's currently displaying.
    /// </summary>
    public interface IBoardVisualizer
    {
        BoardModel Board { get; }
        Vector3 GetWorldPosition(Vector2Int gridPos);
        bool TryGetGridPosition(Vector3 worldPos, out Vector2Int gridPos);
    }
}

