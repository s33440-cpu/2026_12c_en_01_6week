using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    void Awake()
    {
        currentHealth = maxHealth;
    }
    public void TakeDamage(int damage)
    {
        if (damage <= 0)
        {
            return;
        }
        currentHealth -= damage;
        Debug.Log("Player health: " + currentHealth);
        if (currentHealth <= 0)
        {
            Debug.Log("Player died");
        }
    }
}