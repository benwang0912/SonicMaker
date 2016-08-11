using UnityEngine;
using System.Collections;

public class SonicMove : MonoBehaviour {
    private float time;

    public Animator anim;
    public Rigidbody rigid;
    public GameObject ball;

    public float max_Health = 100f;
    public float cur_Health = 0f;
    public GameObject healthBar;
    public bool Died = false;
    public static SonicMove Instance;

    private bool SonicRun = false;
    
    //Audio
    public AudioSource jump_sound;
    public AudioClip auGetHeart;
    public AudioClip auTheCone;
    

    // Use this for initialization
    void Start () {
        Instance = this;
        anim = GetComponent<Animator>();
        cur_Health = max_Health;
        //        InvokeRepeating("decreasehealth", 1f, 1f);
       
//        InvokeRepeating("decreasehealth", 1f, 1f);
        rigid = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        if(GameFunction.Instance.isPlaying == true) {
            if (SonicRun == false) {
                anim.SetTrigger("isPlaying");
                SonicRun = true;
            }
           // anim.SetTrigger("isPlaying");
            //decrease HP, run, jump, throw balls
            if (Died == false)//determine if the game is over
            {
                //record the time
                time += Time.deltaTime;
                //decrease the health bar once a second
                if (time > 1f && time < 2f)
                {
                    decreasehealth();
                    time = 0;//reset the time
                }
                //if he drop, he'll die
                if (transform.position.y == -19) {
                    Died = true;
                }
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    anim.SetTrigger("isJumping");
                    rigid.AddForce(transform.up * 18000.0f);
                    jump_sound.Play();
                }
                else {
                    transform.position += 5.0f * new Vector3(1, 0, 0) * Time.deltaTime;
                }
                if (Input.GetKeyDown(KeyCode.C))
                {
                    GameObject newball = Instantiate(ball);
                    Destroy(newball, 2);
                }
            }
        }

    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Heart")
        {
            AudioSource audio = GetComponent<AudioSource>();
            audio.PlayOneShot(auGetHeart);
            
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
