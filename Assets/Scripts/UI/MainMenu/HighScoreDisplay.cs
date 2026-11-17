using UnityEngine;
using TMPro;

public class HighScoreDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text[] levelScoreTexts; // one per level

    private void OnEnable()
    {
        for (int i = 0; i < levelScoreTexts.Length; i++)
        {
            int levelIndex = i + 1;
            levelScoreTexts[i].text = $"Level {levelIndex}: {HighScoreManager.GetScore(levelIndex)}";
        }
    }
}