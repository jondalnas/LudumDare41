using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapHandler : MonoBehaviour {
	TileHandler[] tilemap;
	Tilemap tm;
	public static Vector2Int size;

	public GameObject tile;

	void Start() {
		tm = transform.GetComponent<Tilemap>();
		GameObject.Find("Movement Arrows").transform.position = tm.origin;
		tm.CompressBounds();
		size = new Vector2Int(tm.size.x, tm.size.y);
		tilemap = new TileHandler[tm.size.x*tm.size.y];

		for (int y = 0; y < size.y; y++) {
			for (int x = 0; x < size.x; x++) {
				tilemap[x + y * size.x] = Instantiate<GameObject>(tile, tm.CellToWorld(new Vector3Int(x, y, 0)+tm.origin), new Quaternion(), transform.Find("Tilemap")).GetComponent<TileHandler>();
				getTile(x, y).tilemapPosition = new Vector2Int(x, y);
			}
		}

		size = new Vector2Int(tm.size.x, tm.size.y);
	}

	public Vector2Int getTileAtPosition(Vector2 pos) {
		Vector3 tile = tm.WorldToCell(pos)-tm.origin;

		return new Vector2Int((int) tile.x, (int) tile.y);
	}
	
	public bool moveCharacter(Vector2Int from, Vector2Int to) {
		if (tilemap[to.x + to.y * size.x].containsCharacter() || !tilemap[from.x + from.y * size.x].containsCharacter()) return false; 

		tilemap[to.x + to.y * size.x].addCharacter(tilemap[from.x + from.y * size.x].getCharacter());
		tilemap[from.x + from.y * size.x].removeCharacter();
		return true;
	}

	public Vector3 roundToTilePosition(Vector3 position) {
		Vector2Int pos = getTileAtPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
		return new Vector3(pos.x, pos.y)+tm.origin;
	}

	public TileHandler getTile(int x, int y) {
		return tilemap[x + y * size.x];
	}
}
