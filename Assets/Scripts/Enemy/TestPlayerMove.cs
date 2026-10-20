using UnityEngine;

public class TestPlayerMove : MonoBehaviour
{
    public float speed = 5f;

    private Rigidbody2D rb;
    private float move;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        move = Input.GetAxisRaw("Horizontal");
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);
    }
}