var BoxCrashSound : AudioSource;

function OnTriggerEnter (col : Collider) {
	this.GetComponent("BoxCollider").enabled = false;
	if (col.gameObject.tag == "Player") {
		ScoreCount.health -= 30;
		Destroy(gameObject);
		BoxCrashSound.Play();
	}
	else {
		Destroy(gameObject);
		BoxCrashSound.Play();
	}

}