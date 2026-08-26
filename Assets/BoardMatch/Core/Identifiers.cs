using System;
using System.Collections.Generic;
using UnityEngine;

namespace BoardMatch.Core
{
    public class Identifiers : MonoBehaviour
    {
        
    }

    [Serializable]
    public class GemSpawn
    {
        public Vector2Int pos;
        public int gemType;
    }
    
    public sealed class Match
    {
        public IReadOnlyList<Vector2Int> Cells { get; }
        public int GemType { get; }
        public MatchOrientation Orientation { get; }

        public Match(IReadOnlyList<Vector2Int> cells, int gemType, MatchOrientation orientation)
        {
            Cells = cells;
            GemType = gemType;
            Orientation = orientation;
        }
    }

    public enum MatchOrientation
    {
        Horizontal, 
        Vertical
    }
}

