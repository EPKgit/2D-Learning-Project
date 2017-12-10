using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

	[SerializeField]protected GameObject player;
	protected Camera cam;
	protected Vector3 offset;
	// Use this for initialization
	void Start ()
	{
		cam = this.GetComponent<Camera>();
		transform.rotation = new Quaternion(0,0,0,0);
		//offset = transform.position - player.transform.position;
	}

	public virtual void FindPlayer()
	{
		player = GameObject.FindGameObjectWithTag ("Player");	
	}


	// Update is called once per frame
	void LateUpdate ()
	{
		//Debug.Log (player.transform.position + " " + transform.position, this);
		if(player!= null)
			transform.position = player.transform.position + new Vector3(0,0,-1);
		if(Input.GetButtonDown("Zoom In"))
		{
			if(cam.orthographicSize > 1)
				cam.orthographicSize--;
		}
		if(Input.GetButtonDown("Zoom Out"))
		{
			if(cam.orthographicSize < 8)
				cam.orthographicSize++;
		}
	}
}
