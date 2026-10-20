using Player;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]


public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerConfig _config;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private Vector2 _groundCheckSize = new Vector2(0.6f, 0.1f);
    [SerializeField] private LayerMask _groundLayer;
    private int _jumpsRemaining = 2;
    private bool _wasGrounded;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private float _xInput;

    private bool _performJump;
    private bool _isGrounded;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {   
        //Walking
        _xInput = Input.GetAxis("Horizontal");

        if (_xInput > 0)
        {
            _spriteRenderer.flipX = false;
        }
        else if (_xInput < 0)
        {
            _spriteRenderer.flipX = true;
        }


        //Jumping
        if (Input.GetButtonDown("Jump") && _jumpsRemaining > 0)
        {
            _performJump = true;
            _jumpsRemaining--;
        }

        _isGrounded = Physics2D.OverlapBox(_groundCheck.position, _groundCheckSize, 0f, _groundLayer);

        if (_isGrounded && !_wasGrounded)
        {
            _jumpsRemaining = 2;
        }

        _wasGrounded = _isGrounded;
    }

    private void FixedUpdate()
    {   
        //Walking
        _rb.linearVelocity = new Vector2(_xInput * _config.Speed, _rb.linearVelocityY);

        //Jumping
        if (_performJump)
        {
            _performJump = false;

            if (_jumpsRemaining == 0)
            {
                _rb.linearVelocity = Vector2.zero;
            }

            _rb.AddForce(new Vector2(0, _config.JumpForce), ForceMode2D.Impulse);
        }
    }

}
