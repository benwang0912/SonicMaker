using UnityEngine;
using System.Collections;

public class MainCameraMove2 : MonoBehaviour {

    private float x, y;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        x = GameObject.Find("Sonic").transform.position.x;
        y = GameObject.Find("Sonic").transform.position.y;
        transform.position = 0.1f * new Vector3(10*x, 10*y+33, -170);
    }
}
