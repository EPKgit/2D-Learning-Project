using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class MenuManager : MonoBehaviour {

	//public static MenuManager instance = null;

	void Awake ()
	{
		/*if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (this.gameObject);
		DontDestroyOnLoad(this);*/
		if(GameManager.instance!=null)
		{
			Destroy(GameManager.instance);
			GameManager.instance = null;
		}

	}

	public void StartRandomGenGame()
	{
		SceneManager.LoadScene("RandomGenGame");
	}
		
	public void StartTesting()
	{
		SceneManager.LoadScene("Testing");
	}
		
}
