using System;
using System.Collections.Generic;
using BoardMatch.Core;
using BoardMatch.Game;
using BoardMatch.Utilities;
using UnityEngine;

namespace BoardMatch.View
{
    public class BoardView : MonoBehaviour, IBoardVisualizer
    {
        [Header("Variables")] 
        [SerializeField] private GemView gemPrefab;
        [SerializeField] private GemVisualConfig visualConfig;
        [SerializeField] private GameConfig gameConfig;
        [SerializeField] private Transform boardOrigin;
        [SerializeField] private float cellSize = 1f;
        [SerializeField] private float moveDuration = 0.15f;
        [SerializeField] private float clearDuration = 0.1f;
        [Space]
        private Dictionary<Vector2Int, GemView> _gemsViews = new();
        public BoardModel Board { get; private set; }

        public void Setup(BoardModel board)
        {
            if (Board != null) UnsubscribeFromBoard();
            
            DefaultValues();
            Board = board;
            BuildInitialGrid();
            SubscribeToBoard();

        }

        private void DefaultValues()
        {
            if (gameConfig)
            {
                moveDuration = gameConfig.moveDuration;
                clearDuration = gameConfig.clearDuration;
                cellSize = gameConfig.cellSize;
            }
           
        }

        private void BuildInitialGrid()
        {
            for (int x = 0; x < Board.Width; x++)
            {
                for (int y = 0; y < Board.Height; y++)
                {
                    var gridPos = new Vector2Int(x, y);
                    SpawnGemView(gridPos, Board.GemType(gridPos));
                }
            }
        }

        private void SubscribeToBoard()
        {
            Board.OnGemSwapped += HandleGemsSwapped;
            Board.OnMatchesCleared += HandleMatchesCleared;
            Board.OnGemsFell += HandleGemsFell;
            Board.OnGemsSpawned += HandleGemsSpawned;
        }
        
        private void UnsubscribeFromBoard()
        {
            Board.OnGemSwapped -= HandleGemsSwapped;
            Board.OnMatchesCleared -= HandleMatchesCleared;
            Board.OnGemsFell -= HandleGemsFell;
            Board.OnGemsSpawned -= HandleGemsSpawned;
        }

        private void HandleGemsSwapped(Vector2Int a, Vector2Int b)
        {
            if (!_gemsViews.TryGetValue(a, out var gemViewA) ||
                !_gemsViews.TryGetValue(b, out var gemViewB))
            {
                MatchLog.Log("Odd behaviour! There is no gem at this location");
                return; 
            }

            _gemsViews[a] = gemViewB;
            _gemsViews[b] = gemViewA;
            
            gemViewA.MoveTo(GetWorldPosition(b), moveDuration);
            gemViewB.MoveTo(GetWorldPosition(a), moveDuration);
        }

        private void HandleMatchesCleared(IReadOnlyList<Match> matches)
        {
            if(matches == null || matches.Count <= 0) return;
            
            var uniqueCells = new HashSet<Vector2Int>();
            foreach (var match in matches)
            {
                foreach (var cell in match.Cells)
                {
                    uniqueCells.Add(cell);
                }
            }

            foreach (var cell in uniqueCells)
            {
                if (_gemsViews.TryGetValue(cell, out var gemView))
                {
                    gemView.ClearAndDestroy(clearDuration);
                }
            }
        }

        private void HandleGemsFell(IReadOnlyList<GemFall> gemFells)
        {
            foreach (var gemFall in gemFells)
            {
                if (!_gemsViews.TryGetValue(gemFall.From, out var gemView)) continue; //could be because a gem fell "through"

                _gemsViews.Remove(gemFall.From);
                _gemsViews[gemFall.To] = gemView;
                gemView.MoveTo(GetWorldPosition(gemFall.To), moveDuration);
            }
        }

        private void HandleGemsSpawned(IReadOnlyList<GemSpawn> spawns)
        {
            foreach (var spawn in spawns)
            {
                SpawnGemView(spawn.Position, spawn.GemType);
            }
        }

        private void SpawnGemView(Vector2Int gridPosition, int gemTypeId)
        {
            GemView clone = Instantiate(gemPrefab, transform); //TOdo can use pool
            Vector3 targetWorldPos = GetWorldPosition(gridPosition);
            Vector3 spawnWorldPos = gameConfig.spawnAboveBoard ? targetWorldPos + new Vector3(0f, (Board.Height - gridPosition.y) * cellSize, 0f)
                : targetWorldPos;
            
            clone.Setup(gemTypeId, visualConfig.GetColor(gemTypeId), spawnWorldPos);
            _gemsViews[gridPosition] = clone;

            if (gameConfig.spawnAboveBoard)
            {
                clone.MoveTo(targetWorldPos, moveDuration);
            }
            
        }
        public Vector3 GetWorldPosition(Vector2Int gridPos)
        {
            Vector3 origin = boardOrigin ? boardOrigin.position : Vector3.zero;
            return origin + new Vector3(gridPos. x * cellSize, gridPos.y * cellSize, 0f);
        }

        public bool TryGetGridPosition(Vector3 worldPos, out Vector2Int gridPos)
        {
            Vector3 origin = boardOrigin ? boardOrigin.position : Vector3.zero;
            Vector3 local = worldPos - origin;
            
            gridPos = new Vector2Int(Mathf.RoundToInt(local.x / cellSize), Mathf.RoundToInt(local.z / cellSize));
            
            return Board != null && Board.IsInsideBoard(gridPos);
        }

        private void OnDestroy()
        {
            if(Board != null) UnsubscribeFromBoard();
        }
    }
}