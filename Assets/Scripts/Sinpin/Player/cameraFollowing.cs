using UnityEngine;
using System.Collections;

public class cameraFollowing : MonoBehaviour {
    [SerializeField]
    private AudioClip bgm_begin;
    public GameObject target;
    public float cameraSize = 0;
    private Vector3 offset;
    private PlayerMoving player;
    public BossScript boss;

    public Transform door1, door2, door3;
    public float doorLerpTime = 0;
    //private Camera cam;
	// Use this for initialization
	void Start () {
        offset = transform.position - target.transform.position;
        player = GameObject.Find("Sonic").GetComponentInChildren<PlayerMoving>();
        boss = GameObject.Find("Boss").GetComponent<BossScript>();
        door1 = GameObject.Find("Wall (257)").transform;
        door2 = GameObject.Find("Wall (259)").transform;
        door3 = GameObject.Find("Wall (262)").transform;
    }
	
	void LateUpdate () {
        transform.position = new Vector3(target.transform.position.x, target.transform.position.y+offset.y, transform.position.z);
        if (!player.grounded)
        {
            if (Camera.main.orthographicSize != (cameraSize + 12))
            {
                float current = Camera.main.orthographicSize;
                Camera.main.orthographicSize = Mathf.Lerp(current, (cameraSize + 12), 2 * Time.deltaTime);
            }
        }
        else
        {
            if(Camera.main.orthographicSize != (cameraSize + 8))
            {
                float current = Camera.main.orthographicSize;
                Camera.main.orthographicSize = Mathf.Lerp(current , (cameraSize + 8), 2 * Time.deltaTime);
            }
        }

        if(boss.revive == true && boss != null)
        {
            cameraSize = 5;
        }
        if(boss == null)
        {
            doorLerpTime += Time.deltaTime;
            door1.position = new Vector3(10.25f, Mathf.Lerp(68.3f,72.8f,doorLerpTime), 0);
            door2.position = new Vector3(10.25f, Mathf.Lerp(66.4f, 71f, doorLerpTime), 0);
            door3.position = new Vector3(10.25f, Mathf.Lerp(70f, 74.6f, doorLerpTime), 0);
            AudioSource temp = transform.GetComponent<AudioSource>();
            temp.clip = bgm_begin;
            temp.Play();
        }
	}

    public void reset()
    {
        cameraSize = 0;
        doorLerpTime = 0;
        door1.position = new Vector3(10.25f, 68.3f, 0);
        door2.position = new Vector3(10.25f, 66.4f, 0);
        door3.position = new Vector3(10.25f, 70f, 0);
        AudioSource temp = transform.GetComponent<AudioSource>();
        temp.clip = bgm_begin;
        temp.Play();
    }
}
