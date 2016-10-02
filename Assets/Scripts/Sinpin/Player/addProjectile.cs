using UnityEngine;
using System.Collections;

public class addProjectile : MonoBehaviour {
    static private PlayerMoving player;
    private genProjectile script;
    private GameObject manager;

    static private GameObject ShootInstruction;
    static bool instructionShowed = false;
    // Use this for initialization
    void Start () {
        if(player == null)
            player = GameObject.Find("Sonic").GetComponent<PlayerMoving>();
        manager = GameObject.Find("ProjectileManager");
        script = manager.GetComponent<genProjectile>();
        if (ShootInstruction == null)
        {
            ShootInstruction = GameObject.Find("ShootInstruction");
            ShootInstruction.SetActive(false);
        }
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.name == "Sonic")
        {
            player.audioSource.clip = player.playerAudio.addAbility;
            player.audioSource.Play();
            script.addProjectileUI();
            script.countDownToGen = 0;
            script.projectileGot++;
            Destroy(this.gameObject);
            if(instructionShowed == false)
            {
                ShootInstruction.SetActive(true);
                Time.timeScale = 0;
                instructionShowed = true;
            }
        }

    }
}
