static var gscore : int = 0;

function OnGUI() {
	GUI.Label (Rect (10, 10, 100, 20), ("Score: " + gscore));
}