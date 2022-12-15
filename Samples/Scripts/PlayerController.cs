using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoftBoiledGames.GhostSpriteEffect.Demo
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private float _movingSpeed = 6f;

        [SerializeField]
        private float _jumpStrength = 8f;

        [SerializeField]
        private bool _isFacingRight = true;

        [SerializeField]
        private float _horizontalInput;

        [SerializeField]
        private bool _isGrounded;

        private Transform _transform;

        private Rigidbody2D _rb2d;

        private SpriteRenderer _spriteRenderer;

        private Animator _animator;

        private GhostSpriteEffect _ghostSpriteEffect;

        private float _previousHorizontalVelocity;

        private void Awake()
        {
            _rb2d = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
            _ghostSpriteEffect = GetComponent<GhostSpriteEffect>();
            _transform = transform;
        }

        private void Update()
        {
            Move();
            Jump();
        }

        private void FixedUpdate()
        {
            _isGrounded = Mathf.Approximately(_rb2d.velocity.y, 0f);
        }

        private void LateUpdate()
        {
            PlayAnimation();
            PlayEffects();
        }

        private void Move()
        {
            _horizontalInput = Input.GetAxisRaw("Horizontal");
            _rb2d.velocity = new Vector2(_horizontalInput * _movingSpeed, _rb2d.velocity.y);
            CheckDirection();
        }

        private void CheckDirection()
        {
            if (_horizontalInput == 0f)
            {
                return;
            }

            if (_horizontalInput > 0f)
            {
                if (!_isFacingRight)
                {
                    FlipDirection();
                }
            }
            else
            {
                if (_isFacingRight)
                {
                    FlipDirection();
                }
            }
        }

        private void FlipDirection()
        {
            _isFacingRight = !_isFacingRight;
            _transform.localScale = new Vector3(_transform.localScale.x * -1f, _transform.localScale.y, _transform.localScale.z);
        }

        private void Jump()
        {
            if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
            {
                _rb2d.AddForce(Vector2.up * _jumpStrength, ForceMode2D.Impulse);
            }
        }

        private void PlayAnimation()
        {
            if (_isGrounded)
            {
                if (_horizontalInput == 0f)
                {
                    _animator.Play("idle");
                }
                else
                {
                    _animator.Play("walk");
                }

            }
            else
            {
                _animator.Play("jump");
            }
        }

        private void PlayEffects()
        {
            if (_isGrounded)
            {
                if (_ghostSpriteEffect.IsPlaying)
                {
                    _ghostSpriteEffect.Stop();
                }
            }
            else
            {
                if (!_ghostSpriteEffect.IsPlaying)
                {
                    _ghostSpriteEffect.Play();
                }
            }
        }
    }    
}
