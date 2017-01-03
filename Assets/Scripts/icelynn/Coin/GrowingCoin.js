var TrapSound : AudioSource;


function OnTriggerEnter (info : Collider) {
	Destroy(gameObject);
	TrapSound.Play();
	// Time += 20;
}