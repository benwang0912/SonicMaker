using UnityEngine;
using System.Collections;

public class ConeMove2 : MonoBehaviour {

    private GameObject Sonic;
    // Use this for initialization
    void Start () {
        Sonic = GameObject.Find("Sonic");
    }
	
	// Update is called once per frame
	void Update () {
        if (SonicMove2.Instance.shieldOn == true)
        {
            if (transform.position.x - Sonic.transform.position.x < 3 && transform.position.x - Sonic.transform.position.x > -3)
            {
                Destroy(this.gameObject);
            }
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "Sonic" || collision.gameObject.tag == "Shield")
        {
            Destroy(this.gameObject);
        }
    }
}
