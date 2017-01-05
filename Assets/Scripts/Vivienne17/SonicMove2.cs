using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System;
using UnityEngine.UI;

public class SonicMove2 : MonoBehaviour
{
    public Animator anim;
    public Rigidbody rigid;
    public GameObject ball;
    public GameObject shield;
    public GameObject magnet;
    public GameObject Sonic;
    public GameObject Shield_1, Shield_2, Shield_3;
    public GameObject Magnet_1, Magnet_2, Magnet_3;
    public Image clear_C, clear_L, clear_E, clear_A, clear_R;
    public GameObject MissionClear;

    public float max_Health = 200f;
    public float cur_Health = 0f;
    public GameObject healthBar;
    public bool Died = false;
    public static SonicMove2 Instance;
    public int collectNum = 0;

    public ParticleSystem shingshing;
    public GameObject shing;

    //Audio
    public AudioSource jump_sound;
    public AudioClip auGetHeart, auTheCone, auPlayingGame, auOver;


    private int field_times = 2;
    private int magnet_times = 2;
    private int skillnum = 0;
    private bool first = true;

    private float time;
    private float jumpTime;
    private float addHPStart;
    private float shieldTime;
    public bool shieldOn = false;

    private float magnetTime;
    public bool magnetOn = false;
    public bool Movable = true;
    // Use this for initialization
    void Start()
    {
        Physics.gravity = new Vector3(0, -9.8F, 0);//set the gravity

        Instance = this;
        anim = GetComponent<Animator>();
        cur_Health = max_Health;
        shingshing.Stop();
        rigid = GetComponent<Rigidbody>();
        //set the skills
        string line;
        StreamReader theReader = new StreamReader(Application.dataPath + "/Scripts/Vivienne17/scorefile.txt", Encoding.Default);
        line = theReader.ReadLine();
        skillnum = Convert.ToInt32(line);
        if (skillnum > 1) {
            Magnet_1.SetActive(true);
            Magnet_2.SetActive(true);
        }
        if (skillnum > 2) {
            Shield_1.SetActive(true);
            Shield_2.SetActive(true);
        }
        transform.position += 7.0f * new Vector3(0, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y == 1 && first == true) {
            first = false;
        }
        if (first == false)
        {
            AudioSource audio = GetComponent<AudioSource>();
            if (!audio.isPlaying)
            {
                audio.PlayOneShot(auPlayingGame, 0.25f);
            }

            //decrease HP, run, jump, throw balls
            if (Died == false && collectNum < 5)//determine if the game is over
            {
                //record the time
                time += Time.deltaTime;
                //pause the particle
                if (Time.time - addHPStart > 2)
                    shingshing.Stop();
                if (Time.time - shieldTime > 4)
                {
                    shieldOn = false;
                }
                if (Time.time - magnetTime > 4)
                {
                    magnetOn = false;
                }
                //decrease the health bar once a second
                if (time > 1f && time < 2f)
                {
                    time = 0;//reset the time
                }
                //if he drop, he'll die
                if (transform.position.y <= 0)
                {
                    Died = true;
                    anim.SetTrigger("isIdling");
                    Level2Control.Instance.OverGame();
                }
                if (cur_Health <= 0)
                {
                    //when the health bar comes to zero, he'll die
                    anim.SetTrigger("isIdling");
                    Died = true;
                    Level2Control.Instance.OverGame();
                }
                //jump high
                if (Input.GetKeyDown(KeyCode.Space) && jumpTime + 1 < Time.time)
                {
                    anim.SetTrigger("isJumping");
                    rigid.AddForce(transform.up * 58000.0f);
                    jumpTime = Time.time;
                    jump_sound.Play();
                }
                //jump higher
                /*               else if (Input.GetKeyDown(KeyCode.B) && transform.position.y < 1.2)
                               {
                                   anim.SetTrigger("isJumping");
                                   rigid.AddForce(transform.up * 50000.0f);

                                   jump_sound.Play();
                               }*/
                //go right
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    if (transform.rotation.y != 90)
                    {
                        Quaternion desireRotate = Quaternion.Euler(0, 90, 0);
                        transform.rotation = desireRotate;
                    }
                    anim.SetTrigger("isPlaying");
                    transform.position += 7.0f * new Vector3(1, 0, 0) * Time.deltaTime;
                }
                //go left
                else if (Input.GetKey(KeyCode.LeftArrow) && Movable == true)
                {
                    if (transform.rotation.y != 270)
                    {
                        Quaternion desireRotate = Quaternion.Euler(0, 270, 0);
                        transform.rotation = desireRotate;
                    }
                    anim.SetTrigger("isPlaying");
                    transform.position -= 7.0f * new Vector3(1, 0, 0) * Time.deltaTime;
                }
                //idle
                else {
                    anim.SetTrigger("isIdling");
                }
                //throw balls
                if (Input.GetKeyDown(KeyCode.Q) && skillnum > 0)
                {
                    GameObject newball = Instantiate(ball);
                    Destroy(newball, 4);
                }
                //awake the shield
                if (Input.GetKeyDown(KeyCode.W) && field_times > 0 && skillnum > 2)
                {
                    if (field_times == 3)
                        Shield_3.SetActive(false);
                    else if (field_times == 2)
                        Shield_2.SetActive(false);
                    else
                        Shield_1.SetActive(false);
                    shieldTime = Time.time;
                    shieldOn = true;
                    field_times = field_times - 1;
                    GameObject newShield = Instantiate(shield);
                    Destroy(newShield, 4);
                }
                //awake the magnet ; need changed
                if (Input.GetKeyDown(KeyCode.E) && magnet_times > 0 && skillnum > 1)
                {
                    if (magnet_times == 3)
                        Magnet_3.SetActive(false);
                    else if (magnet_times == 2)
                        Magnet_2.SetActive(false);
                    else
                        Magnet_1.SetActive(false);
                    magnetTime = Time.time;
                    magnetOn = true;
                    magnet_times = magnet_times - 1;
                    GameObject newMagnet = Instantiate(magnet);
                    Destroy(newMagnet, 4);
                }
            }
            else if (collectNum == 5) {
                //     Level2Control.Instance.Finish();
                MissionClear.SetActive(true);
                anim.SetTrigger("isIdling");
                AudioSource audio1 = GetComponent<AudioSource>();
                audio1.PlayOneShot(auOver);
                if (audio.isPlaying)
                {
                    audio.Stop();
                }
            }
            else {
                AudioSource audio1 = GetComponent<AudioSource>();
                audio1.PlayOneShot(auOver);
                if (audio.isPlaying)
                {
                    audio.Stop();
                }
            }
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "Heart")
        {
            AudioSource audio = GetComponent<AudioSource>();
            audio.PlayOneShot(auGetHeart);

            addHPStart = Time.time;

            if (!shingshing.isPlaying)
                shingshing.Play();

            if (cur_Health + 20f >= max_Health)
            {
                cur_Health = max_Health;
            }
            else {
                cur_Health += 20f;
            }
        }

