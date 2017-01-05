using UnityEngine;
using System.Collections;

public class TrainSpawner : MonoBehaviour {
	
	public GameObject EnemyTrain;
	public int gameState;

	void Start()
	{
		//SpawnTrain();
	}
	
	void Update()
	{

	}

	public void SpawnTrain()
	{
        TrainController trainScript = (TrainController)EnemyTrain.GetComponent(typeof(TrainController));
        trainScript.gameState = gameState;
        Instantiate (EnemyTrain,new Vector3(transform.position.x + (float)Mathf.Cos(-(transform.eulerAngles.y - 90) * Mathf.PI/180), transform.position.y + 2.5f,transform.position.z + (float)Mathf.Sin(-(transform.eulerAngles.y - 90) * Mathf.PI/180)), Quaternion.Euler(270,transform.eulerAngles.y + 90,0));
	}
}
