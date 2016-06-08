using UnityEngine;
using System.Collections;

public class Enemy1AI : MonoBehaviour
{
    //in the Enemy1

    Transform sonic;
    Vector3 faceright, faceleft;
    float original_x, speed = 4f;
    bool face = true;

    // Use this for initialization
    void Start()
    {
        sonic = GameObject.Find("Sonic").transform;
        original_x = transform.localPosition.x;
        faceright = new Vector3(0f, 90f, 0f);
        faceleft = new Vector3(0f, 270f, 0f);
    }

    void patrol()
    {
        if ((transform.localPosition.x - original_x) >= 3f)
        {
            //to left
            face = false;
            transform.localRotation = Quaternion.Euler(faceleft);
        }

        if ((transform.localPosition.x - original_x) <= -3f)
        {
            //to right
            face = true;
            transform.localRotation = Quaternion.Euler(faceright);
        }

        transform.localPosition += face ? Vector3.right * speed * Time.deltaTime : Vector3.left * speed * Time.deltaTime;
    }

    void attack()
    {
        if ((transform.localPosition.x - original_x) <= 10f && (transform.localPosition.x - original_x) >= -10f)
        {
            transform.localRotation = (transform.localPosition.x - sonic.localPosition.x) < 0 ? Quaternion.Euler(faceright) : Quaternion.Euler(faceleft);
            transform.localPosition += (transform.localPosition.x - sonic.localPosition.x) < 0 ? Vector3.right * speed * Time.deltaTime : Vector3.left * speed * Time.deltaTime;
        }
        else
        {
            patrol();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(sonic.position, transform.position) <= 5f && sonic.localScale.x != 0.01f)
        {
            //to attack
            Debug.Log("attack");
            attack();
        }
        else
        {
            //to patrol
            patrol();
        }
    }
}

