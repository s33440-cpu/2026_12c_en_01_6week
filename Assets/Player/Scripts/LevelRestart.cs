using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelRestart : MonoBehaviour
{
    private static bool _isRestarting;

    private void Start()
    {
        _isRestarting = false;
    }

    public void Restart()
    {
        if (_isRestarting)
            return;

        _isRestarting = true;

        Debug.Log("RESTARTING SCENE: " + SceneManager.GetActiveScene().name);
        Debug.Log("SCENE INDEX: " + SceneManager.GetActiveScene().buildIndex);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}