using UnityEngine;
using System.Collections;

public class ExplosionParticle : MonoBehaviour {

	public float time;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		time += Time.deltaTime;
		if(time > 2)
		{
			Destroy(this.gameObject);
		}
	}
}
