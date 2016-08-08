using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Coin : MonoBehaviour
{
    //in the Coin

    //Animator an;
    float r = 0f;
    Rigidbody rb;
    CapsuleCollider c;

    public float rotationspeed, groundreflect;

    void Awake()
    {
        //an = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        c = GetComponent<CapsuleCollider>();
    }

    public void Throw(Vector3 v)
    {
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezePositionZ;
        rb.velocity = v;
        Destroy(gameObject, 4f);
    }

    void OnTriggerEnter(Collider c)
    {
        if(c.transform.tag == "Ground")
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * groundreflect, 0f);
        }
    }

    void Update()
    {
        r = r <= 360f ? r + rotationspeed * Time.deltaTime : r = -360f;
        transform.localRotation = Quaternion.Euler(0f, r, 0f);
    }
}
