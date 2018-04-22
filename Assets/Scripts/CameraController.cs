using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour {
	public float scrollmargin = 0.1f;
	public float scrollSpeed = 0.5f;

	private float maxScroll;
	private float scroll;

	void Start() {
		maxScroll = ((GameObject.Find("Pretty Tiles").GetComponent<Tilemap>().CellToWorld(GameObject.Find("Pretty Tiles").GetComponent<Tilemap>().size) + GameObject.Find("Pretty Tiles").GetComponent<Tilemap>().origin) - Camera.main.ViewportToWorldPoint(Vector2.up)).y;
	}

	void FixedUpdate() {
		if (Input.mousePosition.y < Screen.height * scrollmargin) scroll -= scrollSpeed * ((Screen.height * scrollmargin - Input.mousePosition.y) / (Screen.height * scrollmargin));
		if (Input.mousePosition.y > Screen.height - Screen.height * scrollmargin) scroll += scrollSpeed * -(((Screen.height - Screen.height * scrollmargin) - Input.mousePosition.y) / (Screen.height * scrollmargin));
		
		if (scroll < -maxScroll) scroll = -maxScroll;
		if (scroll > maxScroll) scroll = maxScroll;

		transform.position = Vector3.up * scroll + Vector3.back * 10f + Vector3.right * 0.5f;
	}

	public void reset() {
		scroll = 0;
	}
}
