using UnityEngine;
using System.Collections;

public class ExplosionTest : MonoBehaviour {

	public AudioSource explosionSound;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void explosion(float f)
	{
		AudioSource ex = Instantiate (explosionSound);
		ex.pitch = f;
		ex.Play ();
	}
}
