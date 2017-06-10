using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {

	public void startGameBtn(string firstLevel) {
		SceneManager.LoadScene(firstLevel);
	}
	public void exitGameBtn() {
		Application.Quit ();
	}
	public void quitToMenu() {
		SceneManager.LoadScene("Menu");
	}
}
