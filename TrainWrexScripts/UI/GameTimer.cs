using UnityEngine;
using System.Collections;

public class GameTimer : MonoBehaviour {
	
	float time = 60.0f;
	private TextMesh tm;
	private int prevScore;
	
	void Start () {
		tm = (TextMesh) transform.GetComponent(typeof(TextMesh));
	}
	
	void Update () {
		int score = PlayerPrefs.GetInt ("Score", 0);
		time -= Time.deltaTime;
		time += score - prevScore;
		tm.text = Mathf.RoundToInt(time).ToString();
		prevScore = score;
	}
	
	public int GetTime()
	{
		return Mathf.RoundToInt(time);
	}
}
