using UnityEngine;
using System.Collections;

public class cameraFollowing : MonoBehaviour {
    public GameObject target;
    private Vector3 offset;
    private PlayerMoving player;
    //private Camera cam;
	// Use this for initialization
	void Start () {
        offset = transform.position - target.transform.position;
        player = GameObject.Find("Sonic").GetComponentInChildren<PlayerMoving>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.position = new Vector3(target.transform.position.x, target.transform.position.y+offset.y, transform.position.z);
        if (!player.grounded)
        {
            if (Camera.main.orthographicSize != 9)
            {
                float current = Camera.main.orthographicSize;
                Camera.main.orthographicSize = Mathf.Lerp(current, 9, 2 * Time.deltaTime);
            }
        }
        else
        {
            if(Camera.main.orthographicSize != 5)
            {
                float current = Camera.main.orthographicSize;
                Camera.main.orthographicSize = Mathf.Lerp(current , 5, 2 * Time.deltaTime);
            }
        }
	}
}
