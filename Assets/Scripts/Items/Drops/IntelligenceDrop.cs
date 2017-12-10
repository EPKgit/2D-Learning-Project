using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntelligenceDrop: Equippable
{

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		itemname = "Staff";
		tooltip = "Adds +2 to Intelligence";
		quantity = 1;
		adds.Add(new BonusAddStat(2,"Intelligence"));
	}
}
