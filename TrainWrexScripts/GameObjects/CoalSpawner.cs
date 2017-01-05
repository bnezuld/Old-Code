using UnityEngine;
using System.Collections;

public class CoalSpawner : MonoBehaviour {
	
	public GameObject coal;
	public GameObject TrainCar;
	public GameObject Satalite;
	public GameObject pellet;
	public int gameState;
	private bool coalDisplayed;
	
	// Use this for initialization
	void Start () {
		GameObject g;
		g = (GameObject)Instantiate (coal,new Vector3(transform.position.x, transform.position.y - 2.5f,transform.position.z), Quaternion.Euler(270,0,0));
		g.transform.parent = transform;
	}

	public bool AddTrainCar()
	{
		if (gameState != 2 && !coalDisplayed && Time.timeScale != 0) {//will not spawn in car collecting mode
			GameObject g;
			g = (GameObject)Instantiate (TrainCar, new Vector3 (transform.position.x, transform.position.y + 2.0f, transform.position.z), Quaternion.Euler (270, 0, 0));
			g.transform.parent = transform;//
			return true;
		}
		return false;
	}

	public bool AddSatalite()
	{
		if (!coalDisplayed && Time.timeScale != 0) {
			GameObject g;
			g = (GameObject)Instantiate (Satalite, new Vector3 (transform.position.x, transform.position.y + 2.0f, transform.position.z), Quaternion.Euler (270, 0, 0));
			g.transform.parent = transform;//
			return true;
		}
		return false;
	}

	public bool AddPellet()
	{
		if (!pellet)
			return true;
		if (!coalDisplayed && Time.timeScale != 0) {
			GameObject g;
			g = (GameObject)Instantiate (pellet, new Vector3 (transform.position.x, transform.position.y + 2.0f, transform.position.z), Quaternion.Euler (270, 0, 0));
			g.transform.parent = transform;//
			return true;
		}
		return false;
	}

	// Update is called once per frame
	void Update () {
        coalDisplayed = transform.childCount >= 1;

        if (gameState != 1) {//will not spawn in coal mode
			if (!coalDisplayed && Random.Range (0, 1000) == 1 && Time.timeScale != 0) {
				GameObject g;
				g = (GameObject)Instantiate (coal, new Vector3 (transform.position.x, transform.position.y - 2.5f, transform.position.z), Quaternion.Euler (270, 0, 0));
				g.transform.parent = transform;//
			}
		}
	}
}
