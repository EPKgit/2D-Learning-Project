using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCameraController : CameraController
{

	void LateUpdate ()
	{
		//Debug.Log (player.transform.position + " " + transform.position, this);
		if(player!= null)
			transform.position = player.transform.position + new Vector3(0,0,-1);
	}
}
