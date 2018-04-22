using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSetup : MonoBehaviour {
	public GameObject player;
	public GameObject enemy;

	void Start() {
		Invoke("setupLevel", 0.1f);
	}

	public void setupLevel() {
		TilemapHandler.addPlayer(2, 3, Instantiate<GameObject>(player).transform);
		TilemapHandler.addPlayer(5, 3, Instantiate<GameObject>(player).transform);
		TilemapHandler.addPlayer(9, 3, Instantiate<GameObject>(player).transform);
		TilemapHandler.addPlayer(12, 3, Instantiate<GameObject>(player).transform);
		TilemapHandler.addPlayer(6, 5, Instantiate<GameObject>(player).transform);
		TilemapHandler.addPlayer(8, 5, Instantiate<GameObject>(player).transform);

		TilemapHandler.addPlayer(2, 17, Instantiate<GameObject>(enemy).transform);
		TilemapHandler.addPlayer(5, 17, Instantiate<GameObject>(enemy).transform);
		TilemapHandler.addPlayer(9, 17, Instantiate<GameObject>(enemy).transform);
		TilemapHandler.addPlayer(12, 17, Instantiate<GameObject>(enemy).transform);
		TilemapHandler.addPlayer(6, 15, Instantiate<GameObject>(enemy).transform);
		TilemapHandler.addPlayer(8, 15, Instantiate<GameObject>(enemy).transform);
	}
}
