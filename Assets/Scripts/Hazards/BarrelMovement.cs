using UnityEngine;

public class BarrelMovement : MonoBehaviour
{
    [SerializeField] private bool rollRight = true;

    [SerializeField] private float moveForce = 1f;
    [SerializeField] private float maxSpeed = 5f;

    [SerializeField] private float rotationForce = 10f;
    [SerializeField] private float maxRotationSpeed = 180f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float direction = rollRight ? 1f : -1f;

        rb.AddForce(Vector2.right * direction * moveForce);
        rb.AddTorque(-direction * rotationForce);

        Vector2 velocity = rb.linearVelocity;
        velocity.x = Mathf.Clamp(velocity.x, -maxSpeed, maxSpeed);
        rb.linearVelocity = velocity;

        rb.angularVelocity = Mathf.Clamp(rb.angularVelocity,-maxRotationSpeed,maxRotationSpeed);
    }
}
