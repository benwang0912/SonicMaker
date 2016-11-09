using UnityEngine;
using System.Collections;

public class UIHandler : MonoBehaviour {

    private genProjectile script;

    void Start() {
        script = GameObject.Find("ProjectileManager").GetComponent<genProjectile>();
    }

    public void closeOnClick(GameObject obj)
    {
        obj.SetActive(false);
        Time.timeScale = 1;
    }
}
