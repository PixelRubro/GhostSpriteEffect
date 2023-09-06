using System;
using System.Collections;
using UnityEngine;

namespace VermillionVanguard.GhostSprite
{
    public class GhostSpriteRenderer : MonoBehaviour
    {
        #region Actions
        #endregion

        #region Serialized fields
        #endregion

        #region Non-serialized fields

        private float _activeTimeLeft;

        private Color _initialColor;

        private SpriteRenderer _spriteRenderer;

        private Transform _transform;

        private float _alphaDecrement;

        private Action _deactivationCallback;

        #endregion

        #region Properties
        #endregion

        #region Unity events

        private void Update()
        {
            DecreaseAlpha();
        }

        private void OnEnable()
        {
            if (_spriteRenderer != null)
            {
                _spriteRenderer.color = _initialColor;
            }
        }

        #endregion

        #region Internal methods

        internal void Setup()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _transform = transform;
        }
        
        internal void SetDeactivationCallback(Action deactivationCallback)
        {
            _deactivationCallback = deactivationCallback;
        }
        
        internal void SetInitialColor(Color color)
        {
            _initialColor = color;
        }
        
        internal void SetLifespan(float lifespan)
        {
            _activeTimeLeft = lifespan;
            SetupAlphaDecrement();
        }
        
        internal void SetPosition(Vector2 position)
        {
            _transform.position = position;
        }
        
        internal void SetScale(Vector3 scale)
        {
            _transform.localScale = scale;
        }
        
        internal void SetSpriteRendererValues(SpriteRenderer spriteRenderer)
        {
            _spriteRenderer.sortingOrder = spriteRenderer.sortingOrder;
            _spriteRenderer.sprite = spriteRenderer.sprite;
            _spriteRenderer.flipX = spriteRenderer.flipX;
        }

        #endregion

        #region Private methods

        private void SetupAlphaDecrement()
        {
            _alphaDecrement = 0.005f;
        }
        
        private void DecreaseAlpha()
        {
            if (_spriteRenderer.color.a > 0f)
            {
                var color = _spriteRenderer.color;
                color.a -= _alphaDecrement;
                _spriteRenderer.color = color;
            }
            else
            {
                _deactivationCallback?.Invoke();
                gameObject.SetActive(false);
            }
        }

        #endregion    
    }
}
