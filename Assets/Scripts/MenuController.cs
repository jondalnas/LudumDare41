using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

	public void quit() {
		Application.Quit();
	}

	public void startGame() {
		SceneManager.LoadScene("Field");
	}

	public void exitGame() {
		SceneManager.LoadScene("Menu");
	}
}
