
using System.Collections;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] private int damage = 25;
    [SerializeField] private float damageInterval = 1f;

    private Coroutine damageCoroutine;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (damageCoroutine == null)
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                damageCoroutine = StartCoroutine(DealDamageOverTime(playerHealth));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;
        }
    }

    private IEnumerator DealDamageOverTime(PlayerHealth playerHealth)
    {
        while (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);

            if (playerHealth == null || playerHealth.currentHealth <= 0)
                break;

            yield return new WaitForSeconds(damageInterval);
        }

        damageCoroutine = null;
    }
}
