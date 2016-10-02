using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossStep2 : MonoBehaviour {
    
    Transform player;
    GameObject projectile;
    Vector3 initPosition;
    float currentLerpTime = 0.0f;

    GameObject projectileHold;
    float projectileLerpTime = 0.0f;
    private bool projectileGened = false;
    Material material;
    Color readyColor;

    List<GameObject> projectileBossInScene = new List<GameObject>();
    BossScript boss;
    GameObject explosionE;
    // Use this for initialization
    void Start () {
        player = GameObject.Find("Sonic").transform;
        initPosition = transform.position;
        projectile = Resources.Load("Sinpin/ProjectileBoss", typeof(GameObject)) as GameObject;
        boss = transform.GetComponent<BossScript>();
        explosionE = Resources.Load("Sinpin/Explosion", typeof(GameObject)) as GameObject;
    }
	
	// Update is called once per frame
	void Update () {
	    if(currentLerpTime < 1)
        {
            transform.position = Vector3.Lerp(initPosition, new Vector3(-20, 75, 0), currentLerpTime);
            currentLerpTime += Time.deltaTime;
        }
        if(currentLerpTime >= 1 && !projectileGened)
        {
            genProjectile();
        }

        turnToPlayer();
        
        if (projectileLerpTime < 1 && projectileGened)
        {
            preparingProjectile();
        }
        if (projectileLerpTime >= 0.9 && projectileGened)
        {
            shoot();
        }

        if (boss.step2Health <= 0)
        {
            GameObject e;
            foreach (GameObject p in projectileBossInScene)
            {
                e = Instantiate(explosionE);
                e.transform.position = transform.position;
                Destroy(e, 2f);
                Destroy(p);
            }
            projectileBossInScene.Clear();
            
            e = Instantiate(explosionE);
            e.transform.position = transform.position;
            Destroy(e, 5f);
            Destroy(gameObject);
        }

    }

    void genProjectile()
    {
        projectileGened = true;
        GameObject temp = Instantiate(projectile);
        temp.transform.position = transform.position + new Vector3(2 * transform.forward.x, 1.5f, 0);
        temp.transform.localScale = new Vector3(2, 2, 2);
        temp.transform.parent = transform;
        material = temp.GetComponent<MeshRenderer>().material;
        readyColor = material.color;
        material.color = new Color(60 / 256.0f, 60 / 256.0f, 60 / 256.0f);
        projectileHold = temp;
        projectileBossInScene.Add(temp);
    }

    void turnToPlayer()
    {
        Vector3 playerDir = player.position - transform.position;
        if (playerDir.x < 0)
        {
            transform.rotation = Quaternion.AngleAxis(-90, Vector3.up);
        }
        else
        {
            transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
        }
    }

    void preparingProjectile()
    {
        material.color = Color.Lerp(new Color(60 / 256.0f, 60 / 256.0f, 60 / 256.0f), readyColor, projectileLerpTime);
        projectileHold.transform.localScale = Vector3.Lerp(Vector3.zero, new Vector3(0.8f,0.8f,0.8f), projectileLerpTime);
        projectileLerpTime += Time.deltaTime * 0.3f;
    }

    void shoot()
    {
        GameObject temp = Instantiate(projectile);
        temp.transform.position = projectileHold.transform.position;
        temp.transform.localScale = new Vector3(2,2,2);
        temp.AddComponent<shootAtPlayer>();
        projectileLerpTime = 0;
        projectileBossInScene.Add(temp);
    }

    void OnDisable()
    {
        foreach (GameObject p in projectileBossInScene)
        {
            Destroy(p);
        }
        projectileBossInScene.Clear();
    }
}
