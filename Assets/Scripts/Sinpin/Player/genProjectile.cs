using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class genProjectile : MonoBehaviour {

    private GameObject projectile;
    public GameObject []proj = new GameObject[10];
    public ProjectileScript []script = new ProjectileScript[10];
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
                instantiateProjectile();
            }
        }
        
        if (Input.GetMouseButtonDown(0) && !GameObject.Find("ShootInstruction") && !GameObject.Find("WallJumpInstruction") ) {
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

            setUIColorToGray();
        }
    }

    public void instantiateProjectile()
    {
        proj[ProjectileScript.ProjectileCount] = Instantiate<GameObject>(projectile);
        script[ProjectileScript.ProjectileCount] = proj[ProjectileScript.ProjectileCount].GetComponent<ProjectileScript>();
        proj[ProjectileScript.ProjectileCount].transform.SetParent(transform);
        countDownToGen = 5;
        setUIColorToWhite();
    }

    public void addProjectileUI()
    {
        GameObject obj = Instantiate(projectileUI);
        obj.transform.SetParent(grid.transform,false);
        obj.transform.SetAsLastSibling();
        obj.GetComponent<SpriteRenderer>().color = Color.gray;
        grid.Reposition();
    }
    public void setUIColorToWhite()
    {
        //grid.GetChild(grid.transform.childCount - 1).GetComponent<SpriteRenderer>().color = Color.white;
        //grid.GetChild(grid.transform.childCount - 1).SetAsFirstSibling();
        foreach (Transform child in grid.transform)
        {
            SpriteRenderer temp = child.GetComponent<SpriteRenderer>();
            if (temp.color == Color.gray)
            {
                temp.color = Color.white;
                return;
            }
        }
        grid.Reposition();
    }
    public void setUIColorToGray()
    {
        grid.GetChild(0).GetComponent<SpriteRenderer>().color = Color.gray;
        grid.GetChild(0).SetAsLastSibling();
        grid.Reposition();
    }
    public void resetProjectileUI()
    {
        List<GameObject> oldChildren = new List<GameObject>();
        foreach (Transform child in grid.transform)
        {
            oldChildren.Add(child.gameObject);

            Destroy(child.gameObject);
        }


        for (int i = 0; i < projectileGot; ++i)
        {
            addProjectileUI();
        }

        foreach (GameObject t in oldChildren)
        {
            t.transform.SetAsLastSibling();
        }

        grid.Reposition();
    }
}
