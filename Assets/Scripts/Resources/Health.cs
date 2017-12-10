using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Health : Resource
{
	bool isDead;                                                // Whether the player is dead.
	bool damaged;                                               // True when the player gets damaged.

	/*void Update ()
	{
		if(damaged){
			// ... set the colour of the damageImage to the flash colour.
			//damageImage.color = flashColour;
		}
		// Otherwise...
		else{
			// ... transition the colour back to clear.
			//damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
		}
		// Reset the damaged flag.
		damaged = false;
	}*/


	/*public override void TakeResource (double amount)
	{
		// Set the damaged flag so the screen will flash.
		damaged = true;
		base.TakeResource(amount);
	}*/

	protected override void VerifyResource()
	{
		if (currentResource > maxResource)
			currentResource = maxResource;
		if(currentResource <= 0)
		{
			Death();
		}
	}

	protected virtual void Death ()
	{
		// Set the death flag so this function won't be called again.
		isDead = true;

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