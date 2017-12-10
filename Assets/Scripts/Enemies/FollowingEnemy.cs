using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingEnemy : Enemy {

	protected override bool AttemptMove (int xDir, int yDir)
	{
		if(skipMove)
		{
			skipMove = false;
			return false;
		}

		if (xDir == 0 || yDir == 0)
			base.AttemptMove (xDir, yDir);
		else
		{		
			RaycastHit2D hit;
			bool canMoveX = Move (xDir, 0, out hit);
			bool canMoveY = Move (0, yDir, out hit);
			if (canMoveX && canMoveY) {
				if (Random.Range (0, 2) == 0)
					base.AttemptMove (xDir, 0);
				else
					base.AttemptMove (0, yDir);
			}
			else if(canMoveX)
				base.AttemptMove (xDir, 0);
			else
				base.AttemptMove (0, yDir);
		}

		skipMove = true;
		return false;

	}

	public override void MoveEnemy()
	{
		int xDir = 0;
		int yDir = 0;
		if (Mathf.Abs (target.position.y - transform.position.y) > float.Epsilon)
			yDir = target.position.y > transform.position.y ? 1 : -1;
		if (Mathf.Abs (target.position.x - transform.position.x) > float.Epsilon)
			xDir = target.position.x > transform.position.x ? 1 : -1;
		AttemptMove (xDir, yDir);
	}

}
