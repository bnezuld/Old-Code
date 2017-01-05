using UnityEngine;
using System.Collections;

public class Car : MonoBehaviour {

	bool Destroyed;
	// Use this for initialization
	void Start () {
		Destroyed = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(!Destroyed)
		{
			transform.position = new Vector3(transform.position.x - Mathf.Cos(transform.eulerAngles.y * (Mathf.PI/180)) ,transform.position.y,transform.position.z + Mathf.Sin(transform.eulerAngles.y * (Mathf.PI/180)));
		}
		else
		{
			GetComponent<Rigidbody>().AddForce((Physics.gravity * GetComponent<Rigidbody>().mass) * 2);//add 2 times gravity
			if ( Mathf.Abs(GetComponent<Rigidbody>().velocity.y) <= 0.01) 
			{
				Destroy(gameObject);
			}
		}
	}
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "MainPlayer" || other.tag == "EnemyPlayer" || other.tag == "TrainCar")
		{
			transform.GetComponent<Rigidbody>().velocity = new Vector3 (0, 15, 0);
			transform.GetComponent<Rigidbody>().AddExplosionForce(200, other.transform.position, 10);
			GetComponent<Collider>().isTrigger = false;//only add if you want cars to hit a sollid surface
			if(other.tag == "MainPlayer" && !Destroyed)
			{
				//PlayerPrefs.SetInt ("Score", PlayerPrefs.GetInt ("Score", 0) + 3);
			}
			Destroyed = true;
		}
	}


 	void OnDisable()
	{
		//when it destroyed spawn wrecked car parts
	}
}
