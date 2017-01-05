using UnityEngine;
using System.Collections;

public class Clone_SubLife : MonoBehaviour {

    // Use this for initialization

    void Start () {
    }

    // Update is called once per frame
    void Update() {
        
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "Sonic" || collision.gameObject.tag == "Shield")
        {
            Destroy(this.gameObject);
        }
    }
}
