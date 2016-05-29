using UnityEngine;
using System.Collections;

public static class GameConstants
{
    public enum SonicState
    {
        NORMAL,
        DEAD,
        JUMPING,
        SQUATTING,
        ROLLING
    }
}

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class Sonic : MonoBehaviour {

    //in the Sonic
    Animator animator;
    AnimatorStateInfo first_info;
    BoxCollider mcollider;
    SkinnedMeshRenderer skin1, skin2;
    Rigidbody rb;
    Vector3 original_collidersize, original_collidercenter, squatting_collidersize = new Vector3(1f, 2.243595f, 1.886265f), squatting_collidercenter = new Vector3(0f, 1.075213f, -0.3038063f), shieldposition = new Vector3(-3f, 2f, 0f), original_position, faceright, faceleft, dead;
    GameObject rollingball;
    WaitForSeconds delay = new WaitForSeconds(1.7f), jumpdelay = new WaitForSeconds(.1f);
    GameConstants.SonicState sonicstate;

    [SerializeField]
    float speed, turnspeed, jumpforce;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        mcollider = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        skin1 = transform.FindChild("Cube/Cube_MeshPart0").GetComponent<SkinnedMeshRenderer>();
        skin2 = transform.FindChild("Cube/Cube_MeshPart1").GetComponent<SkinnedMeshRenderer>();
        original_collidersize = mcollider.size;
        original_collidercenter = mcollider.center;
        original_position = transform.localPosition;
        sonicstate = GameConstants.SonicState.NORMAL;
        rollingball = GameObject.Find("RollingBall");
        //rollingball.SetActive(false);

        faceright = new Vector3(0f, 90f, 0f);
        faceleft = new Vector3(0f, 270f, 0f);
        dead = new Vector3(0.01f, 0.01f, 0.01f);

        //faceleft = new Quaternion(0f, Quaternion.Angle, 0f, 0f);
        //faceright = new Quaternion(0f, 180f, 0f, 0f);
    }

    IEnumerator timer()
    {
        //two seconds delay, and then speed up to 3f
        yield return delay;
        animator.speed = 3f;
        speed = 3f;
        mcollider.size = original_collidersize;
        yield break;
    }

    IEnumerator revive()
    {
        transform.localScale = dead;
        //two seconds delay
        yield return delay;
        Debug.Log("Revive");
        sonicstate = GameConstants.SonicState.NORMAL;
        transform.localScale = Vector3.one;
        yield break;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.y > 1f)
        {
            animator.SetBool("Jump", false);
            sonicstate = GameConstants.SonicState.NORMAL;
        }

        switch (collision.transform.name)
        {
            case "Wall":
                skin1.material.color = Color.red;
                skin2.material.color = Color.red;

                break;
            case "Spring":
                if (collision.relativeVelocity.y > 1f)
                {
                    sonicstate = GameConstants.SonicState.JUMPING;
                    animator.SetBool("Jump", true);
                    rb.AddForce(transform.up * 600f);
                }

                break;
            case "Enemy1":
                if (collision.relativeVelocity.y > 1f)
                    Destroy(collision.gameObject);
                else
                    GameOver();

                break;
        }
    }

    public void SuperMode()
    {
        //to slow everything
        skin1.material.color = Color.yellow;
        skin2.material.color = Color.yellow;
        animator.speed = .1f;
        speed = .1f;
    }

    public void NormalMode()
    {
        animator.speed = 2f;
    }

    public void GameOver()
    {
        transform.localPosition = original_position;
        sonicstate = GameConstants.SonicState.DEAD;
        StartCoroutine("revive");
        Debug.Log("GameOver");
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localPosition.y < -10.0f)
            GameOver();

        //first_info = animator.GetCurrentAnimatorStateInfo(1);

        if (Input.GetKeyDown(KeyCode.UpArrow) && sonicstate == GameConstants.SonicState.NORMAL)
        {
            //Jump();

            //isJump = true;
            sonicstate = GameConstants.SonicState.JUMPING;
            animator.SetBool("Jump", true);
            rb.AddForce(transform.up * jumpforce);
            
            //transform.localPosition += transform.up * jumpforce * speed * Time.deltaTime; ;
            //StartCoroutine("jumping");
        }

        //to move
        //going forward action 
        if (Input.GetAxis("Horizontal") > 0f)
        {
            //to turn right
            transform.localRotation = Quaternion.Euler(faceright);
            animator.SetInteger("Mode", 1);
        }

        if (Input.GetAxis("Horizontal") < 0f)
        {
            //to turn left
            transform.localRotation = Quaternion.Euler(faceleft);
            animator.SetInteger("Mode", 1);
        }

        //not 0 => forward
        animator.SetInteger("Mode", Input.GetAxis("Horizontal") != 0f ? 1 : 0);

        //for 3D
        //transform.Rotate(0, Input.GetAxis("Horizontal") * turnspeed * Time.deltaTime, 0);
        transform.localPosition += Vector3.right * Input.GetAxis("Horizontal") * speed * Time.deltaTime; ;


        if (Input.GetKey(KeyCode.DownArrow) && (sonicstate == GameConstants.SonicState.NORMAL || sonicstate == GameConstants.SonicState.SQUATTING))
        {
            sonicstate = GameConstants.SonicState.SQUATTING;
            mcollider.size = squatting_collidersize;
            mcollider.center = squatting_collidercenter;
            animator.SetInteger("Mode", 2);
            Debug.Log("down");
        }

        if(Input.GetKeyDown(KeyCode.Return) && sonicstate == GameConstants.SonicState.SQUATTING)
        {
            gameObject.SetActive(false);
            rollingball.SetActive(true);

            rollingball.transform.localPosition = transform.localPosition + Vector3.up * .7f;

            rollingball.SendMessage("QuickRolling", true);
        }

        /*
        //to create a shield
        if (Input.GetKeyDown(KeyCode.D))
        {
            shield = (GameObject)Instantiate(Resources.Load("Shield"), transform.position + shieldposition, Quaternion.identity);
            //shield.transform.parent = transform;

            Destroy(shield, 3f);
        }
        */

        /*
        //to slide
        if (Input.GetKey(KeyCode.Space))
        {
            //sliding action
            animator.speed = 1.5f;
            mcollider.size = new Vector3(.5f, .5f, .5f);
            animator.SetBool("Space", true);
            StartCoroutine("timer");
        }
        else
        {
            animator.SetBool("Space", false);
        }
        */

    }
}
