var HealSound : AudioSource;

function OnTriggerEnter (col : Collider) {
	if (col.gameObject.tag == "Player")
	{
		ScoreCount.health += 20;
		Destroy(gameObject);
		HealSound.Play();
	}
	else {
		Destroy(gameObject);
		HealSound.Play();
	}
}