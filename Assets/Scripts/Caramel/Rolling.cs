﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Rolling : MonoBehaviour {

    Vector3 rolling = new Vector3(1f, 90f, 0f);
    Material material;
    Rigidbody rb;
    GameObject sonic;
    Color slowrolling = new Color(0.102f, 0.102f, 0.102f, 1f), quickrolling = new Color(0.75f, 0.75f, 0.75f, 1f);
    //Vector3 original_position;
    float rollingspeed, jumpforce = 400f, walkspeed = 5f, vibration = .02f;
    bool isvibration = false, right = true;

    enum SonicMode
    {
        DEAD,
        NORMAL
    }

	// Use this for initialization
	void Start ()
    {
        material = (Material)Resources.Load("Caramel/Materials/Material.001");
        rb = GetComponent<Rigidbody>();
        sonic = GameObject.Find("Sonic");
    }

    void ChangeToSonic(GameConstants.SonicState s)
    {
        sonic.SetActive(true);
        gameObject.SetActive(false);

        GameConstants.velocity = rb.velocity;
        rb.velocity = Vector3.zero;
        sonic.transform.localPosition = transform.localPosition - Vector3.up * .2f;

        switch (s)
        {
            case GameConstants.SonicState.NORMAL:
                GameConstants.sonicstate = GameConstants.SonicState.NORMAL;
                sonic.SendMessage("BackToSonic", GameConstants.SonicState.NORMAL);
                break;
            case GameConstants.SonicState.DEAD:
                sonic.SendMessage("BackToSonic", GameConstants.SonicState.DEAD);
                break;
        }
    }

    public void BackToBall(GameConstants.SonicState s)
    {
        rb.velocity = GameConstants.velocity;

        switch(s)
        {
            case GameConstants.SonicState.JUMPING:
                JumpRolling();
                break;
            case GameConstants.SonicState.TOROLL:
                QuickRolling((sonic.transform.localRotation.eulerAngles.y - 91f) < 0 ? true : false);
                break;
        }
    }

    public void QuickRolling(bool r)
    {
        material.SetColor("_EmissionColor", quickrolling);
        rollingspeed = 1500f;
        //original_position = transform.localPosition;
        isvibration = true;
        right = r;
    }

    public void SlowRolling(float force)
    {
        material.SetColor("_EmissionColor", slowrolling);
        rollingspeed = 900f;
        isvibration = false;

        rb.AddForce((right ? Vector3.right : Vector3.left) * force);
    }

    public void JumpRolling()
    {
        GameConstants.sonicstate = GameConstants.SonicState.JUMPING;
        material.SetColor("_EmissionColor", slowrolling);
        rollingspeed = 900f;
        isvibration = false;
        rb.AddForce(Vector3.up * jumpforce);
    }

    void OnCollisionEnter(Collision collision)
    {
        
        if (collision.relativeVelocity.y > 1f)
        {
            //animator.SetBool("Jump", false);
            ChangeToSonic(GameConstants.SonicState.NORMAL);
        }
        

        switch (collision.transform.name)
        {
            /*
            case "Wall":
                skin1.material.color = Color.red;
                skin2.material.color = Color.red;

                break;
            */

            /*
            case "Spring":
                if (collision.relativeVelocity.y > 1f)
                {
                    GameConstants.sonicstate = GameConstants.SonicState.JUMPING;
                    animator.SetBool("Jump", true);
                    rb.AddForce(transform.up * 600f);
                }

                break;
            */

            case "Enemy1":
                Destroy(collision.gameObject);
                break;
        }
    }



    /*
    public void Vibration(bool b)
    {
        original_position = transform.localPosition;
        isvibration = b;
    }
	*/

    // Update is called once per frame
    void Update ()
    {
        if (transform.localPosition.y < -10.0f)
            ChangeToSonic(GameConstants.SonicState.DEAD);

        //to return to sonic
        if (rb.velocity.magnitude <= 1f && (GameConstants.sonicstate == GameConstants.SonicState.ROLLING))
        {
            gameObject.SetActive(false);
            GameConstants.sonicstate = GameConstants.SonicState.NORMAL;
            sonic.transform.localPosition = transform.localPosition;
            sonic.SetActive(true);
        }

        //to roll
        rolling.x += Time.deltaTime * rollingspeed;
        transform.localRotation = Quaternion.Euler(rolling);

        //to vibrate
        if(isvibration)
        {
            //be shifting??
            transform.localPosition += Vector3.right * vibration * Mathf.Cos(9.42f * Time.time);
        }

        //to keep rolling
        if (Input.GetKeyDown(KeyCode.Return) && GameConstants.sonicstate == GameConstants.SonicState.TOROLL)
        {
            //music!?
        }

        if (Input.GetKeyUp(KeyCode.DownArrow) && GameConstants.sonicstate == GameConstants.SonicState.TOROLL)
        {
            GameConstants.sonicstate = GameConstants.SonicState.ROLLING;
            SlowRolling(600f);
        }

        //to jump in rolling
        if(Input.GetKey(KeyCode.Return) && GameConstants.sonicstate == GameConstants.SonicState.ROLLING)
        {
            JumpRolling();
        }

        if((Input.GetAxis("Horizontal") != 0 && GameConstants.sonicstate == GameConstants.SonicState.JUMPING))
        {
            rb.AddForce(Vector3.right * Input.GetAxis("Horizontal") * walkspeed);
        }

        //to slow only
        if (Input.GetAxis("Horizontal") > 0f && !right)
        {
            //to turn right
            //transform.localRotation = Quaternion.Euler(Vector3.up * 90f);
            //animator.SetInteger("Mode", 1);
            //there is a bug that Sonic sometimes cannot walk!
            rb.AddForce(Vector3.right * walkspeed);
        }

        if (Input.GetAxis("Horizontal") < 0f && right)
        {
            //to turn left
            //transform.localRotation = Quaternion.Euler(Vector3.up * 270f);
            //animator.SetInteger("Mode", 1);
            rb.AddForce(Vector3.left * walkspeed);
        }
    }
}