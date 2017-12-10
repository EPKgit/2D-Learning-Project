using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDController : MonoBehaviour {

	public GameObject inventoryScreen;
	public GameObject minimapScreen;
	public GameObject statsScreen;

	private bool inventoryScreenOpen;
	private bool minimapScreenOpen;
	private bool statsScreenOpen;

	void Start () {
		inventoryScreenOpen = false;
		minimapScreenOpen = false;
		statsScreenOpen = false;
		inventoryScreen.SetActive (inventoryScreenOpen);
		minimapScreen.SetActive (minimapScreenOpen);
		statsScreen.SetActive(statsScreenOpen);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("InventoryScreen"))
		{
			inventoryScreenOpen = !inventoryScreenOpen;
			inventoryScreen.SetActive (inventoryScreenOpen);
		}
		if (Input.GetButtonDown ("Minimap"))
		{
			minimapScreenOpen = !minimapScreenOpen;
			minimapScreen.SetActive (minimapScreenOpen);
		}

		if (Input.GetButtonDown ("CharacterScreen"))
		{
			statsScreenOpen = !statsScreenOpen;
			statsScreen.SetActive (statsScreenOpen);
		}

	}
}
