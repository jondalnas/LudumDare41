using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerHandler : MonoBehaviour {
	public int speed = 6;
	public int kickLength = 15;
	public float kickAccuracy = 0.1f;
	public float highKickHeight = 5;
	public float jumpHeight = 0.5f;
	public int tackle = 50;
	public float playerHeight = 1.75f;

	public Tile[] tiles;

	public Vector2Int tilePosition;

	public void move(TilemapHandler tmh, Vector2Int to) {
		if (tooFarAway(to)) return;

		TileHandler onTile = tmh.getTile(tilePosition.x, tilePosition.y).GetComponent<TileHandler>();

		tmh.moveCharacter(tilePosition, to);

		if (onTile.getHasBall()) {
			onTile.setHasBall(false);
			tmh.getTile(to.x, to.y).GetComponent<TileHandler>().setHasBall(true);
			GameObject.Find("Ball").transform.position = transform.position - new Vector3(0, 0.5f);
		}
	}

	public void kick(TilemapHandler tmh, Vector2Int to) {
		if (outOfKickingRange(to)) return;

		BallController bc = GameObject.Find("Ball").GetComponent<BallController>();
		tmh.getTile(tmh.getTileAtPosition(bc.transform.position).x, tmh.getTileAtPosition(bc.transform.position).y).setHasBall(false);
		bc.transform.position = tmh.getTile(to.x, to.y).transform.position+new Vector3(0.5f, 0.5f);
		bc.updatePosition();
	}

	public bool tooFarAway(Vector2Int to) {
		return Mathf.Abs((to - tilePosition).x) + Mathf.Abs((to - tilePosition).y) > speed;
	}

	public bool outOfKickingRange(Vector2Int to) {
		return (to - tilePosition).sqrMagnitude > kickLength*kickLength;
	}

	public void generateField() {
		Tilemap tm = GameObject.Find("Ball Range").GetComponent<Tilemap>();

		bool[] inRange = new bool[4 * kickLength * kickLength];

		for (int y = -kickLength; y < kickLength; y++) {
			for (int x = -kickLength; x < kickLength; x++) {
				inRange[(x + kickLength) + (y + kickLength) * 2 * kickLength] = x * x + y * y < kickLength * kickLength;
			}
		}

		for (int y = 0; y < 2 * kickLength; y++) {
			for (int x = 0; x < 2 * kickLength; x++) {
				if (inRange[x + y * 2 * kickLength]) {
					Vector3Int pos = new Vector3Int(x-kickLength, y-kickLength, 0);

					bool up = (y+1) < 2 * kickLength &&    inRange[x + (y + 1) * 2 * kickLength];
					bool down = y > 0 &&                   inRange[x + (y - 1) * 2 * kickLength];
					bool right = (x+1) < 2 * kickLength && inRange[(x + 1) + y * 2 * kickLength];
					bool left = x > 0 &&                   inRange[(x - 1) + y * 2 * kickLength];

					if (!up)    tm.SetTile(pos, tiles[1]);
					if (!down)  tm.SetTile(pos, tiles[6]);
					if (!left)  tm.SetTile(pos, tiles[3]);
					if (!right) tm.SetTile(pos, tiles[4]);

					if (!up   && !right) tm.SetTile(pos, tiles[2]);
					if (!up   && !left)  tm.SetTile(pos, tiles[0]);
					if (!down && !right) tm.SetTile(pos, tiles[7]);
					if (!down && !left)  tm.SetTile(pos, tiles[5]);
				}
			}
		}

		tm.CompressBounds();
	}
}
