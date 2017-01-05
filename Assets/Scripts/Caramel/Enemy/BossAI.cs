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
        BlockLeft.SetActive(true);
        BlockRight.SetActive(true);
        BlockUp.SetActive(true);
    }

    void OnCollisionEnter(Collision collision)
    {
        Transform ct = collision.transform;

        switch (ct.tag)
        {
            case "Sonic":
                ct.GetComponent<Sonic>().GetHurt(collision.relativeVelocity);
                break;
            case "RollingBall":
                SoundManager.instance.PlaySoundEffectSource(GameConstants.AttackSoundEffect);
                if (--health >= 7 && bs != BossState.Normal)
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

            case "BlockUp":
                {
                    switch(bam)
                    {
                        case BossAttacMode.Far:
                            SetDirection(Vector3.down);
                            break;
                        case BossAttacMode.Near:
                            //StartCoroutine("NearAttack");
                            SetDirection(Vector3.down);
                            break;
                    }
                }
                break;

            case "Ground":
                {
                    switch (bam)
                    {
                        case BossAttacMode.Far:
                            SetDirection(Vector3.up);
                            break;
                        case BossAttacMode.Near:
                            SetDirection(Vector3.up);
                            //SetDirection(new Vector3(GetDirection().x, -GetDirection().y));
                            break;
                    }
                }
                break;

            case "BlockLeft":
                break;
            case "BlockRight":
                break;
        }
    }

    void Endding()
    {
        BlockLeft.SetActive(false);
        BlockRight.SetActive(false);
        BlockUp.SetActive(false);
        Camera.isMoving = true;
    }

    void Turn()
    {
        shootDirectioin = -shootDirectioin;
        transform.localRotation = Quaternion.Euler(-transform.localRotation.eulerAngles);
    }

    IEnumerator Shoot()
    {
        while(true)
        {
            Transform newBullet = Instantiate(Bullet);

            newBullet.parent = transform;
            newBullet.localScale = Bullet.localScale;
            newBullet.localPosition = bulletInitialPosition;
            newBullet.GetComponent<Rigidbody>().velocity = ShootSpeed * shootDirectioin;
            Destroy(newBullet.gameObject, BulletDisappearTime);

            yield return shootInterval;
        }
    }

    IEnumerator NearAttack()
    {
        Vector3 sonicPosition = sonic.position;
        yield return new WaitForSeconds(1f);

        SetDirection((sonicPosition - transform.position));
    }


    void Update ()
    {
	    if(active)
        {
            float d = sonic.position.x - transform.position.x;
            switch (bs)
            {
                case BossState.Normal:
                    SetIsFaceRight(d > 0f);
                    SetBam(Mathf.Abs(d) >= 10f ? BossAttacMode.Far : BossAttacMode.Near);
                    break;

                case BossState.Anger:
                    SetIsFaceRight(d > 0f);
                    SetBam(Mathf.Abs(d) >= 10f ? BossAttacMode.Far : BossAttacMode.Near);
                    break;

                case BossState.Fury:
                    SetIsFaceRight(d > 0f);
                    SetBam(Mathf.Abs(d) >= 10f ? BossAttacMode.Far : BossAttacMode.Near);
                    break;
                default:
#if UNITY_EDITOR
                    Debug.Log("Unexpected");
#endif
                    break;
            }
        }
	}

    public CameraMoving Camera;
    public GameObject BlockLeft, BlockRight, BlockUp;
    public Transform Bullet;
    public float MovingSpeed;
    
    public float ShootSpeed, BulletDisappearTime, ShootInterval;

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

                        case BossAttacMode.Near:
                            SetDirection(Vector3.up);
                            //StartCoroutine("NearAttack");
                            break;
                    }
                    break;
            }
        }
    }

    bool GetIsFaceRight() { return isFaceRight; }
    void SetIsFaceRight(bool b)
    {
        bool old = isFaceRight;
        if(old != b)
        {
            //on changed
            isFaceRight = b;
            Turn();
        }
    }

    BoxCollider bc;
    Rigidbody rb;
    Transform sonic, rollingBall;
    WaitForSeconds shootInterval;
    Vector3 direction = Vector3.zero;
    Vector3 shootDirectioin = Vector3.left, bulletInitialPosition = new Vector3(0f, .1f, .1f);
    BossState bs = BossState.Normal;
    BossAttacMode bam = BossAttacMode.Near;
    int health = 5;
    bool active = false, isFaceRight = false;
}
