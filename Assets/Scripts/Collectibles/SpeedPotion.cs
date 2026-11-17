using UnityEngine;

public class SpeedPotion : MonoBehaviour
{
    public int amountOfBoost = 1;
    public float duration = 10f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        PotionInventory inventory = other.GetComponent<PotionInventory>();

        if (inventory == null)
            return;

        if (inventory.TryStorePotion(PotionType.Speed))
        {
            Destroy(gameObject);
        }
    }
}
