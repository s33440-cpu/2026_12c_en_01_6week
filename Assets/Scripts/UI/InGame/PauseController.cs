using UnityEngine;

public class PauseController : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject darkOverlay;
    public void PauseGame()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        darkOverlay.SetActive(true);
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        darkOverlay.SetActive(false);
    }
}
