using UnityEngine;

public class HealthPot : MonoBehaviour
{
    public int amountOfHealing = 3;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        PotionInventory inventory = other.GetComponent<PotionInventory>();

        if (inventory == null)
            return;

        if (inventory.TryStorePotion(PotionType.Healing))
        {
            Destroy(gameObject);
        }
    }
}
