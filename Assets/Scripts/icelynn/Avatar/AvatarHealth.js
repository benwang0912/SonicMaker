
var DeathScene : GameObject;
var DeathText : GameObject;
var PlayAgainButton : GameObject;
var ReturnButton : GameObject;
var Avatar : GameObject;

function DeductPoints (hitpoints : int)
{
	ScoreCount.health -= hitpoints;
}

function Update ()
{
	if (ScoreCount.health <= 0) {
		Application.LoadLevel(2);
		/*
		DeathScene.SetActive(true);
		DeathText.SetActive(true);
		PlayAgainButton.SetActive(true);
		ReturnButton.SetActive(true);
		Avatar.SetActive(false);
		*/
	}
}


// Fixed the camera here
function FixedUpdate()
{
	transform.position.z = 15;
}