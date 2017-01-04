var SphereSound : AudioSource;
var HitDamage : int = 10;

function OnTriggerEnter (col : Collider) {
	if (col.gameObject.tag == "Player") {
		this.GetComponent("SphereCollider").enabled = false;
		ScoreCount.health -= HitDamage;
		SphereSound.Play();
		yield WaitForSeconds (1.5);
		this.GetComponent("SphereCollider").enabled = true;
	}

}