using UnityEngine;

public class EnemyDetect : MonoBehaviour
{
    public bool playerDetect = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerDetect = true;
            Debug.Log("Player detected");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerDetect = false;
            Debug.Log("Player exit ");
        }
    }
    
}