        if (collision.gameObject.tag == "Cone")
        {
            AudioSource audio = GetComponent<AudioSource>();
            audio.PlayOneShot(auTheCone);

            decreasehealth();
        }

        if (collision.gameObject.tag == "Coin")
        {
            Level2Control.Instance.AddScore(20);
        }
        if (collision.gameObject.name == "CLEAR_C")
        {
            collectNum += 1;
            Destroy(collision.gameObject);
            Color temp = clear_C.color;
            temp.a = 1f;
            clear_C.color = temp;
            if (collectNum == 5) {
     //           Died = true;
                anim.SetTrigger("isIdling");
                Level2Control.Instance.Finish();
            }
        }
        if (collision.gameObject.name == "CLEAR_L")
        {
            collectNum += 1;
            Destroy(collision.gameObject);
            Color temp = clear_L.color;
            temp.a = 1f;
            clear_L.color = temp;
            if (collectNum == 5)
            {
                anim.SetTrigger("isIdling");
                Level2Control.Instance.Finish();
            }
        }
        if (collision.gameObject.name == "CLEAR_E")
        {
            collectNum += 1;
            Destroy(collision.gameObject);
            Color temp = clear_E.color;
            temp.a = 1f;
            clear_E.color = temp;
            if (collectNum == 5)
            {
         //       Died = true;
                anim.SetTrigger("isIdling");
                Level2Control.Instance.Finish();
            }
        }
        if (collision.gameObject.name == "CLEAR_A")
        {
            collectNum += 1;
            Destroy(collision.gameObject);
            Color temp = clear_A.color;
            temp.a = 1f;
            clear_A.color = temp;
            if (collectNum == 5)
            {
        //        Died = true;
                anim.SetTrigger("isIdling");
                Level2Control.Instance.Finish();
            }
        }
        if (collision.gameObject.name == "CLEAR_R")
        {
            collectNum += 1;
            Destroy(collision.gameObject);
            Color temp = clear_R.color;
            temp.a = 1f;
            clear_R.color = temp;
            if (collectNum == 5)
            {
        //        Died = true;
                anim.SetTrigger("isIdling");
                Level2Control.Instance.Finish();
            }
        }

        if (collision.gameObject.name == "Wall")
        {
            rigid.velocity = Vector3.zero;
        }
    }

    void decreasehealth()
    {
        cur_Health -= 10f;
        if (cur_Health <= 0)
        {
            //when the health bar comes to zero; I need to reset the game or show "GAME OVER"; 
            //not just stand there
            anim.SetTrigger("isIdling");
            Died = true;
            Level2Control.Instance.OverGame();
        }
        float calc_Health = cur_Health / max_Health;
        SetHealthBar(calc_Health);
    }

    public void SetHealthBar(float myHealth)
    {
        healthBar.transform.localScale = new Vector3(myHealth, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
    }
}
