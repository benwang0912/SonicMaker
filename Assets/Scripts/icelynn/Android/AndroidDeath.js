var Android : GameObject;
var AndroidDeathSound : AudioSource;
var AttackBox : GameObject;
var Sphere : GameObject;
var Score : int = 15;


function OnTriggerEnter (col : Collider) {
	this.GetComponent("BoxCollider").enabled = false;
	AttackBox.GetComponent("BoxCollider").enabled = false;
	Sphere.GetComponent("SphereCollider").enabled = false;
	Android.GetComponent("AndroidMove").enabled = false;
	Android.transform.localScale -= new Vector3(0, 0.5, 0);
	AndroidDeathSound.Play();
	// Android.transform.localPosition -= new Vector3(0, 0.4, 0);
	yield WaitForSeconds (0.3);
	ScoreCount.gscore += Score;
	Android.transform.position = Vector3(0, -1000, 0);
}