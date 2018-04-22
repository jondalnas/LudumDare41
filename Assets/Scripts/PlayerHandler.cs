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
	public Vector2Int tilePositionOrigin;
	public Vector2Int tilePositionStart;

	private bool turnEnded;
	private Vector2Int targetTile;
	private bool kicksBall;
	public bool hasBall;

	public bool move(TilemapHandler tmh, Vector2Int to) {
		if (tooFarAway(to)) return false;

		kicksBall = false;

		TileHandler onTile = TilemapHandler.getTile(tilePositionOrigin.x, tilePositionOrigin.y).GetComponent<TileHandler>();

		if (onTile.getCharacter() != null) {
			if ((to - tilePositionOrigin).y == 0) {
				to += Vector2Int.right * (((to - tilePositionOrigin).x < 0) ? -1 : 1);
			}
			else {
				to += Vector2Int.up * (((to - tilePositionOrigin).y < 0) ? -1 : 1);
			}
		}

		tmh.moveCharacter(tilePosition, to);

		if (hasBall = onTile.getHasBall()) {
			GameObject.Find("Ball").GetComponent<BallController>().owner = transform;
			GameObject.Find("Ball").transform.position = transform.position - new Vector3(0, 0.5f);
		}

		return true;
	}

	public void tryMove(TilemapHandler tmh, Vector2Int to) {
		if (!move(tmh, to)) {
			Vector2Int movement;

			if (Mathf.Abs(to.x - tilePositionOrigin.x) > speed) movement = Vector2Int.right * speed * (to.x - tilePositionOrigin.x < 0 ? -1 : 1);
			else movement = Vector2Int.right * (to.x - tilePositionOrigin.x);

			movement.y = (speed - Mathf.Abs(to.x - tilePositionOrigin.x)) * (to.y - tilePositionOrigin.y < 0 ? -1 : 1);

			move(tmh, tilePositionOrigin+movement);
		}
	}

	public void kick(TilemapHandler tmh, Vector2Int to) {
		if (!TilemapHandler.getTile(tilePositionOrigin).getHasBall() || outOfKickingRange(to)) return;

		BallController bc = GameObject.Find("Ball").GetComponent<BallController>();
		bc.transform.position = TilemapHandler.getTile(to).transform.position+new Vector3(0.5f, 0.5f);

		bc.destination = to;

		kicksBall = true;
	}

	public bool tooFarAway(Vector2Int to) {
		return Mathf.Abs((to - tilePositionOrigin).x) + Mathf.Abs((to - tilePositionOrigin).y) > speed;
	}

	public int moveDistanceTo(Vector2Int to) {
		return Mathf.Abs((to - tilePositionOrigin).x) + Mathf.Abs((to - tilePositionOrigin).y);
	}

	public bool outOfKickingRange(Vector2Int to) {
		return (to - tilePosition).sqrMagnitude > kickLength*kickLength;
	}

	public void endTurn() {
		if (tilePosition != tilePositionOrigin) transform.position = TilemapHandler.getTile(tilePositionOrigin).transform.position + new Vector3(0.5f, 1.1f);

		if (TilemapHandler.getTile(tilePositionOrigin).GetComponent<TileHandler>().getHasBall() && (tilePosition != tilePositionOrigin || kicksBall)) {
			if (!kicksBall) {
				TilemapHandler.getTile(tilePosition).GetComponent<TileHandler>().setHasBall(true);
			}

			TilemapHandler.getTile(tilePositionOrigin).GetComponent<TileHandler>().setHasBall(false);

			GameObject.Find("Ball").transform.position = transform.position - new Vector3(0, 0.5f);
		}

		tilePositionOrigin = tilePosition;
		turnEnded = true;
		targetTile = tilePosition;
	}

	void FixedUpdate() {
		if (turnEnded) {
			Vector3 dir = TilemapHandler.getTile(targetTile).transform.position-(transform.position-Vector3.up-new Vector3(0.5f, 0.1f));

			Vector3 movement = Vector3.zero;

			if ((dir.x * Vector3.right).sqrMagnitude > 0.02f) movement = Vector3.right * (dir.x < 0 ? -1 : 1) * 0.1f;
			else if ((dir.y * Vector3.up).sqrMagnitude > 0.02f) movement = Vector3.up * (dir.y < 0 ? -1 : 1) * 0.1f;
			else if (kicksBall) {
				kicksBall = false;
				GameObject.Find("Ball").GetComponent<BallController>().kick();
			} else {
				if (hasBall) GameObject.Find("Ball").GetComponent<BallController>().updatePosition();
				turnEnded = false;
			}
			
			transform.position += movement;

			if (hasBall) GameObject.Find("Ball").transform.position += movement;
			else if (TilemapHandler.getTile(TilemapHandler.getTileAtPosition(transform.position-Vector3.up*0.5f)).getHasBall()) {
				hasBall = true;

				TilemapHandler.getTile(TilemapHandler.getTileAtPosition(transform.position)).GetComponent<TileHandler>().setHasBall(false);
				TilemapHandler.getTile(tilePosition).GetComponent<TileHandler>().setHasBall(true);
			}
		}
	}

	public bool isTurnEnded() {
		return !turnEnded;
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

	public void reset() {
		TilemapHandler.getTile(tilePositionOrigin).removeCharacter();
		TilemapHandler.getTile(tilePositionStart).addCharacter(transform);
		tilePosition = tilePositionOrigin = targetTile = tilePositionStart;
		transform.position = TilemapHandler.getTile(tilePositionStart).transform.position + new Vector3(0.5f, 1.1f);
		turnEnded = false;
		kicksBall = false;
		hasBall = false;
	}
}
