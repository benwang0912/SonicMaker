using UnityEngine;
using System.Collections;

public class enableProjectile : MonoBehaviour
{
    private GameObject projectileManager;
    // Use this for initialization
    void Start()
    {
        projectileManager = Resources.Load("Sinpin/ProjectileManager", typeof(GameObject)) as GameObject;
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.name == "Sonic")
        {
            Instantiate(projectileManager);
            Destroy(this.gameObject);
        }

    }
}