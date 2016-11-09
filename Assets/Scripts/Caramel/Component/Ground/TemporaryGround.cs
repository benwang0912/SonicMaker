using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(Rigidbody))]
public class TemporaryGround : MonoBehaviour
{
    void Awake()
    {
        mc = GetComponent<MeshCollider>();
        rb = GetComponent<Rigidbody>();
    }

    public void Falling()
    {
        mc.convex = true;
        mc.isTrigger = true;
        rb.useGravity = true;
        rb.isKinematic = false;
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

    MeshCollider mc;
    Rigidbody rb;
}
