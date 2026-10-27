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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
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
        patricMove.patrickIsAttacking = false;
    }

}

