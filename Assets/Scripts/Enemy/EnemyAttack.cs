using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public EnemyAttackZone attackZone;
    public PlayerHealth playerHealth;
    public int attackDamage = 10;
    public float attackTime = 2f;
    private float timer = 0f;
    void Update()
    {
        if (attackZone.playerInRange)
        {
            timer += Time.deltaTime;
            if (timer >= attackTime)
            {
                playerHealth.TakeDamage(attackDamage);
                timer = 0f; }
        }
        else {
            timer = 0f;
        }
    }
}
