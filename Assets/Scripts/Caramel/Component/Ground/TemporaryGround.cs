using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class TemporaryGround : MonoBehaviour
{
    void Awake()
    {
        bc = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
    }

    public void Falling()
    {
        bc.isTrigger = true;
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
    }

    void OnCollisionEnter(Collision collision)
    {
        switch(collision.transform.tag)
        {
            case "Sonic":
                transform.parent.GetComponent<PlaneManager>().StartFalling();
                break;
        }
    }

    BoxCollider bc;
    Rigidbody rb;
}
