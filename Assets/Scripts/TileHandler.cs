using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHandler : MonoBehaviour {
	[HideInInspector]
	public Vector2Int tilemapPosition;

	Transform character;
	bool hasCharacter;
	bool hasBall;

	public bool containsCharacter() {
		return hasCharacter;
	}

	public bool containsPlayer() {
		return hasCharacter && character.CompareTag("Player");
	}

	public void removeCharacter() {
		character = null;
		hasCharacter = false;
	}

	public void addCharacter(Transform character) {
		character.position = transform.position+new Vector3(0.5f, 1.1f);
		character.GetComponentInChildren<SpriteRenderer>().sortingOrder = TilemapHandler.size.y-tilemapPosition.y;
		character.GetComponent<PlayerHandler>().tilePosition = tilemapPosition;
		this.character = character;
		hasCharacter = true;
	}

	public Transform getCharacter() {
		return character;
	}

	public bool getHasBall() {
		return hasBall;
	}

	public void setHasBall(bool hasBall) {
		this.hasBall = hasBall;
	}
}