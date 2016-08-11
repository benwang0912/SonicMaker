using UnityEngine;
using System.Collections;

public class AddMaxHealth : MonoBehaviour
{
    static playerStats playerScript;
    // Use this for initialization
    void Start()
    {
        playerScript = GameObject.Find("Sonic").GetComponent<playerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * 35);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Sonic")
        {
                ++playerScript.maxHealth;
                playerScript.Health = playerScript.maxHealth;
                Destroy(this.gameObject);
        }
    }
}

