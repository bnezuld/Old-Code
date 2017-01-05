using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Game_UI : MonoBehaviour {

    TrainEngineMainPlayer MainTrain;
	public Button pauseButton;
	public Button leftButton;
	public Button rightButton;
	public Button straightButton;
	public Button boostButton;
	public Button brakeButton;
	public Button resumeButton;
	public Button quitButton;
	public GameObject pauseMenu;
	public GameObject gameMenu;
	public GameObject gameOverMenu;
	public GameObject mapMenu;
	public GameObject MissionStatment;
	public Text scoreText;
	public Text CoalText;
	public Text mapTimeText;
	public Text timeText;
	public Camera mapCamera;
	public Camera mainCamera;

    public int gameState;
    public string Zone;

	private int timeOfLastTrainSpawn;
	private int trainsSpawned = 0;
	private int totalTrainsSpawned = 15;
	private int trainDestroyTime = 0;
	private bool addedTrainCar = false;
	public int prevScore;
	public float postScore;
	private bool mapActive = false;
	private float time = 60.0f;

	void Start()
	{
		MainTrain = (TrainEngineMainPlayer)GameObject.FindGameObjectWithTag ("MainPlayer").GetComponent (typeof(TrainEngineMainPlayer));
        MainTrain.gameState = gameState;

        GameObject[] enemyTrains = GameObject.FindGameObjectsWithTag("EnemyPlayer");
        trainsSpawned += enemyTrains.Length;
        foreach (GameObject enemys in enemyTrains)
        {
            TrainEngineEnemy enemy = (TrainEngineEnemy)enemys.GetComponent(typeof(TrainEngineEnemy));
            enemy.gameState = gameState;
        }

        GameObject[] TrainCars = GameObject.FindGameObjectsWithTag("TrainCar");
        foreach (GameObject trainCar in TrainCars)
        {
            TrainCarController trainC = (TrainCarController)trainCar.GetComponent(typeof(TrainCarController));
            trainC.gameState = gameState;
        }

        GameObject[] TrainSpawners = GameObject.FindGameObjectsWithTag("TrainSpawner");
        foreach (GameObject TrainSpawner in TrainSpawners)
        {
            TrainSpawner spawner = (TrainSpawner)TrainSpawner.GetComponent(typeof(TrainSpawner));
            spawner.gameState = gameState;
        }

        GameObject[] CoalSpawners = GameObject.FindGameObjectsWithTag("CoalSpawner");
        foreach (GameObject CoalSpawner in CoalSpawners)
        {
            CoalSpawner spawner = (CoalSpawner)CoalSpawner.GetComponent(typeof(CoalSpawner));
            spawner.gameState = gameState;
        }

        PlayerPrefs.SetInt ("Level", Application.loadedLevel - 2);
		timeOfLastTrainSpawn = 0;

        scoreText.text = "score: " + PlayerPrefs.GetInt("Score", 0);
        switch (gameState)
        {
            case 0:
                //scoreText.text = "trains to Destroy: " + PlayerPrefs.GetInt("Score", 0) + "/" + (totalTrainsSpawned);
                break;
            case 1:

                break;
            case 2:

                break;
            case 3:
                totalTrainsSpawned = 30 - PlayerPrefs.GetInt(Zone + "0", 0);
                scoreText.text = "trains to Destroy: " + PlayerPrefs.GetInt("Score", 0) + "/" + (totalTrainsSpawned);
                MainTrain.coal = (PlayerPrefs.GetInt(Zone + "1", 0));
                int CarsCollected = PlayerPrefs.GetInt(Zone + "2", 0);
                PlayerPrefs.SetString("zone", Zone);
                break;
        }

        Time.timeScale = 0;//pause game

		PlayerPrefs.SetInt ("Score", 0);//set score for the game to zero (reset it)

        //load sound bank
        uint bankID;
        AkSoundEngine.LoadBank("Trainwrex.bnk", AkSoundEngine.AK_DEFAULT_POOL_ID, out bankID);
        AkSoundEngine.PostEvent("Gameplay_Train_Loop_Stop", MainTrain.gameObject);

    }

	public void startGame()
	{
		Time.timeScale = 1;
		MissionStatment.SetActive (false);
        AkSoundEngine.PostEvent("Gameplay_Train_Loop", MainTrain.gameObject);
    }

    public void pauseGame()
	{
		if(Time.timeScale == 1)
		{
			Time.timeScale = 0;
			gameMenu.SetActive(false);
			pauseMenu.SetActive(true);
            AkSoundEngine.PostEvent("Gameplay_Train_Loop_Stop", MainTrain.gameObject);
        }
		else
		{
			Time.timeScale = 1;
			gameMenu.SetActive(true);
			pauseMenu.SetActive(false);
            AkSoundEngine.PostEvent("Gameplay_Train_Loop", MainTrain.gameObject);
        }
	}

	public void quitGame()
	{
		Application.LoadLevel(1);
		Time.timeScale = 1;
	}

	public void finishGame()
	{
		Time.timeScale = 1;
		Application.LoadLevel(2);
	}

	public void boost()
	{
		MainTrain.Boost();
		if(boostButton.image.color == Color.white)
			boostButton.image.color = Color.red;
	}

	public void brake()
	{
		MainTrain.Brake(!MainTrain.brake);
		if(brakeButton.image.color == Color.white)
			brakeButton.image.color = Color.red;
	}

	public void map(bool b)
	{
		mapActive = b;
		if(b)
		{
			mapMenu.SetActive(true);
			gameMenu.SetActive(false);
			mapCamera.gameObject.SetActive(true);
			mainCamera.gameObject.SetActive(false);
			Time.timeScale = 0;
		}else
		{
			mapMenu.SetActive(false);
			gameMenu.SetActive(true);
			mapCamera.gameObject.SetActive(false);
			mainCamera.gameObject.SetActive(true);
			Time.timeScale = 1;
		}
	}

	void FixedUpdate()
	{
        switch (gameState)
        {
            case 1://coal collecting mode

                break;
            case 2://Train Car collecting mode

                break;
            default://all other modes
                timeText.text = "" + (int)time;
                time -= Time.fixedDeltaTime;
                break;
        }

        //Debug.LogError(GameObject.FindGameObjectsWithTag("EnemyPlayer").Length + "");
        timeOfLastTrainSpawn++;
		if (timeOfLastTrainSpawn > 100) {
			if (GameObject.FindGameObjectsWithTag("EnemyPlayer").Length < 4 && trainsSpawned < totalTrainsSpawned)
            {
				GameObject[] spawner = new GameObject[10];
				spawner = GameObject.FindGameObjectsWithTag ("TrainSpawner");
				if (spawner.Length > 0) {
					int spawnIndex = Random.Range (0, spawner.Length);
					spawner [spawnIndex].SendMessage ("SpawnTrain");
					trainsSpawned++;
					timeOfLastTrainSpawn = 0;
				}
			}
		}

		int trains = GameObject.FindGameObjectsWithTag ("MainPlayer").Length;
		if(trains < 1)
		{
            EndGame();
			//trainDestroyTime++;
		}

        switch (gameState)
        {
            case 0:
                if (trainDestroyTime > 100 || PlayerPrefs.GetInt("Score", 0) >= totalTrainsSpawned || time <= 0.5f)
                {
                    gameMenu.SetActive(false);
                    pauseMenu.SetActive(false);
                    mapMenu.SetActive(false);
                    gameOverMenu.SetActive(true);
                    Time.timeScale = 0;
                    PlayerPrefs.SetInt(Zone + gameState, PlayerPrefs.GetInt("Score", 0));
                }
                break;
            case 1://coal collecting
                int coal = GameObject.FindGameObjectsWithTag("Coal").Length;
                if (coal < 1 || trainDestroyTime > 100)
                {
                    gameMenu.SetActive(false);
                    pauseMenu.SetActive(false);
                    mapMenu.SetActive(false);
                    gameOverMenu.SetActive(true);
                    Time.timeScale = 0;
                    PlayerPrefs.SetInt(Zone + gameState, PlayerPrefs.GetInt("Score", 0));
                }
                break;
            case 2://car collecting
                int addTrainCars = GameObject.FindGameObjectsWithTag("AddTrainCar").Length;
                if (trainDestroyTime > 100 || addTrainCars < 1)
                {
                    gameMenu.SetActive(false);
                    pauseMenu.SetActive(false);
                    mapMenu.SetActive(false);
                    gameOverMenu.SetActive(true);
                    Time.timeScale = 0;
                    PlayerPrefs.SetInt(Zone + gameState, PlayerPrefs.GetInt("Score", 0));
                }
                break;
            case 3://boss/survival
                if (trainDestroyTime > 100 || PlayerPrefs.GetInt("Score", 0) >= totalTrainsSpawned || time <= 0.5f)
                {
                    gameMenu.SetActive(false);
                    pauseMenu.SetActive(false);
                    mapMenu.SetActive(false);
                    gameOverMenu.SetActive(true);
                    Time.timeScale = 0;
                    PlayerPrefs.SetInt(Zone + gameState, PlayerPrefs.GetInt("Score", 0));
                }
                break;
            default:
                break;
        }

		if (MainTrain.coal >= 10) {
			boostButton.image.color = Color.white;
		}

		if (MainTrain.brake == false) {
			brakeButton.image.color = Color.white;
		}

		if(MainTrain)
		{
            switch (gameState)
            {
                case 3://boss/survival
                    CoalText.text = "Coal: " + MainTrain.coal;
                    break;
                default:
                    CoalText.text = "Coal: " + MainTrain.coal + "/50";
                    break;
            }
		}

		int totalCoalSpawners = GameObject.FindGameObjectsWithTag ("CoalSpawner").Length;
		GameObject[] coalSpawner = new GameObject[totalCoalSpawners];
		coalSpawner = GameObject.FindGameObjectsWithTag ("CoalSpawner");

		if(MainTrain && prevScore != PlayerPrefs.GetInt("Score", 0))
		{
            switch (gameState)
            {
                case 3://boss/survival
                    scoreText.text = "trains to Destroy: " + PlayerPrefs.GetInt("Score", 0) + "/" + (totalTrainsSpawned);
                    break;
                default:
                    scoreText.text = "score: " + PlayerPrefs.GetInt("Score", 0);
                    break;
            }
        }

		if(PlayerPrefs.GetInt("Score", 0)%25.0f == 0 && prevScore != PlayerPrefs.GetInt("Score", 0))
		{
			addedTrainCar = true;
			int x = Random.Range(0, totalCoalSpawners);
			CoalSpawner coalSpawnerScript = (CoalSpawner)coalSpawner[x].GetComponent(typeof(CoalSpawner));
			while(x < totalCoalSpawners)
			{
				coalSpawnerScript = (CoalSpawner)coalSpawner[x].GetComponent(typeof(CoalSpawner));
				if(coalSpawnerScript.AddTrainCar())
				{
					break;
				}
				x = Random.Range(0, totalCoalSpawners);
			}
		}
		if(PlayerPrefs.GetInt("Score", 0)%20.0f == 0 && prevScore != PlayerPrefs.GetInt("Score", 0))
		{
			int x = Random.Range(0, totalCoalSpawners);
			CoalSpawner coalSpawnerScript = (CoalSpawner)coalSpawner[x].GetComponent(typeof(CoalSpawner));
			while(x < totalCoalSpawners)
			{
				coalSpawnerScript = (CoalSpawner)coalSpawner[x].GetComponent(typeof(CoalSpawner));
				if(coalSpawnerScript.AddPellet())
				{
					break;
				}
				x = Random.Range(0, totalCoalSpawners);
			}
		}
		if(PlayerPrefs.GetInt("Score", 0)%20.0f == 0 && prevScore != PlayerPrefs.GetInt("Score", 0))
		{
			int x = Random.Range(0, totalCoalSpawners);
			CoalSpawner coalSpawnerScript = (CoalSpawner)coalSpawner[x].GetComponent(typeof(CoalSpawner));
			while(x < totalCoalSpawners)
			{
				coalSpawnerScript = (CoalSpawner)coalSpawner[x].GetComponent(typeof(CoalSpawner));
				if(coalSpawnerScript.AddSatalite())
				{
					break;
				}
				x = Random.Range(0, totalCoalSpawners);
			}
		}
		prevScore = PlayerPrefs.GetInt ("Score", 0);
	}
	void Update()
	{
		if (Input.GetKeyDown ("space"))
			boost ();
        if (Input.GetKeyDown("w"))
        {
            straight();
        }
        if (Input.GetKeyDown("a"))
        {
            turnLeft();
        }
        if (Input.GetKeyDown("d"))
        {
            turnRight();
        }
        if (Input.GetKeyDown ("s"))
		    brake();
		if (Input.GetKeyDown ("escape"))
			pauseGame ();
        if (Input.GetKeyDown("e"))
            MainTrain.detatchLastTrainCar();

        if (mapActive)
		{
			MainTrain.mapTime -= Time.fixedDeltaTime;
			mapTimeText.text = "" + (int)MainTrain.mapTime;
			if(MainTrain.mapTime <= 0)
			{
				map(false);
				MainTrain.mapTime = 0;
			}
		}

        if (MainTrain.nextTurn == 0)
        {
            straightButton.image.color = Color.yellow;
            rightButton.image.color = Color.white;
            leftButton.image.color = Color.white;
        }
    }
    public void turnLeft()
    {
        if (leftButton.image.color == Color.yellow)
        {
            leftButton.image.color = Color.white;
            MainTrain.nextTurn = 0;
        }
        else
        {
            MainTrain.nextTurn = -1;
            straightButton.image.color = Color.white;
            rightButton.image.color = Color.white;
            leftButton.image.color = Color.yellow;
        }
    }

    public void straight()
    {
        MainTrain.nextTurn = 0;
        straightButton.image.color = Color.yellow;
        rightButton.image.color = Color.white;
        leftButton.image.color = Color.white;
    }

    public void turnRight()
    {
        if (rightButton.image.color == Color.yellow)
        {
            rightButton.image.color = Color.white;
            MainTrain.nextTurn = 0;
        }
        else
        {
            MainTrain.nextTurn = 1;
            straightButton.image.color = Color.white;
            rightButton.image.color = Color.yellow;
            leftButton.image.color = Color.white;
        }
    }

    void EndGame()
    {
        gameMenu.SetActive(false);
        pauseMenu.SetActive(false);
        mapMenu.SetActive(false);
        gameOverMenu.SetActive(true);
        Time.timeScale = 0;
        PlayerPrefs.SetInt("TrainDestroyedScore", PlayerPrefs.GetInt("Score", 0));
    }


}
