using UnityEngine;
using System.Collections;

public class LoadLevel : MonoBehaviour 
{

	public int delay;

	public IEnumerator Delay()
	{
		yield return new WaitForSeconds (delay);
	}

	public void LoadScene(int level)
	{			
		Application.LoadLevel (level);
	}
}
