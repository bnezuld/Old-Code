using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Score: MonoBehaviour {

	// Use this for initialization
	private	Text scoreTextMesh;
	private int score;

	void Start () {
		scoreTextMesh = GetComponent<Text>();
		score = 0;
		PlayerPrefs.SetInt ("Score", score);
		scoreTextMesh.text = "Score: " + score;
	}

	void Update()
	{
		scoreTextMesh.text = "Score: " + PlayerPrefs.GetInt ("Score", score);
	}
	
	// Update is called once per frame
	void UpdateScore (int addAmount) {
		//score += addAmount;
		//scoreTextMesh.text = "Score: " + score;
		//PlayerPrefs.SetInt ("Score", score);
	}

}
