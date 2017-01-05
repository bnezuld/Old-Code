using UnityEngine;
using System.Collections;

public class StraightTrack : MonoBehaviour {

	void Start()
	{

	}

	void Update()
	{

	}

	void OnTriggerEnter(Collider other) 
	{
        if (other.GetComponent<TrainController>() != null && other.GetType() == typeof(BoxCollider))
        {
            //print(Vector3.Dot(transform.right.normalized, -other.transform.up.normalized) + " " + other.tag);

            if (Vector3.Dot(transform.right.normalized, -other.transform.up.normalized) > 0.5)//facing eachother
            {
                other.GetComponent<TrainController>().straightTrack(transform.eulerAngles.y);
            }
        }

        /*if ((other.gameObject.tag == "Player" || other.gameObject.tag == "MainPlayer" || other.gameObject.tag == "EnemyPlayer")  && other.GetType() == typeof(BoxCollider)) 
		{
			PlayerController playerController;
			playerController = other.gameObject.GetComponent <PlayerController> ();
			playerController.straightTrack(this.transform.eulerAngles.y);
			if(transform.rotation.eulerAngles.y == 0 || transform.rotation.eulerAngles.y == 180)
				playerController.transform.position = new Vector3(playerController.transform.position.x, playerController.transform.position.y, transform.position.z);
			if(transform.rotation.eulerAngles.y == 90 || transform.rotation.eulerAngles.y == 270)
				playerController.transform.position = new Vector3(transform.position.x, playerController.transform.position.y, playerController.transform.position.z);
		}else if(other.tag == "TrainCar" && other.GetType() == typeof(BoxCollider))
		{
			TrainCar TrainCar;
			TrainCar = other.gameObject.GetComponent <TrainCar> ();
			TrainCar.straightTrack(this.transform.eulerAngles.y);
            
            //Debug.LogError((int)transform.rotation.eulerAngles.y);
            if (TrainCar.getHeadTrainName() == "MainPlayer")
			if((int)transform.rotation.eulerAngles.y == 0 || (int)transform.rotation.eulerAngles.y == 180)
			{
				//Debug.LogError("train straightened 0,180");
				TrainCar.transform.position = new Vector3(TrainCar.transform.position.x, TrainCar.transform.position.y, transform.position.z);
			}else if((int)transform.rotation.eulerAngles.y == 90 || (int)transform.rotation.eulerAngles.y == 270)
			{
				//Debug.LogError("train straightened 90,270");
				TrainCar.transform.position = new Vector3(transform.position.x, TrainCar.transform.position.y, TrainCar.transform.position.z);
			}

		}*/
	}
}
