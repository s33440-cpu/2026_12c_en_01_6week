using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    void Awake()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            TakeDamage(5);
        }
    }
    public void TakeDamage(int damage)
    {
        if (damage <= 0) {
            return;
        }

        currentHealth -= damage;
        Debug.Log("Heath" + currentHealth);
        if (currentHealth <= 0) {
            Destroy(gameObject);
        }
    }
}


