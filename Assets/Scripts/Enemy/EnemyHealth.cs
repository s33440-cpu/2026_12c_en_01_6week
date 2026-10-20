using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    private Animator animator;
    private bool isDead = false;
    void Awake()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            TakeDamage(100);
        }
    }
    public void TakeDamage(int damage)
    {
        if (damage <= 0 || isDead) {
            return;
        }

        currentHealth -= damage;
        Debug.Log("Heath" + currentHealth);
        if (currentHealth <= 0) {
           Die();
        }
        else
        {
            animator.SetTrigger("Hit");
        }
    }

    void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("Hit");
        EnemyMove movement = GetComponent<EnemyMove>();

        if (movement != null)
        {
            movement.enabled = false;
        }
        EnemyAttack attack = GetComponent<EnemyAttack>();
        if(attack != null)
        {
            attack.enabled = false;
        }
        Rigidbody2D body = GetComponent<Rigidbody2D>();
        if(body != null)
        { body.linearVelocity = Vector2.zero;
        }
        Destroy(gameObject, 1.1f);
        }
    }
    


