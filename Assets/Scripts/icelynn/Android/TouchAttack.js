var HitSound : AudioSource;
var HitDamage : int = 5;

function OnTriggerEnter (col : Collider) {
	if (col.gameObject.tag == "Player") {
		this.GetComponent("BoxCollider").enabled = false;
		ScoreCount.health -= HitDamage;
		HitSound.Play();
		yield WaitForSeconds (1.5);
		this.GetComponent("BoxCollider").enabled = true;
	}

}