using UnityEngine;
using System.Collections;

public class shootAtPlayer : MonoBehaviour {

    Vector3 shootAt;
    Vector3 origin;
    float lerpTime = 0;
	// Use this for initialization
	void Start () {
        shootAt = GameObject.Find("Sonic").transform.position;
        origin = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	    if(lerpTime < 1)
        {
            transform.position = Vector3.Lerp(origin, shootAt, lerpTime);
            lerpTime += Time.deltaTime;
        }
	}
}
