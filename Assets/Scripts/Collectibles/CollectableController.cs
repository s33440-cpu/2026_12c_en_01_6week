
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CollectableController : MonoBehaviour
{
    public static CollectableController Instance;

    private int score = 0;
    private int amountOfDiamods = 0;
    private int amountOfGold = 0;
    private int amountOfKeys = 0;

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text diamondText;
    [SerializeField] private Slider GoldBar;
    [SerializeField] private Slider HpBar;
    [SerializeField] private TMP_Text KeyText;  

    private void Awake()
    {
        Instance = this;
    }

    public void AddSilver(int amount)
    {
        score += amount;
        UpdateScoreText();
    }
    public void AddDiamond(int amount)
    {
        amountOfDiamods += 1;
        score += amount;
        UpdateScoreText();
        UpdateDiamondText();
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
        UpdateKeyText();
    }
    public void AddHP(int amountOfHealing)
    {
        FindAnyObjectByType<PlayerHealth>().currentHealth += amountOfHealing;
        UpdateHealthBar();
    }
    public void TakeDmg(int damage)
    {
        FindAnyObjectByType<PlayerHealth>().currentHealth -= damage;
        UpdateHealthBar();
    }
    public void AddSpeed(int amountOfBoost)
    {
        FindAnyObjectByType<PlayerMovement>()._speed += amountOfBoost;
    }


    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }
    private void UpdateDiamondText()
    {
        diamondText.text = "Diamonds: " + amountOfDiamods;
    }
    private void UpdateGoldBar()
    {
        GoldBar.value = amountOfGold;
    }
    private void UpdateHealthBar()
    {
        HpBar.value = FindAnyObjectByType<PlayerHealth>().currentHealth;
    }
    private void UpdateKeyText()
    {
        KeyText.text = "Keys: " + amountOfKeys;
    }

    public bool UseKey()
    {
        if (amountOfKeys <= 0)
        {
            Debug.Log("You don't have any keys!");
            return false;
        }

        amountOfKeys--;
        UpdateKeyText();
        Debug.Log("Key used! Keys remaining: " + amountOfKeys);
        return true;
    }
}
