using UnityEngine;
using System.Collections;

public class Clone_SubLife : MonoBehaviour {
    private float xPos;

	// Use this for initialization
	void Start () {
        xPos = transform.position.x;
    }
	
	// Update is called once per frame

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Sonic")
        {
            /* while (xPos-transform.position.x > 5) {
                 transform.Rotate(Vector3.forward * Time.deltaTime * 10);
             }*/
            Destroy(this.gameObject);
        }
    }
}
