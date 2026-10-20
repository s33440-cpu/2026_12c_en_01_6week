
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int _baseHealth = 100;
    private LevelRestart _levelRestart;

    public int currentHealth;

    private void Awake()
    {
        currentHealth = _baseHealth;
        _levelRestart = GetComponent<LevelRestart>();
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0 || currentHealth <= 0)
        {
            return;
        }

        currentHealth = Mathf.Max(currentHealth - damage, 0);

        Debug.Log("Player health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("Player died");

            if (_levelRestart != null)
            {
                _levelRestart.Restart();
            }
            else
            {
                Debug.LogError("LevelRestart component is missing.");
            }
        }
    }
}
