using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;
using System;

[System.Serializable]
public class PlayerAudio
{
    public AudioClip walk;
    public AudioClip jump;
    public AudioClip addLife;
    public AudioClip addMaxLife;
    public AudioClip addProjectile;
    public AudioClip addAbility;
    public AudioClip save;
    public AudioClip dead;
    public AudioClip end;
}


[RequireComponent(typeof(Rigidbody))]
public class PlayerMoving : MonoBehaviour {
    public PlayerAudio playerAudio = new PlayerAudio();
    public AudioSource audioSource;
    private Animator animator;
    public Rigidbody rb;
    public float speed =10.0f;
    //public float turn_speed = 60.0f;
    public Vector3 moveDirection = Vector3.zero;
    private float jumpHeight=40f;
    //---------------------------------------ground checking
    public LayerMask groundLayer;
    public Transform groundCheck;
    Collider[] groundCollisions;
    float groundCheckRadius = 0.2f;
    public bool grounded = false;
    //--------------------------------------wall checking
    public bool wallJumpLearned = false;
    public LayerMask wallLayer;
    public Transform wallCheck;
    Collider[] wallCollisions;
    float wallCheckRadius = 0.8f;
    private bool hitWall = false;

    private float rotateLerpTime = 0;
    private BlurOptimized blur;
    private float downSample = 0;
    static private GameObject clearSprite;
    static private UILabel clearTime;
    public float timeCounter = 0;
    static private UILabel clearKD;
    GameObject continueButton, restartButton;
    private GameObject icon;
    // Use this for initialization
    void Start () {
        audioSource = GetComponent<AudioSource>();
        loadAudioClip();
        animator = gameObject.GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        blur = Camera.main.GetComponent<BlurOptimized>();
        blur.downsample = 0;
        blur.enabled = false;

        if (clearSprite == null)
        {
            clearSprite = GameObject.Find("Clear");
            clearSprite.transform.localScale = new Vector3(0, 1, 1);
            clearSprite.SetActive(false);
            clearTime = GameObject.Find("ClearTime").GetComponent<UILabel>();
            clearTime.gameObject.SetActive(false);
            clearKD = GameObject.Find("ClearKD").GetComponent<UILabel>();
            clearKD.gameObject.SetActive(false);
            continueButton = GameObject.Find("ContinueButton");
            continueButton.SetActive(false);
            restartButton = GameObject.Find("RestartButton");
            restartButton.SetActive(false);
        }
        icon = GameObject.Find("Icon");

        jumpHeight = 65;
    }

