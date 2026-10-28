using UnityEngine;

public class HealthPot : MonoBehaviour
{
    [SerializeField] private int amountOfHealing = 3;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CollectableController.Instance.AddHP(amountOfHealing);
            Destroy(gameObject);
        }
    }
}
