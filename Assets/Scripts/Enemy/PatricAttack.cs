using UnityEngine;

public class PatricAttack : MonoBehaviour
{
    public PatricMove patricMove;
    public EnemyAttackZone enemyAttackZone;
    public PlayerHealth playerHealth;

    public int biteDamage = 10;
    public float coolDown = 2f;

    private float biteTimer = 0f;
    private Animator animator;
    private Rigidbody2D rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (enemyAttackZone == null || patricMove == null)
            return;

        if (biteTimer > 0f)
        {
            biteTimer -= Time.deltaTime;
        }

        if (enemyAttackZone.playerInRange && biteTimer <= 0f && !patricMove.patrickIsAttacking)
        {
            patricMove.patrickIsAttacking = true;

            if (rb != null)
            {
                rb.linearVelocity = new Vector2(
                    0f,
                    rb.linearVelocity.y
                );
            }

            animator.SetTrigger("Attack");

            biteTimer = coolDown;
        }
    }

    public void BiteDamage()
    {
        if (enemyAttackZone == null || !enemyAttackZone.playerInRange)
            return;

        if (playerHealth != null && Vector2.Distance(transform.position, playerHealth.transform.position) < 2f)
        {
            playerHealth.TakeDamage(biteDamage);
        }
    }

    public void EndBite()
    {
        Debug.Log("PATRICK: Normal attack finished");
        if (patricMove != null)
        {
            patricMove.patrickIsAttacking = false;
        }
    }
}