
using UnityEngine;

public class ChestAnimationEvents : MonoBehaviour
{
    private Chest chest;

    private void Awake()
    {
        chest = GetComponentInParent<Chest>();
    }

    public void OnOpeningFinished()
    {
        chest.OnOpeningFinished();
    }
}
