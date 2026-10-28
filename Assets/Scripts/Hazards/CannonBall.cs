using UnityEngine;

public class CannonBall : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private GameObject explosionPrefab;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        { 
            CollectableController.Instance.TakeDmg(damage);
            Destroy(gameObject);
        }
            Instantiate(explosionPrefab,GetComponent<Transform>().position, GetComponent<Transform>().rotation);
            Destroy(gameObject);
    }
}
