
using UnityEngine;

public class SilverCoin : MonoBehaviour
{
    [SerializeField] private int value = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CollectableController.Instance.AddSilver(value);
            Destroy(gameObject);
        }
    }
}
