using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{

	public const int numStats = 4;
	public string[] statsName = new string[numStats];
	public string[] statsDesc = new string[numStats];

	private GameObject statsScreen;
	[SerializeField]private Text[] statsText;

	private BaseStat[] stats;

	private List<Resource> resources;

	void Awake()
	{
		resources = new List<Resource>();
		resources.Add(gameObject.GetComponent<PlayerHealth>());
		resources.Add(gameObject.GetComponent<PlayerMana>());

		stats = new BaseStat[numStats];
		for(int x = 0; x < stats.Length; x++)
			stats[x] = new BaseStat(statsName[x],statsDesc[x], 10);	

		statsScreen = GameObject.Find("/InGameCanvas/HUD/StatsScreen");

		statsText = new Text[numStats];
		for(int x = 0; x < numStats; x++)
			statsText[x] = (statsScreen.transform.FindChild ("Layout").transform.FindChild ("Stat" + x).GetComponent<Text>());

		UpdateText(true);
	}

	void Update()
	{
		if(Input.GetButtonDown("Ability 4"))
		{
			AddAddStatBonus("all",new BonusAddStat(1,"000"));
		}
	}


	public void UpdateText(bool first)
	{
		for(int x = 0; x < numStats; x++)
		{
			statsText[x].text = stats[x].getName() + ": "+ stats[x].GetFinalValue() + "\n" + stats[x].getDesc();
		}
		if(!first)
			resources.ForEach(x=>x.CheckStat());
	}

	public int GetStatValueByName(string name)
	{
		for(int x = 0; x < numStats; x++)
			if(stats[x].getName() == name)
				return stats[x].GetFinalValue();
		return -1;
	}

	public int GetStatValueByIndex(int index)
	{
		if(index > numStats)
			return -1;
		return stats[index].GetFinalValue();
	}

	public BaseStat GetStatByName(string name)
	{
		for(int x = 0; x < numStats; x++)
			if(stats[x].getName() == name)
				return stats[x];
		return null;
	}

	public void AddAddStatBonus(string name, BonusAddStat bonus)
	{
		if(name == "all")
			foreach(BaseStat b in stats)
				b.AddAddBonus(bonus);
		
		for(int x = 0; x < numStats; x++)
			if(stats[x].getName() == name)
				stats[x].AddAddBonus(bonus);
		UpdateText(false);
	}

	public void AddMulStatBonus(string name, BonusMulStat bonus)
	{
		if(name == "all")
			foreach(BaseStat b in stats)
				b.AddMulBonus(bonus);

		for(int x = 0; x < numStats; x++)
			if(stats[x].getName() == name)
				stats[x].AddMulBonus(bonus);
		UpdateText(false);
	}

	public void RemoveAddStatBonus(string name, BonusAddStat bonus)
	{
		if(name == "all")
			foreach(BaseStat b in stats)
				b.RemoveAddBonus(bonus);

		for(int x = 0; x < numStats; x++)
			if(stats[x].getName() == name)
				stats[x].RemoveAddBonus(bonus);
		UpdateText(false);
	}

	public void RemoveMulStatBonus(string name, BonusMulStat bonus)
	{
		if(name == "all")
			foreach(BaseStat b in stats)
				b.RemoveMulBonus(bonus);

		for(int x = 0; x < numStats; x++)
			if(stats[x].getName() == name)
				stats[x].RemoveMulBonus(bonus);
		UpdateText(false);
	}
}
