using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerMana : Resource
{
	
	public Slider resourceSlider;
	public Image resourceFill;
	public Text resourceText;

	private void OnDisable()
	{
		GameManager.instance.playerCurrentManaPoints = currentResource;
		GameManager.instance.playerMaxManaPoints = maxResource;
	}

	private void Awake ()
	{
		resourceSlider = GameObject.Find("/InGameCanvas/HUD/ManaUI/PlayerManaSlider").GetComponent<Slider>();
		resourceFill = GameObject.Find("/InGameCanvas/HUD/ManaUI/PlayerManaSlider/Fill").GetComponent<Image>();
		resourceText = GameObject.Find("/InGameCanvas/HUD/ManaUI/PlayerManaSlider/PlayerManaText").GetComponent<Text>();

		stats = gameObject.GetComponent<PlayerStats>();
		currentStatName = "Intelligence";
		currentStat = stats.GetStatValueByName(currentStatName);

		if (GameManager.instance == null)
			SetResource(currentStat*10);
		else {
			SetResource(GameManager.instance.playerMaxManaPoints);
			SetResource(GameManager.instance.playerCurrentManaPoints);
		}

		base.Awake();
		this.CheckStat();
	}

	protected override void UpdateResource()
	{
		resourceText.text = (int)currentResource + "/" + (int)maxResource;
		resourceSlider.maxValue = (int)maxResource;
		resourceSlider.value = (int)currentResource;
	}

	public virtual bool UseResource (double amount)
	{
		if(currentResource>=amount)
		{
			currentResource-= amount;
			UpdateResource();
			return true;
		}
		resourceText.text = "Out of Mana";
		return false;
	}

}