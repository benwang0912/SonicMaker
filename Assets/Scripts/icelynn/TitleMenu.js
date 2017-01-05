
// #pragma strict

import UnityEngine.SceneManagement;

function Start() {
	SceneManager.LoadScene("frigidzone", LoadSceneMode.Single);
}

function Quit() {
	SceneManager.LoadScene("EndingQuit", LoadSceneMode.Single);
}


// public class TitleButtonClass extends MonoBehaviour {
// 	function Start() {
// 		SceneManager.LoadScene("frigidzone", LoadSceneMode.Additive);
// 	}

// 	function Quit() {
// 		SceneManager.LoadScene("EndingQuit", LoadSceneMode.Additive);
// 	}


// }

