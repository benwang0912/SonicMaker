using UnityEngine;
using System.Collections;

public class SonicMove : MonoBehaviour {
    private float time;
    private float addHPStart;

    public Animator anim;
    public Rigidbody rigid;
    public GameObject ball;
    public GameObject shield;
    public GameObject Sonic;

    private int field_times = 2;

    public float max_Health = 200f;
    public float cur_Health = 0f;
    public GameObject healthBar;
    public bool Died = false;
    public static SonicMove Instance;

    private bool SonicRun = false;


    public ParticleSystem shingshing;
    public GameObject shing;

    //Audio
    public AudioSource jump_sound;
    public AudioClip auGetHeart;
    public AudioClip auTheCone;

    public AudioClip auPlayingGame;

    // Use this for initialization
    void Start () {
        Instance = this;
        anim = GetComponent<Animator>();
        cur_Health = max_Health;
        shingshing.Stop();

        //        InvokeRepeating("decreasehealth", 1f, 1f);
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
                if (Input.GetKeyDown(KeyCode.Space) && transform.position.y < 1.2)
                {
                    anim.SetTrigger("isJumping");
                    rigid.AddForce(transform.up * 35000.0f);
                    jump_sound.Play();
                }
                else {
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

            if (cur_Health >= 80f)
            {
                cur_Health = 100.0f;
            }
            else {
                cur_Health += 20f;
            }
        }

        if (collision.gameObject.name == "cone1_fbx")
        {
            AudioSource audio = GetComponent<AudioSource>();
            audio.PlayOneShot(auTheCone);

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
            Destroy(collision.gameObject);
            UnityEngine.SceneManagement.SceneManager.LoadScene("ViviLevel2");
        }
    }

    void decreasehealth() {
        cur_Health -= 5f;
        if (cur_Health <= 0) {
            //when the health bar comes to zero; I need to reset the game or show "GAME OVER"; 
            //not just stand there
            anim.SetTrigger("isIdling");
//            cur_Health = max_Health;
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
