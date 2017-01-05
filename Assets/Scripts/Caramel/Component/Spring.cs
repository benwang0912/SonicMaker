using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Spring : MonoBehaviour
{
    //in the Spring
    public enum SpringMode
    {
        Up,
        Down,
        Left,
        Right
    }

    public SpringMode sm;
    public static float UDVelocity = 40f, RLVelocity = 40f;
    
    void Awake()
    {
        an = GetComponent<Animator>();
    }

    void Start()
    {
        rollingBall = Game.rollingball.gameObject;
        rollingBallRb = rollingBall.GetComponent<Rigidbody>();
    }
    
    void Jumptime()
    {
        an.SetBool("Jump", false);
    }
    
    void OnCollisionEnter(Collision collision)
    {
        bool active = false;
        
        if(collision.transform.tag == "Sonic" || collision.transform.tag == "RollingBall")
            switch (sm)
            {
                case SpringMode.Up:
                    if (active = collision.contacts[0].point.y - transform.position.y > surface)
                    {
                        collision.rigidbody.velocity = UDVelocity * Vector3.up;
                    }
                    else
                    {
                        Game.ComponentNotEffected();
                    }

                    break;

                case SpringMode.Right:
                    if (active = collision.contacts[0].point.x - transform.position.x > surface)
                    {
                        collision.rigidbody.velocity = RLVelocity * Vector3.right;
                    }
                    else
                    {
                        Game.ComponentNotEffected();
                    }

                    break;

                case SpringMode.Down:
                    if (active = collision.contacts[0].point.y - transform.position.y < surface)
                    {
                        collision.rigidbody.velocity = UDVelocity * Vector3.down;
                    }
                    else
                    {
                        Game.ComponentNotEffected();
                    }

                    break;

                case SpringMode.Left:
                    if (active = collision.contacts[0].point.x - transform.position.x < surface)
                    {
                        collision.rigidbody.velocity = RLVelocity * Vector3.left;
                    }
                    else
                    {
                        Game.ComponentNotEffected();
                    }

                    break;

                default:
                    return;
            }

        if (active)
        {
            //spring animation
            SoundManager.instance.PlaySoundEffectSource(GameConstants.SpringSoundEffect);
            an.SetBool("Jump", true);
            Invoke("Jumptime", .1f);
        }
    }

    GameObject rollingBall;
    Rigidbody rollingBallRb;
    
    Animator an;
    float surface = 2.5f;
}
