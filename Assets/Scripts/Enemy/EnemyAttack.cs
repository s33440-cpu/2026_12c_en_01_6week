using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public EnemyAttackZone attackZone;
    public PlayerHealth playerHealth;
    public int attackDamage = 10;
    public float attackTime = 2f;
    private float timer = 0f;
    private Animator animator;
public GameObject attackEffect;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (attackZone.playerInRange)
        {
            timer += Time.deltaTime;
            if (timer >= attackTime)
            {
                animator.SetTrigger("Attack");
             //   playerHealth.TakeDamage(attackDamage);
                timer = 0f; }
        }
        else {
            timer = 0f;
        }
    }

    public void DealDamage()
    {
        if(attackEffect != null)
        {
            attackEffect.SetActive(true);
        }
        if (attackZone.playerInRange && playerHealth.currentHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
        }
    }
    public void HideAttackEffect()
    {
        if (attackEffect != null)
        {
            attackEffect.SetActive(false);
        }
    }
}
