using System.Collections;
using UnityEngine;

public class CollapsingPlatform : MonoBehaviour
{
    public float warningDuration = 2f;
    public float strongShakeDuration = 1f;
    public float fallingDuration = 1.5f;
    public float respawnDelay = 4f;
    public float normalShakeStrength = 0.04f;
    public float strongShakeStrength = 0.06f;
    public float maximumTiltAngle = 4f;
    public float fallingGravity = 2f;
    private Rigidbody2D platformRigidbody;
    private BoxCollider2D platformCollider;
    private SpriteRenderer platformSprite;
    private Vector3 startingPosition;
    private Quaternion startingRotation;
    private bool isCollapsing = false;

    private void Awake()
    {
        platformRigidbody = GetComponent<Rigidbody2D>();
        platformCollider = GetComponent<BoxCollider2D>();
        platformSprite = GetComponent<SpriteRenderer>();
        startingPosition = transform.position;
        startingRotation = transform.rotation;
        platformRigidbody.bodyType = RigidbodyType2D.Kinematic;
        platformRigidbody.gravityScale = 0f;
        platformRigidbody.freezeRotation = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isCollapsing)
        {
            return;
        }

        if (!collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y < -0.5f)
            {
                isCollapsing = true;
                StartCoroutine(CollapsePlatform());
                break;
            }
        }
    }

    private IEnumerator CollapsePlatform()
    {
        yield return StartCoroutine(ShakePlatform(warningDuration, normalShakeStrength, false));

        yield return StartCoroutine(ShakePlatform(strongShakeDuration, strongShakeStrength, true));
        platformRigidbody.bodyType = RigidbodyType2D.Dynamic;
        platformRigidbody.gravityScale = fallingGravity;
        yield return new WaitForSeconds(fallingDuration);
        platformRigidbody.simulated = false;
        platformSprite.enabled = false;
        platformCollider.enabled = false;
        yield return new WaitForSeconds(respawnDelay);
        platformRigidbody.bodyType = RigidbodyType2D.Kinematic;
        platformRigidbody.gravityScale = 0f;
        transform.SetPositionAndRotation(startingPosition, startingRotation);
        platformRigidbody.linearVelocity = Vector2.zero;
        platformRigidbody.angularVelocity = 0f;
        Physics2D.SyncTransforms();
        platformRigidbody.simulated = true;
        platformSprite.enabled = true;
        platformCollider.enabled = true;
        isCollapsing = false;
    }
    
    
    private IEnumerator ShakePlatform(float duration, float strength, bool shouldTilt)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.fixedDeltaTime;
            float horizontalShake = Random.Range(-strength, strength);

            platformRigidbody.MovePosition(
                startingPosition + new Vector3(horizontalShake, 0f, 0f));
            if (shouldTilt)
            {
                float tiltAngle = Random.Range(-maximumTiltAngle, maximumTiltAngle);

                platformRigidbody.MoveRotation(
                    startingRotation.eulerAngles.z + tiltAngle);
            }
            else {
                platformRigidbody.MoveRotation(startingRotation.eulerAngles.z);
            }
            yield return new WaitForFixedUpdate();
        }
        platformRigidbody.MovePosition(startingPosition);
        platformRigidbody.MoveRotation(startingRotation.eulerAngles.z);
        yield return new WaitForFixedUpdate();
    }
}