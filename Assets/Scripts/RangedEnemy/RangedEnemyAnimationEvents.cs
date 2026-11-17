using UnityEngine;

public class RangedEnemyAnimationEvents : MonoBehaviour
{
    private RangedEnemy enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<RangedEnemy>();
    }

    // Keep the same name you used in the Animation Event
    public void FireProjectile()
    {
        if (enemy != null)
            enemy.FireProjectile();
    }
}