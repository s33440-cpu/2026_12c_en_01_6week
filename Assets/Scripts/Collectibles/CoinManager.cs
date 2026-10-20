
using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;

    private int _score = 0;

    private void Start()
    {
        UpdateScoreUI();
    }

    public void AddCoin()
    {
        _score++;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        _scoreText.text = "Coins: " + _score;
    }
}
