using UnityEngine;
using System.Collections;

public class genProjectile : MonoBehaviour {

    private GameObject projectile;
    private GameObject []proj = new GameObject[3];
    private ProjectileScript []script = new ProjectileScript[3];
    private float countDownToGen = 5.0f;
    // Use this for initialization
    void Start () {
        projectile = Resources.Load("Sinpin/Projectile", typeof(GameObject)) as GameObject;
        if (ProjectileScript.ProjectileCount < 3 && proj[0] == null)
        {
            proj[0] = Instantiate<GameObject>(projectile);
            script[0] = proj[0].GetComponent<ProjectileScript>();
        }
        if (ProjectileScript.ProjectileCount < 3 && proj[1] == null)
        {
            proj[1] = Instantiate<GameObject>(projectile);
            script[1] = proj[1].GetComponent<ProjectileScript>();
        }
        if (ProjectileScript.ProjectileCount < 3 && proj[2] == null)
        {
            proj[2] = Instantiate<GameObject>(projectile);
            script[2] = proj[2].GetComponent<ProjectileScript>();
        }
    }

    // Update is called once per frame
    void Update(){
        if(ProjectileScript.ProjectileCount < 3)
        {
            countDownToGen -= Time.deltaTime;
            if (countDownToGen<0)
            {
                proj[ProjectileScript.ProjectileCount] = Instantiate<GameObject>(projectile);
                script[ProjectileScript.ProjectileCount] = proj[ProjectileScript.ProjectileCount].GetComponent<ProjectileScript>();
                countDownToGen = 5;
            }
        }
        
        if (Input.GetMouseButtonDown(0)) {
            projectileLaunch();
        }
    }
    void projectileLaunch()
    {
        if (ProjectileScript.ProjectileCount > 0)
        {
            Destroy(script[ProjectileScript.ProjectileCount - 1]);
            proj[ProjectileScript.ProjectileCount - 1].AddComponent<ProjectileFlyScript>();
            ProjectileScript.ProjectileCount--;
        }
    }
}
