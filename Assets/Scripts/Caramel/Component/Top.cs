using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class Top : MonoBehaviour
{
    public Rigidbody rb;
    public Transform sonic, stage;
    public float upforce, forwardforce;
    public BoxCollider bc;
    public CapsuleCollider cc;
    public UILabel coins;
    
    void Awake ()
    {
        walkspeed = sonic.GetComponent<Sonic>().walkspeed;
	}

    void OnCollisionEnter(Collision collision)
    {
        Transform ct = collision.transform;

        switch(ct.tag)
        {
            case "Sonic":
                if(collision.relativeVelocity.y < -2f)
                {
                    start = true;
                    rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;

                    if (stage != null)
                        Destroy(stage.gameObject);

                    if (ct.name == "RollingBall")
                    {
                        ct.GetComponent<Rolling>().ChangeToSonic(GameConstants.SonicState.NORMAL);
                        sonic.gameObject.SendMessage("Ontop");
                    }
                }

                break;

            case "Ground":
                Vector3 f = new Vector3(-Mathf.Sign(collision.relativeVelocity.x), 0f) * forwardforce;
                rb.velocity = f;
                if (start)
                {
                    sonic.GetComponent<Rigidbody>().velocity = f;
                }

                break;

            case "Coin":
                Destroy(collision.gameObject);
                Game.coins += 1;
                coins.text = Game.coins.ToString();
                break;

            default:
                break;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        switch(collision.transform.tag)
        {
            case "Sonic":
                start = false;
                rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
                sonic.GetComponent<Sonic>().Normal();
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localPosition.y < -10.0f)
            Destroy(gameObject);

        if (start)
        {
            if(!sonic.gameObject.activeSelf)
            {
                Debug.Log("exit");
                start = false;
                rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
                return;
            }

            //to move
            //going forward action 
            if (Input.GetAxis("Horizontal") > 0f)
            {
                rb.AddForce(transform.forward * walkspeed + transform.up * upforce);
            }

            if (Input.GetAxis("Horizontal") < 0f)
            {
                rb.AddForce(transform.forward * walkspeed + transform.up * upforce);
            }
        }
	}

    bool start = false;
    float walkspeed;
}
