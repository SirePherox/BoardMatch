using BoardMatch.Core;
using BoardMatch.View;
using UnityEngine;

namespace BoardMatch.Game
{
    /// <summary> Scene entry point and composition root </summary>
    public class GameOrchestrator : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private MatchConfig matchConfig;
        [SerializeField] private BoardView boardView;
        [SerializeField] private BoardInputController boardInputController;

        [Header("Variables")] 
        private BoardModel _board;
        
        private void Start()
        {
           SetupAGame();
        }

        
        private void SetupAGame()
        {
            IRandomGemProvider unityRandom = new UnityRandomGemProvider();
            _board = new BoardModel(matchConfig, unityRandom);
            
            boardView.Setup(_board);
            boardInputController.Setup(boardView);
        }
    }
}

