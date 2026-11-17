using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class FinalEnding : MonoBehaviour
{
    public AudioSource backgroundMusic;
    public SpriteRenderer background;
    public Camera mainCamera;
    public Transform cameraPoint;
    public MonoBehaviour playerMovement;
    public float zoomSize = 2.5f;
    public float zoomDuration = 2f;
    private CameraFollow cameraFollow;
    private bool cameraStarted = false;
    public float musicFadeTime = 2f;
    public float backgroundFadeTime = 3f;
    private bool musicStarted = false;
    private bool backgroundStarted = false;
    public Image fadePanel;
    public GameObject endingText;
    public float fadeDuration = 2f;
    public float endingTextDuration = 3f;
    private bool endingStarted = false;    
    private void Start()
    {
        if (mainCamera == null) {
            mainCamera = Camera.main;
        }
        if (mainCamera != null) {
            cameraFollow = mainCamera.GetComponent<CameraFollow>();
        }
        if (fadePanel != null) {
            Color color = fadePanel.color;
            color.a = 0f;
            fadePanel.color = color;
            fadePanel.gameObject.SetActive(true);
        } 
        if (endingText != null) {
            endingText.SetActive(false);
        }
    }
    public void OnDialogueLine(int line)
    {
        if (line == 3 && !musicStarted)
        {
            musicStarted = true;
            StartCoroutine(FadeMusic());
        }

        if (line == 6 && !cameraStarted) {
            cameraStarted = true;
            if (playerMovement != null) {
                playerMovement.enabled = false;
            }
            StartCoroutine(ZoomCamera());
        }
        if (line == 7 && !backgroundStarted) {
            backgroundStarted = true;
            StartCoroutine(ChangeBackground());
        }
    }

    private IEnumerator FadeMusic()
    {
        if (backgroundMusic == null) {
            yield break;
        }
        float startVolume = backgroundMusic.volume;
        float timer = 0f;

        while (timer < musicFadeTime)
        {
            timer += Time.deltaTime;
            float t = timer / musicFadeTime;
            backgroundMusic.volume = Mathf.Lerp(startVolume, 0f, t);
            yield return null;
        }
        backgroundMusic.volume = 0f;
    }

    private IEnumerator ChangeBackground()
    {
        if (background == null) {
            yield break;
        }
        Color startColor = background.color;
        Color endColor = new Color(0.5f, 0.1f, 0.1f, 1f);
        float timer = 0f;
        while (timer < backgroundFadeTime) {
            timer += Time.deltaTime;
            float t = timer / backgroundFadeTime;
            background.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }
        background.color = endColor;
    }

    public void StartEnding()
    {
        if (!endingStarted)
        {
            endingStarted = true;
            StartCoroutine(PlayEnding());
        }
    }
    private IEnumerator ZoomCamera()
    {
        if (mainCamera == null || cameraPoint == null)
        {
          //  Debug.LogWarning("Camera or Camera Point is missing!");
            yield break;
        }

        if (cameraFollow != null)
        {
            cameraFollow.enabled = false;
        }

        Vector3 startPosition = mainCamera.transform.position;

        Vector3 targetPosition = new Vector3(
            cameraPoint.position.x,
            cameraPoint.position.y,
            startPosition.z
        );

        float startSize = mainCamera.orthographicSize;
        float timer = 0f;

        while (timer < zoomDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / zoomDuration);
            mainCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            mainCamera.orthographicSize = Mathf.Lerp(startSize, zoomSize, t);
            yield return null;
        }
        mainCamera.transform.position = targetPosition;
        mainCamera.orthographicSize = zoomSize;
    }
    private IEnumerator PlayEnding()
    {
        if (fadePanel != null)
        {
            float timer = 0f;
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                float alpha = Mathf.Clamp01(timer / fadeDuration);
                Color color = fadePanel.color;
                color.a = alpha;
                fadePanel.color = color;
                yield return null;
            }
            Color finalColor = fadePanel.color;
            finalColor.a = 1f;
            fadePanel.color = finalColor;
        }
        if (endingText != null) {
            endingText.SetActive(true);
        }
        yield return new WaitForSeconds(endingTextDuration);
        SceneManager.LoadScene("MainMenu");
    }
}