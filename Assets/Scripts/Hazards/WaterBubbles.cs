using UnityEngine;

public class WaterBubbles : MonoBehaviour
{
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play(0,0, Random.Range(0f,1f));
    }
}
