using UnityEngine;
using System.Collections;

public class BossScript : MonoBehaviour {
    Material material;
    Color targetColor;
    public bool revive = false;
    private bool attack;
    public int step1Health = 3;
    public int step2Health = 3;
    float currentLerpTime = 0.0f;

    Rigidbody rb;

    Transform targetPosition;
    public GameObject obstacle;
    Transform ground1, ground2;
    Vector3 ground1Origin, ground2Origin;
    // Use this for initialization
    void Start () {
        material = Resources.Load("Sinpin/Materials/Stone_Vein_Gray", typeof(Material)) as Material;
        material.color = new Color(60/256.0f,60/256.0f,60/256.0f);
        targetColor = (Resources.Load("Common Resources/Materials/android", typeof(Material)) as Material).color;

        rb = transform.GetComponent<Rigidbody>();
        targetPosition = GameObject.Find("Sonic").transform;

        ground1 = GameObject.Find("Ground (239)").transform;
        ground2 = GameObject.Find("Ground (282)").transform;
        ground1Origin = ground1.position;
        ground2Origin = ground2.position;
        obstacle = GameObject.Find("Obstacle");
    }
	
	// Update is called once per frame
	void Update () {
        if (revive == true && currentLerpTime <0.03 )
        {
            currentLerpTime += Time.deltaTime * 0.01f;
            material.color = Color.Lerp(material.color, targetColor, currentLerpTime);
            ground1.position = new Vector3(Mathf.Lerp(ground1Origin.x, -18.72f, currentLerpTime*33), ground1Origin.y , 0);
            ground2.position = new Vector3(Mathf.Lerp(ground2Origin.x, -16.86f, currentLerpTime * 33), ground2Origin.y, 0);
            if (currentLerpTime >= 0.03)
            {
                transform.GetComponent<BoxCollider>().isTrigger = false;
                rb.useGravity = true;
                attack = true;
            }
        }
        //material.color = targetColor;
        if (attack)
        {
            PlayerMoving player = GameObject.Find("Sonic").AddComponent<PlayerMoving>();
            player.groundCheck = player.transform.GetChild(3);
            player.groundLayer = LayerMask.GetMask("Ground");
            player.wallCheck = player.transform.GetChild(4);
            player.wallLayer = LayerMask.GetMask("Wall");

            gameObject.AddComponent<enemyMoving>();
            enemyMoving temp = transform.GetComponent<enemyMoving>();
            transform.GetComponent<BoxCollider>().size = new Vector3(2.51f,3.83f, 2);
            temp.jumpHeight = 80.0f;
            temp.jumpDistance = 20.0f;
            temp.chaseDistance = 16;
            temp.groundLayer = LayerMask.GetMask("Ground");
            temp.groundCheck = transform.GetChild(1);
            obstacle.SetActive(false);
            attack = false;
        }

        if(step1Health <= 0 && transform.GetComponent<enemyMoving>()!=null)
        {
            Destroy(transform.GetComponent<enemyMoving>());
            rb.velocity = Vector3.zero;
            rb.useGravity = false;
            gameObject.AddComponent<BossStep2>();
        }
	}
    
    public void resetGround() //called by checkPointManager
    {
        ground1.position = ground1Origin;
        ground2.position = ground2Origin;
        obstacle.SetActive(true);
    }

}
