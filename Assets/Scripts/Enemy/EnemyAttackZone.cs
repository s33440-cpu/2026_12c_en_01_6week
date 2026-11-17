using UnityEngine;

public class EnemyAttackZone : MonoBehaviour
{
    public bool playerInRange = false;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            playerInRange = true;
           // Debug.Log("Player in attack range");
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            playerInRange = false;
           // Debug.Log("Player left attack range");
        }
    }
}
