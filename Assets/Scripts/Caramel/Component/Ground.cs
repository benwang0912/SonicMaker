using UnityEngine;
using System.Collections;

public class Ground : MonoBehaviour {

    //in the Ground
    // Use this for initialization
    public int groundcount;

    void Start ()
    {
        for (int i = 0; i<groundcount; ++i)
        {
            transform.GetChild(i).localPosition = new Vector3(i - groundcount/2, 0f, 0f);
        }

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
