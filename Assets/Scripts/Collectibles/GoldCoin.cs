using UnityEngine;

public class GoldCoin : MonoBehaviour
{
    [SerializeField] private int value = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CollectableController.Instance.AddGold(value);
            Destroy(gameObject);
        }
    }
}
