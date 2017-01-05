using UnityEngine;
using System.Collections;

public class GameMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}//start

	void Update () {
		if (Input.GetButtonDown ("Fire1")) //check for mouse right click
		{
			Ray ray = GetComponent<Camera>().ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
			RaycastHit hit;
			//Debug.Log (ray);
			if (Physics.Raycast(ray, out hit))
			{
				Button(hit);
			}
		}
		foreach( Touch touch in Input.touches ){
			if( touch.phase == TouchPhase.Began )
			{
				Ray ray = GetComponent<Camera>().ScreenPointToRay(new Vector3(touch.position.x, touch.position.y, 0));
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit))
				{
					Button(hit);					
				}				
			}
			
			
		}
	}//update

	void Button(RaycastHit hit)//passes 
	{
		//main menu
		if(hit.transform.name == "StartGame")
		{
			Application.LoadLevel("LoadLevelScreen");
		}
		
		if(hit.transform.name == "LoadGame")
		{
			Application.LoadLevel("LoadGameScreen");
		}

		if(hit.transform.name == "Options")
		{
			Application.LoadLevel("OptionScreen");
		}

		if(hit.transform.name == "ExitGame")
		{
			Application.Quit();
		}
			
		//load game and load level
		if(hit.transform.name == "MainMenu")
		{
			Application.LoadLevel("MainMenu");
		}
		
		//load level
		if(hit.transform.name == "Level1")//if the preivious level is completed and the game object Level2 is hit
		{
			PlayerPrefs.SetInt("Level",1);
			Application.LoadLevel("Level1");
		}

		for(int i = 2; i <= 3/*maxlevels*/; i++)
		{
			if(hit.transform.name == "Level" + i)//if the preivious level is completed and the game object Level2 is hit
			{
				if(true)//PlayerPrefs.GetInt(PlayerPrefs.GetString("currentPlayer", "player1") + "LevelUnlock", i-1) >= i)//used to check level unlock
				{
					PlayerPrefs.SetInt("Level", i);
					Application.LoadLevel("Level" + i);
				}else
				{
					Debug.Log("you must beat level all preivious levels to play this level " + i);
					//display message saying "you must beat level all preivious levels to play this level"
				}
			}
		}

		if(hit.transform.name == "DeletePlayer")
		{
			for(int i = 1; i <= PlayerPrefs.GetInt("LevelUnlock",1); i++)
				PlayerPrefs.DeleteKey("HighScoreLevel" + i);
			PlayerPrefs.DeleteKey("LevelUnlock");
			Application.LoadLevel("MainMenuScreen");
		}

	}//button
}//class