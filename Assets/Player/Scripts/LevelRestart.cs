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

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}