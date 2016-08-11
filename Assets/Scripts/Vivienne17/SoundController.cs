using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour {

    public AudioClip auStart;
    public AudioClip auGo;
    public AudioClip auOver;

    private float time;

    private bool countdown_3 = true;
    private bool countdown_2 = true;
    private bool countdown_1 = true;
    private bool go = true;
    private bool over = true;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (GameFunction.Instance.startPlay == true)
        {
            time += Time.deltaTime;

            if (time < 1 && countdown_3 == true)
            {
                AudioSource audio = GetComponent<AudioSource>();
                audio.PlayOneShot(auStart);
                countdown_3 = false;
            }
            else if (time > 1 && time < 2 && countdown_2 == true)
            {
                AudioSource audio = GetComponent<AudioSource>();
                audio.PlayOneShot(auStart);
                countdown_2 = false;
            }
            else if (time > 2 && time < 3 && countdown_1 == true)
            {
                AudioSource audio = GetComponent<AudioSource>();
                audio.PlayOneShot(auStart);
                countdown_1 = false;
            }
            else if (time > 3 && time < 4 && go == true)
            {
                AudioSource audio = GetComponent<AudioSource>();
                audio.PlayOneShot(auGo);
                go = false;
            }

            if (SonicMove.Instance.Died == true && over == true)
            {
                AudioSource audio = GetComponent<AudioSource>();
                audio.PlayOneShot(auOver);
                over = false;
            }
        }
    }
}
