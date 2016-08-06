using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]
public class Sonic : MonoBehaviour
{
    //in the Sonic
    public Vector3 ontopccenter = new Vector3(0f, .8f, 0f);
    public float ontopcheight = 2f, ontopcradius = 3f, upforce;
    //public GameObject sonicontop;

    Animator animator;
    CapsuleCollider ccollider;
    SkinnedMeshRenderer skin1, skin2;
    Rigidbody rb;
    Vector3 occenter, sccenter = new Vector3(0f, 1.075213f, -0.3038063f), original_position, dead;
    GameObject rollingball;
    WaitForSeconds delay = new WaitForSeconds(1.7f);
    Color ocskin1, ocskin2, hcskin1, hcskin2;
    float ocheight, ocradius;
    int ocdirection = 1, ontopcdirection = 2;
    bool hurting = false;

    //squatting_collidersize = new Vector3(1f, 2.243595f, 1.886265f)
    public float walkspeed, scheight;
    public UILabel time, coins;

    enum BallMode
    {
        JUMPING,
        TOROLL
    }

    // Use this for initialization
    void Awake()
    {
        animator = GetComponent<Animator>();
        ccollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        skin1 = transform.FindChild("Cube/Cube_MeshPart0").GetComponent<SkinnedMeshRenderer>();
        skin2 = transform.FindChild("Cube/Cube_MeshPart1").GetComponent<SkinnedMeshRenderer>();
        ocheight = ccollider.height;
        occenter = ccollider.center;
        ocradius = ccollider.radius;
        original_position = transform.localPosition;
        Game.sonicstate = GameConstants.SonicState.NORMAL;

        rollingball = GameObject.Find("RollingBall");
        rollingball.SetActive(false);

        //faceright = new Vector3(0f, 90f, 0f);
        //faceleft = new Vector3(0f, 270f, 0f);
        dead = new Vector3(0.01f, 0.01f, 0.01f);

        ocskin1 = skin1.material.color;
        ocskin2 = skin2.material.color;
        hcskin1 = Color.red;
        hcskin2 = Color.red;
        

        //faceleft = new Quaternion(0f, Quaternion.Angle, 0f, 0f);
        //faceright = new Quaternion(0f, 180f, 0f, 0f);
    }

    void ChangeToBall(GameConstants.SonicState s)
    {
        rollingball.SetActive(true);
        gameObject.SetActive(false);

        if(ccollider.height != ocheight)
        {
            ccollider.center = occenter;
            ccollider.height = ocheight;
            ccollider.radius = ocradius;
            ccollider.direction = ocdirection;
        }

        Game.velocity = rb.velocity;
        rb.velocity = Vector3.zero;
        rollingball.transform.localPosition = transform.localPosition + Vector3.up * .5f;

        switch (s)
        {
            case GameConstants.SonicState.JUMPING:
                rollingball.SendMessage("BackToBall", GameConstants.SonicState.JUMPING);
                break;
            case GameConstants.SonicState.TOROLL:
                Game.sonicstate = GameConstants.SonicState.TOROLL;
                rollingball.SendMessage("BackToBall", GameConstants.SonicState.TOROLL);
                break;
        }
    }

    public void BackToSonic(GameConstants.SonicState s)
    {
        rb.velocity = Game.velocity;
        ccollider.height = ocheight;
        ccollider.center = occenter;
        ccollider.radius = ocradius;

        if(hurting)
        {
            skin1.material.color = ocskin1;
            skin2.material.color = ocskin2;
            hurting = false;
        }

        switch(s)
        {
            case GameConstants.SonicState.NORMAL:
                break;
            case GameConstants.SonicState.DEAD:
                GameOver();
                break;
        }
    }


    IEnumerator revive()
    {
        transform.localScale = dead;
        //two seconds delay
        yield return delay;
        Debug.Log("Revive");
        Game.sonicstate = GameConstants.SonicState.NORMAL;
        transform.localScale = Vector3.one;
        ccollider.height = ocheight;
        ccollider.center = occenter;
        yield break;
    }

