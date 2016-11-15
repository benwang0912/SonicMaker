var RightPointX : float = 130.03;
var RightPointY : float = 0.522;
var LeftPointX : float = 132.53;
var LeftPointY : float = 1.96;
var Direction : int = 1;

function Update () {
	if (Direction == 1) {
		transform.Translate(Vector3.left * 0.5 * Time.deltaTime, Space.World);
		transform.Translate(Vector3.up * -0.2 * Time.deltaTime, Space.World);
		Direction = 1;
	}

	if (this.transform.position.x < RightPointX || this.transform.position.y > RightPointY) {
		Direction = 2;
	}

	if (Direction == 2) {
		transform.Translate(Vector3.left * -0.5 * Time.deltaTime, Space.World);
		transform.Translate(Vector3.up * 0.2 * Time.deltaTime, Space.World);
		Direction = 2;
	}

	if (this.transform.position.x > LeftPointX || this.transform.position.y < LeftPointY) {
		Direction = 1;
	}
}