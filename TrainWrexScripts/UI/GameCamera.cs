using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour {

	public GameObject GameOverButton;
	public GameObject PauseGameScreen;
	public GameObject GameTimer;
	public TrainEngineMainPlayer MainTrain;
	private int TrainAmount, timeOfLastTrainSpawn;
	private bool GameOver = false;
	private GameTimer time;

	private int left, up;

	// Use this for initialization
	void Start () {

		time = (GameTimer) GameTimer.GetComponent(typeof(GameTimer));
	}

	public void OnGUI() {
		Event e = Event.current;
		//if(e.isKey)
		if (e.keyCode == KeyCode.Escape && GameOver == false) {
			PauseGameScreen.SetActive(true);
			Time.timeScale = 0;
		}

	}

	void FixedUpdate()
	{
		/*int i = GameObject.FindGameObjectsWithTag("MainPlayer").Length;//gets total number of game objects with the "player" tag
		if ((i < 1 || time.GetTime() <= 0 || PlayerPrefs.GetInt ("Score", 0) > 100) && !GameOver )//i < TrainAmount) for anytrain destroyed game over
		{
			//Instantiate(GameOverButton, new Vector3(transform.position.x,transform.position.y - 1,transform.position.z), Quaternion.Euler(90,0,0));
			GameOverButton.SetActive(true);
			GameOver = true;
			
			Time.timeScale = 0;
			//Application.LoadLevel("GameOverScreen");
		}*/
	}

	// Update is called once per frame
	void Update () {

	}

}
