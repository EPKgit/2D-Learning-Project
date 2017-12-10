using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : Item {

	//public Image icon;
	//public string itemname, tooltip;
	//public int quantity;

	public override bool UseItem(GameObject user)
	{
		PlayerHealth temp = user.GetComponent<PlayerHealth> ();
		if (temp == null)
			return false;
		temp.RestoreResource(50);
		quantity -= 1;
		return true;
	}
	// Use this for initialization
	protected override void Start () {
		base.Start ();
		itemname = "Battery";
		tooltip = "Heals the player for 100 health.";
		quantity = 1;
	}

}
