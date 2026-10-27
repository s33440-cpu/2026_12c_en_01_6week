using UnityEngine;
using System.Collections;

public class PatricDash : MonoBehaviour
{
    public PatricMove patricMove;
    public EnemyDetect patrickDetect;
    public Transform player;
    public float dashSpeed = 8f;
    public float dashDuration = 0.45f;
    public float dashCooldown = 5f;
    public float warningDuration = 0.6f;
    public float dashTriggerDistance = 4f;
    private Rigidbody2D rb;
    private Animator animator;
    private bool isDashing = false;
    private float cooldownTimer = 0f;
    public int dashDamage = 15;
    public float knockbackForce = 6f;
    private bool hasHitPlayer = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if (isDashing || patricMove == null || player == null ||
            patrickDetect == null)
            return;
        float distance = Vector2.Distance(
            transform.position,
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

    IEnumerator DashCoroutine()
    {
        isDashing = true;
        patricMove.patrickIsAttacking = true;
        float direction = player.position.x > transform.position.x ? 1f : -1f;
        if (direction == 1f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        animator.SetBool("IsMoving", false);
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(warningDuration);
        hasHitPlayer = false;
        dashIsActive = true;
        float timer = 0f;

        while (timer < dashDuration)
        {
            rb.linearVelocity = new Vector2(
                direction * dashSpeed,
                rb.linearVelocity.y
            );
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        dashIsActive = false;
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        cooldownTimer = dashCooldown;
        patricMove.patrickIsAttacking = false;
        isDashing = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryDamagePlayer(collision.gameObject);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        TryDamagePlayer(collision.gameObject);
    }

    private bool dashIsActive = false;
    private void TryDamagePlayer(GameObject target)
    {
        if (!isDashing || !dashIsActive || hasHitPlayer)
            return;

        if (!target.CompareTag("Player"))
            return;

        PlayerHealth health = target.GetComponent<PlayerHealth>();

        if (health == null)
            return;

        hasHitPlayer = true;

        health.TakeDamage(dashDamage);

        Rigidbody2D playerRb = target.GetComponent<Rigidbody2D>();

        if (playerRb != null)
        {
            float direction = Mathf.Sign(
                target.transform.position.x - transform.position.x
            );

            if (direction == 0f)
                direction = 1f;

            playerRb.AddForce(
                new Vector2(direction, 0.35f).normalized * knockbackForce,
                ForceMode2D.Impulse
            );
        }
    }
}

