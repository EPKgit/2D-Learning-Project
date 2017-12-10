using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyHealth : Health
{
	[SerializeField]Animator anim;
	public int max;

	private void Awake ()
	{
		//anim = GetComponent <Animator> ();
		base.Awake();
		maxResource = max;
		currentResource = max;
	}

	/*public override void TakeResource (double amount)
	{
		base.TakeResource(amount);
		anim.SetTrigger ("PlayerHit");
	}*/

	protected override void Death ()
	{
		Enemy temp = this.gameObject.GetComponent<Enemy>();
		temp.Death();
	}       
}