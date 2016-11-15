var CoinSound : AudioSource;

function OnTriggerEnter (info : Collider) {
	Destroy(gameObject);
	CoinSound.Play();
	ScoreCount.gscore += 1;
}