using UnityEngine;
using System.Collections;

public class BallMove : MonoBehaviour {

    float xSpeed = 4.0f;
    float ySpeed = 2.4f;
    float xPos;
    private GameObject Sonic;
	// Use this for initialization
	void Start () {
        //      xPos = transform.position.x;
        Sonic = GameObject.Find("Sonic");
        transform.position = GameObject.Find("Sonic").transform.position;
        transform.position += new Vector3(0, 2, 0);
        if (Sonic.transform.rotation.y != 90) {
            xSpeed = xSpeed * -1;
        }
    }
	
	// Update is called once per frame
	void Update () {
   
        transform.position += xSpeed * Time.deltaTime * Vector3.right;
    //    transform.position += ySpeed * Time.deltaTime * Vector3.up + (transform.position.x - xPos) * 0.7f * Time.deltaTime * Vector3.down;
    }
}
