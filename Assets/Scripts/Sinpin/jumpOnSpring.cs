using UnityEngine;
using System.Collections;
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
public class jumpOnSpring : MonoBehaviour {
    private Rigidbody rb;
    private Animator animator;
    private float bounceHeight = 400.0f;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        bounceHeight = rb.mass * 400;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnCollisionEnter(Collision collision)
    {   
        if (collision.gameObject.tag == "Spring" && collision.relativeVelocity.y>0)
        {
            rb.AddForce(transform.up * bounceHeight);
            animator.SetInteger("Jump", 1);
        }

    }
    void OnCollisionExit(Collision collision)
    {
        animator.SetInteger("Jump", 0);
    }
}
