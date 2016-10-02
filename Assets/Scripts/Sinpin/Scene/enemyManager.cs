using UnityEngine;
using System.Collections;

public class enemyManager : MonoBehaviour {

    private GameObject enemyPrefab;
    private GameObject[] enemys = new GameObject[5];
    private Vector3[] respawnPosition = new Vector3 [5];
    private float[] countDown = new float[5];
    // Use this for initialization
    void Start () {
        int i = 0;
        enemyPrefab = Resources.Load("Sinpin/Enemy", typeof(GameObject)) as GameObject;
	    foreach(Transform child in transform)
        {
            enemys[i] = child.gameObject;
            respawnPosition[i] = new Vector3(child.transform.position.x, child.transform.position.y, 0);
            countDown[i] = 15;
            ++i;
        }
	}
	
	// Update is called once per frame
	void Update () {
	    for(int i = 0; i < enemys.Length; ++i)
        {
            if(enemys[i] == null)
            {
                countDown[i] -= Time.deltaTime;
                if(countDown[i] < 0)
                {
                    countDown[i] = 15;
                    enemys[i] = Instantiate(enemyPrefab);
                    enemys[i].transform.position = respawnPosition[i];
                }
            }
        }
	}
}
