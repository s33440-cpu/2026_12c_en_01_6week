using UnityEngine;

public class PatricMove : MonoBehaviour
{
    public Transform patrickLeftPoint;
    public Transform patrickRightPoint;
    public Transform patrickPlayer;
    public EnemyDetect patrickDetect;
    public float patrickWalkSpeed = 2f;
    public float patrickChaseSpeed = 3.5f;
    public float patrickStopDistance = 1.2f;
    public float patrickJumpPower = 4f;
    public float patrickNormalJumpTime = 3f;
    public float patrickFastJumpTime = 0.7f;
    public bool patrickIsAttacking = false;
    private Rigidbody2D patrickBody;
    private Animator patrickAnimator;
    private int patrickDirection = 1;
    private Vector3 patrickStartPosition;
    private float patrickJumpTimer = 0f;
    private bool patrickIsGrounded = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        patrickBody = GetComponent<Rigidbody2D>();
        patrickAnimator = GetComponent<Animator>();
        patrickStartPosition = transform.position;
        if (patrickDetect == null)
        {
            patrickDetect = GetComponentInChildren<EnemyDetect>();
        }
    }

    // Update is called once per frame
    void Update(){
if (patrickIsAttacking == false)
        {
            patrickJumpTimer += Time.deltaTime;
            if (patrickDetect != null && patrickDetect.playerDetect == true)
            {
                if (patrickJumpTimer >= patrickFastJumpTime) {
                    if (patrickIsGrounded == true)
                    {
                        patrickBody.AddForce(
                            Vector2.up * patrickJumpPower,
                            ForceMode2D.Impulse
                        );

                        patrickJumpTimer = 0f;
                        patrickIsGrounded = false;
                    }
                }
            } else
                {
                    patrickJumpTimer = 0f;
                }
        }
        patrickAnimator.SetBool("IsGrounded", patrickIsGrounded);
        patrickAnimator.SetFloat("VerticalSpeed", patrickBody.linearVelocity.y);
    }
    void FixedUpdate()
    {
        if (patrickIsAttacking == true)
        {
            patrickAnimator.SetBool("IsMoving", false);
            return;
        }
        else
        {
            float patrickCurrentSpeed = patrickWalkSpeed;
            if (patrickDetect != null && patrickDetect.playerDetect == true && patrickPlayer != null)
            {
                patrickCurrentSpeed = patrickChaseSpeed;
                if (patrickPlayer.position.x > transform.position.x)
                {
                    patrickDirection = 1;
                }else
                {
                    patrickDirection = -1;
                }
                float patrickDistance = Mathf.Abs(
                    patrickPlayer.position.x - transform.position.x);
                if (patrickDistance < patrickStopDistance)
                {
                    patrickCurrentSpeed = 0f;
                }
            }else {
            float patrickDistanceHome = Mathf.Abs(
                patrickStartPosition.x - transform.position.x);
            if (patrickDistanceHome > 0.1f)
            {
                patrickCurrentSpeed = patrickWalkSpeed;
                if (patrickStartPosition.x > transform.position.x) {
                    patrickDirection = 1;
                }else {
                    patrickDirection = -1;
                }
            }else {
                patrickCurrentSpeed = 0f;
            }
        }
            patrickBody.linearVelocity = new Vector2(
                patrickCurrentSpeed * patrickDirection,
                patrickBody.linearVelocity.y);
            if (patrickDirection == 1)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            if (Mathf.Abs(patrickBody.linearVelocity.x) > 0.1f)
            {
                patrickAnimator.SetBool("IsMoving", true);
            }else
            {
                patrickAnimator.SetBool("IsMoving", false);
            }
        }
    }
    void OnCollisionStay2D(Collision2D patrickCollision)
    {
        if (patrickCollision.gameObject.name == "Ground")
        {
            patrickIsGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D patrickCollision)
    {
        if (patrickCollision.gameObject.name == "Ground")
        {
            patrickIsGrounded = false;
        }
    }
}
