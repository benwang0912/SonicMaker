using UnityEngine;
using System.Collections;

public class Dropping_Floor : MonoBehaviour {

    private float floor_xpos;
    public GameObject Sonic;

    private bool test = true;

	// Use this for initialization
	void Start () {
        floor_xpos = transform.position.x;
        Sonic = GameObject.Find("Sonic");
	}
	
	// Update is called once per frame
	void Update () {
        if (test == true) {

            if (floor_xpos - Sonic.transform.position.x < 20 || transform.position.y> -20) {
                transform.position -= 5.0f * new Vector3(0, 1, 0) * Time.deltaTime;
            }

        }
	}
}
