using UnityEngine;

[DefaultExecutionOrder(1000)]
[RequireComponent(typeof(SpriteRenderer))]
public class Parallax : MonoBehaviour
{
    [Tooltip("1 = stuck to camera (far away), 0 = moves with the world")]
    [Range(0f, 1f)] public float factorX = 0.8f;
    [Tooltip("1 = always vertically centered on camera (recommended for short background bands)")]
    [Range(0f, 1f)] public float factorY = 1f;

    Transform cam;
    Vector3 camStart;
    Vector3 layerStart;
    float tileWidth;

    void Start()
    {
        cam = Camera.main.transform;
        camStart = cam.position;
        layerStart = transform.position;
        tileWidth = GetComponent<SpriteRenderer>().sprite.bounds.size.x * transform.lossyScale.x;
    }
    void LateUpdate()
    {
        Vector3 delta = cam.position - camStart;
        float x = layerStart.x + delta.x * factorX;
        float y = layerStart.y + delta.y * factorY;
        x += Mathf.Round((cam.position.x - x) / tileWidth) * tileWidth;
        transform.position = new Vector3(x, y, layerStart.z);
    }
}