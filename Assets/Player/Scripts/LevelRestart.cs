using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelRestart : MonoBehaviour
{
    public void Restart()
    {
        Debug.Log("RESTART CALLED");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}