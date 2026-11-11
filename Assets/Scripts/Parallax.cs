using UnityEngine;

// Runs after camera-follow scripts (including Cinemachine) so the background doesn't jitter.
[DefaultExecutionOrder(1000)]
[RequireComponent(typeof(SpriteRenderer))]
public class Parallax : MonoBehaviour
{
    [Tooltip("The camera to follow. Leave empty to use the camera tagged MainCamera.")]
    public Camera targetCamera;

    [Tooltip("1 = stuck to camera (far away), 0 = moves with the world")]
    [Range(0f, 1f)] public float factorX = 0.8f;

    [Tooltip("1 = always vertically centered on camera (recommended for short background bands)")]
    [Range(0f, 1f)] public float factorY = 1f;

    [Tooltip("Move the layer to the camera's position when the level starts, so it works in any scene without manual placement.")]
    public bool centerOnCamera = true;

    [Header("Cave exit")]
    [Tooltip("Stop following the camera once it passes Cave Exit X (for levels where the cave ends).")]
    public bool stopAtBoundary = false;

    [Tooltip("World X position where the cave ends. Set this per level.")]
    public float caveExitX = 100f;

    Transform cam;
    Vector3 camStart;
    Vector3 layerStart;
    float tileWidth;
    float halfWidth;

    void Start()
    {
        if (targetCamera == null) targetCamera = Camera.main;
        if (targetCamera == null)
        {
            Debug.LogError($"Parallax on '{name}': no camera found. Drag your camera into Target Camera, " +
                           "or set your camera's Tag to MainCamera.", this);
            enabled = false;
            return;
        }

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr.sprite == null)
        {
            Debug.LogError($"Parallax on '{name}': the Sprite Renderer has no sprite assigned (or it is Missing).", this);
            enabled = false;
            return;
        }

        cam = targetCamera.transform;
        camStart = cam.position;
        layerStart = centerOnCamera
            ? new Vector3(cam.position.x, cam.position.y, transform.position.z)
            : transform.position;

        // Width of ONE copy of the image, used for seamless wrapping.
        tileWidth = sr.sprite.bounds.size.x * Mathf.Abs(transform.lossyScale.x);
        // Half the full (tiled) width, used to stop the right edge at the cave exit.
        halfWidth = sr.bounds.extents.x;

        if (tileWidth <= 0f)
        {
            Debug.LogError($"Parallax on '{name}': sprite width is zero, can't scroll.", this);
            enabled = false;
        }
    }

    void LateUpdate()
    {
        Vector3 delta = cam.position - camStart;
        float x = layerStart.x + delta.x * factorX;
        float y = layerStart.y + delta.y * factorY;

        bool pastExit = stopAtBoundary && cam.position.x >= caveExitX;

        // Infinite scrolling: jump by whole image widths to stay centered on the camera.
        if (!pastExit)
            x += Mathf.Round((cam.position.x - x) / tileWidth) * tileWidth;

        if (stopAtBoundary)
        {
            // Keep the right edge of the background at the cave exit.
            x = Mathf.Min(x, caveExitX - halfWidth);

            // Outside the cave, park the layer at its starting height.
            if (pastExit) y = layerStart.y;
        }

        transform.position = new Vector3(x, y, layerStart.z);
    }
}