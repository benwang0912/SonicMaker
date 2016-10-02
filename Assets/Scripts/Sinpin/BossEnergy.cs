using UnityEngine;
using System.Collections;

public class BossEnergy : MonoBehaviour {

    public bool follow = false;
    private bool flyIn = false;
    private GameObject player;
    private Transform boss;
    float currentLerpTime = 0.0f;

    static private GameObject BossInstruction;

    [SerializeField]
    private AudioClip bossTheme;
	// Use this for initialization
	void Start () {
        player = GameObject.Find("Sonic");
        if(BossInstruction == null)
            BossInstruction = GameObject.Find("BossInstruction");
        BossInstruction.SetActive(false);
	}
	
	// Update is called once per frame
	void LateUpdate ()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * 35);
        if (follow)
        {
            currentLerpTime += Time.deltaTime*0.5f;
            transform.position = Vector3.Lerp(transform.position, player.transform.position + new Vector3(0, 4, 0), currentLerpTime);
        }
        if (flyIn)
        {
            currentLerpTime += Time.deltaTime * 0.05f;
            transform.position = Vector3.Lerp(transform.position, boss.position + new Vector3(1, 3, 0), currentLerpTime);
            if(currentLerpTime >= 0.08)
            {
                Destroy(transform.gameObject);
                GameObject.Find("Boss").GetComponent<BossScript>().revive = true;
            }
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if(other.name == "Sonic")
        {
            follow = true;
        }else if(other.name == "Boss")
        {
            BossInstruction.SetActive(true);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.name == "Sonic")
        {
            follow = false;
            currentLerpTime = 0;
        }else if(other.name == "Boss")
        {
            BossInstruction.SetActive(false);
        }
    }
    void OnTriggerStay(Collider other)
    {
        if(other.name == "Boss" && flyIn == false)
        {
            if (Input.GetKey(KeyCode.U))
            {
                AudioSource audioSource = Camera.main.GetComponent<AudioSource>();
                audioSource.clip = bossTheme;
                audioSource.Play();
                flyIn = true;
                follow = false;
                currentLerpTime = 0;
                boss = other.transform;
                if (GameObject.Find("Sonic").GetComponent<PlayerMoving>())
                    Destroy(GameObject.Find("Sonic").GetComponent<PlayerMoving>());
            }
        }
    }

    void OnDisable()
    {
        if(BossInstruction != null)
           BossInstruction.SetActive(false);
    }
}
