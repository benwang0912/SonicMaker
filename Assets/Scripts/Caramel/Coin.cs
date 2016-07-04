using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CapsuleCollider))]
public class Coin : MonoBehaviour
{
    //in the Coin

    //Animator an;
    //WaitForSeconds jumptime = new WaitForSeconds(0.1f);
    public float rotationspeed;
    float r = 0f;

    void Awake()
    {
        //an = GetComponent<Animator>();
    }

    void Update()
    {
        r = r <= 360f ? r + rotationspeed * Time.deltaTime : r = -360f;
        transform.localRotation = Quaternion.Euler(0f, r, 0f);
    }
}
