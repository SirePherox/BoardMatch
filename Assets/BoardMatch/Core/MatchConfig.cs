using UnityEngine;

namespace BoardMatch.Core
{
    [CreateAssetMenu(fileName = "MatchConfig", menuName = "BoardMatch/MatchConfig")]
    public class MatchConfig : ScriptableObject
    {
        [Header("Board Dimensions")] 
        [Range(3, 8)] public int width = 4;
        [Range(3, 8)] public int height = 4;

        [Header("Match Rules")] 
        [Range(3, 5)] public int minMatchCount = 3;

        [Header("Gems Types")] 
        [Tooltip("Assign UNIQUE id based for each gem type")]
        public int[] availableGemTypes = new int[] { 0, 1, 2, 3, 4 };
    }

}
