using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CrabFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform stopPoint;

    [SerializeField] private float speed = 3.5f;
    [SerializeField] private float followDistance = 1.2f;
    [SerializeField] private float jumpForce = 7f;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 0.15f;

    private Rigidbody2D rb;
    private Rigidbody2D playerRb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float direction;
    private bool isGrounded;
    private bool stopped;

    private bool playerWasJumping;
    private bool jumpRequested;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (player != null)
        {
            playerRb = player.GetComponent<Rigidbody2D>();
        }
    }

    private void Update()
    {
        isGrounded = groundCheck != null &&
            Physics2D.OverlapCircle(
                groundCheck.position,
                groundCheckRadius,
                groundLayer
            );

        if (player == null || stopped)
        {
            direction = 0;
            UpdateAnimation();
            return;
        }

        if (stopPoint != null &&
            player.position.x >= stopPoint.position.x)
        {
            stopped = true;
            direction = 0;
            jumpRequested = false;

            UpdateAnimation();
            return;
        }

        float distanceX = player.position.x - transform.position.x;

        if (Mathf.Abs(distanceX) > followDistance)
        {
            direction = Mathf.Sign(distanceX);

            if (spriteRenderer != null)
            {
                spriteRenderer.flipX = direction > 0;
            }
        }
        else
        {
            direction = 0;
        }

        // Проверяем начало прыжка игрока
        if (playerRb != null)
        {
            bool playerJumping = playerRb.linearVelocity.y > 2f;

            // Реагируем только на начало прыжка
            if (playerJumping && !playerWasJumping)
            {
                if (isGrounded && Mathf.Abs(distanceX) < 3f)
                {
                    jumpRequested = true;
                }
            }

            playerWasJumping = playerJumping;
        }

        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(
            direction * speed,
            rb.linearVelocity.y
        );

        if (jumpRequested)
        {
            jumpRequested = false;

            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                0f
            );

            rb.AddForce(
                Vector2.up * jumpForce,
                ForceMode2D.Impulse
            );
        }
    }

    private void UpdateAnimation()
    {
        if (animator == null)
            return;

        animator.SetBool("IsGrounded", isGrounded);

        animator.SetBool(
            "IsWalking",
            isGrounded && direction != 0
        );

        animator.SetFloat(
            "VerticalSpeed",
            rb.linearVelocity.y
        );
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;

            Gizmos.DrawWireSphere(
                groundCheck.position,
                groundCheckRadius
            );
        }
    }
}