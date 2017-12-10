using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrengthDrop : Equippable
{

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		itemname = "Sword";
		tooltip = "Adds +2 to Strength";
		quantity = 1;
		adds.Add(new BonusAddStat(2,"Strength"));
	}
}
