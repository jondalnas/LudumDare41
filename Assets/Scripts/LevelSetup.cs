using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSetup : MonoBehaviour {
	TilemapHandler tmh;
	public GameObject player;

	void Start() {
		Invoke("setupLevel", 0.1f);
	}

	public void setupLevel() {
		tmh = GameObject.Find("Grid").GetComponentInChildren<TilemapHandler>();
		tmh.getTile(3, 6).addCharacter(Instantiate<GameObject>(player).transform);
		tmh.getTile(5, 6).addCharacter(Instantiate<GameObject>(player).transform);
		tmh.getTile(3, 9).addCharacter(Instantiate<GameObject>(player).transform);
		tmh.getTile(1, 7).addCharacter(Instantiate<GameObject>(player).transform);
		tmh.getTile(6, 8).addCharacter(Instantiate<GameObject>(player).transform);
		tmh.getTile(7, 5).addCharacter(Instantiate<GameObject>(player).transform);
	}
}
