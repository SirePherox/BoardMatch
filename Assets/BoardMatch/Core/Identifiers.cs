using System;
using System.Collections.Generic;
using UnityEngine;

namespace BoardMatch.Core
{
    public class Identifiers : MonoBehaviour
    {
        
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
    
    /// <summary>An existing gem shifting from one cell to another during gravity resolution.</summary>
    public readonly struct GemFall
    {
        public readonly Vector2Int From;
        public readonly Vector2Int To;
        public readonly int GemType;

        public GemFall(Vector2Int from, Vector2Int to, int gemType)
        {
            From = from;
            To = to;
            GemType = gemType;
        }
    }
    
    /// <summary>A brand-new gem created to refill an empty cell after a match was cleared.</summary>
    public readonly struct GemSpawn
    {
        public readonly Vector2Int Position;
        public readonly int GemType;

        public GemSpawn(Vector2Int position, int gemType)
        {
            Position = position;
            GemType = gemType;
        }
    }
}

