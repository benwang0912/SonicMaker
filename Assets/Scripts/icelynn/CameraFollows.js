var Player : Transform;
var DistanceFromPlayerZ : float = -5;
var DistanceFromPlayerY : float = -7;
// var StaticCameraY : float = 3;

function Update () {
	transform.position.z = Player.position.z - DistanceFromPlayerZ;
	transform.position.y = Player.position.y - DistanceFromPlayerY;
	transform.position.x = Player.position.x;
}

// function LateUpdate () {
// 	GetComponent.<Camera>().main.transform.position.y = StaticCameraY;
// }

