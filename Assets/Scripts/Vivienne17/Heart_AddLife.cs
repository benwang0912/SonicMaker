using UnityEngine;
using System.Collections;

public class Heart_AddLife : MonoBehaviour {
<<<<<<< HEAD

=======
    
>>>>>>> d3dfe028790c50ca31b2b612d7efabd9cf03227b
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.up * Time.deltaTime * 35);
    }

    void OnTriggerEnter(Collider collision) {
        if (collision.gameObject.name == "Sonic" || collision.gameObject.tag == "Shield") {
            Destroy(this.gameObject);
        }
    }
}
