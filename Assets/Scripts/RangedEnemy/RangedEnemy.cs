using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float shootInterval = 2f;   // time between attacks (should be longer than the attack animation)
    [SerializeField] private float projectileSpeed = 8f;
    [SerializeField] private int projectileDamage = 10;
    [SerializeField] private float shootRange = 12f;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private bool spriteFacesRight = true;   // which way the sprite faces by default

    private static readonly int AttackHash = Animator.StringToHash("Attack");

    private Transform player;
    private Collider2D[] myColliders;
    private float timer;
    private float facingDirection = 1f;

    private void Start()
    {
        PlayerHealth ph = FindAnyObjectByType<PlayerHealth>();
        if (ph != null)
            player = ph.transform;

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        myColliders = GetComponentsInChildren<Collider2D>();
        timer = shootInterval;
    }

    private void Update()
    {
        if (player == null)
            return;

        if (!PlayerInRange())
            return;   // stays on the idle sprite, no attack triggered

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            StartAttack();
            timer = shootInterval;
        }
    }

    private bool PlayerInRange()
    {
        return Vector2.Distance(transform.position, player.position) <= shootRange;
    }

    private void StartAttack()
    {
        // Face the player once, at the start of the attack, so it doesn't flip mid-animation
        facingDirection = Mathf.Sign(player.position.x - transform.position.x);

        Vector3 scale = transform.localScale;
        float baseSign = spriteFacesRight ? 1f : -1f;
        scale.x = Mathf.Abs(scale.x) * facingDirection * baseSign;
        transform.localScale = scale;

        animator.SetTrigger(AttackHash);
    }

    // Called by an Animation Event on the frame where the shot should leave the enemy
    public void FireProjectile()
    {
        // If the player left range during the wind-up, skip the shot
        if (player == null || !PlayerInRange())
            return;

        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;

        Projectile p = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
        p.Launch(new Vector2(facingDirection, 0f), projectileSpeed, projectileDamage, myColliders);
    }
}