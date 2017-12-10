using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject
{
	public int playerDamage;

	protected Animator animator;
	protected Transform target;
	protected bool skipMove;

	protected void OnDisable()
	{
		GameManager.instance.RemoveEnemyFromList(this);
		boxCollider.enabled = false;
	}

	protected virtual void Awake()
	{
		animator = GetComponent<Animator> ();
		target = GameObject.FindGameObjectWithTag ("Player").transform;
		GameManager.instance.AddEnemyToList (this);
		base.Awake ();
	}

	public virtual void MoveEnemy(){}

	protected override bool OnCantMove(GameObject objHit)
	{
		PlayerHealth hitPlayer = objHit.GetComponent<PlayerHealth>();
		if (hitPlayer == null)
			return false;
		hitPlayer.TakeResource (playerDamage);
		animator.SetTrigger ("EnemyAttack");
		return false;
	}

	public virtual void Death()
	{
		this.boxCollider.enabled = false;
		animator.SetTrigger("EnemyDeath");
		GameManager.instance.RemoveEnemyFromList(this);
		Destroy(this.gameObject, 1.67f );
	}

}
