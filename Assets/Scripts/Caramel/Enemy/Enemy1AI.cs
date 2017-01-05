using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy1AI : MonoBehaviour
{
    //in the Enemy1

    public float speed;
    
    Vector3 faceright, faceleft;
    Rigidbody rb;
    float original_x;
    bool face = true;

    // Use this for initialization
    void Awake()
    {
        original_x = transform.position.x;
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        Transform ct = collision.transform;

        switch(ct.name)
        {
            case "Sonic":
                ct.GetComponent<Sonic>().GetHurt(collision.relativeVelocity);
                break;
            case "RollingBall":
                SoundManager.instance.PlaySoundEffectSource(GameConstants.AttackSoundEffect);
                Destroy(gameObject);
                break;
        }
    }

    void patrol()
    {
        if ((transform.position.x - original_x) >= 3f)
        {
            //to left
            face = false;
            transform.localRotation = Quaternion.Euler(Vector3.up * 270f);
        }

        if ((transform.position.x - original_x) <= -3f)
        {
            //to right
            face = true;
            transform.localRotation = Quaternion.Euler(Vector3.up * 90f);
        }

        //to stay the same velocity
        rb.velocity = face ? Vector3.right * speed : Vector3.left * speed;
    }

    void attack()
    {
        if ((transform.position.x - original_x) <= 10f && (transform.position.x - original_x) >= -10f)
        {
            transform.localRotation = (transform.position.x - Game.sonic.position.x) < 0 ? Quaternion.Euler(Vector3.up * 90f) : Quaternion.Euler(Vector3.up * 270f);
            rb.velocity = (transform.position.x - Game.sonic.position.x) < 0 ? Vector3.right * speed : Vector3.left * speed;
        }
        else
        {
            patrol();
        }
    }
    
    void Update()
    {
        
        if (Vector3.Distance(Game.sonic.position, transform.position) <= 5f && Game.sonicstate != GameConstants.SonicState.DEAD)
        {
            //to attack
            attack();
        }
        else
        {
            //to patrol
            patrol();
        }
    }
}

