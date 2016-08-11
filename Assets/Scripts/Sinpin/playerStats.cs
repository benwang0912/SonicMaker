using UnityEngine;
using System.Collections;

public class playerStats : MonoBehaviour {
    Rigidbody rb;
    //---------------------------------------player stats
    public float Health = 2.0f;
    public float maxHealth = 2.0f;
    private float deathCountDown = 2.0f;
    private Transform lastPosition;
    public Vector3 revivePosition;
 
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        if (isDead())
        {
            rb.velocity = Vector3.zero;
            transform.position = revivePosition + new Vector3(0.0f,1.0f,0.0f);
            if(Health == 0)
            {
                Health = 1;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Health -= 1;
            rb.AddForce(new Vector3(collision.relativeVelocity.x/ Mathf.Abs(collision.relativeVelocity.x)*2, 1.5f, 0.0f)*rb.mass*100);

            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            enemyRb.AddForce(new Vector3(-collision.relativeVelocity.x / Mathf.Abs(collision.relativeVelocity.x) * 2, 1.5f, 0.0f) * enemyRb.mass * 100);
        }else if(collision.gameObject.name == "WallJumpAbility")
        {
            PlayerMoving temp = transform.GetComponent<PlayerMoving>();
            temp.wallJumpLearned = true;
            Destroy(collision.gameObject);
        }
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
