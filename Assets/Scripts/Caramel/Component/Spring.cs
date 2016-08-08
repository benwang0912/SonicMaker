﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class Spring : MonoBehaviour
{
    //in the Spring
    public enum SpringMode
    {
        Up,
        Down,
        Left,
        Right
    }

    public SpringMode sm;
    public float springforce;
    
    void Awake()
    {
        an = GetComponent<Animator>();
    }
    
    IEnumerator Jumptime()
    {
        yield return jumptime;
        an.SetBool("Jump", false);
    }
    
    void OnCollisionEnter(Collision collision)
    {
        bool active = false;

        switch(sm)
        {
            case SpringMode.Up:
                if (active = collision.contacts[0].point.y - transform.position.y > surface)
                {
                    collision.rigidbody.AddForce(Vector3.up * springforce);
                }

                break;

            case SpringMode.Right:
                if(active = collision.contacts[0].point.x - transform.position.x > surface)
                {
                    collision.rigidbody.AddForce(Vector3.right * springforce);
                }

                break;

            case SpringMode.Down:
                if (active = collision.contacts[0].point.y - transform.position.y < surface)
                {
                    collision.rigidbody.AddForce(Vector3.down * springforce);
                }
                break;

            case SpringMode.Left:
                if (active = collision.contacts[0].point.x - transform.position.x < surface)
                {
                    collision.rigidbody.AddForce(Vector3.left * springforce);
                }

                break;

            default:
                return;
        }

        if(active)
        {
            //spring animation
            an.SetBool("Jump", true);
            StartCoroutine("Jumptime");
        }
    }

    Animator an;
    WaitForSeconds jumptime = new WaitForSeconds(0.1f);
    float surface = 2.5f;
}
