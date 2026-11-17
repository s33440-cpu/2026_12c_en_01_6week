using UnityEngine;
using UnityEngine.SceneManagement;

public class CrabFinal : MonoBehaviour
{
    public int requiredCoins = 22;
    public string nextLevelName = "Level_2";
    public CrabCutscene cutscene;

    public bool CanFinishLevel()
    {
        if (CollectableController.Instance == null)
        {
            return false;
        }

        return CollectableController.Instance.GetSilverCount() >= requiredCoins;
    }

    public void FinishLevel()
    {
        if (!CanFinishLevel()) {
            return;
        }
        int levelIndex = SceneManager.GetActiveScene().buildIndex;
        bool isNewRecord = HighScoreManager.TrySaveScore(levelIndex, FindAnyObjectByType<CollectableController>().score);
        if (cutscene != null) {
            cutscene.nextLevelName = nextLevelName;
            cutscene.StartCutscene();
        }
     //   else {
    //        Debug.LogWarning("CrabCutscene is not assigned!");
  //      }
    }
}