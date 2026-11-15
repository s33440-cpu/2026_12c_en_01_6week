using UnityEngine;

public class BirdMovement : MonoBehaviour
{
    public float speed = 1f;
    public float distance = 15f;
    public float waveHeight = 0.1f;
    public float waveSpeed = 2f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float x = Mathf.Repeat(Time.time * speed, distance);
        float y = Mathf.Sin(Time.time * waveSpeed) * waveHeight;

        transform.position = new Vector3(
            startPosition.x + x,
            startPosition.y + y,
            startPosition.z
        );
    }
}