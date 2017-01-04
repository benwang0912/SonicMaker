var CrashSound : AudioSource;
var Flame : GameObject;
var FlameBox : GameObject;

function OnTriggerEnter (info : Collider) {
	Destroy(FlameBox);
	CrashSound.Play();
	Flame.SetActive(true);

}