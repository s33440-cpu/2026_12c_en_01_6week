using UnityEngine;
using System.Collections;

public class PatricDash : MonoBehaviour
{
    [Header("References")]
    public PatricMove patricMove;
    public EnemyDetect patrickDetect;
    public Transform player;

    [Header("Dash Settings")]
    public float dashSpeed = 8f;
    public float dashDuration = 0.45f;
    public float dashCooldown = 5f;
    public float warningDuration = 0.6f;
    public float dashTriggerDistance = 4f;

    [Header("Ground Check")]
    public LayerMask groundLayer;
    public float groundCheckDistance = 1.5f;
    public float groundCheckForward = 0.6f;

    [Header("Damage")]
    public int dashDamage = 15;
    public float knockbackForce = 6f;

    private Rigidbody2D rb;
    private Animator animator;
    private Collider2D bodyCollider;

    private bool isDashing = false;
    private bool dashIsActive = false;
    private bool hasHitPlayer = false;

    private float cooldownTimer = 0f;
    private float lastDirection = 1f;

    private Vector3 originalScale;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        bodyCollider = GetComponent<Collider2D>();

        originalScale = transform.localScale;
    }

    private void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if (isDashing ||
            patricMove == null ||
            patrickDetect == null ||
            player == null)
        {
            return;
        }

        float distance = Vector2.Distance(
            rb.position,
            player.position
        );

        if (patrickDetect.playerDetect &&
            distance <= dashTriggerDistance &&
            cooldownTimer <= 0f &&
            !patricMove.patrickIsAttacking)
        {
            StartCoroutine(DashCoroutine());
        }
    }

    private IEnumerator DashCoroutine()
    {
        isDashing = true;
        patricMove.patrickIsAttacking = true;

        // Stop normal movement
        rb.linearVelocity = new Vector2(
            0f,
            rb.linearVelocity.y
        );

        animator.SetBool("IsMoving", false);

        // Determine direction before warning
        float direction = GetPlayerDirection();

        FaceDirection(direction);

        // Play attack animation
        animator.ResetTrigger("Attack");
        animator.SetTrigger("Attack");

        // Warning before dash
        yield return new WaitForSeconds(warningDuration);

        // Update direction immediately before moving
        direction = GetPlayerDirection();

        FaceDirection(direction);

        Debug.Log(
            "PATRICK DASH START | Direction: " + direction +
            " | Player X: " + player.position.x +
            " | Patrick X: " + rb.position.x
        );

        hasHitPlayer = false;
        dashIsActive = true;

        float timer = 0f;

        while (timer < dashDuration)
        {
            if (!HasGroundAhead(direction))
            {
                Debug.Log("PATRICK: Edge detected");
                break;
            }

            rb.linearVelocity = new Vector2(
                direction * dashSpeed,
                rb.linearVelocity.y
            );

            timer += Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }

        // Stop dash
        dashIsActive = false;

        rb.linearVelocity = new Vector2(
            0f,
            rb.linearVelocity.y
        );

        cooldownTimer = dashCooldown;

        patricMove.patrickIsAttacking = false;
        isDashing = false;

        Debug.Log("PATRICK: Dash finished successfully");
    }

    private float GetPlayerDirection()
    {
        float difference = player.position.x - rb.position.x;

        // Avoid random direction when positions are almost equal
        if (Mathf.Abs(difference) < 0.15f)
        {
            return lastDirection;
        }

        lastDirection = difference > 0f ? 1f : -1f;

        return lastDirection;
    }

    private void FaceDirection(float direction)
    {
        Vector3 scale = transform.localScale;

        scale.x = direction > 0f
            ? Mathf.Abs(originalScale.x)
            : -Mathf.Abs(originalScale.x);

        transform.localScale = scale;
    }

    private bool HasGroundAhead(float direction)
    {
        float halfWidth = bodyCollider != null
            ? bodyCollider.bounds.extents.x
            : 0.3f;

        float checkX = rb.position.x +
                       direction * (halfWidth + groundCheckForward);

        float checkY = bodyCollider != null
            ? bodyCollider.bounds.min.y + 0.1f
            : rb.position.y;

        Vector2 checkPosition = new Vector2(
            checkX,
            checkY
        );

        RaycastHit2D hit = Physics2D.Raycast(
            checkPosition,
            Vector2.down,
            groundCheckDistance,
            groundLayer
        );

        Debug.DrawRay(
            checkPosition,
            Vector2.down * groundCheckDistance,
            Color.red,
            0.1f
        );

        return hit.collider != null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryDamagePlayer(collision.gameObject);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        TryDamagePlayer(collision.gameObject);
    }

    private void TryDamagePlayer(GameObject target)
    {
        if (!isDashing || !dashIsActive || hasHitPlayer)
        {
            return;
        }

        if (!target.CompareTag("Player"))
        {
            return;
        }

        PlayerHealth health = target.GetComponent<PlayerHealth>();

        if (health == null)
        {
            return;
        }

        hasHitPlayer = true;

        health.TakeDamage(dashDamage);

        Rigidbody2D playerRb = target.GetComponent<Rigidbody2D>();

        if (playerRb != null)
        {
            float direction = Mathf.Sign(
                target.transform.position.x - transform.position.x
            );

            if (direction == 0f)
            {
                direction = lastDirection;
            }

            Vector2 knockbackDirection = new Vector2(
                direction,
                0.35f
            ).normalized;

            playerRb.AddForce(
                knockbackDirection * knockbackForce,
                ForceMode2D.Impulse
            );
        }
    }
}