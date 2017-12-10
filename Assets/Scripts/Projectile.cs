using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MovingObject
{
	[SerializeField]private string name;
	[SerializeField]private double damage;

	private bool customDir;
	[SerializeField]private Vector3 customDest;
	private Animator animator;

	protected override void Awake()
	{
		GameManager.instance.AddProjectileToList (this);
		customDir = false;
		customDest = new Vector3();
		base.Awake ();
	}

	public void setName(string s)
	{name = s;}

	public bool MoveProjectile()//returns true on self destruction
	{
		int xDir;
		int yDir;
		if(customDir && (transform.position - customDest).sqrMagnitude < float.Epsilon)
		{
			GameManager.instance.RemoveProjectileFromList(this);
			Destroy(this.gameObject);
			return true;
		}
		else if (customDir)
		{
			xDir = (int)(customDest.x - this.transform.position.x);
			yDir = (int)(customDest.y - this.transform.position.y);
			return AttemptMove (xDir, yDir);
		}
		else
		{
			xDir = getXDir();
			yDir = getYDir();
			return AttemptMove (xDir, yDir);
		}
		return false;
	}

	public void setCustomDestination(Vector3 destination)
	{
		customDir = true;
		customDest = new Vector3((int)destination.x, (int)destination.y, (int)destination.z);
	}

		
	public void setDirection (Direction d)
	{
		currDir = d;
		if(currDir == Direction.UP)
			this.transform.Rotate(Vector3.forward*270);
		if(currDir == Direction.DOWN)
			this.transform.Rotate(Vector3.forward*90);
		if (currDir == Direction.RIGHT)
			this.transform.Rotate(Vector3.forward*180);
	}

	public int getXDir()
	{
		switch(currDir)
		{
		case Direction.UP:{ return 0;} break;
		case Direction.RIGHT:{ return 1;} break;
		case Direction.DOWN:{ return 0; } break;
		case Direction.LEFT:{ return -1; } break;
		}
		return -3;
	}

	public int getYDir()
	{
		switch(currDir)
		{
		case Direction.UP:{ return 1;} break;
		case Direction.RIGHT:{ return 0;} break;
		case Direction.DOWN:{ return -1; } break;
		case Direction.LEFT:{ return 0; } break;
		}
		return -3;
	}

	protected override bool OnCantMove(GameObject objHit)
	{
		EnemyHealth hitEnemy = objHit.GetComponent<EnemyHealth>();
		PlayerHealth hitPlayer = objHit.GetComponent<PlayerHealth>();
		Projectile hitProj = objHit.GetComponent<Projectile>();

		if(hitProj != null)
			return false;
		GameManager.instance.RemoveProjectileFromList(this);
		if(hitPlayer != null)
			hitPlayer.TakeResource(damage);
		if (hitEnemy != null)
			hitEnemy.TakeResource(damage);
		Destroy(this.gameObject);
		return true;
	}

	public void setDamage(double d)
	{
		damage = d;
	}

}
