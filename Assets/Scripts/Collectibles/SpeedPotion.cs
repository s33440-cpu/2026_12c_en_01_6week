using System.Collections;
using UnityEngine;

public class SpeedPotion : MonoBehaviour
{
    [SerializeField] private int amountOfBoost = 1;
    [SerializeField] private float duration = 10f;
    private bool collected = false;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (collected) return;
            collected = true;
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            
            StartCoroutine(ApplySpeedBoost());
        }
    }
    private IEnumerator ApplySpeedBoost()
    {
        float originalSpeed = FindAnyObjectByType<PlayerMovement>()._speed;
        CollectableController.Instance.AddSpeed(amountOfBoost);
        yield return new WaitForSeconds(duration);
        FindAnyObjectByType<PlayerMovement>()._speed = originalSpeed;
        Destroy(gameObject);
    }
}
