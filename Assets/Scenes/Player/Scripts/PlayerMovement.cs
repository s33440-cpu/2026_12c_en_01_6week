using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class BasicPlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rb;
    private float _xInput;
    [SerializeField] private float _speed = 5;

    private bool _performJump;
    private bool _isGrounded;
    private bool _touchingWall;
    private int _jumpsRemaining = 2;
    [SerializeField] private float _jumpForce = 5;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        _xInput = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && _jumpsRemaining > 0)
        {
            _performJump = true;
            _jumpsRemaining--;
        }
    }

    private void FixedUpdate()
    {
        float horizontalSpeed = _xInput * _speed;

        if (!_isGrounded && _touchingWall)
        {
            horizontalSpeed = 0;
        }

        _rb.linearVelocity = new Vector2(horizontalSpeed, _rb.linearVelocityY);

        if (_performJump)
        {
            _performJump = false;
            _rb.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        _isGrounded = true;
        _jumpsRemaining = 2;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        _isGrounded = false;
        _touchingWall = false;
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        foreach (ContactPoint2D contact in other.contacts)
        {
            if (Mathf.Abs(contact.normal.x) > 0.7f)
            {
                _touchingWall = true;
                return;
            }
        }
    }

}
