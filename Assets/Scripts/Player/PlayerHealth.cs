using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : Health
{
	/*
	public Image damageImage;                                   // Reference to an image to flash on the screen on being hurt.
	public AudioClip deathClip;                                 // The audio clip to play when the player dies.
	public float flashSpeed = 5f;                               // The speed the damageImage will fade at.
	public Color flashColour = new Color(1f, 0f, 0f, 0.1f);     // The colour the damageImage is set to, to flash.
	*/
	public Slider resourceSlider;
	public Image resourceFill;
	public Text resourceText;

	[SerializeField]private Animator anim;

	/*
	// Reference to the Animator component.
	AudioSource playerAudio;                                    // Reference to the AudioSource component.
	PlayerMovement playerMovement;                              // Reference to the player's movement.
	PlayerShooting playerShooting;                              // Reference to the PlayerShooting script.
	*/

	private void OnDisable()
	{
		GameManager.instance.playerCurrentHealthPoints = currentResource;
		GameManager.instance.playerMaxHealthPoints = maxResource;
	}

	private void Awake ()
	{
		anim = GetComponent <Animator> ();

		resourceSlider = GameObject.Find("/InGameCanvas/HUD/HealthUI/PlayerHealthSlider").GetComponent<Slider>();
		resourceFill = GameObject.Find("/InGameCanvas/HUD/HealthUI/PlayerHealthSlider/Fill").GetComponent<Image>();
		resourceText = GameObject.Find("/InGameCanvas/HUD/HealthUI/PlayerHealthSlider/PlayerHealthText").GetComponent<Text>();

		stats = gameObject.GetComponent<PlayerStats>();
		currentStatName = "Constitution";
		currentStat = stats.GetStatValueByName(currentStatName);

		if (GameManager.instance == null)
			SetResource( currentStat * 10);
		else {
			SetResource(GameManager.instance.playerMaxHealthPoints);
			SetResource(GameManager.instance.playerCurrentHealthPoints);
		}

		base.Awake();
		this.CheckStat();
		/*
		playerAudio = GetComponent <AudioSource> ();
		playerMovement = GetComponent <PlayerMovement> ();
		playerShooting = GetComponentInChildren <PlayerShooting> ();
		*/
	}

	public override void TakeResource (double amount)
	{
		base.TakeResource(amount);
		anim.SetTrigger ("PlayerHit");
	}
		
	protected override void UpdateResource()
	{

		resourceText.text = (int)currentResource + "/" + (int)maxResource;
		resourceSlider.maxValue = (int)maxResource;
		resourceSlider.value = (int)currentResource;

		double percentage = (double)currentResource/maxResource;
		if(percentage >= 0.75)
			resourceFill.color = Color.green;
		else if(percentage >= 0.50)
			resourceFill.color = Color.yellow;
		else if(percentage >= 0.25)
			resourceFill.color = Color.red;

	}
		

	protected override void Death ()
	{
		base.Death();
		StartCoroutine(GameManager.instance.GameOver());
		// Turn off any remaining shooting effects.
		//playerShooting.DisableEffects ();

		// Tell the animator that the player is dead.
		//anim.SetTrigger ("Die");

		// Set the audiosource to play the death clip and play it (this will stop the hurt sound from playing).
		//playerAudio.clip = deathClip;
		//playerAudio.Play ();

		// Turn off the movement and shooting scripts.
		//playerMovement.enabled = false;
		//playerShooting.enabled = false;
	}       
}