using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Rigidbody))]
public class PlayerMoving : MonoBehaviour {
    private Animator animator;
    public Rigidbody rb;
    public float speed =10.0f;
    //public float turn_speed = 60.0f;
    public Vector3 moveDirection = Vector3.zero;
    private float jumpHeight=40f;
    //---------------------------------------ground checking
    public LayerMask groundLayer;
    public Transform groundCheck;
    Collider[] groundCollisions;
    float groundCheckRadius = 0.2f;
    public bool grounded = false;
    //--------------------------------------wall checking
    public bool wallJumpLearned = false;
    public LayerMask wallLayer;
    public Transform wallCheck;
    Collider[] wallCollisions;
    float wallCheckRadius = 0.8f;
    private bool hitWall = false;

    // Use this for initialization
    void Start () {
        animator = gameObject.GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        jumpHeight = 100;
    }

// Update is called once per frame
    void Update ()
    {
        showAnimation();
        
        if (Input.GetAxis("Jump") > 0 && grounded && !animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            rb.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
        }
        
        if (Input.GetKey(KeyCode.RightArrow)|| Input.GetKey(KeyCode.D))
        {   
            Quaternion desireRotate = Quaternion.Euler(0, 90, 0);
            transform.rotation = desireRotate;
            if (grounded)
            {
                transform.position += new Vector3(1, 0, 0) * speed * Time.deltaTime;
            }
            else {
                transform.position += new Vector3(0.6f, 0, 0) * speed * Time.deltaTime;
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            Quaternion desireRotate = Quaternion.Euler(0, -90, 0);
            transform.rotation = desireRotate;
            if (grounded)
            {
                transform.position += new Vector3(-1, 0, 0) * speed * Time.deltaTime;
            }
            else {
                transform.position += new Vector3(-0.6f, 0, 0) * speed * Time.deltaTime;
            }
        }

        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
            {
                animator.speed = 1.6f;
                transform.position -= new Vector3(0, 10f, 0) * Time.deltaTime;
            }
            else if(!grounded)
            {
                transform.position -= new Vector3(0, 10f, 0) * Time.deltaTime;
            }
        }
        else
        {
            animator.speed = 1;
        }

        if (wallJumpLearned)
        {
            if (hitWall && !grounded)
            {

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    animator.SetInteger("Anim", 0);
                    animator.SetInteger("wallJump", 1);
                    rb.AddForce(transform.up * 800 - transform.forward * 400, ForceMode.Impulse);

                    //--------------flip
                    Vector3 theScale = transform.localScale;
                    theScale.x *= -1;
                    transform.localScale = theScale;
                }
                else
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("WallJump"))
                        animator.SetInteger("wallJump", 0);
                }
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("WallJump"))
            {
                animator.SetInteger("wallJump", 0);
            }
        }
	}
    
    void FixedUpdate()
    {
        groundCollisions = Physics.OverlapSphere(groundCheck.position, groundCheckRadius, groundLayer);
        if (groundCollisions.Length > 0)
            grounded = true;
        else
            grounded = false;
        
        wallCollisions = Physics.OverlapSphere(wallCheck.position, wallCheckRadius, wallLayer);
        if (wallCollisions.Length > 0)
            hitWall = true;
        else
            hitWall = false;
    }

    void showAnimation()
    {
        if((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && grounded)
        {
            animator.SetInteger("Anim", 1);
        }
        else
        {
            animator.SetInteger("Anim", 0);
        }
        if (Input.GetKey(KeyCode.Space) && grounded)
        {
            animator.SetInteger("Jump", 1);
        }else if(animator.GetCurrentAnimatorStateInfo(0).IsName("Jump") && animator.GetInteger("Jump") == 1 )
        {
            animator.SetInteger("Jump", 0);
        }
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Jump") && grounded)
        {
            animator.Play("Idle");
        }
    }
}
