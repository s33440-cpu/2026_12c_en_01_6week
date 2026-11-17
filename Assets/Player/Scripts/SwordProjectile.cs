
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class SwordProjectile : MonoBehaviour
{
    [SerializeField] private float _speed = 12f;
    [SerializeField] private float _maxDistance = 10f;
    [SerializeField] private int _damage = 25;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private Vector2 _direction;
    private Vector2 _startPosition;
    private bool _initialized;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _rigidbody.gravityScale = 0f;
        _rigidbody.freezeRotation = true;

        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    public void Initialize(bool facingRight)
    {
        _direction = facingRight ? Vector2.right : Vector2.left;
        _startPosition = transform.position;
        _initialized = true;

        if (_spriteRenderer != null)
            _spriteRenderer.flipX = !facingRight;

        _rigidbody.linearVelocity = _direction * _speed;
    }

    private void Update()
    {
        if (!_initialized)
            return;

        float distance = Vector2.Distance(_startPosition, transform.position);

        if (distance >= _maxDistance)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Projectile collided with: " + other.name);
        EnemyHealth enemy = other.GetComponentInParent<EnemyHealth>();

        if (enemy != null)
        {
            enemy.TakeDamage(_damage);
            Destroy(gameObject);
            return;
        }

        RangedEnemy rangedEnemy = other.GetComponentInParent<RangedEnemy>();

        if (rangedEnemy != null)
        {
            rangedEnemy.TakeDamage(_damage);
            Destroy(gameObject);
        }
    }
}
