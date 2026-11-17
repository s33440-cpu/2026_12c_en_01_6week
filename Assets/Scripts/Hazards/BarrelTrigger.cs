using UnityEngine;

public class BarrelTrigger : MonoBehaviour
{
    [SerializeField] private BarrelMovement[] barrels;   // one or more barrels to start

    private bool triggered;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered)
            return;

        if (other.GetComponentInParent<PlayerHealth>() == null)
            return;   // not the player

        triggered = true;

        foreach (BarrelMovement barrel in barrels)
        {
            if (barrel != null)
                barrel.Activate();
        }
    }
}