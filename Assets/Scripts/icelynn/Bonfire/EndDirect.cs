using UnityEngine;
using System.Collections;


public class EndDirect : MonoBehaviour {

	void OnTriggerEnter(Collider hit) {
		if (hit.gameObject.tag == "Player") {
			Application.LoadLevel(3);
		}
	}
}
