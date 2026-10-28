using UnityEngine;

public class Diamond : MonoBehaviour
{
    [SerializeField] private int value = 50;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CollectableController.Instance.AddDiamond(value);
            Destroy(gameObject);
        }
    }
}
