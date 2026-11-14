using UnityEngine;

public class Water : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            LevelRestart restart = other.GetComponent<LevelRestart>();

            if (restart != null)
                restart.Restart();
        }
    }
}