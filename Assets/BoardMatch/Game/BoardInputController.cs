using System;
using BoardMatch.View;
using UnityEngine;

namespace BoardMatch.Game
{
    public class BoardInputController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Camera mainCamera;
        
        [Header("Variables")]
        private IBoardVisualizer _boardVisualizer;
        private Vector2Int _selectedCell;
        private bool _hasSelection;

        private void Update()
        {
            HandleClicksInUpdate();
        }

        public void Setup(IBoardVisualizer boardVisualizer)
        {
            CacheComponents();
            _boardVisualizer = boardVisualizer;
            _hasSelection = false;
        }
        private void CacheComponents()
        {
            //Fallback ONLY
            if(!mainCamera) mainCamera = Camera.main;
        }

        private void HandleClicksInUpdate()
        {
            if(_boardVisualizer == null || _boardVisualizer.Board == null) return;

            if (Input.GetMouseButton(0))
            {
                HandleClick(Input.mousePosition); //Todo move to New input system
            }
        }

        private void HandleClick(Vector3 mousePosition)
        {
            if(!mainCamera) return;
            
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePosition);
            worldPos.z = 0;

            if (!_boardVisualizer.TryGetGridPosition(worldPos, out Vector2Int clickedCell))
                return;

            if (!_hasSelection)
            {
                _selectedCell = clickedCell;
                _hasSelection = true;
                return;
            }

            if (_selectedCell == clickedCell)
            {
                _hasSelection = false; //deselect
                return;
            }

            if (_boardVisualizer.Board.AreAdjacent(_selectedCell, clickedCell))
            {
                _boardVisualizer.Board.TrySwapCells(_selectedCell, clickedCell);
                _hasSelection = false;
            }
            else
            {
                _selectedCell = clickedCell; //new selection
            }
        }
    }

}
