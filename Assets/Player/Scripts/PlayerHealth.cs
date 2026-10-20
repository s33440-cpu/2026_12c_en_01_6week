using Player;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private PlayerConfig _playerConfig;
    [SerializeField] private int _currentHealth;

    private void Awake()
    {
        _currentHealth = _playerConfig.BaseHealth;
    }

    public void Heal(int healAmount)
    {
        if (healAmount < 0)
        {
            return;
        }

        if(_currentHealth +  healAmount > _playerConfig.MaxHealth)
        {
            _currentHealth = _playerConfig.MaxHealth;
            return;
        }

        _currentHealth += healAmount;
    }
}
