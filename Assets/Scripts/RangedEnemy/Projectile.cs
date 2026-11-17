using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifetime = 5f;   // safety cleanup if it never hits anything

    private Rigidbody2D rb;
    private int damage;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezePositionY
                       | RigidbodyConstraints2D.FreezeRotation;
    }

    public void Launch(Vector2 direction, float speed, int damage, Collider2D[] shooterColliders)
    {
        this.damage = damage;

        // Ignore only the shooter's colliders (the enemy keeps its colliders for other interactions)
        Collider2D[] myColliders = GetComponents<Collider2D>();
        foreach (Collider2D mine in myColliders)
        {
            foreach (Collider2D theirs in shooterColliders)
            {
                if (theirs != null)
                    Physics2D.IgnoreCollision(mine, theirs);
            }
        }

        rb.linearVelocity = direction.normalized * speed;   // use rb.velocity on older Unity versions
        Destroy(gameObject, lifetime);
    }

    // Non-trigger projectile collider
    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleHit(collision.collider);
    }

    // Trigger projectile collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        HandleHit(other);
    }

    private void HandleHit(Collider2D other)
    {
        if (other.GetComponentInParent<PlayerHealth>() != null)
        {
            CollectableController.Instance.TakeDmg(damage);
        }

        Destroy(gameObject);
    }
}