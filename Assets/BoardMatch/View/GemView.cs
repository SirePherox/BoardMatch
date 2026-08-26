using System;
using System.Collections;
using UnityEngine;

namespace BoardMatch.View
{
    public class GemView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        [Header("Variables")]
        public int GemTypeId { get; private set; }
        private Coroutine _activeMove;

        private void Start()
        {
            CacheRenderer();
        }

        private void CacheRenderer()
        {
            //Fallback ONLY, incase of Null Reference in editor
            if(!spriteRenderer) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        public void Setup(int gemType, Color color, Vector3 worldPosition)
        {
            CacheRenderer();
                
            GemTypeId = gemType;
            spriteRenderer.color = color;
            transform.position = worldPosition;
            transform.localScale = Vector3.one;
        }

        public void MoveTo(Vector3 targetWorldPos, float duration)
        {
            if(_activeMove != null) StopCoroutine(_activeMove);

            _activeMove = StartCoroutine(MoveCoroutine(targetWorldPos, duration));
        }

        public void ClearAndDestroy(float duration)
        {
            StartCoroutine(ClearDestroyRoutine(duration));
        }

        private IEnumerator MoveCoroutine(Vector3 targetWorldPos, float duration)
        {
            Vector3 startPos = transform.position;
            float elapsedTime = 0;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                transform.position = Vector3.Lerp(startPos, targetWorldPos, elapsedTime / duration);
                yield return null;
            }
            transform.position = targetWorldPos;
            _activeMove = null;
        }

        private IEnumerator ClearDestroyRoutine(float duration)
        {
            Vector3 startScale = transform.localScale;
            float elapsedTime = 0;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                transform.localScale = Vector3.Lerp(startScale, Vector3.zero, elapsedTime / duration);
                yield return null;
            }
            Destroy(gameObject);
        }
    }

}
