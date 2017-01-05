using UnityEngine;
using System.Collections;
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
public class jumpOnSpring : MonoBehaviour {
    private PlayerMoving player;
    private Rigidbody rb;
    private Animator animator;
    private float bounceHeight = 400.0f;
    // Use this for initialization
    void Start () {
        player = GameObject.Find("Sonic").GetComponent<PlayerMoving>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        bounceHeight = rb.mass * 500;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnCollisionEnter(Collision collision)
    {   
        if (collision.gameObject.tag == "Spring" && collision.contacts[0].normal.y > 0)
        {
            if (player.audioSource.isPlaying && player.audioSource.clip == player.playerAudio.walk)
                player.audioSource.Stop();
            if (!player.audioSource.isPlaying && !(player.audioSource.clip == player.playerAudio.jump))
            {
                player.audioSource.clip = player.playerAudio.jump;
                player.audioSource.Play();
            }
            else if (!player.audioSource.isPlaying && player.audioSource.clip == player.playerAudio.jump)
            {
                player.audioSource.Play();
            }
            rb.AddForce(transform.up * bounceHeight);
            Debug.Log("XD");
            animator.SetInteger("Jump", 1);
        }

    }
    void OnCollisionExit(Collision collision)
    {
        animator.SetInteger("Jump", 0);
    }
}
