using UnityEngine;
using System.Collections;

public class cannonShooter90 : MonoBehaviour
{
    private GameObject bullet;
    private GameObject b;
    private float travelSpeed = 20f;
    private Animator animator;
    private float counter = 1.0f;
    // Use this for initialization
    void Start()
    {
        bullet = Resources.Load("Sinpin/fireBall", typeof(GameObject)) as GameObject;
        animator = gameObject.GetComponentInChildren<Animator>();
        animator.speed = 3.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (b == null && counter <= 0)
        {
            animator.SetInteger("shoot", 1);
            b = Instantiate(bullet);
            b.transform.position = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
            counter = 2.0f;
        }
        else if (b == null)
        {
            counter -= Time.deltaTime;
            return;
        }
        else
            animator.SetInteger("shoot", 0);
        b.transform.position += transform.up * travelSpeed * Time.deltaTime;
        counter -= Time.deltaTime;
        Destroy(b, 2.0f);
    }
}
