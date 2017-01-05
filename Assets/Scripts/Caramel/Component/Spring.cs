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
    public float UDVelocity = 30f, RLVelocity = 30f;
    
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
                }

                break;

            case SpringMode.Right:
                if(active = collision.contacts[0].point.x - transform.position.x > surface)
                {
                    collision.rigidbody.velocity = RLVelocity * Vector3.right;
                }

                break;

            case SpringMode.Down:
                if (active = collision.contacts[0].point.y - transform.position.y < surface)
                {
                    collision.rigidbody.velocity = UDVelocity * Vector3.down;
                }
                break;

            case SpringMode.Left:
                if (active = collision.contacts[0].point.x - transform.position.x < surface)
                {
                    collision.rigidbody.velocity = RLVelocity * Vector3.left;
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
