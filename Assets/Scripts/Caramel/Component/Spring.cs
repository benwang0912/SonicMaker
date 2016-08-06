using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class Spring : MonoBehaviour
{
    //in the Spring

    Animator an;
    WaitForSeconds jumptime = new WaitForSeconds(0.1f);

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
        //Debug.Log(collision.relativeVelocity);

        if(collision.relativeVelocity.y != 0f)
        {
            //to jump
            an.SetBool("Jump", true);
            StartCoroutine("Jumptime");
        }
    }
}
