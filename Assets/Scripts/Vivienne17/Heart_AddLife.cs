using UnityEngine;
using System.Collections;

public class Heart_AddLife : MonoBehaviour {

<<<<<<< HEAD
    // Use this for initialization
    void Start () {
=======
	// Use this for initialization
	void Start () {
>>>>>>> 4e3eafc5927d66dd3cfc939a0a54e4667af8070e
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.up * Time.deltaTime * 35);
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.name == "Sonic") {
            Destroy(this.gameObject);
        }
    }
}
