
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private Vector2 _attackSize = new Vector2(0.5f, 0.5f);
    [SerializeField] private float _attackCooldown = 0.5f;

    private float _nextAttackTime;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        UpdateAttackPoint();

        if (PlayerModeManager.Instance == null)
            return;

        if (PlayerModeManager.Instance.CurrentMode != PlayerMode.Sword)
            return;

        if (Input.GetButtonDown("Fire1") && Time.time >= _nextAttackTime)
        {
            Attack();
            _nextAttackTime = Time.time + _attackCooldown;
        }
    }

    private void UpdateAttackPoint()
    {
        Vector3 localPosition = _attackPoint.localPosition;

        localPosition.x = Mathf.Abs(localPosition.x);

        if (_spriteRenderer.flipX)
        {
            localPosition.x *= -1;
        }

        _attackPoint.localPosition = localPosition;
    }

    private void Attack()
    {
        _animator.SetTrigger("Attack");

        Collider2D[] hitObjects = Physics2D.OverlapBoxAll(
            _attackPoint.position,
            _attackSize,
            0f
        );

        foreach (Collider2D hitObject in hitObjects)
        {
            DestructibleObject destructible =
                hitObject.GetComponentInParent<DestructibleObject>();

            if (destructible != null)
            {
                destructible.DestroyObject();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_attackPoint.position, _attackSize);
    }
}
 