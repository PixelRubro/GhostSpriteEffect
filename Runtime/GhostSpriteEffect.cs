using UnityEngine;

namespace SoftBoiledGames.GhostSpriteEffect
{
    [RequireComponent(typeof(ObjectPooler))]
    public class GhostSpriteEffect : MonoBehaviour
    {
        #region Actions
        #endregion

        #region Serialized fields

        [Tooltip("Ghost sprite spawn color.")]
        [SerializeField] 
        private Color _spriteInitialColor = new Color(1f, 1f, 1f, 0.2f);

        [Tooltip("Interval between each sprite copy spawn.")]
        [SerializeField] 
        private float _spriteSpawnInterval = 0.025f;

        [Tooltip("Spawned ghost lifespan from spawning to its alpha color value becoming zero.")]
#if UNITY_EDITOR
        [InspectorAttributes.DisableInPlayMode]
#endif
        [SerializeField] 
        private float _spriteLifespan = 0.2f;

        [Tooltip("Limits the number of sprite copies spawned.")]
        [SerializeField] 
#if UNITY_EDITOR
        [InspectorAttributes.DisableInPlayMode]
#endif
        private bool _limitSpriteSpawning;

        [Tooltip("Maximum number of sprites to be spawned.")]
        [SerializeField] 
#if UNITY_EDITOR
        [InspectorAttributes.DisableInPlayMode]
        private int _initialSpriteCopies = 10;
#endif

        #endregion

        #region Non-serialized fields

        private SpriteRenderer _spriteRenderer;

        private ObjectPooler _objectPooler;

        private bool _isSetupFinished;

        private Transform _transform;

        private float _currentSpawnTimeLeft;

        private bool _isPlaying;

        #endregion

        #region Properties

        public bool IsPlaying => _isPlaying;

        #endregion

        #region Unity events

        private void Awake()
        {
            RegisterComponents();
        }

        private void Start()
        {
            SetupPool();
        }

        private void LateUpdate()
        {
            CheckSpawnInterval();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Starts the spawning of sprite copies at the provided interval.
        /// </summary>
        public void Play()
        {
            if (_isSetupFinished == false)
            {
                return;
            }

            _isPlaying = true;
            _currentSpawnTimeLeft = _spriteSpawnInterval;
        }

        /// <summary>
        /// Stops the sprite spawning immediately.
        /// </summary>
        public void Stop()
        {
            if (_isSetupFinished == false)
            {
                return;
            }
            
            _isPlaying = false;
        }
        
        /// <summary>
        /// Spawns a single sprite copy.
        /// </summary>
        public void Spawn()
        {
            if (_isSetupFinished == false)
            {
                return;
            }
            
            SpawnSprite();
        }
        
        /// <summary>
        /// Spawns a single sprite copy at the provided position.
        /// </summary>
        /// <param name="position">Position to spawn the sprite copy.</param>
        public void Spawn(Vector2 position)
        {
            if (_isSetupFinished == false)
            {
                return;
            }
            
            SpawnSprite(position);
        }

        #endregion

        #region Private methods

        private void RegisterComponents()
        {
            _objectPooler = GetComponent<ObjectPooler>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _transform = transform;
        }

        private void SetupPool()
        {
            _objectPooler.InitializePool(_initialSpriteCopies, _limitSpriteSpawning, CreateGhostSpritePrefab());
            _isSetupFinished = true;
        }

        private void CheckSpawnInterval()
        {
            if (_isPlaying == false)
            {
                return;
            }

            if (_currentSpawnTimeLeft > 0f)
            {
                _currentSpawnTimeLeft -= Time.deltaTime;
                return;
            }

            _currentSpawnTimeLeft = _spriteSpawnInterval;
            SpawnSprite();
        }

        private void SpawnSprite()
        {
            var ghostSprite = GetConfiguredPooledSprite();

            if (ghostSprite != null)
            {
                ghostSprite.SetPosition(_transform.position);
            }
        }

        private void SpawnSprite(Vector2 position)
        {
            var ghostSprite = GetConfiguredPooledSprite();

            if (ghostSprite != null)
            {
                ghostSprite.SetPosition(position);
            }
        }

        private GhostSpriteRenderer GetConfiguredPooledSprite()
        {
            var ghostSprite = _objectPooler.Pop();

            if (ghostSprite == null)
            {
                return null;
            }

            ghostSprite.SetInitialColor(_spriteInitialColor);
            ghostSprite.SetLifespan(_spriteLifespan);
            ghostSprite.SetSpriteRendererValues(_spriteRenderer);
            ghostSprite.SetScale(_transform.localScale);

            if (gameObject.activeInHierarchy == false)
            {
                gameObject.SetActive(true);
            }

            return ghostSprite;
        }

        private GhostSpriteRenderer CreateGhostSpritePrefab()
        {
            var prefabObject = Instantiate(new GameObject(), null, false);
            prefabObject.name = "GhostEffectSpritePrefab";
            prefabObject.AddComponent<GhostSpriteRenderer>();
            prefabObject.AddComponent<SpriteRenderer>();
            prefabObject.SetActive(false);
            return prefabObject.GetComponent<GhostSpriteRenderer>();
        }

        #endregion    
    }
}
