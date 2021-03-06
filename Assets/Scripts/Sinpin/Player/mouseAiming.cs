﻿using UnityEngine;
using System.Collections;

public class mouseAiming : MonoBehaviour {
    private Vector3 mousePosition;
    private Vector3 shootDirection;
    private float counter = 0.318f;
    GameObject explosionE, explosionG;
    private GameObject enemyDropItem;
    public static int enemyKilled = 0;
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

        enemyDropItem = Resources.Load("Sinpin/HeartForRecovery", typeof(GameObject)) as GameObject;

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
        if (other.gameObject.tag == "Enemy" && other.gameObject.name != "Boss")
        {
            GameObject e = Instantiate(explosionE);
            e.transform.position = transform.position;
            Destroy(e, 5f);

            //drop item
            float odds = Random.Range(0.0f, 1.0f);
            if (odds <= 0.3f)
            {
                GameObject drop = Instantiate(enemyDropItem);
                drop.transform.position = new Vector3(transform.position.x, transform.position.y, 0.0f);
            }

            Destroy(gameObject);
            Destroy(other.gameObject);
            ++enemyKilled;

        } else if (other.gameObject.name == "Boss" && other.GetComponent<BossScript>().revive == true) {
            GameObject e = Instantiate(explosionE);
            e.transform.position = transform.position;
            Destroy(e, 5f);

            BossScript boss = other.GetComponent<BossScript>();
            if(boss.step1Health > 0)
            {
                boss.step1Health -= 1;
                Destroy(gameObject);
            }
            else if(boss.step1Health <= 0 && boss.step2Health > 0)
            {
                boss.step2Health -= 1;
                Destroy(gameObject);
            }
        }
        else if (other.gameObject.name == "Sonic")
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
