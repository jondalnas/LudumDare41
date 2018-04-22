using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {
	[HideInInspector]
	public Vector2Int destination;
	public float height;

	public GameObject ownGoal, enemyGoal;

	private TilemapHandler tmh;

	private bool rolling;
	private Vector2Int startRoll;

	public Vector3 startPosition;
	public Vector2Int tilePosition;

	public Transform owner;

	void Start () {
		startPosition = transform.position;
		tmh = GameObject.Find("Playing Area").GetComponent<TilemapHandler>();
		Invoke("updatePosition", 0.1f);
	}
	
	void FixedUpdate () {
		if (rolling) {
			Vector3 dir = TilemapHandler.getTile(destination).transform.position - transform.position + new Vector3(0.5f, 0.5f);
			
			transform.position += new Vector3(dir.x, dir.y).normalized*0.15f;

			if (dir.sqrMagnitude < 0.15f || (TilemapHandler.getTile(TilemapHandler.getTileAtPosition(transform.position)).containsCharacter() && startRoll != TilemapHandler.getTileAtPosition(transform.position))) {
				updatePosition();
				rolling = false;
			}
		}
	}

	public void updatePosition() {
		TilemapHandler.clearBall();
		tilePosition = TilemapHandler.getTileAtPosition(transform.position);
		TilemapHandler.getTile(tilePosition).setHasBall(true);

		if (enemyGoal.GetComponent<Collider2D>().OverlapPoint(transform.position)) {
			Score.playerScore++;
			TilemapHandler.reset();
		}

		if (ownGoal.GetComponent<Collider2D>().OverlapPoint(transform.position)) {
			Score.enemyScore++;
			TilemapHandler.reset();
		}
	}

	public void kick() {
		rolling = true;
		startRoll = TilemapHandler.getTileAtPosition(transform.position);
	}

	public bool turnEnded() {
		return !rolling;
	}

	public void reset() {
		rolling = false;
		height = 0;
		TilemapHandler.getTile(TilemapHandler.getTileAtPosition(transform.position)).setHasBall(false);
		transform.position = startPosition;
		updatePosition();
	}
}
