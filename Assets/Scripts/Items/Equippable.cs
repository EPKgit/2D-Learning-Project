using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equippable : Item
{
	public List<BonusAddStat> adds;
	public List<BonusMulStat> muls;

	public override bool UseItem(GameObject user)
	{
		return false;
	}
	// Use this for initialization
	protected virtual void Start () {
		base.Start ();
		hasStats = true;
		isEquippable = true;
		adds = new List<BonusAddStat>();
		muls = new List<BonusMulStat>();
	}
}

