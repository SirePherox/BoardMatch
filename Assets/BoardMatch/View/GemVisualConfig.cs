using System;
using UnityEngine;

namespace BoardMatch.View
{
    [CreateAssetMenu(fileName = "VisualConfig", menuName = "BoardMatch/ GemVisualConfig")]
    public class GemVisualConfig : ScriptableObject
    {
        [Tooltip("Maps each gem type id (from MatchConfig.availableGemTypes) to a color for rendering.")]
        public GemVisualEntry[] entries = Array.Empty<GemVisualEntry>();

        public Color GetColor(int gemTypeId)
        {
            foreach (var entry in entries)
            {
                if (entry.gemTypeId == gemTypeId)
                    return entry.color;
            }

            Debug.LogWarning($"GemVisualConfig has no entry for gem type {gemTypeId}; defaulting to magenta.");
            return Color.magenta;
        }
    }
}

