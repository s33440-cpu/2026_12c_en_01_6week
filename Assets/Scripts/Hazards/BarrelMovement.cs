using UnityEngine;

public class BarrelMovement : MonoBehaviour
{
    [SerializeField] private bool rollRight = true;

    [SerializeField] private float moveForce = 1f;
    [SerializeField] private float maxSpeed = 5f;

    [SerializeField] private float rotationForce = 10f;
    [SerializeField] private float maxRotationSpeed = 180f;

    [Header("Activation")]
    [SerializeField] private bool startActive = false;   // leave off so a trigger can start the barrel

    private Rigidbody2D rb;
    private bool isActive;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (startActive)
        {
            isActive = true;
        }
        else
        {
            // Kinematic keeps the barrel perfectly still (no gravity, no sliding on slopes)
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }

    // Call this from a trigger (or any other script) to start the barrel rolling
    public void Activate()
    {
        if (isActive)
            return;

        isActive = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    private void FixedUpdate()
    {
        if (!isActive)
            return;

        float direction = rollRight ? 1f : -1f;

        rb.AddForce(Vector2.right * direction * moveForce);
        rb.AddTorque(-direction * rotationForce);

        Vector2 velocity = rb.linearVelocity;
        velocity.x = Mathf.Clamp(velocity.x, -maxSpeed, maxSpeed);
        rb.linearVelocity = velocity;

        rb.angularVelocity = Mathf.Clamp(rb.angularVelocity, -maxRotationSpeed, maxRotationSpeed);
    }
}