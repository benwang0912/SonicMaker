var Android : GameObject;
var AndroidDeathSound : AudioSource;

function OnTriggerEnter (col : Collider) {
	this.GetComponent("BoxCollider").enabled = false;
	Android.GetComponent("AndroidMove").enabled = false;
	Android.transform.localScale -= new Vector3(0, 0.5, 0);
	AndroidDeathSound.Play();
	// Android.transform.localPosition -= new Vector3(0, 0.4, 0);
	yield WaitForSeconds (0.3);
	Android.transform.position = Vector3(0, -1000, 0);
}