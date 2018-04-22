using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapHandler : MonoBehaviour {
	static Tilemap tm;
	static TileHandler[] tilemap;
	public static List<Transform> players = new List<Transform>();
	public static List<Transform> enemies = new List<Transform>();

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

	public static Vector2Int getTileAtPosition(Vector2 pos) {
		Vector3 tile = tm.WorldToCell(pos)-tm.origin;

		if (tile.x < 0) tile.x = 0;
		if (tile.y < 0) tile.y = 0;
		if (tile.x >= tm.size.x-1) tile.x = tm.size.x-1;
		if (tile.y >= tm.size.y-1) tile.y = tm.size.y-1;

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

	public static void addPlayer(int x, int y, Transform player) {
		TileHandler tile = getTile(x, y);

		if (tile.containsCharacter()) return;

		player.GetComponent<PlayerHandler>().tilePositionOrigin = player.GetComponent<PlayerHandler>().tilePositionStart = new Vector2Int(x, y);
		tile.addCharacter(player);

		if (player.CompareTag("Player"))
			players.Add(player);
		else if (player.CompareTag("Enemy"))
			enemies.Add(player);
	}

	public static void endTurn() {
		foreach (Transform player in players) {
			player.GetComponent<PlayerHandler>().endTurn();
		}
	}

	public static bool hasTurnEnded() {
		foreach (Transform player in players) {
			if (!player.GetComponent<PlayerHandler>().isTurnEnded()) return false;
		}

		foreach (Transform enemy in enemies) {
			if (!enemy.GetComponent<PlayerHandler>().isTurnEnded()) return false;
		}

		return GameObject.Find("Ball").GetComponent<BallController>().turnEnded();
	}

	public static void endEnemyTurn() {
		foreach (Transform enemy in enemies) {
			enemy.GetComponent<PlayerHandler>().endTurn();
		}
	}

	public static void reset() {
		foreach (Transform player in players) player.GetComponent<PlayerHandler>().reset();
		foreach (Transform enemy in enemies) enemy.GetComponent<PlayerHandler>().reset();

		foreach (TileHandler tile in tilemap) {
			tile.setHasBall(false);
		}

		GameObject.Find("Ball").GetComponent<BallController>().reset();

		Camera.main.GetComponent<CameraController>().reset();
	}

	public static void clearBall() {
		foreach (TileHandler tile in tilemap) {
			tile.setHasBall(false);
		}
	}

	public static TileHandler getTile(int x, int y) {
		if (x < 0) x = 0;
		if (y < 0) y = 0;
		if (x >= size.x-1) x = size.x-1;
		if (y >= size.y-1) y = size.y-1;

		return tilemap[x + y * size.x];
	}

	public static TileHandler getTile(Vector2Int tile) {
		return getTile(tile.x, tile.y);
	}
}
