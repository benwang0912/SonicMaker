using UnityEngine;
using System.Collections;

public class cannonBullet : MonoBehaviour {
    GameObject explosionE, explosionG;

    // Use this for initialization
    void Start () {
        explosionE = Resources.Load("Sinpin/Explosion", typeof(GameObject)) as GameObject;
        explosionG = Resources.Load("Sinpin/ExplosionOnGround", typeof(GameObject)) as GameObject;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            GameObject e = Instantiate(explosionE);
            e.transform.position = transform.position;
            Destroy(e, 5f);
            Destroy(gameObject);
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "Cannon")
        {
            //do nothing
        }
        else if(other.gameObject.name == "Sonic")
        {
            GameObject e = Instantiate(explosionE);
            e.transform.position = transform.position;
            Destroy(e, 5f);
            Destroy(gameObject);
            playerStats p;
            p = other.gameObject.GetComponent<playerStats>();
            p.Health -= 2; 
        }
        else
        {

            GameObject e = Instantiate(explosionG);
            e.transform.position = transform.position;
            Destroy(e, 5f);

            Destroy(gameObject);
        }
    }
}
