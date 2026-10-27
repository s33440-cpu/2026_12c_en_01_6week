using UnityEngine;

public class EnemyAttackZone : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
