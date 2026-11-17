using UnityEngine;

public class ExplosionAnimationEvents : MonoBehaviour
{
    public void OnExplosionFinished()
    {
        Destroy(gameObject);
    }
}
