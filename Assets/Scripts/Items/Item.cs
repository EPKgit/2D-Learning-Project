using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


public abstract class Item : MonoBehaviour {

	public Sprite icon;
	protected string itemname, tooltip;
	protected bool isEquippable, hasStats;
	public int quantity;

	public abstract bool UseItem (GameObject user);

	protected virtual void Start () {
		icon = gameObject.GetComponent<SpriteRenderer> ().sprite;
		isEquippable = false;
		hasStats = false;
	}

}
