using UnityEngine;
using System.Collections;

public class checkPointSpin : MonoBehaviour {
    private float speed = 360;
    private checkPoint script;
	// Use this for initialization
	void Start () {
        script = transform.root.GetComponent<checkPoint>();
	}
	
	// Update is called once per frame
	void Update () {
        if(!script.activated)
            transform.Rotate(new Vector3 (0, 0 ,1 ), speed * Time.deltaTime);
    }
}
