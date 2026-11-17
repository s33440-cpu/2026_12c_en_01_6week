
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    private Animator animator;
    private bool isDead = false;

    private void Awake()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0 || isDead)
            return;

        currentHealth = Mathf.Max(currentHealth - damage, 0);

        Debug.Log(gameObject.name + " health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        else if (animator != null)
        {
            animator.SetTrigger("Hit");
        }
    }

    private void Die()
    {
        isDead = true;

        if (animator != null)
        {
            animator.SetTrigger("Die");
            animator.ResetTrigger("Attack");
            animator.ResetTrigger("Hit");
        }

        EnemyMove enemyMove = GetComponent<EnemyMove>();
        if (enemyMove != null)
            enemyMove.enabled = false;

        EnemyAttack enemyAttack = GetComponent<EnemyAttack>();
        if (enemyAttack != null)
            enemyAttack.enabled = false;

        PatricMove patricMove = GetComponent<PatricMove>();
        if (patricMove != null)
            patricMove.enabled = false;

        PatricAttack patricAttack = GetComponent<PatricAttack>();
        if (patricAttack != null)
            patricAttack.enabled = false;

        Rigidbody2D body = GetComponent<Rigidbody2D>();
        if (body != null)
            body.linearVelocity = Vector2.zero;

        Destroy(gameObject, 1.1f);
    }
}
