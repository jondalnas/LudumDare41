using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Controller : MonoBehaviour {
	public GameObject selected;
	public Tile[] tiles;

	public static TileHandler selectedTile;

	private TilemapHandler tmh;
	private GameObject cursor;
	private GameObject br;
	private GameObject ma;

	private bool isKicking;

	void Start () {
		tmh = GameObject.Find("Grid").GetComponentInChildren<TilemapHandler>();
		cursor = GameObject.Find("Cursor");
		br = GameObject.Find("Ball Range");
		ma = GameObject.Find("Movement Arrows");
	}

	void Update () {
		Vector3 tile = tmh.roundToTilePosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));

		if (selectedTile == null)
			cursor.transform.position = tile + new Vector3(0.5f, 0.5f);
		else {
			if (Input.GetButton("Kick")) {
				br.SetActive(true);
				ma.SetActive(false);
				isKicking = true;
			} else{
				br.SetActive(false);
				ma.SetActive(true);
				isKicking = false;

				generateArrow(tile - GameObject.Find("Playing Area").GetComponent<Tilemap>().origin);
			}
		}

		if (Input.GetMouseButtonDown(0)) {
			Vector2Int highlightedTile = tmh.getTileAtPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			if (selectedTile == null) {
				if (tmh.getTile(highlightedTile.x, highlightedTile.y).containsCharacter()) {
					selectedTile = tmh.getTile(highlightedTile.x, highlightedTile.y);

					selected.transform.position = selectedTile.transform.position + new Vector3(0.5f, 0.5f);
					selected.SetActive(true);

					br.SetActive(true);
					selectedTile.getCharacter().GetComponent<PlayerHandler>().generateField();
					br.transform.position += br.GetComponent<Tilemap>().WorldToCell(selectedTile.getCharacter().transform.position)*new Vector3Int(1, 1, 0)-new Vector3Int(0, 1, 0);
					br.SetActive(false);
				}
			} else if (!isKicking) {
				if (selectedTile.tilemapPosition != highlightedTile) {
					selectedTile.getCharacter().GetComponent<PlayerHandler>().move(tmh, highlightedTile);
				}

				selectedTile = null;
				ma.SetActive(false);
				selected.SetActive(false);
				br.GetComponent<Tilemap>().ClearAllTiles();
			} else {
				selectedTile.getCharacter().GetComponent<PlayerHandler>().kick(tmh, highlightedTile);

				selectedTile = null;
				ma.SetActive(false);
				selected.SetActive(false);
				br.GetComponent<Tilemap>().ClearAllTiles();
			}
		}

		if (Input.GetMouseButtonDown(1)) {
			selectedTile = null;
			ma.SetActive(false);
			selected.SetActive(false);
			br.GetComponent<Tilemap>().ClearAllTiles();
		}
	}

	public void generateArrow(Vector3 end) {
		ma.GetComponent<Tilemap>().ClearAllTiles();

		Vector2Int curr = selectedTile.tilemapPosition;
		Vector2Int endPos = new Vector2Int((int) end.x, (int) end.y);

		for (int dir = (curr - endPos).x; (dir = (curr - endPos).x) != 0 && (dir < 100 && dir > -100);) {

			if (dir < 0) curr += new Vector2Int(1, 0);
			else curr -= new Vector2Int(1, 0);

			if ((curr - endPos).x == 0) ma.GetComponent<Tilemap>().SetTile(new Vector3Int(curr.x, curr.y, 0), tiles[(curr - endPos).y==0?(dir < 0 ? 7 : 9) : (((dir < 0)?5:4)-((curr - endPos).y>0?2:0))]);
			else ma.GetComponent<Tilemap>().SetTile(new Vector3Int(curr.x, curr.y, 0), tiles[0]);
		}

		for (int dir = (curr - endPos).y; (dir = (curr - endPos).y) != 0 && (dir < 100 && dir > -100);) {

			if (dir < 0) curr += new Vector2Int(0, 1);
			else curr -= new Vector2Int(0, 1);

			if ((curr - endPos).y == 0) ma.GetComponent<Tilemap>().SetTile(new Vector3Int(curr.x, curr.y, 0), tiles[dir < 0 ? 6 : 8]);
			else ma.GetComponent<Tilemap>().SetTile(new Vector3Int(curr.x, curr.y, 0), tiles[1]);
		}

		if (selectedTile.getCharacter().GetComponent<PlayerHandler>().tooFarAway(endPos)) ma.GetComponent<Tilemap>().color = Color.red;
		else ma.GetComponent<Tilemap>().color = Color.white;
		ma.SetActive(true);
	}
}
