using UnityEngine;
using System.Collections;

public class playerStats : MonoBehaviour {
    Rigidbody rb;
    //---------------------------------------player stats
    private float Health = 10.0f;
    private float deathCountDown = 2.0f;
    private Transform lastPosition;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        if (isDead())
        {
            transform.position = Vector3.zero;
            Health = 10;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Health -= 2;
            rb.AddForce(new Vector3(collision.relativeVelocity.x/ Mathf.Abs(collision.relativeVelocity.x)*2, 1.5f, 0.0f)*rb.mass*100);
            Debug.Log(Health);
        }
    }
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy" && collision.gameObject.transform.position.x == lastPosition.position.x)
        {
            rb.AddForce(new Vector3(collision.gameObject.transform.forward.x*0.3f , 0.1f, 0.0f) * rb.mass * 100);
        }
        lastPosition = collision.gameObject.transform;
    }
    private bool isDead()
    {
        RaycastHit hit;
        if (!Physics.Raycast(transform.position + new Vector3(0, 1, 0), -transform.up, out hit))
        {
            deathCountDown -= Time.deltaTime;
        }
        else
        {
            deathCountDown = 2.0f;
        }
        if (Health <= 0)
        {
            return true;
        }
        if (deathCountDown <= 0)
            return true;
        return false;
    }
}
