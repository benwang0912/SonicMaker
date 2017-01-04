
function OnGUI() {
	GUI.Label (Rect (Screen.width/2 - 50, Screen.height/2, 200, 25), "You got the treasure!");
	if (GUI.Button(Rect(Screen.width/2 - 25, Screen.height/2 + 20 ,80,25),"Play Again!")) {
		Application.LoadLevel("level1");
	}
}