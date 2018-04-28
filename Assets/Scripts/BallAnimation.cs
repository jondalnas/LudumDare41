using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallAnimation : MonoBehaviour {
	
	public void sortingLayer(int layer) {
		GetComponent<Canvas>().sortingOrder = layer;
	}

	public void changeScene() {
		GameObject.Find("-Game-").GetComponent<MenuController>().startGame();
	}
}
