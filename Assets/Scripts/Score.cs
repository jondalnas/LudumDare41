using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {
	public static int playerScore, enemyScore;

	public GameObject score;

	void Start () {
		
	}

	void Update () {
		score.GetComponent<Text>().text = playerScore + "|" + enemyScore;
	}
}
