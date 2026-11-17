
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float _speed = 5f;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private Vector2 _groundCheckSize = new Vector2(0.6f, 0.1f);
    [SerializeField] private LayerMask _groundLayer;

    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip landingSound;
    [SerializeField] private ParticleSystem landingDust;
    private AudioSource audioSource;

    [SerializeField] private float _coyoteTime = 0.033f;

    private int _jumpsRemaining = 2;
    private bool _wasGrounded;
    private bool _performJump;
    private bool _isGrounded;

    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private Collider2D _playerCollider;
    private Collider2D _ignoredPlatform;

    private float _xInput;
    private float _coyoteTimeCounter;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _playerCollider = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        _xInput = Input.GetAxis("Horizontal");

        if (_spriteRenderer != null)
        {
            if (_xInput > 0)
            {
                _spriteRenderer.flipX = false;
            }
            else if (_xInput < 0)
            {
                _spriteRenderer.flipX = true;
            }
        }

        if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) && _isGrounded)
        {
            TryDropThroughPlatform();
        }

        if (Input.GetButtonDown("Jump") && (_jumpsRemaining > 0 || _coyoteTimeCounter > 0f))
        {
            _performJump = true;
            _jumpsRemaining--;
            _coyoteTimeCounter = 0f;

            _animator.SetBool("IsGrounded", false);
            _animator.SetTrigger("Jump");
        }

        _isGrounded = CheckGrounded();

        if (_isGrounded)
        {
            _coyoteTimeCounter = _coyoteTime;
        }
        else
        {
            _coyoteTimeCounter -= Time.deltaTime;
        }

        _animator.SetBool("IsFalling", !_isGrounded && _rb.linearVelocityY < 0);

        if (_isGrounded && !_wasGrounded)
        {
            _jumpsRemaining = 2;

            if (audioSource != null && landingSound != null)
            {
                audioSource.PlayOneShot(landingSound);
            }

            if (landingDust != null)
            {
                landingDust.Play();
            }

            _animator.SetBool("IsGrounded", true);
            _animator.SetBool("IsFalling", false);
            _animator.SetTrigger("Land");
        }

        _wasGrounded = _isGrounded;
    }

    private bool CheckGrounded()
    {
        Collider2D[] groundColliders = Physics2D.OverlapBoxAll(
            _groundCheck.position,
            _groundCheckSize,
            0f,
            _groundLayer
        );

        foreach (Collider2D groundCollider in groundColliders)
        {
            if (groundCollider != _ignoredPlatform)
            {
                return true;
            }
        }

        return false;
    }

    private void TryDropThroughPlatform()
    {
        Collider2D[] groundColliders = Physics2D.OverlapBoxAll(
            _groundCheck.position,
            _groundCheckSize,
            0f,
            _groundLayer
        );

        foreach (Collider2D groundCollider in groundColliders)
        {
            if (groundCollider.GetComponent<PlatformEffector2D>() != null)
            {
                StartCoroutine(DropThroughPlatform(groundCollider));
                return;
            }
        }
    }

    private IEnumerator DropThroughPlatform(Collider2D platformCollider)
    {
        if (_playerCollider == null)
            yield break;

        _ignoredPlatform = platformCollider;
        _coyoteTimeCounter = 0f;

        Physics2D.IgnoreCollision(_playerCollider, platformCollider, true);

        float timeout = Time.time + 1.5f;

        while (Time.time < timeout &&
               _playerCollider.bounds.max.y >= platformCollider.bounds.min.y)
        {
            yield return new WaitForFixedUpdate();
        }

        Physics2D.IgnoreCollision(_playerCollider, platformCollider, false);
        _ignoredPlatform = null;
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = new Vector2(
            _xInput * _speed,
            _rb.linearVelocityY
        );

        if (_performJump)
        {
            _performJump = false;

            if (_jumpsRemaining == 0)
            {
                _rb.linearVelocity = Vector2.zero;
            }

            _rb.AddForce(
                new Vector2(0, _jumpForce),
                ForceMode2D.Impulse
            );

            if (audioSource != null && jumpSound != null)
            {
                audioSource.PlayOneShot(jumpSound);
            }
        }

        _animator.SetFloat("XInputAbs", Math.Abs(_xInput));
    }
}
