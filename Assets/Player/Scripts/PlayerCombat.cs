
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private Vector2 _attackSize = new Vector2(0.5f, 0.5f);
    [SerializeField] private float _attackCooldown = 0.5f;
    [SerializeField] private int _attackDamage = 25;
    [SerializeField] private SlashVFX _slashVFX;
    [SerializeField] private Transform _slashTransform;

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
        if (_attackPoint != null && _spriteRenderer != null)
        {
            Vector3 localPosition = _attackPoint.localPosition;
            localPosition.x = Mathf.Abs(localPosition.x);

            if (_spriteRenderer.flipX)
                localPosition.x *= -1;

            _attackPoint.localPosition = localPosition;
        }

        if (_slashTransform != null && _spriteRenderer != null)
        {
            Vector3 slashPosition = _slashTransform.localPosition;
            slashPosition.x = Mathf.Abs(slashPosition.x);

            if (_spriteRenderer.flipX)
                slashPosition.x *= -1;

            _slashTransform.localPosition = slashPosition;

            Vector3 slashScale = _slashTransform.localScale;
            slashScale.x = Mathf.Abs(slashScale.x);

            if (_spriteRenderer.flipX)
                slashScale.x *= -1;

            _slashTransform.localScale = slashScale;
        }
    }

    private void Attack()
    {
        if (_slashVFX != null)
            _slashVFX.Play();

        if (_animator != null)
            _animator.SetTrigger("Attack");

        if (_attackPoint == null)
            return;

        Collider2D[] hitObjects = Physics2D.OverlapBoxAll(
            _attackPoint.position,
            _attackSize,
            0f
        );

        HashSet<EnemyHealth> damagedEnemies = new HashSet<EnemyHealth>();

        foreach (Collider2D hitObject in hitObjects)
        {
            DestructibleObject destructible =
                hitObject.GetComponentInParent<DestructibleObject>();

            if (destructible != null)
            {
                destructible.DestroyObject();
                continue;
            }

            EnemyHealth enemy =
                hitObject.GetComponentInParent<EnemyHealth>();

            if (enemy != null && damagedEnemies.Add(enemy))
            {
                enemy.TakeDamage(_attackDamage);
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
