using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Rigidbody))]
public class enemyMoving : MonoBehaviour {

    Transform targetPosition;
    Rigidbody RB;
    private float jumpDistance = 10.0f;
    private Vector3 idleJumpCenter;
    private bool forward = true;

    private bool chasePlayer = false;
    private float distance;

    public LayerMask groundLayer;
    public Transform groundCheck;
    Collider[] groundCollisions;
    float groundCheckRadius = 0.2f;
    bool grounded = false;
    // Use this for initialization
    void Start () {
        targetPosition = GameObject.Find("Sonic").transform;
        idleJumpCenter = new Vector3 (transform.position.x + transform.forward.x*jumpDistance, transform.position.y, 0);
        RB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //distance = Vector3.Distance(transform.position, targetPosition.position) - 1.25f; //because center of enemy is at (x, 1.25, z)
        Debug.Log(RB);

        if (chasePlayer == false)
        {
            if (forward == true && grounded)
            {
                
                RB.AddForce(transform.up*20+transform.forward*jumpDistance);
               
            }
            else if(forward == false && grounded)
            {
                
                RB.AddForce(transform.up * 20 + transform.forward * jumpDistance);
                
            }
        }
    }
    void FixedUpdate()
    {
        groundCollisions = Physics.OverlapSphere(groundCheck.position, groundCheckRadius, groundLayer);
        if (groundCollisions.Length > 0)
        {
            if (forward == false && !grounded && !chasePlayer)
            {
                transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
                forward = true;
            }
            else if (forward == true && !grounded && !chasePlayer)
            {
                forward = false;
                transform.rotation = Quaternion.AngleAxis(-90, Vector3.up);
            }
            grounded = true;
        }
        else {
            
            grounded = false;
        }
    }
}
