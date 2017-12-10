using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpookyGhost : FollowingEnemy {


	protected override bool AttemptMove (int xDir, int yDir)
	{
		skipMove = false;
		return base.AttemptMove(xDir,yDir);
	}
	protected override bool OnCantMove(GameObject objHit)
	{
		Wall hitWall = objHit.GetComponent<Wall>();
		if(hitWall != null)
		{
			toIgnore.Add(objHit.GetComponent<BoxCollider2D>());
			MoveEnemy();
			return false;
		}

		PlayerHealth hitPlayer = objHit.GetComponent<PlayerHealth>();
		if (hitPlayer == null)
			return false;
		hitPlayer.TakeResource (playerDamage);
		return false;
	}

}
