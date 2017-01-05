using UnityEngine;
using System.Collections;

public class CarSpawner : MonoBehaviour {

	
	public GameObject policeCar;
	public GameObject semi;
	public GameObject oldBenz;
	public GameObject van;
	public GameObject lee;
	public GameObject truck;
	private int timeOfLastCarSpawn;
	private int RandomSpawnTime;
	
	void Start()
	{
		SpawnTrain();
		RandomSpawnTime = Random.Range (100, 500);
	}
	
	void FixedUpdate()
	{
		timeOfLastCarSpawn++;
		if (timeOfLastCarSpawn > RandomSpawnTime)
		{
			SpawnTrain();
			RandomSpawnTime = Random.Range (100, 500);
		}
	}
	
	public void SpawnTrain()
	{
		int rand = (int)Random.Range (0, 6);
		if(rand == 0)
		{
			Instantiate (policeCar,new Vector3(transform.position.x, transform.position.y + 0.5f ,transform.position.z), Quaternion.Euler(270,transform.localRotation.eulerAngles.y + 180,0));
		}else if(rand == 1)
		{
			Instantiate (semi,new Vector3(transform.position.x, transform.position.y + 2.0f ,transform.position.z), Quaternion.Euler(270,transform.localRotation.eulerAngles.y + 180,0));
		}else if(rand == 2)
		{
			Instantiate (oldBenz,new Vector3(transform.position.x, transform.position.y + 1.0f ,transform.position.z), Quaternion.Euler(270,transform.localRotation.eulerAngles.y + 180,0));
		}else if(rand == 3)
		{
			Instantiate (van,new Vector3(transform.position.x, transform.position.y + 1.5f ,transform.position.z), Quaternion.Euler(270,transform.localRotation.eulerAngles.y + 180,0));
		}else if(rand == 4)
		{
			Instantiate (lee,new Vector3(transform.position.x, transform.position.y + 0.5f ,transform.position.z), Quaternion.Euler(270,transform.localRotation.eulerAngles.y + 180,0));
		}else if(rand == 5)
		{
			Instantiate (truck,new Vector3(transform.position.x, transform.position.y + 0.75f ,transform.position.z), Quaternion.Euler(270,transform.localRotation.eulerAngles.y + 180,0));
		}

		timeOfLastCarSpawn = 0;
	}
	
	void OnTriggerExit(Collider other) 
	{
		
	}
	
	void OnTriggerEnter(Collider other) 
	{
		if (other.tag == "Car")
		{
			Destroy(other.gameObject);
		}
	}
}
