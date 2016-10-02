using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class checkPointManager : MonoBehaviour {

    //resources
    static private GameObject addProjectileGot;
    static private GameObject addMaxHeart;
    static private GameObject wallJumpAbility;
    static private GameObject bossPrefab;
    static private GameObject bossEnergyPrefab;

    //gameObject in scene
    static private cameraFollowing cam;
    static private GameObject player;
    static private PlayerMoving playerController;
    static private playerStats state;
    static private GameObject projectileManager;
    static private BossScript boss;
    static private GameObject AddHeartManager;
    static private GameObject AddProjectileManager;

    //saving data
    static private Vector3 position;
    static private float maxHealth;   
    static private float curHealth;   
    static private int projectileGot;
    static private int curProjectCount;
    static private bool wallJumpLearned;
    static private Vector3 wallJumpAbilityPosition;
    static private GameObject bossEnergy;
    static private Vector3 bossEnergyPosition;
    static List<GameObject> addHeart = new List<GameObject>();
    static private Vector3[] addHeartPosition = new Vector3[4];
    static List<GameObject> addProjectile = new List<GameObject>();
    static private Vector3[] addProjectilePosition = new Vector3[3];

    // Use this for initialization
    void Start () {

        addMaxHeart = Resources.Load("Sinpin/HeartForMaxHealth", typeof(GameObject)) as GameObject;
        addProjectileGot = Resources.Load("Sinpin/addProjectile", typeof(GameObject)) as GameObject;
        wallJumpAbility = Resources.Load("Sinpin/WallJumpAbility", typeof(GameObject)) as GameObject;
        bossPrefab = Resources.Load("Sinpin/Boss", typeof(GameObject)) as GameObject;
        bossEnergyPrefab = Resources.Load("Sinpin/BossEnergy", typeof(GameObject)) as GameObject;

        cam = Camera.main.GetComponent<cameraFollowing>();
        player = GameObject.Find("Sonic");
        playerController = player.GetComponent<PlayerMoving>();
        state = player.GetComponent<playerStats>();
        projectileManager = GameObject.Find("ProjectileManager");
        wallJumpAbilityPosition = GameObject.Find("WallJumpAbility").transform.position;
        bossEnergy = GameObject.Find("BossEnergy");
        boss = GameObject.Find("Boss").GetComponent<BossScript>();
        AddHeartManager = GameObject.Find("AddHeartManager");
        AddProjectileManager = GameObject.Find("AddProjectileManager");
    }
	
    static public void save(Vector3 savePosition)
    {
        position = savePosition + new Vector3(0,2,0);
        maxHealth = state.maxHealth;
        curHealth = state.Health;
        projectileGot = projectileManager.GetComponent<genProjectile>().projectileGot;
        curProjectCount = projectileManager.transform.childCount;
        wallJumpLearned = playerController.wallJumpLearned;
        bossEnergyPosition = bossEnergy.transform.position;

        addHeart.Clear();
        int i = 0;
        foreach(Transform child in AddHeartManager.transform)
        {
            addHeart.Add(child.gameObject);
            addHeartPosition[i] = child.transform.position;
            ++i;
        }

        addProjectile.Clear();
        i = 0;
        foreach (Transform child in AddProjectileManager.transform)
        {
            addProjectile.Add(child.gameObject);
            addProjectilePosition[i] = child.transform.position;
            ++i;
        }
    }
    static public void load()
    {
        //reset position
        player.transform.position = position;

        //reset heart UI
        resetAddHeart();

        //reset projectile UI
        resetProjectile();

        //reset wall jump ability
        if(wallJumpLearned == false && playerController.wallJumpLearned == true)
        {
            playerController.wallJumpLearned = false;
            GameObject t = Instantiate(wallJumpAbility);
            t.transform.position = wallJumpAbilityPosition;
        }

        //reset boss
        resetBoss();
    }

    private static void resetAddHeart()
    {
        //regenereate addMaxHeart
        for (int i = 0; i < addHeart.Count; ++i)
        {
            if (addHeart[i] == null)
            {
                addHeart[i] = Instantiate(addMaxHeart);
                addHeart[i].transform.position = addHeartPosition[i];
                addHeart[i].transform.SetParent(AddHeartManager.transform);
            }
        }

        //reset heart UI
        state.Health = curHealth;
        state.maxHealth = maxHealth;
    }

    private static void resetProjectile()
    {
        genProjectile script = projectileManager.GetComponent<genProjectile>();

        //reset projectile UI
        script.projectileGot = projectileGot;
        script.resetProjectileUI();
        
        //delete all projectile
        foreach (Transform t in projectileManager.transform)
        {
            ProjectileScript.ProjectileCount -= 1;
            Destroy(t.gameObject);
        }

        //regenerate new projectile
        for (int i = 0; i < curProjectCount; ++i)
        {
            script.instantiateProjectile();
        }

        //regenerate addProjectile
        for(int i =0;i<addProjectile.Count;++i)
        {
            if(addProjectile[i] == null)
            {
                addProjectile[i] = Instantiate(addProjectileGot);
                addProjectile[i].transform.position = addProjectilePosition[i];
                addProjectile[i].transform.SetParent(AddProjectileManager.transform);
            }                    
        }
    }

    private static void resetBoss()
    {
        if (boss.revive == false)
            return;
        else
        {
            boss.resetGround();
            Destroy(boss.gameObject);

            GameObject bossTemp = Instantiate(bossPrefab);
            boss = bossTemp.GetComponent<BossScript>();
            boss.transform.name = "Boss";

            GameObject temp = Instantiate(bossEnergyPrefab);
            bossEnergy = temp;
            bossEnergy.transform.position = bossEnergyPosition;

            cam.boss = boss;
            cam.reset();
        }
    }
}
