using UnityEngine;
using System.Collections;

public class BottleFall : MonoBehaviour
{
    public float fallDuration = 0.4f;
    public float fallDistance = 0.25f;

    private bool hasFallen = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasFallen)
        {
            hasFallen = true;
            StartCoroutine(Fall(other.transform.position.x));
        }
    }

    private IEnumerator Fall(float playerX)
    {
        float direction = playerX < transform.position.x ? 1f : -1f;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + new Vector3(
            direction * fallDistance,
            -0.1f,
            0f
        );

        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = startRotation *
                                 Quaternion.Euler(0f, 0f, -90f * direction);

        float timer = 0f;

        while (timer < fallDuration)
        {
            timer += Time.deltaTime;

            float progress = Mathf.Clamp01(timer / fallDuration);
            progress = Mathf.SmoothStep(0f, 1f, progress);

            transform.position = Vector3.Lerp(
                startPosition,
                endPosition,
                progress
            );

            transform.rotation = Quaternion.Lerp(
                startRotation,
                endRotation,
                progress
            );

            yield return null;
        }

        transform.position = endPosition;
        transform.rotation = endRotation;
    }
}