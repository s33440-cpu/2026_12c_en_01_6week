using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private float _xInput;
    [SerializeField] private float _speed = 5;

    private bool _performJump;
    private bool _isGrounded;
    [SerializeField] private float _jumpForce = 5;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        _xInput = Input.GetAxis("Horizontal");

        if (_xInput > 0)
        {
            _spriteRenderer.flipX = false;
        }
        else if (_xInput < 0)
        {
            _spriteRenderer.flipX = true;
        }

        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _performJump = true;
        }
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = new Vector2(_xInput * _speed, _rb.linearVelocityY);
        if (_performJump)
        {
            _performJump = false;
            _rb.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Impulse);
        }
    }

    //TODO: is crappy redo later
    private void OnCollisionEnter2D(Collision2D other)
    {
        _isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        _isGrounded = false;
    }
}
