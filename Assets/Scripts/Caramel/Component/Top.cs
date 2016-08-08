using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class Top : MonoBehaviour
{
    public Transform stage;
    public float upforce, forwardvelocity;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CapsuleCollider>();

        hurt = (v) =>
        {
            DestoryTop();
            Game.sonic.GetComponent<Sonic>().GetHurt -= hurt;
        };
    }

    void Start()
    {
        walkspeed = Game.sonic.GetComponent<Sonic>().walkspeed;
	}

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.transform.tag)
        {
            case "Sonic":
                if(!start && collision.contacts[0].point.y > transform.position.y + 1f)
                {
                    start = true;
                    rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;

                    if(stage != null)
                        Destroy(stage.gameObject);

                    if (collision.transform.name == "RollingBall")
                    {
                        collision.transform.GetComponent<Rolling>().ChangeToSonic(GameConstants.SonicState.NORMAL);
                    }

                    Game.sonic.GetComponent<Sonic>().Ontop();
                    //Game.sonic.GetComponent<Sonic>().GetHurt += hurt;
                }

                break;
                
            case "Ground":
                //start = true
                Vector3 f = new Vector3(-Mathf.Sign(collision.relativeVelocity.x), 0f) * forwardvelocity;
                rb.velocity = f;
                Game.sonic.GetComponent<Rigidbody>().velocity = f;

                break;

            case "Coin":
                if(start)
                {
                    Destroy(collision.gameObject);
                    Game.coins += 1;
                    Game.coinslabel.text = Game.coins.ToString();
                }

                break;

            default:
                break;
        }
    }

    void DestoryTop()
    {
        start = false;
        cc.isTrigger = true;
        rb.velocity = Vector3.zero;
        Game.sonic.GetComponent<Sonic>().Normal();
    }
    
    void Update()
    {
        if (transform.localPosition.y < -10.0f)
            Destroy(gameObject);
        
        if (start)
        {
            if(Vector3.Distance(transform.position, Game.sonic.position) > 5f || Game.sonicstate != GameConstants.SonicState.NORMAL)
            {
                DestoryTop();
                return;
            }

            //to move and fly
            if (Input.GetAxis("Horizontal") != 0f)
            {
                rb.AddForce(Vector3.right * walkspeed * Input.GetAxis("Horizontal") + transform.up * upforce);
            }
        }
	}

    Rigidbody rb;
    CapsuleCollider cc;
    Ray ray;
    RaycastHit rch;
    Action<Vector3> hurt;
    float walkspeed;
    bool start = false;
}
