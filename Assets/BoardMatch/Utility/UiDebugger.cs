using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BoardMatch.Utility
{
    public class UiDebugger : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private Button quitButton;
        [SerializeField] private Text fpsText;
        
        [Header("Variables")]
        private int _frameCount;
        private float _elapsedTime;
        
        private const float DisplayInterval = 1f;

        private void Start()
        {
           SubscribeToEvents();
        }

        void Update()
        {
            ShowFrameRate();
        }

        private void SubscribeToEvents()
        {
            if(quitButton)   quitButton.onClick.AddListener(QuitGame);
        }
        private void ShowFrameRate()
        {
            _frameCount++;
            _elapsedTime += Time.unscaledDeltaTime;

            if (_elapsedTime < DisplayInterval) return;
            
            fpsText.text = $"{(int)(_frameCount/_elapsedTime)} fps";
            _elapsedTime = 0f;
            _frameCount = 0;
        }

        private void QuitGame()
        {
            Application.Quit();
        }

        private void OnDestroy()
        {
            if(quitButton)   quitButton.onClick.RemoveListener(QuitGame);
        }
    }

}
