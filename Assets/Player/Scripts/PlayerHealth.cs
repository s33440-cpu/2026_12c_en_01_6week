
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int _baseHealth = 100;
    [SerializeField] private float _invulnerabilityDuration = 1f;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private AudioSource BGM;
    [SerializeField] private AudioSource deathSound;

    private LevelRestart _levelRestart;
    private float _nextDamageTime;

    public int currentHealth { get; private set; }
    public int MaxHealth => _baseHealth;

    public event Action<int, int> HealthChanged;

    private void Awake()
    {
        currentHealth = _baseHealth;
        _levelRestart = GetComponent<LevelRestart>();
    }

    private void Start()
    {
        HealthChanged?.Invoke(currentHealth, _baseHealth);
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0 || currentHealth <= 0)
            return;

        if (Time.time < _nextDamageTime)
            return;

        _nextDamageTime = Time.time + _invulnerabilityDuration;

        currentHealth = Mathf.Max(currentHealth - damage, 0);

        Debug.Log("Player health: " + currentHealth);

        HealthChanged?.Invoke(currentHealth, _baseHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("Player died");
            deathSound.Play();
            BGM.Stop();
            deathScreen.SetActive(true);
            FindAnyObjectByType<PauseController>().darkOverlay.SetActive(true);
            int levelIndex = SceneManager.GetActiveScene().buildIndex;
            bool isNewRecord = HighScoreManager.TrySaveScore(levelIndex, FindAnyObjectByType<CollectableController>().score);
            Time.timeScale = 0f;
        }
    }

    public void Heal(int amount)
    {
        if (amount <= 0 || currentHealth <= 0)
            return;

        currentHealth = Mathf.Min(currentHealth + amount, _baseHealth);

        Debug.Log("Player healed. Health: " + currentHealth);

        HealthChanged?.Invoke(currentHealth, _baseHealth);
    }
}
