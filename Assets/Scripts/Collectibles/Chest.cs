
using UnityEngine;
using TMPro;

public class Chest : MonoBehaviour
{
    public GameObject potionPrefab;
    public Transform potionSpawnPoint;

    private bool playerNearby;
    private bool isOpened;

    private void Update()
    {
        if (!playerNearby || isOpened)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            OpenChest();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other);
        if (!other.CompareTag("Player"))
            return;

        playerNearby = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerNearby = false;
    }

    private void OpenChest()
    {
        if (isOpened)
            return;

        // Consume one key. Do not open if there are no keys.
        if (!CollectableController.Instance.UseKey())
            return;

        isOpened = true;

        // Spawn the potion above the chest.
        if (potionPrefab != null && potionSpawnPoint != null)
        {
            Instantiate(
                potionPrefab,
                potionSpawnPoint.position,
                potionSpawnPoint.rotation
            );
        }
        Debug.Log("Chest opened!");
    }
}
