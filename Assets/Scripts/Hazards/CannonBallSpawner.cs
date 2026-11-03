using UnityEngine;

public class CannonBallSpawner : MonoBehaviour
{
    [SerializeField] private GameObject cannonBall;
    [SerializeField] private float spawnInterval = 2f;
    private float timer = 0f;
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            Instantiate(cannonBall,transform.position, Quaternion.identity);
            timer = 0f;
        }
    }
}

