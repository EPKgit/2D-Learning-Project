using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Empty : Item {

	//public Image icon;
	//public string itemname, tooltip;
	//public int quantity;

	public override bool UseItem(GameObject user)
	{
		return false;
	}
	// Use this for initialization
	protected override void Start () {
		base.Start ();
		itemname = "";
		tooltip = "";
		quantity = 1;
	}
}
