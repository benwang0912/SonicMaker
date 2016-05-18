using UnityEngine;
using System.Collections;

public class ProjectileFlyScript : MonoBehaviour {
    Transform playerPosition;
    GameObject target;
    private float counter = 0.318f;
    Vector3 shootDir;
    private float shootRange = 20.0f;
    private bool aimed = false;
    private bool straightShoot = false;
    private Vector3 startPosition;
    Vector3 firstNode;
    Vector3 secondNode;
    private Vector3 midPoint;

    private float startAngle = 155; //sin = 0 ,cos = -1
    private int a = 3, b = 6;
    // Use this for initialization
    void Start () {
        playerPosition = GameObject.Find("Sonic").transform;
        target = FindClosestEnemy();
        if (target != null)
        {
            aimTarget(target);
        }
        else
        {
            shootDir = playerPosition.forward;
        }
        
    }
	
	// Update is called once per frame
	void Update () {
        
        if (aimed == false)
        {
            transform.position += shootDir;
            counter -= Time.deltaTime;
            if (counter < 0)
                Destroy(gameObject);
        }
        else //if(straightShoot == true)
        {
            transform.position += shootDir;
            counter -= Time.deltaTime;
            if (counter < 0)
                Destroy(gameObject);
        }
    /*    else
        {
            //if(transform.position.x < midPoint.x)
            //{
                transform.position = new Vector3(firstNode.x +b * Mathf.Sin(startAngle), firstNode.y +a * Mathf.Cos(startAngle), 0);
                startAngle += Time.deltaTime;
            //}
            counter -= Time.deltaTime;
            //if (counter < 0)
            //    Destroy(gameObject);
        }*/
    }

    private void aimTarget(GameObject target)
    {
        float distance = (new Vector3(target.transform.position.x, target.transform.position.y-1, 0) - playerPosition.transform.position).sqrMagnitude;
        if (distance < 64)    //enemy and player too close, do straight shoot
        {
            aimed = true;
            straightShoot = true;
            shootDir = (new Vector3(target.transform.position.x, target.transform.position.y - 1, 0) - playerPosition.transform.position).normalized;
            shootDir.z = 0;
        }
        else if (distance < shootRange * shootRange)    //enemy in range, do special shoot
        {
            aimed = true;
        }
        else                //enemy out of range
        {
            shootDir = playerPosition.forward;
        }

        if (aimed == true && straightShoot == false)     //setting special shoot route
        {
            /* Vector3 direction = (target.transform.position - playerPosition.transform.position);
             firstNode = playerPosition.transform.position + direction / 3;
             firstNode.z = 0;
             secondNode = playerPosition.transform.position + 2 * direction / 3;
             secondNode.z = 0;
             midPoint = playerPosition.transform.position + (direction)/2;
             midPoint.z = 0;*/
            shootDir = (new Vector3(target.transform.position.x, target.transform.position.y - 1, 0) - playerPosition.transform.position).normalized;
        }
    }
    
    private GameObject FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        float tempDistance;
        foreach (GameObject enemy in enemies)
        {
            tempDistance = (enemy.transform.position - playerPosition.transform.position).sqrMagnitude;
            if (tempDistance < distance)
            {
                closest = enemy;
                distance = tempDistance; 
            }
        }

        return closest;
    }
}
