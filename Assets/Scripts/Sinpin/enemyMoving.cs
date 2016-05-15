using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Rigidbody))]
public class enemyMoving : MonoBehaviour {

    Transform targetPosition;
    Rigidbody RB;
    private float jumpDistance = 10.0f;
    private bool forward = true;

    public LayerMask groundLayer;
    public Transform groundCheck;
    Collider[] groundCollisions;
    float groundCheckRadius = 0.2f;
    bool grounded = false;
    // Use this for initialization
    void Start () {
        targetPosition = GameObject.Find("Sonic").transform;
        RB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!ChasingPlayer())
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
        else
        {   
            Vector3 playerDir = targetPosition.position - transform.position;
            if(playerDir.x < 0)
            {
                transform.rotation = Quaternion.AngleAxis(-90, Vector3.up);
            }
            else
            {
                transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
            }
            if(grounded)
                RB.AddForce(transform.up * 20 + new Vector3 (playerDir.x / Mathf.Abs(playerDir.x), 0, 0) * jumpDistance);
        }
    }
    void FixedUpdate()
    {
        groundCollisions = Physics.OverlapSphere(groundCheck.position, groundCheckRadius, groundLayer);
        if (groundCollisions.Length > 0)
        {
            if (forward == false && !grounded && !ChasingPlayer())
            {
                transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
                forward = true;
            }
            else if (forward == true && !grounded && !ChasingPlayer())
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
    private bool ChasingPlayer()
    {
        float distance;
        distance = Vector3.Distance(transform.position, targetPosition.position) - 1.25f; //because center of enemy is at (x, 1.25, z)
        if (distance < 8)
            return true;
        else
            return false;
    }
}
