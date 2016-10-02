using UnityEngine;
using System.Collections;

public class checkPoint : checkPointManager {
    static private PlayerMoving player;
    public bool activated = false;
    private ParticleSystem pillarR, pillarL, footGlow;
    private Component[] children;
    private GameObject[] checkPointList;
    private checkPoint[] CPScript;
    private GameObject star;
    private playerStats playerScript;

    static private GameObject saveInstruction;
    
    // Use this for initialization
    void Start() {
        if(player == null)
            player = GameObject.Find("Sonic").GetComponent<PlayerMoving>();
        star = Resources.Load("Sinpin/starParticle", typeof(GameObject)) as GameObject;
        playerScript = GameObject.Find("Sonic").GetComponent<playerStats>();
        children = GetComponentsInChildren<ParticleSystem>();
        checkPointList = GameObject.FindGameObjectsWithTag("CheckPoint");
        pillarL = GetSystem("pillarL");
        pillarR = GetSystem("pillarR");
        footGlow = GetSystem("footGlow");
        if (saveInstruction == null)
        {
            saveInstruction = GameObject.Find("SaveInstruction");
            saveInstruction.SetActive(false);
        }

	}
	
	// Update is called once per frame
	void Update () {
	    if(activated == false)
        {
            if(!pillarL.isPlaying)
                pillarL.Play();
            if (!pillarR.isPlaying)
                pillarR.Play();
            if (!footGlow.isPlaying)
            {
                footGlow.Simulate(1);
                footGlow.Play();
            }
           
        }else
        {
            if (pillarL.isPlaying)
                pillarL.Stop();
            if (pillarR.isPlaying)
                pillarR.Stop();
            if (footGlow.isPlaying)
                footGlow.Stop();
        }
    }
    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.name == "Sonic" && activated == false)
        {
            foreach (GameObject cp in checkPointList)
            {
                cp.GetComponent<checkPoint>().activated = false;
            }
            player.audioSource.clip = player.playerAudio.save;
            player.audioSource.Play();
            save(transform.position);

            //show saved animation
            GameObject s = Instantiate(star);
            s.transform.position = transform.position;
            Destroy(s, 2);

            activated = true;

            saveInstruction.SetActive(true);
        }
        else if (other.gameObject.name == "Sonic")
        {
            saveInstruction.SetActive(true);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.name == "Sonic")
        {
            saveInstruction.SetActive(false);
        }
    }
    private ParticleSystem GetSystem(string systemName)
    {
        
        foreach (ParticleSystem childParticleSystem in children)
        {
            if (childParticleSystem.name == systemName)
            {
                return childParticleSystem;
            }
        }
        return null;
    }
    void OnTriggerStay( Collider other )
    {
        if(other.name == "Sonic")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                player.audioSource.clip = player.playerAudio.save;
                player.audioSource.Play();
                save(transform.position);

                //show saved animation
                GameObject s = Instantiate(star);
                s.transform.position = transform.position;
                Destroy(s, 2);

            }
        }
    }
}
