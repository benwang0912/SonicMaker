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
    public static float UDVelocity = 50f, RLVelocity = 50f;
    
    void Awake()
    {
        an = GetComponent<Animator>();
    }
    
    void Jumptime()
    {
        an.SetBool("Jump", false);
    }
    
    void OnCollisionEnter(Collision collision)
    {
        bool active = false;
        
        switch (sm)
        {
            case SpringMode.Up:
                if (active = collision.contacts[0].point.y - transform.position.y > surface)
                {
                    collision.rigidbody.velocity = UDVelocity * Vector3.up;
                    Debug.Log("up");
                }

                break;

            case SpringMode.Right:
                if(active = collision.contacts[0].point.x - transform.position.x > surface)
                {
                    collision.rigidbody.velocity = RLVelocity * Vector3.right;
                    Debug.Log("right");
                }

                break;

            case SpringMode.Down:
                if (active = collision.contacts[0].point.y - transform.position.y < surface)
                {
                    collision.rigidbody.velocity = UDVelocity * Vector3.down;
                    Debug.Log("down");
                }
                break;

            case SpringMode.Left:
                if (active = collision.contacts[0].point.x - transform.position.x < surface)
                {
                    collision.rigidbody.velocity = RLVelocity * Vector3.left;
                    Debug.Log("left");
                }

                break;

            default:
                return;
        }

        if(active)
        {
            //spring animation
            SoundManager.instance.PlaySoundEffectSource(GameConstants.SpringSoundEffect);
            an.SetBool("Jump", true);
            Invoke("Jumptime", .1f);
        }
    }

    Animator an;
    float surface = 2.5f;
}