    // Update is called once per frame
    void Update()
    {
        if (end() || Input.GetKey(KeyCode.Tab))
        {
            return;
        }

        timeCounter += Time.deltaTime;

        showAnimation();
        
        if (Input.GetAxis("Jump") > 0 && grounded && !animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            if (Time.timeScale > 0)
            {
                if (audioSource.isPlaying && audioSource.clip == playerAudio.walk)
                    audioSource.Stop();
                if (!audioSource.isPlaying && !(audioSource.clip == playerAudio.jump))
                {
                    audioSource.clip = playerAudio.jump;
                    audioSource.Play();
                }
                else if (!audioSource.isPlaying && audioSource.clip == playerAudio.jump)
                {
                    audioSource.Play();
                }
            }
            rb.AddForce(transform.up * jumpHeight * 2.5f, ForceMode.Impulse);
        }
        
        if (Input.GetKey(KeyCode.RightArrow)|| Input.GetKey(KeyCode.D))
        {   
            Quaternion desireRotate = Quaternion.Euler(0, 90, 0);
            transform.rotation = desireRotate;
            if (grounded)
            {
                if (Time.timeScale > 0)
                {
                    if (!audioSource.isPlaying && !(audioSource.clip == playerAudio.walk))
                    {
                        audioSource.clip = playerAudio.walk;
                        audioSource.Play();
                    }
                    else if (!audioSource.isPlaying && audioSource.clip == playerAudio.walk)
                    {
                        audioSource.Play();
                    }
                }
                transform.position += new Vector3(1, 0, 0) * speed * Time.deltaTime;
            }
            else {
                transform.position += new Vector3(0.6f, 0, 0) * speed * Time.deltaTime;
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            Quaternion desireRotate = Quaternion.Euler(0, -90, 0);
            transform.rotation = desireRotate;
            if (grounded)
            {
                if (Time.timeScale > 0)
                {
                    if (!audioSource.isPlaying && !(audioSource.clip == playerAudio.walk))
                    {
                        audioSource.clip = playerAudio.walk;
                        audioSource.Play();
                    }
                    else if (!audioSource.isPlaying && audioSource.clip == playerAudio.walk)
                    {
                        audioSource.Play();
                    }
                }
                transform.position += new Vector3(-1, 0, 0) * speed * Time.deltaTime;
            }
            else {
                transform.position += new Vector3(-0.6f, 0, 0) * speed * Time.deltaTime;
            }
        }

        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
            {
                animator.speed = 1.6f;
                transform.position -= new Vector3(0, 10f, 0) * Time.deltaTime;
            }
            else if(!grounded)
            {
                transform.position -= new Vector3(0, 10f, 0) * Time.deltaTime;
            }
        }
        else
        {
            animator.speed = 1;
        }

        if (wallJumpLearned)
        {
            if (hitWall && !grounded)
            {

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (Time.timeScale > 0)
                    {
                        if (audioSource.isPlaying && audioSource.clip == playerAudio.walk)
                            audioSource.Stop();
                        if (!audioSource.isPlaying && !(audioSource.clip == playerAudio.jump))
                        {
                            audioSource.clip = playerAudio.jump;
                            audioSource.Play();
                        }
                        else if (!audioSource.isPlaying && audioSource.clip == playerAudio.jump)
                        {
                            audioSource.Play();
                        }
                    }
                    animator.SetInteger("Anim", 0);
                    animator.SetInteger("wallJump", 1);
                    rb.AddForce(transform.up * 800 - transform.forward * 400, ForceMode.Impulse);

                    //--------------flip
                    Vector3 theScale = transform.localScale;
                    theScale.x *= -1;
                    transform.localScale = theScale;
                }
                else
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("WallJump"))
                        animator.SetInteger("wallJump", 0);
                }
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("WallJump"))
            {
                animator.SetInteger("wallJump", 0);
            }
        }
	}
    
    void FixedUpdate()
    {
        groundCollisions = Physics.OverlapSphere(groundCheck.position, groundCheckRadius, groundLayer);
        if (groundCollisions.Length > 0)
            grounded = true;
        else {
            grounded = false;
        }
        
        wallCollisions = Physics.OverlapSphere(wallCheck.position, wallCheckRadius, wallLayer);
        if (wallCollisions.Length > 0)
            hitWall = true;
        else
            hitWall = false;
    }

    void loadAudioClip()
    {
        playerAudio.walk = Resources.Load("Sinpin/walk", typeof(AudioClip)) as AudioClip;
        playerAudio.jump = Resources.Load("Vivienne17/CasualGameSounds/DM-CGS-07", typeof(AudioClip)) as AudioClip;
        playerAudio.addAbility = Resources.Load("Vivienne17/CasualGameSounds/DM-CGS-45", typeof(AudioClip)) as AudioClip;
        playerAudio.addLife = Resources.Load("Vivienne17/CasualGameSounds/DM-CGS-26", typeof(AudioClip)) as AudioClip;
        playerAudio.addMaxLife = Resources.Load("Vivienne17/CasualGameSounds/DM-CGS-26", typeof(AudioClip)) as AudioClip;
        playerAudio.addProjectile = Resources.Load("Vivienne17/CasualGameSounds/DM-CGS-45", typeof(AudioClip)) as AudioClip;
        playerAudio.save = Resources.Load("Vivienne17/CasualGameSounds/DM-CGS-28", typeof(AudioClip)) as AudioClip;
        playerAudio.dead = Resources.Load("Vivienne17/CasualGameSounds/DM-CGS-16", typeof(AudioClip)) as AudioClip;
        playerAudio.end = Resources.Load("Sinpin/end", typeof(AudioClip)) as AudioClip;
    }

    void showAnimation()
    {
        if((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && grounded)
        {
            animator.SetInteger("Anim", 1);
        }
        else
        {
            animator.SetInteger("Anim", 0);
        }
        if (Input.GetKey(KeyCode.Space) && grounded)
        {
            animator.SetInteger("Jump", 1);
        }else if(animator.GetCurrentAnimatorStateInfo(0).IsName("Jump") && animator.GetInteger("Jump") == 1 )
        {
            animator.SetInteger("Jump", 0);
        }
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Jump") && grounded)
        {
            animator.Play("Idle");
        }
    }

    private bool end()
    {
        if (animator.GetInteger("End") == 1)
        {
            if (transform.position.z > 24)
            {
                return true;
            }
            if (blur.enabled == false && transform.position.z > 12) //delay blur
            {
                blur.enabled = true;
            }
            if (blur.enabled == true)   //gradually blur
            {
                if (blur.blurSize < 3)
                    blur.blurSize += Time.deltaTime * 0.5f;
                if(blur.blurSize > 0.25f && clearSprite.activeSelf == false)    //show ending UISprite
                {
                    audioSource.clip = playerAudio.end;
                    audioSource.Play();

                    clearSprite.SetActive(true);
                    icon.SetActive(false);
                    
                    clearTime.gameObject.SetActive(true);
                    TimeSpan time = TimeSpan.FromSeconds(timeCounter);
                    clearTime.text = string.Format("Total time: {0:0}:{1:00}:{2:00}", time.Hours, time.Minutes, time.Seconds);

                    clearKD.gameObject.SetActive(true);
                    int kill = mouseAiming.enemyKilled;
                    int dead = playerStats.deadTime;
                    clearKD.text = string.Format("K/D : {0:0}/{1:0}", kill, dead);

                    continueButton.SetActive(true);
                    restartButton.SetActive(true);
                }
            }
            transform.position += new Vector3(0, 0, 1f) * Time.deltaTime * 5;

            float rot = Mathf.Lerp(90, 0, rotateLerpTime);
            rb.MoveRotation(Quaternion.Euler(new Vector3(0, rot, 0)));
            
            rotateLerpTime += Time.deltaTime;

            return true;
        }
        return false;
    }
}
