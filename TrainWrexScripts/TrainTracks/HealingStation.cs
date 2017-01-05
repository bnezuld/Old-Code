using UnityEngine;
using System.Collections;

public class HealingStation : MonoBehaviour {

	public GameObject redGlow;
	public GameObject greenGlow;

	private TrainController train;
	private int num;

	// Use this for initialization
	void Start () {
	
	}

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "Player" || other.gameObject.tag == "MainPlayer")//destroys train when its either 
		{
			if(other.gameObject.tag == "MainPlayer")//BUG: runs through this code twice and add 2 to the score instead of just 1
			{
				train = (TrainController)other.gameObject.GetComponent(typeof(TrainController));
				redGlow.SetActive(true);
				if(train.getHealth() >= train.getMaxHealth())
				{
					redGlow.SetActive(false);
					greenGlow.SetActive(true);
				}
			}
		}
	}

	void OnTriggerExit(Collider other) {
		if(other.gameObject.tag == "Player" || other.gameObject.tag == "MainPlayer")//destroys train when its either 
		{
			if(other.gameObject.tag == "MainPlayer")//BUG: runs through this code twice and add 2 to the score instead of just 1
			{
				train = null;
				redGlow.SetActive(false);
				greenGlow.SetActive(false);
			}
		}
	} 
	
	// Update is called once per frame
	void FixedUpdate () {
			if(train)
			{
				num++;
				if(num > 25)
				{
					num = 0;
					train.heal(1);
				}
				if(train.getHealth() >= train.getMaxHealth())
				{
					redGlow.SetActive(false);
					greenGlow.SetActive(true);
				}
			}
	}
}
