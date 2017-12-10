using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour {

	public GameObject Manager;

	void Awake ()
	{
		if(GameManager.instance==null)
			Instantiate (Manager);
	}

}
