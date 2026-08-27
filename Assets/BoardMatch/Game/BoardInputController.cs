using System;
using BoardMatch.Utilities;
using BoardMatch.View;
using UnityEngine;

namespace BoardMatch.Game
{
    public class BoardInputController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Camera mainCamera;
        [SerializeField] private GameObject selectionRing;
        
        [Header("Variables")]
        private IBoardVisualizer _boardVisualizer;
        private Vector2Int _selectedCell;
        private bool _hasSelection;
        private Vector3 _clickWorldPos;
        
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

            if (Input.GetMouseButtonDown(0))
            {
                HandleClick(Input.mousePosition); //Todo move to New input system
            }
        }

        private void HandleClick(Vector3 mousePosition)
        {
            if(!mainCamera) return;
            
            _clickWorldPos = mainCamera.ScreenToWorldPoint(mousePosition);
            _clickWorldPos.z = 0;

            if (!_boardVisualizer.TryGetGridPosition(_clickWorldPos,
                    out Vector2Int clickedCell))
            {
                return;
            }
                

            if (!_hasSelection)
            {
                _selectedCell = clickedCell;
                _hasSelection = true;
                
                SelectItem();
                return;
            }

            if (_selectedCell == clickedCell)
            {
                _hasSelection = false; //deselect
                _selectedCell = new Vector2Int(-1, -1);
                Deselect();
                return;
            }

            if (_boardVisualizer.Board.AreAdjacent(_selectedCell, clickedCell))
            {
                _boardVisualizer.Board.TrySwapCells(_selectedCell, clickedCell);
                _hasSelection = false;
                Deselect();
            }
            else
            {
                _selectedCell = clickedCell; //new selection
                SelectItem();
            }
        }

        private void SelectItem()
        {
            if (!selectionRing || _boardVisualizer == null) return;
            
            selectionRing.SetActive(true);
            selectionRing.transform.position = _boardVisualizer.GetWorldPosition(_selectedCell);
        }

        private void Deselect()
        {
            if (!selectionRing || _boardVisualizer == null) return;
            
            selectionRing.SetActive(false);
        }
    }

}
