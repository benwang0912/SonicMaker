using UnityEngine;
using System.Collections;

public class Clone_SubLife : MonoBehaviour {
 //   private float xPos;

	// Use this for initialization
	void Start () {
 //       xPos = transform.position.x;
    }

    // Update is called once per frame
    void Update() {
        if (SonicMove2.Instance.shieldOn == true) {
            if (transform.position.x - SonicMove2.Instance.Sonic.transform.position.x < 3 && transform.position.x - SonicMove2.Instance.Sonic.transform.position.x > -3) {
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
