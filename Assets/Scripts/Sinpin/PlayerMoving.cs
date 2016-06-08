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
    public LayerMask wallLayer;
    public Transform wallCheck;
    Collider[] wallCollisions;
    float wallCheckRadius = 0.8f;
    private bool hitWall = false;
    private bool doubleJump = false;
    // Use this for initialization
    void Start () {
        animator = gameObject.GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        jumpHeight = rb.mass * 60;
    }

// Update is called once per frame
    void Update () {
        if (Input.GetKey(KeyCode.RightArrow)|| Input.GetKey(KeyCode.D))
        {
            Quaternion desireRotate = Quaternion.Euler(0, 90, 0);
            transform.rotation = desireRotate;
            if (grounded)
            {
                animator.SetInteger("Anim", 1);
                transform.position += new Vector3(1, 0, 0) * speed * Time.deltaTime;
            }
            else {
                transform.position += new Vector3(0.6f, 0, 0) * speed * Time.deltaTime;
                
            }
        }
        else
        {
            animator.SetInteger("Anim", 0);
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            Quaternion desireRotate = Quaternion.Euler(0, -90, 0);
            transform.rotation = desireRotate;
            if (grounded)
            {
                animator.SetInteger("Anim", 1);
                transform.position += new Vector3(-1, 0, 0) * speed * Time.deltaTime;
            }
            else {
                transform.position += new Vector3(-0.6f, 0, 0) * speed * Time.deltaTime;
                
            }
        }
        
        if (Input.GetAxis("Jump")>0 && grounded)
        {
            animator.SetInteger("Anim", 0);
            animator.SetInteger("Jump", 1);
            rb.AddForce(transform.up * jumpHeight);
            grounded = false;
        }
        else
        {
            animator.SetInteger("Jump", 0);
        }
        if ( hitWall && !grounded && !doubleJump ){
            
            if (Input.GetAxis("Jump") > 0  )
            {
        
                animator.SetInteger("Anim", 0);
                animator.SetInteger("wallJump", 1);
                rb.AddForce(transform.up*100  - transform.forward*100, ForceMode.Impulse);
                if (!doubleJump)
                {
                       doubleJump = true;
                }
                //--------------flip
                Vector3 theScale = transform.localScale;
                theScale.x *= -1;
                transform.localScale = theScale;
            }
            else
            {
                doubleJump = false;
                animator.SetInteger("wallJump", 0);
            }
        }
        else
        {
            //doubleJump = false;
            animator.SetInteger("wallJump", 0);
        }
        if (grounded || hitWall)
            doubleJump = false;
        
        
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

    
}
