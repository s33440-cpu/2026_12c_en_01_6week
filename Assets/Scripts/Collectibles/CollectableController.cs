using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CollectableController : MonoBehaviour
{
    public static CollectableController Instance;

    private int score = 0;
    private int amountOfDiamods = 0;
    private int amountOfGold = 0;
    private int amountOfKeys = 0;
    private int amountOfSilver = 0;

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private Slider GoldBar;
    [SerializeField] private Slider HpBar;

    [Header("Collectable Icons")]
    [SerializeField] private Image diamondImage;
    [SerializeField] private Image keyImage;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Start with both icons blacked out
        UpdateDiamondImage();
        UpdateKeyImage();
    }

    public void AddSilver(int amount)
    {
        amountOfSilver += amount;
        score += amount;
        UpdateScoreText();
    }

    public void AddDiamond(int amount)
    {
        amountOfDiamods += 1;
        score += amount;
        UpdateScoreText();
        UpdateDiamondImage();
    }

    public void AddGold(int amount)
    {
        score += amount;
        amountOfGold += 1;
        UpdateGoldBar();
        UpdateScoreText();
    }

    public void AddKey()
    {
        amountOfKeys += 1;
        UpdateKeyImage();
    }

    public void AddHP(int amountOfHealing)
    {
        PlayerHealth playerHealth = FindAnyObjectByType<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.Heal(amountOfHealing);
        }
    }

    public void TakeDmg(int damage)
    {
        PlayerHealth playerHealth = FindAnyObjectByType<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
    }

    public void AddSpeed(int amountOfBoost)
    {
        FindAnyObjectByType<PlayerMovement>()._speed += amountOfBoost;
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }

    private void UpdateDiamondImage()
    {
        SetIconState(diamondImage, amountOfDiamods > 0);
    }

    private void UpdateKeyImage()
    {
        SetIconState(keyImage, amountOfKeys > 0);
    }

    // Black = silhouette, white = the sprite's normal colors
    private void SetIconState(Image icon, bool owned)
    {
        if (icon == null)
            return;

        icon.color = owned ? Color.white : Color.black;
    }

    private void UpdateGoldBar()
    {
        GoldBar.value = amountOfGold;
    }

    private PlayerHealth playerHealth;

    private void OnEnable()
    {
        playerHealth = FindAnyObjectByType<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.HealthChanged += UpdateHealthBar;
        }
    }

    private void OnDisable()
    {
        if (playerHealth != null)
        {
            playerHealth.HealthChanged -= UpdateHealthBar;
        }
    }

    private void UpdateHealthBar(int current, int maximum)
    {
        if (HpBar != null)
        {
            HpBar.maxValue = maximum;
            HpBar.value = current;
        }
    }
    public bool UseKey()
    {
        if (amountOfKeys <= 0)
        {
            Debug.Log("You don't have any keys!");
            return false;
        }

        amountOfKeys--;
        UpdateKeyImage(); // goes back to black when the last key is used
        Debug.Log("Key used! Keys remaining: " + amountOfKeys);
        return true;
    }

    public int GetSilverCount()
    {
        return amountOfSilver;
    }
}