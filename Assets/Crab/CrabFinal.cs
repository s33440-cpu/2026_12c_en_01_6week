using UnityEngine;
using UnityEngine.SceneManagement;

public class CrabFinal : MonoBehaviour
{
    public int requiredCoins = 10;
    public string nextLevelName = "Level_2";

    public bool CanFinishLevel()
    {
        if (CollectableController.Instance == null)
            return false;

        return CollectableController.Instance.GetSilverCount() >= requiredCoins;
    }

    public void FinishLevel()
    {
        if (CanFinishLevel())
        {
            SceneManager.LoadScene(nextLevelName);
        }
    }
}