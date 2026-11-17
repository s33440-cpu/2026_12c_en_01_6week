using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class CrabCutscene : MonoBehaviour
{
    public Camera mainCamera;
    public Transform cameraPoint;
    public Image fadePanel;
    private AudioSource audioSource;
    public AudioClip evilLaugh;
    public string nextLevelName = "Level_2";
    public float zoomSize = 3f;
    public float zoomDuration = 1.5f;
    public float fadeDuration = 2f;
    public float laughDelay = 1f;
    private Behaviour cameraFollow;
    private bool cutsceneStarted = false;
    public MonoBehaviour playerMovement;
    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (mainCamera != null)
        {
            cameraFollow = mainCamera.GetComponent<CameraFollow>();
           // if (cameraFollow == null)
           // {
           //     Debug.LogWarning("Camera Follow script was not found!");
          //  }
        }

        AudioSource[] sources = GetComponents<AudioSource>();

        if (sources.Length >= 2)
        {
            audioSource = sources[1];
        }

        if (fadePanel != null)
        {
            Color color = fadePanel.color;
            color.a = 0f;
            fadePanel.color = color;
            fadePanel.gameObject.SetActive(true);
            fadePanel.raycastTarget = false;
        }
    }
//АААААААААААААААААААААААААА
    public void StartCutscene()
    {
        if (cutsceneStarted) {
            return;
        }
        cutsceneStarted = true;
        
        if (playerMovement != null) {
            playerMovement.enabled = false;
        }
        StartCoroutine(PlayCutscene());
    }
    private IEnumerator PlayCutscene()
    {
        if (mainCamera == null || cameraPoint == null) {
           // Debug.LogError("Main Camera or Camera Point is missing!");
            cutsceneStarted = false;
            yield break;
        }
        if (cameraFollow != null)
        {
            cameraFollow.enabled = false;
        }
        Vector3 startPosition = mainCamera.transform.position;
        Vector3 targetPosition = new Vector3(cameraPoint.position.x, cameraPoint.position.y, startPosition.z);
        float startSize = mainCamera.orthographicSize;
        float timer = 0f;
        while (timer < zoomDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / Mathf.Max(zoomDuration, 0.01f));
            t = t * t * (3f - 2f * t);
            mainCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            mainCamera.orthographicSize = Mathf.Lerp(startSize, zoomSize, t);

            yield return null;
        }
        mainCamera.transform.position = targetPosition;
        mainCamera.orthographicSize = zoomSize;
        yield return new WaitForSeconds(laughDelay);

        if (audioSource != null && evilLaugh != null)
        {
            audioSource.PlayOneShot(evilLaugh);
        }

        if (fadePanel != null)
        {
            fadePanel.raycastTarget = true;
            timer = 0f;
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                float alpha = Mathf.Clamp01(timer / Mathf.Max(fadeDuration, 0.01f));
                Color color = fadePanel.color;
                color.a = alpha;
                fadePanel.color = color;
                yield return null;
            }
            Color finalColor = fadePanel.color;
            finalColor.a = 1f;
            fadePanel.color = finalColor;
        }
        SceneManager.LoadScene(nextLevelName);
    }
}