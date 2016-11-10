using UnityEngine;
using System.Collections;

public class MainCameraMove : MonoBehaviour {

    private float x, y;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (SonicMove.Instance.Died == false)
        {
            x = GameObject.Find("Sonic").transform.position.x;
            transform.position = new Vector3(x + 6, 5f, -12);
        }
        else {
            x = GameObject.Find("Sonic").transform.position.x;
            y = GameObject.Find("Sonic").transform.position.y;
            transform.position = new Vector3(x + 6, y + 4, -12);
        }

    }
}
