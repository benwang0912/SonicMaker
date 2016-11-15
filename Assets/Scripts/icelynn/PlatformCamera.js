var Hero : Transform;
var bshLogo : Texture2D;

function Update () {
	transform.position.x = Hero.position.x;
	transform.position.y = Hero.position.y + 2;
}

function OnGUI() {
	GUI.DrawTexture(Rect(10,10,80,90), bshLogo, ScaleMode.StretchToFill, true);
}