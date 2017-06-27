using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {
	public void startSceneBtn(string scene) {
		SceneManager.LoadScene(scene);
	}
	public void exitGameBtn() {
		Application.Quit ();
	}
	public void quitToMenu() {
		SceneManager.LoadScene("Menu");
	}
}
