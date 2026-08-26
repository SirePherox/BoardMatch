using System;
using BoardMatch.Core;
using BoardMatch.Utilities;
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
        [SerializeField] private GameConfig gameConfig;
        
        [Header("Variables")] 
        private BoardModel _board;
        
        private void Start()
        {
           SetupAGame();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                int[,] grid = boardView.Board.GetGridForTesting();
                MatchLog.FormatGrid(grid);
            }
        }

        private void SetupAGame()
        {
            IRandomGemProvider unityRandom = new UnityRandomGemProvider(gameConfig.boardSeed != -1 ? gameConfig.boardSeed : null);
            _board = new BoardModel(matchConfig, unityRandom);
            
            boardView.Setup(_board);
            boardInputController.Setup(boardView);
        }
    }
}

