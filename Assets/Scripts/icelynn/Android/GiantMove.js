
var RightPoint : float = 26.54;
var LeftPoint : float = 40.13;
var Direction : int = 1;
var MoveSpeedMultipier : float = 0.5;

function Update () {
	if (Direction == 1) {
		transform.Translate(Vector3.left * MoveSpeedMultipier * Time.deltaTime, Space.World);
		Direction = 1;
	}

	if (this.transform.position.x < RightPoint) {
		Direction = 2;
		//transform.Rotate(-2, 180, 0, Space.World);
	}

	if (Direction == 2) {
		transform.Translate(Vector3.left * -MoveSpeedMultipier * Time.deltaTime, Space.World);
		Direction = 2;
	}

	if (this.transform.position.x > LeftPoint) {
		Direction = 1;
		//transform.Rotate(2, -180, 0, Space.World);
	}
}