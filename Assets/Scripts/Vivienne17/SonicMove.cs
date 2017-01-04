using UnityEngine;
using System.IO;
using System.Text;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class SonicMove : MonoBehaviour {
    private float time;
    private float addHPStart;

    public Animator anim;
    public Rigidbody rigid;
    public GameObject ball;
    public GameObject shield;
    public GameObject Sonic;

    public Image skill_1;
    public Image skill_2;
    public Image skill_3;
    private int skillNum = 0;//record the number of the skills

    private int field_times = 2;//the shield can be use st most three times

    public float max_Health = 200f;
    public float cur_Health = 0f;
    public GameObject healthBar;
    public bool Died = false;
    public static SonicMove Instance;

    private bool SonicRun = false;

    private float waitingTime;//the time between changing screen
    private float arrivalTime;
    private bool endOrNot = false;

    public ParticleSystem shingshing;
    public GameObject shing;

    //Audio
    public AudioSource jump_sound;
    public AudioClip auGetHeart;
    public AudioClip auTheCone;

    public AudioClip auPlayingGame;

    static int tripState = Animator.StringToHash("tripping");

    // Use this for initialization
    void Start () {
        Physics.gravity = new Vector3(0, -9.8F, 0);//set the gravity

        Instance = this;
        anim = GetComponent<Animator>();
        max_Health = 200f;
        cur_Health = max_Health;
        shingshing.Stop();
        skill_1.enabled = false;
        skill_2.enabled = false;
        skill_3.enabled = false;
        rigid = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        if (GameFunction.Instance.isPlaying == true)
        {
            AudioSource audio = GetComponent<AudioSource>();
            if (!audio.isPlaying)
            {
                audio.PlayOneShot(auPlayingGame, 0.25f);
            }

            if (SonicRun == false)
            {
                anim.SetTrigger("isPlaying");
                SonicRun = true;
                skill_1.enabled = true;
                skill_2.enabled = true;
                skill_3.enabled = true;
            }
            // anim.SetTrigger("isPlaying");
            //decrease HP, run, jump, throw balls
            if (Died == false)//determine if the game is over
            {
                //record the time
                time += Time.deltaTime;
                //pause the particle
                if (Time.time - addHPStart > 2)
                    shingshing.Stop();

                //the time that should change the screen
                if(Time.time - arrivalTime > waitingTime && endOrNot == true)
                    UnityEngine.SceneManagement.SceneManager.LoadScene("ViviLevel2");
                //decrease the health bar once a second
                if (time > 1f && time < 2f)
                {
                    decreasehealth();
                    time = 0;//reset the time
                }
                //if he drop, he'll die
                if (transform.position.y <= 0)
                {
                    Died = true;
                    anim.SetTrigger("isIdling");
                    GameFunction.Instance.OverGame();
                }
                if (Input.GetKeyDown(KeyCode.B) && transform.position.y < 1.2)
                {
                    anim.SetTrigger("isJumping");
                    rigid.AddForce(transform.up * 50000.0f);
                }
                else if (Input.GetKeyDown(KeyCode.Space) && transform.position.y < 1.2)
                {
                    anim.SetTrigger("isJumping");
                    rigid.AddForce(transform.up * 35000.0f);
                    jump_sound.Play();
                }
                else if (anim.GetCurrentAnimatorStateInfo(0).fullPathHash != tripState) {
                        transform.position += 7.0f * new Vector3(1, 0, 0) * Time.deltaTime;
                }
  /*              if (Input.GetKeyDown(KeyCode.V))
                {
                    GameObject newball = Instantiate(ball);
                    Destroy(newball, 4);
                }*/
                if (Input.GetKeyDown(KeyCode.C) && field_times>0)
                {
                    field_times = field_times - 1;
                    GameObject newShield = Instantiate(shield);
                    Destroy(newShield, 4);
                }
            }
            else {
                if (audio.isPlaying)
                {
                    audio.Stop();
                }
            }
        }

    }

    void OnTriggerEnter(Collider collision) {
        if (collision.gameObject.name == "Heart")
        {
            AudioSource audio = GetComponent<AudioSource>();
            audio.PlayOneShot(auGetHeart);

            addHPStart = Time.time;

            if (!shingshing.isPlaying)
                shingshing.Play();

            if (cur_Health + 60f >= max_Health)
            {
                cur_Health = max_Health;
            }
            else {
                cur_Health += 60f;
            }
        }

        if (collision.gameObject.tag == "Cone")
        {
            AudioSource audio = GetComponent<AudioSource>();
            audio.PlayOneShot(auTheCone);
            anim.SetTrigger("isTripping");

            if (cur_Health <= 10f)
            {
                cur_Health = 0.0f;
            }
            else {
                cur_Health -= 10f;
            }
        }

        if (collision.gameObject.tag == "Coin")
        {
            GameFunction.Instance.AddScore(20);
        }

        if (collision.gameObject.tag == "Star")
        {
            AudioSource audio = GetComponent<AudioSource>();
            audio.PlayOneShot(auGetHeart);
            Destroy(collision.gameObject);
            if (skillNum == 0)
            {
                skillNum = 1;
                Color temp = skill_1.color;
                temp.a = 1f;
                skill_1.color = temp;
            }
            else if (skillNum == 1)
            {
                skillNum = 2;
                Color temp = skill_2.color;
                temp.a = 1f;
                skill_2.color = temp;
            }
            else if (skillNum == 2) {
                skillNum = 3;
                Color temp = skill_3.color;
                temp.a = 1f;
                skill_3.color = temp;
            }
            
        }
        if (collision.gameObject.name == "Entrance")
        {
            FileStream fs = new FileStream(Application.dataPath + "/Scripts/Vivienne17/scorefile.txt", FileMode.Create);
            StreamWriter theWriter = new StreamWriter(fs);

            theWriter.WriteLine(GameFunction.Instance.Score);
            theWriter.Close();

            FileStream fs1 = new FileStream(Application.dataPath + "/Scripts/Vivienne17/skillfile.txt", FileMode.Create);
            StreamWriter theWriter1 = new StreamWriter(fs1);

            theWriter1.WriteLine(skillNum);
            theWriter1.Close();

            endOrNot = true;
            arrivalTime = Time.time;
            waitingTime = GameObject.Find("Control_Sound").GetComponent<Fading>().BeginFade(1);
        }
    }

    void decreasehealth() {
        cur_Health -= 5f;
        if (cur_Health <= 0) {
            anim.SetTrigger("isIdling");
            Died = true;
            GameFunction.Instance.OverGame();
        }
        float calc_Health = cur_Health / max_Health;
        SetHealthBar(calc_Health);
    }

    public void SetHealthBar(float myHealth) {
        healthBar.transform.localScale = new Vector3(myHealth, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
    }
}
