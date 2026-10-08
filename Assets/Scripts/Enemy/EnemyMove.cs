using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public Transform rPoint;
    public Transform lPoint;
    public float enemySpeed = 2f;
    public float jumpPower = 4f;
    public float timeJump = 3f;
    public float jumpTimer = 0f;
    private Rigidbody2D enemyBody;
    private int moveSide = 1; 
    private bool isGrounded = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
        jumpTimer += Time.deltaTime;
        if (jumpTimer >= timeJump && isGrounded)
        {
            enemyBody.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            jumpTimer = 0f;
            isGrounded = false;
        }
//        if (Input.GetKeyDown(KeyCode.Space))
// {
//            Debug.Log("SPACE PRESSED");
//            enemyBody.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
//        }


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        enemyBody.linearVelocity = new Vector2(enemySpeed*moveSide, enemyBody.linearVelocity.y);
        if (transform.position.x <= lPoint.position.x)
        {
            moveSide = 1;
        }

        if (transform.position.x >= rPoint.position.x)
        {
            moveSide = -1;
        }
//not see flip now, but later wiil be
        if (moveSide == 1)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        if (moveSide == -1)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Ground")
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Ground")
        {
            isGrounded = false;
        }
    }
}
