using UnityEngine;
using System.Collections;

public class Clone_SubLife : MonoBehaviour {
 //   private float xPos;

	// Use this for initialization
	void Start () {
 //       xPos = transform.position.x;
    }
	
	// Update is called once per frame

/*    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Sonic")
        {
            
            Destroy(this.gameObject);
        }
    }*/

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "Sonic" || collision.gameObject.tag == "Shield")
        {
            Destroy(this.gameObject);
        }
    }
}
