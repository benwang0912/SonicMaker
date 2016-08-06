using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]
public class Sonic : MonoBehaviour
{
    //in the Sonic

    public Vector3 ontopccenter = new Vector3(0f, .8f, 0f);
    public float ontopcheight, ontopcradius, scheight, walkspeed, springforce, throwcoinforce, hurtforce;
    public UILabel time, coins;
    public GameObject rollingball;


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
    }

    void ChangeToBall(GameConstants.SonicState s)
    {
        rollingball.SetActive(true);
        gameObject.SetActive(false);

        if(ccollider.height != ocheight)
            Normal();

        Game.velocity = rb.velocity;
        rb.velocity = Vector3.zero;
        rollingball.transform.localPosition = transform.localPosition + Vector3.up * .5f;

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

    void throwcoin(Coin coin)
    {
        Vector3 v = new Vector3(Random.Range(-3f, 3f), Random.Range(4f, 8f), 0f);
        coin.Throw(v * 150f);
        --Game.coins;
    }


    IEnumerator revive()
    {
        transform.localScale = deadsize;

        //two seconds delay
        yield return delay;

        Game.sonicstate = GameConstants.SonicState.NORMAL;
        transform.localScale = Vector3.one;
        Normal();
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
        switch (collision.transform.tag)
        {
            case "Spring_X":
                float s = Mathf.Sign(collision.relativeVelocity.x);
                rb.AddForce(Vector3.right * springforce * s);
                transform.localRotation = s > 0f ? Quaternion.Euler(Vector3.up * 90f) : Quaternion.Euler(Vector3.up * 270f);

                break;
                
            case "Coin":
                Destroy(collision.gameObject);
                Game.coins += 1;
                coins.text = Game.coins.ToString();

                break;

            case "Enemy":
                if(!hurting)
                {
                    if (Game.coins != 0)
                    {
                        hurting = true;

                        Coin coin = ((GameObject)Instantiate(Resources.Load("Caramel/Components/Coin"), transform.localPosition + Vector3.up * 10f, Quaternion.identity)).GetComponent<Coin>();
                        throwcoin(coin);

                        while (Game.coins > 0)
                        {
                            coin = ((GameObject)Instantiate(coin.gameObject, transform.localPosition + Vector3.up * 10f, Quaternion.identity)).GetComponent<Coin>();
                            throwcoin(coin);
                        }
                        coins.text = Game.coins.ToString();

                        rb.AddForce((collision.relativeVelocity.x > 0f ? Vector3.right : Vector3.left) * hurtforce);

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
    
    void GameOver()
    {
        transform.localPosition = original_position;
        rb.velocity = Vector3.zero;
        Game.sonicstate = GameConstants.SonicState.DEAD;
        StartCoroutine("revive");
        Debug.Log("GameOver");
    }

    void Update()
    {
        //falling
        if (transform.localPosition.y < -10.0f)
            GameOver();

        if(Game.sonicstate != GameConstants.SonicState.DEAD)
        {
            //game time setting
            Game.time += Time.deltaTime;
            time.text = ((int)Game.time / 60).ToString() + " : " + ((int)Game.time % 60).ToString();

            //to move
            //going forward action 
            if (Input.GetAxis("Horizontal") > 0f)
            {
                //to turn right
                transform.localRotation = Quaternion.Euler(Vector3.up * 90f);
                rb.AddForce(transform.forward * walkspeed);
            }

            if (Input.GetAxis("Horizontal") < 0f)
            {
                //to turn left
                transform.localRotation = Quaternion.Euler(Vector3.up * 270f);
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
            
            //to toroll state
            if (Input.GetKeyDown(KeyCode.Return) && Game.sonicstate == GameConstants.SonicState.SQUATTING)
            {
                ChangeToBall(GameConstants.SonicState.TOROLL);
            }

            //to jump
            if (Input.GetKeyDown(KeyCode.Return) && Game.sonicstate == GameConstants.SonicState.NORMAL)
            {
                ChangeToBall(GameConstants.SonicState.JUMPING);
            }
        }
    }

    Animator animator;
    CapsuleCollider ccollider;
    SkinnedMeshRenderer skin1, skin2;
    Rigidbody rb;
    Vector3 occenter, sccenter = new Vector3(0f, 1.075213f, -0.3038063f), original_position, deadsize = new Vector3(0.01f, 0.01f, 0.01f);
    WaitForSeconds delay = new WaitForSeconds(1.7f);
    Color ocskin1, ocskin2, hcskin1, hcskin2;
    float ocheight, ocradius;
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
