using UnityEngine;
using System.Collections;

public class BallMove : MonoBehaviour {

    float xSpeed = 3.6f;
    float ySpeed = 2.4f;
    float xPos;
	// Use this for initialization
	void Start () {
        xPos = transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
   
        transform.position += xSpeed * Time.deltaTime * Vector3.right;
        transform.position += ySpeed * Time.deltaTime * Vector3.up + (transform.position.x - xPos) * 0.7f * Time.deltaTime * Vector3.down;
    }
}
