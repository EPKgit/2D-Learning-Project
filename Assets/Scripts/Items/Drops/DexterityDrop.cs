using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DexterityDrop : Equippable
{

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		itemname = "Bow";
		tooltip = "Adds +2 to Dexterity";
		quantity = 1;
		adds.Add(new BonusAddStat(2,"Dexterity"));
	}
}
