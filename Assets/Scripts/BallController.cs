using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {
	[HideInInspector]
	public Vector2Int destination;
	public float height;

	private TilemapHandler tmh;
	void Start () {
		tmh = GameObject.Find("Playing Area").GetComponent<TilemapHandler>();
		Invoke("updatePosition", 0.1f);
	}
	
	void Update () {
	}

	public void updatePosition() {
		tmh.getTile(tmh.getTileAtPosition(transform.position).x, tmh.getTileAtPosition(transform.position).y).setHasBall(true);
	}
}
