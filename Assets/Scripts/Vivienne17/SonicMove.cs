using UnityEngine;
using System.Collections;

public class SonicMove : MonoBehaviour {


    Animator anim;
    public GameObject ball;
    public float max_Health = 100f;
    public float cur_Health = 0f;
    public GameObject healthBar;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        cur_Health = max_Health;
        InvokeRepeating("decreasehealth", 1f, 1f);
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Space)) {
            anim.SetTrigger("isJumping");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            GameObject newball = Instantiate(ball);
            Destroy(newball, 2);
        }
    /*    if (HP <= 0) {
            anim.SetTrigger("isIdling");
            HP = 30;
        }
        HP = HP - 3 * Time.deltaTime;*/

    }
    void decreasehealth() {
        cur_Health -= 5f;
        if (cur_Health <= 0) {
            anim.SetTrigger("isIdling");
            cur_Health = max_Health;
        }
        float calc_Health = cur_Health / max_Health;
        SetHealthBar(calc_Health);
    }

    public void SetHealthBar(float myHealth) {
        healthBar.transform.localScale = new Vector3(myHealth, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
    }
}
