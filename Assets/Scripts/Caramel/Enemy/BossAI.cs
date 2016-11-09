using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class BossAI : MonoBehaviour
{
    void Awake()
    {
        bc = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;

        shootInterval = new WaitForSeconds(ShootInterval);
    }
    
	void Start ()
    {
        sonic = Game.sonic;
        rollingBall = Game.rollingball;
	}

    public void StartBossFight()
    {
        active = true;
        SetDirection(Vector3.up);

        //to controll camera?
        Camera.isMoving = false;

        //to active the block wall
        Block0.SetActive(true);
        Block1.SetActive(true);
    }

    void OnCollisionEnter(Collision collision)
    {
        Transform ct = collision.transform;

        switch (ct.name)
        {
            case "Sonic":
                ct.GetComponent<Sonic>().GetHurt(collision.relativeVelocity);
                break;
            case "RollingBall":
                if(--health >= 7 && bs != BossState.Normal)
                {
                    //to change bs to normal
                    bs = BossState.Normal;
                    rb.velocity = MovingSpeed * GetDirection();
                }
                else if(health >= 4 && bs != BossState.Anger)
                {
                    //to change bs to anger
                    bs = BossState.Anger;
                    rb.velocity = MovingSpeed * GetDirection();
                }
                else if(health >= 1 && bs != BossState.Fury)
                {
                    //to change bs to fury
                    bs = BossState.Fury;
                    rb.velocity = MovingSpeed * GetDirection();
                    //to change to red?
                }
                else if(health <= 0 && bs != BossState.Death)
                {
                    //boss fight endding
                    bs = BossState.Death;
                    Endding();
                    Destroy(gameObject);
                }

                ct.GetComponent<Rolling>().JumpBack(collision.relativeVelocity);
                break;
        }
    }

    void Endding()
    {
        Block0.SetActive(false);
        Block1.SetActive(false);
        Camera.isMoving = true;
    }

    IEnumerator Shoot()
    {
        while(true)
        {
            Transform newBullet = Instantiate(Bullet);

            newBullet.parent = transform;
            newBullet.localScale = Bullet.localScale;
            newBullet.localPosition = BulletInitialPosition;
            newBullet.GetComponent<Rigidbody>().AddForce(ShootForce * ShootDirectioin);
            Destroy(newBullet.gameObject, BulletDisappearTime);

            yield return shootInterval;
        }
    }
    
    void Update ()
    {
	    if(active)
        {
            switch(bs)
            {
                case BossState.Normal:
                    if (Mathf.Abs(transform.position.x - sonic.position.x) >= 5f)
                    {
                        SetBam(BossAttacMode.Far);
                        SetDirection(transform.position.y > UpperBound ? Vector3.down : transform.position.y < LowerBound ? Vector3.up : direction);
                    }
                    else
                    {
                        SetBam(BossAttacMode.Near);
                        SetDirection(transform.position.y > UpperBound ? Vector3.down : transform.position.y < LowerBound ? Vector3.up : direction);
                    }
                    break;

                case BossState.Anger:
                    if (Mathf.Abs(transform.position.x - sonic.position.x) >= 5f)
                    {
                        SetBam(BossAttacMode.Far);
                        SetDirection(transform.position.y > UpperBound ? Vector3.down : transform.position.y < LowerBound ? Vector3.up : direction);
                    }
                    else
                    {
                        SetBam(BossAttacMode.Near);
                        SetDirection(transform.position.y > UpperBound ? Vector3.down : transform.position.y < LowerBound ? Vector3.up : direction);
                    }
                    break;

                case BossState.Fury:
                    if (Mathf.Abs(transform.position.x - sonic.position.x) >= 5f)
                    {
                        SetBam(BossAttacMode.Far);
                        SetDirection(transform.position.y > UpperBound ? Vector3.down : transform.position.y < LowerBound ? Vector3.up : direction);
                    }
                    else
                    {
                        SetBam(BossAttacMode.Near);
                        SetDirection(transform.position.y > UpperBound ? Vector3.down : transform.position.y < LowerBound ? Vector3.up : direction);
                    }
                    break;
                default:
#if UNITY_EDITOR
                    Debug.Log("Unexpected");
#endif
                    break;
            }

            if(rb.velocity.normalized != GetDirection())
            {
                rb.velocity = MovingSpeed * GetDirection();
            }
        }
	}

    public CameraMoving Camera;
    public GameObject Block0, Block1;
    public Transform Bullet;
    public float MovingSpeed, UpperBound, LowerBound;

    public Vector3 ShootDirectioin, BulletInitialPosition;
    public float ShootForce, BulletDisappearTime, ShootInterval;

    enum BossState
    {
        Normal,
        Anger,
        Fury,
        Death
    }

    enum BossAttacMode
    {
        Near,
        Far
    }

    Vector3 GetDirection() { return direction; }
    void SetDirection(Vector3 v)
    {
        Vector3 old = direction;
        if (old != v)
        {
            //on changed
            direction = v;
            rb.velocity = MovingSpeed * direction;
        }
    }

    BossAttacMode GetBam() { return bam; }
    void SetBam(BossAttacMode b)
    {
        BossAttacMode old = bam;
        if (old != b)
        {
            //on changed
            bam = b;
            StopAllCoroutines();

            switch (bs)
            {
                case BossState.Normal:
                    switch (bam)
                    {
                        case BossAttacMode.Far:
                            StartCoroutine("Shoot");
                            break;
                    }
                    break;
            }
        }
    }

    BoxCollider bc;
    Rigidbody rb;
    Transform sonic, rollingBall;
    WaitForSeconds shootInterval;
    Vector3 direction = Vector3.zero;
    BossState bs = BossState.Normal;
    BossAttacMode bam = BossAttacMode.Near;
    int health = 10;
    bool active = false;
}
