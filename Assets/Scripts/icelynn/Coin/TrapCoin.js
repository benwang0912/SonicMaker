var TrapSound : AudioSource;
var TrapBox : GameObject;
var TrapBox2 : GameObject;

function OnTriggerEnter (info : Collider) {
	ScoreCount.gscore += 20;
	Destroy(gameObject);
	TrapSound.Play();
	TrapBox.SetActive(true);
	TrapBox2.SetActive(true);

}