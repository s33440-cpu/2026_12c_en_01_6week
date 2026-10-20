using UnityEngine;
using Player;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private PlayerConfig _config;
    private LevelRestart _levelRestart;

    public int currentHealth;

    private void Awake()
    {
        currentHealth = _config.BaseHealth;
        _levelRestart = GetComponent<LevelRestart>();
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
        Debug.Log("Damage source: " + System.Environment.StackTrace);
        if (currentHealth <= 0)
        {
            Debug.Log("Player died");
            _levelRestart.Restart();
        }
    }
}