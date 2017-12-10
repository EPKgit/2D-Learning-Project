using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

	public const int inventorySize = 16;
	public GameObject inventoryScreen;
	public Player player;
	public Item blank;


	[SerializeField]private List<Item> inv;
	private List<Sprite> itemImages;
	private List<GameObject> layoutImages;

	private bool isScreenOpen;

	// Use this for initialization
	void Start () {
		inventoryScreen = GameObject.Find("/InGameCanvas/HUD/InventoryScreen");

		inv = new List<Item> ();
		if (GameManager.instance.playerCurrentInventory != null) 
			inv.AddRange(GameManager.instance.playerCurrentInventory);			

		itemImages = new List<Sprite> ();
		for(int x = 0; x < inv.Count; x++)
			itemImages.Add (inv [x].icon);

		layoutImages = new List<GameObject> ();
		for(int x = 0; x < inventorySize; x++)
			layoutImages.Add(inventoryScreen.transform.FindChild ("Layout").transform.FindChild ("Slot" + x).gameObject);
		
		CheckInv ();
	}


	private void OnDisable()
	{
		GameManager.instance.playerCurrentInventory.Clear ();
		GameManager.instance.playerCurrentInventory.AddRange(inv);
	}


	public bool AddItem(Item i)
	{
		if (isFull ())
			return false;
		inv.Add (i);
		Equippable temp = i as Equippable;
		if(temp != null)
		{
			temp.adds.ForEach(x => player.GetComponent<PlayerStats>().AddAddStatBonus(x.ID,x));
			temp.muls.ForEach(x => player.GetComponent<PlayerStats>().AddMulStatBonus(x.ID,x));
		}
		itemImages.Add (i.icon);
		CheckInv ();
		return true;
	}

	public bool RemoveItem(Item i)
	{
		if (inv.Contains (i)) {
			inv.Remove (i);
			Equippable temp = i as Equippable;
			if(temp != null)
			{
				temp.adds.ForEach(x => player.GetComponent<PlayerStats>().RemoveAddStatBonus(x.ID,x));
				temp.muls.ForEach(x => player.GetComponent<PlayerStats>().RemoveMulStatBonus(x.ID,x));
			}
			itemImages.Remove (i.icon);
			CheckInv ();
			return true;
		}
		return false;
	}

	public bool isFull()
	{
		return inv.Count >= inventorySize;
	}

	public void CheckInv()
	{
		for (int x = 0; x < inv.Count; x++) {
			if (inv [x].quantity == 0)
				RemoveItem (inv [x]);
		}
		for(int x = 0; x < layoutImages.Count; x++)
		{
			if (x < itemImages.Count)
				layoutImages [x].GetComponent<Image> ().sprite = itemImages [x];
			else
				layoutImages [x].GetComponent<Image> ().sprite = null;
		}

	}

	// Update is called once per frame
	void Update () {

		if (Input.GetButtonDown ("Item 1")) {
			if (inv.Count >= 1) {
				if(inv [0].UseItem (gameObject))
				{
					CheckInv ();
					GameManager.instance.AdvanceTurn();
				}
			}
		}
		if (Input.GetButtonDown ("Item 2")) {
			if (inv.Count >= 2) {
				if(inv [1].UseItem (gameObject))
				{
					CheckInv ();
					GameManager.instance.AdvanceTurn();
				}
			}
		}
		if (Input.GetButtonDown ("Item 3")) {
			if (inv.Count >= 3) {
				if(inv [2].UseItem (gameObject))
				{
					CheckInv ();
					GameManager.instance.AdvanceTurn();
				}
			}
		}
		if (Input.GetButtonDown ("Item 4")) {
			if (inv.Count >= 4) {
				if(inv [3].UseItem (gameObject))
				{
					CheckInv ();
					GameManager.instance.AdvanceTurn();
				}
			}
		}
	}
}
