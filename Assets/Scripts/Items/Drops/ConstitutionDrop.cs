using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstitutionDrop : Equippable
{

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		itemname = "Shield";
		tooltip = "Adds +2 to Constitution";
		quantity = 1;
		adds.Add(new BonusAddStat(2,"Constitution"));
	}
}