    IEnumerator hurt()
    {
        skin1.material.color = hcskin1;
        skin2.material.color = hcskin2;
        yield return delay;

        skin1.material.color = ocskin1;
        skin2.material.color = ocskin2;
        hurting = false;
        yield break;
    }

    void OnCollisionEnter(Collision collision)
    {
        /*
        if (collision.relativeVelocity.y > 1f)
        {
            //animator.SetBool("Jump", false);
            GameConstants.sonicstate = GameConstants.SonicState.NORMAL;
        }
        */
        switch (collision.transform.tag)
        {
            /*
            case "Wall":
                skin1.material.color = Color.red;
                skin2.material.color = Color.red;

                break;
            */
            
            case "Spring_X":

                /*
                GameConstants.sonicstate = GameConstants.SonicState.JUMPING;
                animator.SetBool("Jump", true);
                rb.AddForce(transform.up * 600f);
                */

                float s = Mathf.Sign(collision.relativeVelocity.x);
                rb.AddForce(Vector3.right * 30f * s);
                transform.localRotation = s > 0f ? Quaternion.Euler(Vector3.up * 90f) : Quaternion.Euler(Vector3.up * 270f);

                break;
                
            case "Coin":

                //if(!hurting)
                //{
                Destroy(collision.gameObject);
                Game.coins += 1;
                coins.text = Game.coins.ToString();
                //}

                break;
                
                /*
            case "Spring (2)":
                if (collision.relativeVelocity.x < 0f)
                {
                    rb.AddForce(Vector3.right * 500f);
                }

                break;
                */

            case "Enemy":
                if(!hurting)
                {
                    if (Game.coins != 0)
                    {
                        hurting = true;

                        Coin coin = ((GameObject)Instantiate(Resources.Load("Caramel/Components/Coin"), transform.localPosition + Vector3.up * 10f, Quaternion.identity)).GetComponent<Coin>();
                        Vector3 v = new Vector3(Random.Range(-3f, 3f), Random.Range(4f, 8f), 0f);
                        coin.Throw(v * 150f);
                        --Game.coins;

                        while (Game.coins > 0)
                        {
                            coin = ((GameObject)Instantiate(coin.gameObject, transform.localPosition + Vector3.up * 10f, Quaternion.identity)).GetComponent<Coin>();
                            v = new Vector3(Random.Range(-1f, 1f), Random.Range(4f, 8f), 0f);
                            coin.Throw(v * 150f);
                            --Game.coins;
                        }

                        rb.AddForce((collision.relativeVelocity.x > 0f ? Vector3.right : Vector3.left) * 700f);
                        coins.text = Game.coins.ToString();

                        StartCoroutine("hurt");
                    }
                    else
                    {
                        GameOver();
                    }
                }

                break;
        }
    }

    public void Ontop()
    {
        Debug.Log("sonic top");
        ccollider.center = ontopccenter;
        ccollider.height = ontopcheight;
        ccollider.radius = ontopcradius;
        ccollider.direction = ontopcdirection;
    }

