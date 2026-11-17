using UnityEngine;

public class RangedEnemyHealthBar : MonoBehaviour
{
    public RangedEnemy enemyHealth;
    public Transform healthBarFill;

    void Update()
    {
        float healthPercent = (float)enemyHealth.currentHealth / enemyHealth.maxHealth;
        healthBarFill.localScale = new Vector3(healthPercent, 0.15f, 1);
        healthBarFill.localPosition = new Vector3((healthPercent - 1) / 2, 0, 0);
    }
}
