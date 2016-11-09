using UnityEngine;
using System.Collections;

public class HeartScript : MonoBehaviour
{
    static private PlayerMoving player;
    static playerStats playerScript;
    private Rigidbody rb;
    // Use this for initialization
    void Start()
    {
        if (player == null)
            player = GameObject.Find("Sonic").GetComponent<PlayerMoving>();
        if(playerScript == null)
            playerScript = GameObject.Find("Sonic").GetComponent<playerStats>();
        rb = transform.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
     /*   if(transform.position.y <= 0)
        {
            rb.constraints = RigidbodyConstraints.FreezePosition;
        }*/
        transform.Rotate(Vector3.up * Time.deltaTime * 35);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0.0f);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Sonic")
        {
            if (playerScript.Health < playerScript.maxHealth)
            {
                player.audioSource.clip = player.playerAudio.addMaxLife;
                player.audioSource.Play();
                ++playerScript.Health;
                Destroy(this.gameObject);
            }
        }
    }
}

