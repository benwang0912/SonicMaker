using UnityEngine;
using System.Collections;

public class playerStats : MonoBehaviour {
    Rigidbody rb;
    private Animator animator;
    private PlayerMoving player;
    //---------------------------------------player stats
    public float Health = 2.0f;
    public float maxHealth = 2.0f;
    private float deathCountDown = 2.0f;
    private Transform lastPosition;
    public Vector3 revivePosition;
    static public int deadTime = 0;

    BossEnergy bossEnergy;
    GameObject explosionE;

    private GameObject WallJumpInstruction;
    private GameObject EndInstruction;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        animator = gameObject.GetComponentInChildren<Animator>();
        player = GameObject.Find("Sonic").GetComponent<PlayerMoving>();
        bossEnergy = GameObject.Find("BossEnergy").GetComponent<BossEnergy>();
        explosionE = Resources.Load("Sinpin/Explosion", typeof(GameObject)) as GameObject;
        WallJumpInstruction = GameObject.Find("WallJumpInstruction");
        WallJumpInstruction.SetActive(false);
        EndInstruction = GameObject.Find("EndInstruction");
        EndInstruction.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (isDead())
        {
            player.audioSource.clip = player.playerAudio.dead;
            player.audioSource.Play();
            rb.velocity = Vector3.zero;
            ++deadTime;
            checkPointManager.load();
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
            player.audioSource.clip = player.playerAudio.addAbility;
            player.audioSource.Play();
            player.wallJumpLearned = true;
            WallJumpInstruction.SetActive(true);
            Time.timeScale = 0;
            Destroy(collision.gameObject);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "ProjectileBoss(Clone)")
        {
            Health -= 1;
            GameObject e = Instantiate(explosionE);
            e.transform.position = transform.position;
            Destroy(e, 5f);
            Destroy(other.gameObject);
        }
        if (other.name == "EndDistrict")
        {
            EndInstruction.SetActive(true);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.name == "EndDistrict")
        {
            EndInstruction.SetActive(false);
        }
    }
    void OnTriggerStay(Collider other)
    {
        if(other.name == "EndDistrict")
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                animator.SetInteger("End", 1);
            }
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
