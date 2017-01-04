using UnityEngine;
using System.Collections;

public class WallControl : MonoBehaviour {

    public GameObject healthBar;
    private float max_Health = 100f;
    private float cur_Health = 0f;
    private GameObject Sonic;

    // Use this for initialization
    void Start () {
        Sonic = GameObject.Find("Sonic");
    }
	
	// Update is called once per frame
	void Update () {
        if (Sonic.transform.position.x < 165 && Sonic.transform.position.x > 163)
        {
            if (Sonic.transform.position.y > 0 && Sonic.transform.position.y < 1.1) {
                SonicMove2.Instance.Movable = false;
            }
            else {
                SonicMove2.Instance.Movable = true;
            }
        }
        else {
            SonicMove2.Instance.Movable = true;
        }
	}

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "VBall") {
            Destroy(collision.gameObject);
            decreasehealth();
        }
    }

    public void decreasehealth()
    {
        cur_Health -= 5f;
        if (cur_Health <= 0)
        {
            Destroy(this);
        }
        float calc_Health = cur_Health / max_Health;
        SetHealthBar(calc_Health);
    }

    void SetHealthBar(float myHealth)
    {
        healthBar.transform.localScale = new Vector3(myHealth, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
    }
}
