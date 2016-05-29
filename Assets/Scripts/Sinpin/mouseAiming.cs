using UnityEngine;
using System.Collections;

public class mouseAiming : MonoBehaviour {
    private Vector3 mousePosition;
    private Vector3 shootDirection;
    private float counter = 0.318f;
    GameObject explosionE, explosionG;

    // Use this for initialization
    void Start () {

        //mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        explosionE = Resources.Load("Sinpin/Explosion", typeof(GameObject)) as GameObject;
        explosionG = Resources.Load("Sinpin/ExplosionOnGround", typeof(GameObject)) as GameObject;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        mousePosition = ray.origin;
        mousePosition.y -= 1;
        mousePosition.z = 0;
        shootDirection = (mousePosition - transform.position).normalized;
        
    }
	
	// Update is called once per frame
	void Update () {
        transform.position += shootDirection;
        transform.position = new Vector3 (transform.position.x, transform.position.y, 0);
        counter -= Time.deltaTime;
        if (counter < 0)
            Destroy(gameObject);
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            GameObject e = Instantiate(explosionE);
            e.transform.position = transform.position;
            Destroy(e, 5f);
            Destroy(gameObject);
            Destroy(other.gameObject);
        }else if(other.gameObject.name == "Sonic")
        {
            //donothing
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
