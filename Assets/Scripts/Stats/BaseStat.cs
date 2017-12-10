using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStat
{

	private int baseValue;
	private List<BonusAddStat> addBonuses;
	private List<BonusMulStat> mulBonuses;
	private string statName, statDescription;

	public BaseStat(string m_name, string m_desc, int m_base)
	{
		addBonuses = new List<BonusAddStat>();
		mulBonuses = new List<BonusMulStat>();
		baseValue = m_base;
		statName = m_name;
		statDescription = m_desc;
	}

	public int GetFinalValue()
	{
		double temp = baseValue;
		addBonuses.ForEach(x => temp += x.additive);
		mulBonuses.ForEach(x => temp *= x.multiplier);
		return (int)temp;
	}

	public string getName()
	{
		return statName;
	}

	public string getDesc()
	{
		return statDescription;
	}

	public void AddAddBonus(BonusAddStat b)
	{
		addBonuses.Add(b);
	}

	public void RemoveAddBonus(BonusAddStat b)
	{
		addBonuses.Remove(b);
	}

	public void AddMulBonus(BonusMulStat b)
	{
		mulBonuses.Add(b);
	}

	public void RemoveMulBonus(BonusMulStat b)
	{
		mulBonuses.Remove(b);
	}
}
