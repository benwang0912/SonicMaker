using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class Stair : MonoBehaviour
{
    void Awake()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Sonic")
        {
            transform.parent.parent.GetComponent<RotatingStairManager>().OnStair(id);
        }
    }

    public int id;
}
