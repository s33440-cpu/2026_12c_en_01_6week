
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private Vector2 _groundCheckSize = new Vector2(0.6f, 0.1f);
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _coyoteTime = 0.033f;

    private int _jumpsRemaining = 2;
    private bool _wasGrounded;
    private bool _performJump;
    private bool _isGrounded;

    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private float _xInput;
    private float _coyoteTimeCounter;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
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

        if (Input.GetButtonDown("Jump") && (_jumpsRemaining > 0 || _coyoteTimeCounter > 0f))
        {
            _performJump = true;
            _jumpsRemaining--;
            _coyoteTimeCounter = 0f;

            _animator.SetBool("IsGrounded", false);
            _animator.SetTrigger("Jump");
        }

        _isGrounded = Physics2D.OverlapBox(
            _groundCheck.position,
            _groundCheckSize,
            0f,
            _groundLayer
        );

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
            _animator.SetBool("IsGrounded", true);
            _animator.SetBool("IsFalling", false);
            _animator.SetTrigger("Land");
        }

        _wasGrounded = _isGrounded;
        
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

           
        }

        _animator.SetFloat("XInputAbs", Math.Abs(_xInput));
    }
}
