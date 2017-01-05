
// #pragma strict

import UnityEngine.SceneManagement;

function PlayAgain() {
	SceneManager.LoadScene("frigidzone", LoadSceneMode.Single);
}

function Menu() {
	SceneManager.LoadScene("TitleMenu", LoadSceneMode.Single);
}


// public class DeathButtonClass extends MonoBehaviour {
// 	function PlayAgain() {
// 		SceneManager.LoadScene("frigidzone", LoadSceneMode.Additive);
// 	}

// 	function Menu() {
// 		SceneManager.LoadScene("TitleMenu", LoadSceneMode.Additive);
// 	}

// }



