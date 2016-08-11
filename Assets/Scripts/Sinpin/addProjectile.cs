using UnityEngine;
using System.Collections;

public class addProjectile : MonoBehaviour {
    private genProjectile script;
    private GameObject manager;
	// Use this for initialization
	void Start () {
        manager = GameObject.Find("ProjectileManager");
        script = manager.GetComponent<genProjectile>();
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.name == "Sonic")
        {
            script.addProjectileUI();
            script.countDownToGen = 0;
            script.projectileGot++;
            Destroy(this.gameObject);
        }

    }
}
