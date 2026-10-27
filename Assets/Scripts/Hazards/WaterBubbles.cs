using UnityEngine;

public class WaterBubbles : MonoBehaviour
{
    private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play(0,0, Random.Range(0f,1f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
