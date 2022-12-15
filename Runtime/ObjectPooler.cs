using System.Collections.Generic;
using UnityEngine;

namespace SoftBoiledGames.GhostSpriteEffect
{
    internal class ObjectPooler : MonoBehaviour
    {
        #region Serialized fields

        [SerializeField]
#if UNITY_EDITOR
        [InspectorAttributes.ReadOnly]
#endif
        private int _currentSize;

        #endregion

        #region Non-serialized fields

        private bool _isExpansible;

        private int _size = 12;

        private GhostSpriteRenderer _poolableObjectPrefab;

        private Stack<GhostSpriteRenderer> _pool;

        private Transform _spriteCopiesParent;

        #endregion

        #region Constant fields

        private const int ExpansionSize = 2;

        #endregion

        #region Unity events
        #endregion

        #region Public methods

        internal void InitializePool(int capacity, bool isExpansible, GhostSpriteRenderer ghostSpritePrefab)
        {
            CreatePool(isExpansible);
            AssignPrefab(ghostSpritePrefab);
            CreateSpriteCopiesParent();
            FillPool(capacity);
        }

        /// <summary>
        /// Retrieves an object from the pool.
        /// </summary>
        internal GhostSpriteRenderer Pop()
        {
            var ghostSprite = GetInactivePooledObject();

            if (ghostSprite != null)
            {
                ResetObject(ghostSprite);
                return ghostSprite;
            }

            if (_isExpansible)
            {
                ExpandPool();
                return Pop();
            }

            return null;
        }

        #endregion

        #region Private methods

        private void CreatePool(bool isExpansible)
        {
            _pool = new Stack<GhostSpriteRenderer>();
            _isExpansible = isExpansible;
        }

        private void AssignPrefab(GhostSpriteRenderer prefabObject)
        {
            _poolableObjectPrefab = prefabObject;
        }

        private void CreateSpriteCopiesParent()
        {
            _spriteCopiesParent = Instantiate(new GameObject(), null, false).transform;
            _spriteCopiesParent.transform.position = Vector3.zero;
        }

        private void FillPool(int objectCount)
        {
            for (int i = 0; i < objectCount; i++)
            {
                var poolableObject = CreateObject();
                _pool.Push(poolableObject);
            }
        }

        private GhostSpriteRenderer CreateObject()
        {
            var ghostSprite = Instantiate<GhostSpriteRenderer>(_poolableObjectPrefab, _spriteCopiesParent, false);
            ghostSprite.Setup();
            ghostSprite.gameObject.SetActive(false);
            ghostSprite.SetDeactivationCallback(() => Return(ghostSprite));
            return ghostSprite;
        }

        private void ExpandPool()
        {
            _size += ExpansionSize;
            FillPool(ExpansionSize);
        }

        /// <summary>
        /// Return to the pool an object that belongs to it.
        /// </summary>
        /// <param name="poolableObject"></param>
        private void Return(GhostSpriteRenderer poolableObject)
        {
            if (poolableObject.gameObject.activeInHierarchy == true)
            {
                poolableObject.gameObject.SetActive(false);
            }

            if (poolableObject.gameObject.activeInHierarchy == false)
            {
                _pool.Push(poolableObject);
            }
        }

        private GhostSpriteRenderer GetInactivePooledObject()
        {
            if (_pool.Count <= 0)
            {
                return null;
            }

            return _pool.Pop();
        }

        private void ResetObject(GhostSpriteRenderer ghostSprite)
        {
            ghostSprite.transform.position = Vector3.zero;
            ghostSprite.transform.rotation = Quaternion.identity;
        }

        #endregion
    }
}
