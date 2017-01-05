using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {

	public GameObject Score;
    public GameObject CoinsTotal;

    private Text scoreTextMesh;
    private Text CoinTotalTextMesh;
    private string highScorePref, levelUnlockPref;
	public int level;

	void Start () {
		level = PlayerPrefs.GetInt("Level", 1);
		highScorePref = "HighScoreLevel" + level;
		levelUnlockPref = "LevelUnlock";
		if(level > PlayerPrefs.GetInt(levelUnlockPref,1))//if current level is locked
		{
			PlayerPrefs.SetInt(levelUnlockPref,level);//unlock current level
		}
		scoreTextMesh = Score.GetComponent<Text>();
		scoreTextMesh.text = "score: " + PlayerPrefs.GetInt ("Score", 0);

        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins", 0) + 1);
        CoinTotalTextMesh = CoinsTotal.GetComponent<Text>();
        CoinTotalTextMesh.text = "coins: " + PlayerPrefs.GetInt("Coins", 0);
        
        if (PlayerPrefs.GetInt ("Score", 0) > PlayerPrefs.GetInt (highScorePref, 0)) 
		{
			scoreTextMesh.text += "\nprevious highscore: " + PlayerPrefs.GetInt (highScorePref);
			PlayerPrefs.SetInt(highScorePref, PlayerPrefs.GetInt ("Score"));
			scoreTextMesh.text += "\nnew highscore: " + PlayerPrefs.GetInt ("Score");//PlayerPrefs.GetInt(highScorePref, 0);
		}else
		{
			scoreTextMesh.text += "\nhighscore: " + PlayerPrefs.GetInt (highScorePref);
		}
	}

	public void loadLevelAdd(int i)
	{
		Application.LoadLevel (level + 2 + i);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
