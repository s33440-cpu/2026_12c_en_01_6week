using UnityEngine;
using Player;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private PlayerConfig _config;

    public int currentHealth;

    private void Awake()
    {
        currentHealth = _config.BaseHealth;
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0)
        {
            return;
        }

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        Debug.Log("Player health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("Player died");
        }
    }
}