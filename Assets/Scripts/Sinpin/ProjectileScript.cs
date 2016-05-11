using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour {
    Transform playerPosition;
    public float radius = 2;
    public static int ProjectileCount = 0;

    int Num;
	// Use this for initialization
	void Start () {
        playerPosition = GameObject.Find("sonic3").transform;
        ProjectileCount++;
        Num = ProjectileCount;
    }

	// Update is called once per frame
	void LateUpdate () {
        transform.position = new Vector3 ((playerPosition.transform.position.x) + radius * Mathf.Sin(Time.time*3 + 40*Num), 
                                             playerPosition.transform.position.y + 2, 
                                                playerPosition.transform.position.z + radius*Mathf.Cos(Time.time*3+40*Num));

        //transform.RotateAround(transform.parent.position, Vector3.up, 120*Time.deltaTime);

        Vector3 dir = transform.position - playerPosition.position;
        Vector3 emmitDir = Vector3.Cross(transform.up, dir - new Vector3(0, dir.y,0)).normalized;
        transform.rotation = Quaternion.LookRotation(emmitDir);
    }
    
}
