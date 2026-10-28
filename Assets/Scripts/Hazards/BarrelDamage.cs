using UnityEngine;

public class BarrelDamage : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CollectableController.Instance.TakeDmg(damage);
            Destroy(transform.parent.gameObject);
        }
    }
}
