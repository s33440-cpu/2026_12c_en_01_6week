using UnityEngine;

public static class HighScoreManager
{
    private const string KeyPrefix = "HighScore_Level_";

    public static bool TrySaveScore(int levelIndex, int score)
    {
        string key = KeyPrefix + levelIndex;
        int currentBest = PlayerPrefs.GetInt(key, 0);

        if (score > currentBest)
        {
            PlayerPrefs.SetInt(key, score);
            PlayerPrefs.Save();
            return true;
        }
        return false;
    }

    public static int GetScore(int levelIndex)
    {
        return PlayerPrefs.GetInt(KeyPrefix + levelIndex, 0);
    }
}