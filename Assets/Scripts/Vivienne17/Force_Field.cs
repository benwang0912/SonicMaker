using UnityEngine;
using System.Collections;

public class Force_Field : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = GameObject.Find("Sonic").transform.position;
        transform.position += 2.0f * new Vector3(0, 1, 0);
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "Heart")
        {
            AudioSource audio = GetComponent<AudioSource>();
            audio.PlayOneShot(SonicMove.Instance.auGetHeart);

     //       addHPStart = Time.time;

     //       if (!shingshing.isPlaying)
     //           shingshing.Play();

            if (SonicMove.Instance.cur_Health >= 80f)
            {
                SonicMove.Instance.cur_Health = 100.0f;
            }
            else {
                SonicMove.Instance.cur_Health += 20f;
            }
        }

        if (collision.gameObject.name == "cone1_fbx")
        {
            /*AudioSource audio = GetComponent<AudioSource>();
            audio.PlayOneShot(auTheCone);

            if (cur_Health <= 10f)
            {
                cur_Health = 0.0f;
            }
            else {
                cur_Health -= 10f;
            }*/
        }

        if (collision.gameObject.tag == "Coin")
        {
            GameFunction.Instance.AddScore(20);
        }
    }
}
