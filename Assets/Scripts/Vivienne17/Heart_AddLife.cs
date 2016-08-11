using UnityEngine;
using System.Collections;

public class Heart_AddLife : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
	
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
