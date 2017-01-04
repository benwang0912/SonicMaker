using UnityEngine;
using System.Collections;

public class ClearMove : MonoBehaviour {

    private float this_xpos, this_ypos;
    private float x_distance, y_distance;
    public GameObject Sonic;

	// Use this for initialization
	void Start () {
        this_xpos = transform.position.x;
        this_ypos = transform.position.y;
        Sonic = GameObject.Find("Sonic");
    }
	
	// Update is called once per frame
	void Update () {

        /*      if (this_xpos - Sonic.transform.position.x < 15 && this_xpos - Sonic.transform.position.x > -15) {
                  this_xpos = (this_xpos - Sonic.transform.position.x);
              }
              if (this_xpos - Sonic.transform.position.x < 7 ) {
                  transform.position += 10.0f * new Vector3(0, 1, 0) * Time.deltaTime;
              }*/
        if (SonicMove2.Instance.magnetOn == true)
        {
            if (transform.position.x - Sonic.transform.position.x < 18 && transform.position.x - Sonic.transform.position.x > 0)
            {
                transform.position -= 10.0f * new Vector3(1, 0, 0) * Time.deltaTime;
            }
            else if (transform.position.x - Sonic.transform.position.x > -18 && transform.position.x - Sonic.transform.position.x < 0)
            {
                transform.position += 10.0f * new Vector3(1, 0, 0) * Time.deltaTime;
            }
        }
    }

}
