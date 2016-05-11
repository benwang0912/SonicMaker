using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Rigidbody))]
public class Animator_trig : MonoBehaviour {
    private Animator animator;
    public Rigidbody rb;
    public float speed =10.0f;
    //public float turn_speed = 60.0f;
    public Vector3 moveDirection = Vector3.zero;
    public float jumpHeight=40f;
    //---------------------------------------ground checking
    public LayerMask groundLayer;
    public Transform groundCheck;
    Collider[] groundCollisions;
    float groundCheckRadius = 0.2f;
    bool grounded = false;

    // Use this for initialization
    void Start () {
        animator = gameObject.GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    /*bool IsGrounded(){
        RaycastHit hit;
        //Debug.DrawRay(transform.position, transform.up * 10f, Color.yellow);
        if (Physics.Raycast(transform.position, -Vector3.up, out hit,0.5f))
        {
            Debug.Log(hit.collider.gameObject.name + Vector3.Distance(transform.position, hit.collider.transform.position));
            if (Vector3.Distance(transform.position, hit.collider.transform.position) <= 0.2f)
                return true;
        }
        
        return false;
    }*/

// Update is called once per frame
    void Update () {
        if (Input.GetKey(KeyCode.RightArrow)|| Input.GetKey(KeyCode.D))
        {
            Quaternion desireRotate = Quaternion.Euler(0, 90, 0);
            transform.rotation = desireRotate;
            if (grounded)
                animator.SetInteger("Anim",1);
            transform.position += new Vector3(1,0,0) *  speed * Time.deltaTime;
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
                animator.SetInteger("Anim", 1);
            transform.position += new Vector3(-1, 0, 0) * speed * Time.deltaTime;
        }
        
        if (Input.GetAxis("Jump")>0 && grounded)
        {
            animator.SetInteger("Jump", 1);
            rb.AddForce(transform.up * jumpHeight);
            grounded = false;
        }
        else
        {
            animator.SetInteger("Jump", 0);
        }
       // IsGrounded();
       /* float turn = Input.GetAxis("Horizontal");
        transform.Rotate(0,turn*turn_speed*Time.deltaTime,0);*/
        if (transform.position.y<-10)
        {
            transform.position = Vector3.zero;
        }
	}
    void FixedUpdate()
    {
        groundCollisions = Physics.OverlapSphere(groundCheck.position, groundCheckRadius, groundLayer);
        if (groundCollisions.Length > 0)
            grounded = true;
        else
            grounded = false;
    }
}
