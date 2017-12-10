using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class Resource : MonoBehaviour
{
	[SerializeField]protected double maxResource;                           
	[SerializeField]protected double currentResource;                           
	/*public Slider resourceSlider;
	public Image resourceFill;
	public Text resourceText;*/

	protected PlayerStats stats;
	protected double currentStat;
	protected string currentStatName;

	public double getMax()
	{
		return maxResource;
	}

	protected virtual void Awake ()
	{
		UpdateResource();
	}


	public virtual bool UseResource (double amount)
	{
		if(currentResource>=amount)
		{
			currentResource-= amount;
			UpdateResource();
			return true;
		}
		return false;
	}

	public virtual void TakeResource(double amount)
	{
		currentResource -= amount;
		VerifyResource ();
		UpdateResource();
	}

	public virtual void RestoreResource (double amount)
	{
		currentResource+= amount;
		VerifyResource ();
		UpdateResource();
	}

	public virtual void SetResource(double amount)
	{
		if(amount > maxResource)
			maxResource = currentResource = amount;
		else
			currentResource = amount;
		UpdateResource();
	}

	protected virtual void VerifyResource()
	{
		if (currentResource > maxResource)
			currentResource = maxResource;
		if(currentResource <= 0)
		{
			currentResource = 0;
		}
	}

	protected virtual void UpdateResource()
	{
		/*
		resourceText.text = (int)currentResource + "/" + (int)maxResource;
		resourceSlider.maxValue = (int)maxResource;
		resourceSlider.value = (int)currentResource;
		*/
	}

	public virtual void GainMaxResource(double amount)
	{
		maxResource += amount;
		currentResource += amount;
		VerifyResource ();
		UpdateResource();
	}

	public virtual void LoseMaxResource(double amount)
	{
		maxResource -= amount;
		VerifyResource ();
		UpdateResource();
	}

	public virtual void SetMaxResource(double amount)
	{
		
		if (maxResource < amount)
			currentResource+= (amount - maxResource);	
		maxResource = amount;
		VerifyResource ();
		UpdateResource();
	}

	public void CheckStat()
	{
		if(currentStat != stats.GetStatValueByName(currentStatName))
		{
			currentStat = stats.GetStatValueByName(currentStatName);
			SetMaxResource(currentStat*10);
		}
	}

		
}