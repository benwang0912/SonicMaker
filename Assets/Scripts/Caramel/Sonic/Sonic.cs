using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]
public class Sonic : MonoBehaviour
{
    //in the Sonic

    public Vector3 ontopccenter = new Vector3(0f, .8f, 0f);
    public float ontopcheight, ontopcradius, scheight, walkspeed, hurtxv, hurtyv;
    public UILabel time, coins;
    public GameObject rollingball;
    public Action<Vector3> GetHurt;
    

    void Awake()
    {
        animator = GetComponent<Animator>();

        ccollider = GetComponent<CapsuleCollider>();
        ocheight = ccollider.height;
        occenter = ccollider.center;
        ocradius = ccollider.radius;

        rb = GetComponent<Rigidbody>();
        skin1 = transform.FindChild("Cube/Cube_MeshPart0").GetComponent<SkinnedMeshRenderer>();
        skin2 = transform.FindChild("Cube/Cube_MeshPart1").GetComponent<SkinnedMeshRenderer>();
        original_position = transform.localPosition;
        Game.sonicstate = GameConstants.SonicState.NORMAL;
        
        rollingball.SetActive(false);

        ocskin1 = skin1.material.color;
        ocskin2 = skin2.material.color;
        hcskin1 = Color.red;
        hcskin2 = Color.red;

        Game.sonic = transform;
        Game.rollingball = rollingball.transform;
        Game.coinslabel = coins;
        Game.timelabel = time;

        //GetHurt action
        GetHurt = (relativeVelocity) =>
         {
             if (!hurting)
             {
                 if (Game.coins != 0)
                 {
                     hurting = true;

                     Coin.ThrowAllCoins(transform.position);

                     rb.velocity = new Vector3(-Mathf.Sign(relativeVelocity.x) * hurtxv, hurtyv);

                     skin1.material.color = hcskin1;
                     skin2.material.color = hcskin2;
                     
                     Invoke("hurt", delay);
                 }
                 else
                 {
                     GameOver();
                 }
             }
         };
    }

    public void ChangeToBall(GameConstants.SonicState s)
    {
        rollingball.SetActive(true);
        gameObject.SetActive(false);

        //isground = false;

        if(ccollider.height != ocheight)
            Normal();

        Game.velocity = rb.velocity;
        rb.velocity = Vector3.zero;
        rollingball.transform.localPosition = transform.localPosition + Vector3.up;

        switch (s)
        {
            case GameConstants.SonicState.JUMPING:
                rollingball.SendMessage("BackToBall", GameConstants.SonicState.JUMPING);
                return;
            case GameConstants.SonicState.TOROLL:
                Game.sonicstate = GameConstants.SonicState.TOROLL;
                rollingball.SendMessage("BackToBall", GameConstants.SonicState.TOROLL);
                return;
        }
    }

    public void BackToSonic(GameConstants.SonicState s)
    {
        rb.velocity = Game.velocity;

        if (ccollider.height != ocheight)
            Normal();

        if (hurting)
        {
            skin1.material.color = ocskin1;
            skin2.material.color = ocskin2;
            hurting = false;
        }

        switch(s)
        {
            case GameConstants.SonicState.NORMAL:
                return;
            case GameConstants.SonicState.DEAD:
                GameOver();
                return;
        }
    }

    public void Jump(Vector3 v)
    {
        rb.AddForce(v);
    }

    void revive()
    {
        Game.sonicstate = GameConstants.SonicState.NORMAL;
        transform.localScale = Vector3.one;
        gameObject.SetActive(true);
        Normal();
    }

    void hurt()
    {
        skin1.material.color = ocskin1;
        skin2.material.color = ocskin2;
        hurting = false;
    }
    
    void SetMovingSpeed()
    {
        //isground
        groundray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(groundray, out groundrch))
        {
            if (groundrch.distance < .8f)
            {
                //to calculate the movingdirection
                Vector3 normal = groundrch.normal;

                float y = -(normal.x * facedirection) / normal.y;
                movingdirection = new Vector3(facedirection, y);
            }
            else
            {
                movingdirection = Vector3.zero;
            }
        }

        Debug.Log("movingdirection = " + movingdirection);
    }

    public void Ontop()
    {
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

        //this can fix that sonic will be "in ground" after being hurt on top
        //why!?
        ccollider.enabled = false;
        ccollider.enabled = true;
    }

    void GameOver()
    {
        Debug.Log("GameOver");
        transform.localPosition = original_position;
        //transform.localScale = deadsize;
        gameObject.SetActive(false);
        rb.velocity = Vector3.zero;
        Game.sonicstate = GameConstants.SonicState.DEAD;
        Invoke("revive", delay);
    }

    void Update()
    {
        //falling
        if (transform.localPosition.y < -10.0f)
            GameOver();

        //game time setting
        Game.time += Time.deltaTime;
        time.text = ((int)Game.time / 60).ToString() + " : " + ((int)Game.time % 60).ToString();
        
        //bigger than 1f => walking animation
        animator.SetInteger("Mode", rb.velocity.magnitude > 1f ? 1 : 0);
        
        switch (Game.sonicstate)
        {
            case GameConstants.SonicState.NORMAL:

                //to move
                if (Input.GetAxis("Horizontal") != 0f)
                {
                    facedirection = Mathf.Sign(Input.GetAxis("Horizontal"));
                    transform.localRotation = Quaternion.Euler((facedirection == 1f ? 90f : 270f) * Vector3.up);
                    SetMovingSpeed();
                    rb.AddForce(movingdirection * walkspeed);
                }

                //to squat
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    Game.sonicstate = GameConstants.SonicState.SQUATTING;
                    ccollider.height = scheight;
                    ccollider.center = sccenter;

                    if (rb.velocity.x < 5f && rb.velocity.x > -5f)
                    {
                        rb.velocity = Vector3.zero;
                    }

                    animator.SetInteger("Mode", 2);
                }
                else if (Input.GetKeyDown(KeyCode.Return))
                {
                    //to jump
                    ChangeToBall(GameConstants.SonicState.JUMPING);
                }

                break;

            case GameConstants.SonicState.SQUATTING:
                //to squat
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    ccollider.height = scheight;
                    ccollider.center = sccenter;

                    if (rb.velocity.x < 5f && rb.velocity.x > -5f)
                    {
                        rb.velocity = Vector3.zero;
                    }

                    animator.SetInteger("Mode", 2);
                }
                else if (Input.GetKeyUp(KeyCode.DownArrow))
                {
                    //from squatting to normal
                    Game.sonicstate = GameConstants.SonicState.NORMAL;
                    ccollider.height = ocheight;
                    ccollider.center = occenter;
                }

                //to toroll state
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    ChangeToBall(GameConstants.SonicState.TOROLL);
                }

                break;

            case GameConstants.SonicState.DEAD:
                return;
        }
    }

    Animator animator;
    CapsuleCollider ccollider;
    SkinnedMeshRenderer skin1, skin2;
    Rigidbody rb;
    Vector3 occenter, sccenter = new Vector3(0f, 1.075213f, -0.3038063f), original_position, deadsize = new Vector3(0.01f, 0.01f, 0.01f), movingdirection;
    Color ocskin1, ocskin2, hcskin1, hcskin2;
    Ray groundray;
    RaycastHit groundrch;
    float ocheight, ocradius, facedirection = 1f, delay = 1.7f;
    int ocdirection = 1, ontopcdirection = 2;
    bool hurting = false;
    
    enum BallMode
    {
        JUMPING,
        TOROLL
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


}
