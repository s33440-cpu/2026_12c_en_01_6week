
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

    [Header("Ranged Attack")]
    [SerializeField] private SwordProjectile _swordProjectilePrefab;
    [SerializeField] private Transform _projectileSpawnPoint;
    [SerializeField] private float _chargeTime = 2f;
    [SerializeField] private float _rangedAttackCooldown = 1f;

    private float _nextAttackTime;
    private float _nextRangedAttackTime;
    private float _holdStartTime;
    private bool _holdingAttack;
    private bool _projectileThrown;

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

        HandleAttackInput();
    }

    private void HandleAttackInput()
    {
        if (Input.GetButtonDown("Fire1") && !_holdingAttack)
        {
            _holdingAttack = true;
            _projectileThrown = false;
            _holdStartTime = Time.time;
        }

        if (!_holdingAttack)
            return;

        bool rangedUnlocked = PlayerModeManager.Instance.RangedAttackUnlocked;

        if (Input.GetButton("Fire1") &&
            rangedUnlocked &&
            !_projectileThrown &&
            Time.time - _holdStartTime >= _chargeTime &&
            Time.time >= _nextRangedAttackTime)
        {
            ThrowSword();
            _projectileThrown = true;
            _nextRangedAttackTime = Time.time + _rangedAttackCooldown;
        }

        if (Input.GetButtonUp("Fire1"))
        {
            if (!_projectileThrown && Time.time >= _nextAttackTime)
            {
                Attack();
                _nextAttackTime = Time.time + _attackCooldown;
            }

            _holdingAttack = false;
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
        HashSet<RangedEnemy> damagedRangedEnemies = new HashSet<RangedEnemy>();

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
                enemy.TakeDamage(_attackDamage);

            RangedEnemy rangedEnemy =
                hitObject.GetComponentInParent<RangedEnemy>();

            if (rangedEnemy != null && damagedRangedEnemies.Add(rangedEnemy))
                rangedEnemy.TakeDamage(_attackDamage);
        }
    }

    private void ThrowSword()
    {
        if (_swordProjectilePrefab == null || _projectileSpawnPoint == null)
            return;

        bool facingRight = !_spriteRenderer.flipX;


        SwordProjectile projectile = Instantiate(
            _swordProjectilePrefab,
            _projectileSpawnPoint.position,
            Quaternion.identity
        );

        projectile.Initialize(facingRight);
    }

    private void OnDrawGizmosSelected()
    {
        if (_attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(_attackPoint.position, _attackSize);
        }

        if (_projectileSpawnPoint != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(_projectileSpawnPoint.position, 0.15f);
        }
    }
}
