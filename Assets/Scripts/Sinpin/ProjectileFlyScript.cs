using UnityEngine;
using System.Collections;

public class ProjectileFlyScript : MonoBehaviour {
    Transform playerPosition;
    private float counter = 0.5f;
    Vector3 shootDir;
    // Use this for initialization
    void Start () {
        playerPosition = GameObject.Find("sonic3").transform;
        shootDir = playerPosition.forward;
    }
	
	// Update is called once per frame
	void Update () {
        transform.position += shootDir;
        counter -= Time.deltaTime;
        if (counter<0)
            Destroy(gameObject);
	}
}