    public void Normal()
    {
        ccollider.height = ocheight;
        ccollider.center = occenter;
        ccollider.radius = ocradius;
        ccollider.direction = ocdirection;
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.transform.tag == "Coin")
        {
            ++Game.coins;
            coins.text = Game.coins.ToString();
            Destroy(c.gameObject);
        }
    }

    void GameOver()
    {
        transform.localPosition = original_position;
        rb.velocity = Vector3.zero;
        Game.sonicstate = GameConstants.SonicState.DEAD;
        StartCoroutine("revive");
        Debug.Log("GameOver");
    }
    

    /*
IEnumerator timer()
{
    //two seconds delay, and then speed up to 3f
    yield return delay;
    animator.speed = 3f;
    speed = 3f;
    mcollider.size = original_collidersize;
    yield break;
}
*/

    /*
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
*/

    // Update is called once per frame
    void Update()
    {
        if (transform.localPosition.y < -10.0f)
            GameOver();

        if(Game.sonicstate != GameConstants.SonicState.DEAD)
        {
            Game.time += Time.deltaTime;
            time.text = ((int)Game.time / 60).ToString() + " : " + ((int)Game.time % 60).ToString();

            //first_info = animator.GetCurrentAnimatorStateInfo(1);

            //to move
            //going forward action 
            if (Input.GetAxis("Horizontal") > 0f)
            {
                //to turn right
                transform.localRotation = Quaternion.Euler(Vector3.up * 90f);
                //animator.SetInteger("Mode", 1);
                //there is a bug that Sonic sometimes cannot walk!
                rb.AddForce(transform.forward * walkspeed);
            }

            if (Input.GetAxis("Horizontal") < 0f)
            {
                //to turn left
                transform.localRotation = Quaternion.Euler(Vector3.up * 270f);
                //animator.SetInteger("Mode", 1);
                rb.AddForce(transform.forward * walkspeed);
            }

            //not 0 => forward, reset to 0
            animator.SetInteger("Mode", rb.velocity != Vector3.zero ? 1 : 0);

            //for 3D
            //transform.Rotate(0, Input.GetAxis("Horizontal") * turnspeed * Time.deltaTime, 0);
            //transform.localPosition += Vector3.right * Input.GetAxis("Horizontal") * speed * 

            //to squat
            if (Input.GetKey(KeyCode.DownArrow) && (Game.sonicstate == GameConstants.SonicState.NORMAL || Game.sonicstate == GameConstants.SonicState.SQUATTING))
            {
                Game.sonicstate = GameConstants.SonicState.SQUATTING;
                ccollider.height = scheight;
                ccollider.center = sccenter;
                animator.SetInteger("Mode", 2);
            }
            //from squatting to normal
            if (Input.GetKeyUp(KeyCode.DownArrow) && Game.sonicstate == GameConstants.SonicState.SQUATTING)
            {
                Game.sonicstate = GameConstants.SonicState.NORMAL;
                ccollider.height = ocheight;
                ccollider.center = occenter;
            }


            if (Input.GetKeyDown(KeyCode.Return) && (Game.sonicstate == GameConstants.SonicState.SQUATTING || Game.sonicstate == GameConstants.SonicState.TOROLL))
            {
                ChangeToBall(GameConstants.SonicState.TOROLL);

                /*
                GameConstants.sonicstate = GameConstants.SonicState.ROLLING;
                gameObject.SetActive(false);
                rollingball.SetActive(true);

                rollingball.transform.localPosition = transform.localPosition + Vector3.up * .7f;
                rollingball.SendMessage("QuickRolling", (transform.localRotation.eulerAngles.y - 91f) < 0 ? 1 : 0);
                */
            }

            //to jump
            if (Input.GetKeyDown(KeyCode.Return) && Game.sonicstate == GameConstants.SonicState.NORMAL)
            {
                //GameConstants.sonicstate = GameConstants.SonicState.JUMPING;
                //animator.SetBool("Jump", true);
                //rb.AddForce(transform.up * jumpforce);
                ChangeToBall(GameConstants.SonicState.JUMPING);

                //rollingball

                //transform.localPosition += transform.up * jumpforce * speed * Time.deltaTime; ;
                //StartCoroutine("jumping");
            }
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

        /*
if (Input.GetKeyDown(KeyCode.UpArrow) && GameConstants.sonicstate == GameConstants.SonicState.NORMAL)
{
    GameConstants.sonicstate = GameConstants.SonicState.JUMPING;
    animator.SetBool("Jump", true);
    rb.AddForce(transform.up * jumpforce);
    //rollingball.SetActive(true);
    //gameObject.SetActive(false);

    //rollingball

    //transform.localPosition += transform.up * jumpforce * speed * Time.deltaTime; ;
    //StartCoroutine("jumping");
}
*/

    }
}
