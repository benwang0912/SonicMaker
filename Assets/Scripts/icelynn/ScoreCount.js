static var gscore : int = 0;
static var health : int = 100;
var timer_i : int = 1;

function OnGUI() {
	GUI.Label (Rect (30, 30, 300, 40), ("Score: " + gscore));
	GUI.Label (Rect (30, 10, 300, 20), ("Health: " + health));
}

function Update() {
	if ( timer_i == (Time.deltaTime/60) )
	{
		health -= 10;
		timer_i = timer_i + 1;
	}
}