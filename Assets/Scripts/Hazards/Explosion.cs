using System.Collections;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float duration = 2f;
    [SerializeField] private float maxSize = 5f;

    private float timer = 0f;
    private Vector3 initialScale;

    private void Start()
    {
        initialScale = transform.localScale;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        float progress = timer / duration;

        transform.localScale = Vector3.Lerp(initialScale, initialScale * maxSize, progress);

        if (timer >= duration)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CollectableController.Instance.TakeDmg(damage);
        }

    }
}

   
