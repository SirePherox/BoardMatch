using UnityEngine;

namespace BoardMatch.Game
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "BoardMatch/GameConfig")]
    public class GameConfig : ScriptableObject
    {
       [Range(0.1f, 0.4f)] [Tooltip("Time it takes for a gem change position")]
       public float moveDuration = 0.15f;
       
       [Range(0.08f, 0.25f)] [Tooltip("Time it takes for a gem to destroy after a match")]
       public float clearDuration = 0.1f;

       [Tooltip("Should new gems appear at the top of the board?")]
       public bool spawnAboveBoard = true;
    }

}
