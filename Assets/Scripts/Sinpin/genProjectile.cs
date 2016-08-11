using UnityEngine;
using System.Collections;

public class genProjectile : MonoBehaviour {

    private GameObject projectile;
    private GameObject []proj = new GameObject[10];
    private ProjectileScript []script = new ProjectileScript[10];
    public float countDownToGen = 5.0f;
    public int projectileGot = 0;

    private UIGrid grid;
    private GameObject projectileUI;
    // Use this for initialization
    void Start () {
        projectile = Resources.Load("Sinpin/Projectile", typeof(GameObject)) as GameObject;
        grid = GameObject.Find("EnergyBallLayout").GetComponentInChildren<UIGrid>();
        projectileUI = Resources.Load("Sinpin/energyBallUI", typeof(GameObject)) as GameObject;
    }

    // Update is called once per frame
    void Update(){
        if(ProjectileScript.ProjectileCount < projectileGot)
        {
            countDownToGen -= Time.deltaTime;
            if (countDownToGen<0)
            {
                proj[ProjectileScript.ProjectileCount] = Instantiate<GameObject>(projectile);
                script[ProjectileScript.ProjectileCount] = proj[ProjectileScript.ProjectileCount].GetComponent<ProjectileScript>();
                countDownToGen = 5;
                setUIColorToWhite();
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
            proj[ProjectileScript.ProjectileCount - 1].AddComponent<mouseAiming>();
            ProjectileScript.ProjectileCount--;

            grid.GetChild(0).GetComponent<SpriteRenderer>().color = Color.gray;
            grid.GetChild(0).SetAsLastSibling();
            grid.Reposition();
        }
    }

    public void addProjectileUI()
    {
        GameObject obj = Instantiate(projectileUI);
        obj.transform.SetParent(grid.transform,false);
        obj.transform.SetAsLastSibling();
        obj.GetComponent<SpriteRenderer>().color = Color.gray;
        grid.Reposition();
    }
    private void setUIColorToWhite()
    {
        grid.GetChild(grid.transform.childCount - 1).GetComponent<SpriteRenderer>().color = Color.white;
        grid.GetChild(grid.transform.childCount - 1).SetAsFirstSibling();
        grid.Reposition();
    }
}
