using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MovingObject
{

	public GameObject tempRock;
	public GameObject tempFire;

	public int pointsPerFood;
	public int pointsPerSoda;
	public float restartLevelDelay = 1f;
	public float manaRegenRate = 0.015f;

	private Animator animator;
	private PlayerHealth health;
	private PlayerMana mana;
	private Inventory inv;
	private PlayerStats stats;


	protected override void Awake ()
	{
		animator = GetComponent<Animator> ();
		health = GetComponent<PlayerHealth> ();
		mana =  GetComponent<PlayerMana> ();
		inv = GetComponent<Inventory> ();
		stats = GetComponent<PlayerStats>();
		base.Awake ();
	}

	public void FinishPlayersTurn(bool eots)
	{
		// timers tick down
		if(eots)
			EndOfTurnEffects();
		GameManager.instance.AdvanceTurn();
	}

	protected override bool AttemptMove (int xDir, int yDir)
	{
		potentialMoves.Clear();
		base.AttemptMove (xDir, yDir);
		FinishPlayersTurn(true);
		return false;
	}

	public void EndOfTurnEffects()
	{
		mana.RestoreResource(mana.getMax()*manaRegenRate);
	}

	// Update is called once per frame
	void Update ()
	{

		if (!(GameManager.instance.GetTurn() == 1))
			return;
		int horizontal = (int) Input.GetAxisRaw("Horizontal");
		int vertical = (int) Input.GetAxisRaw("Vertical");

		if( horizontal != 0)
			vertical = 0;

		if (horizontal == -1) 
			currDir = Direction.LEFT;
		if (horizontal == 1)
			currDir = Direction.RIGHT;
		if (vertical == -1)
			currDir = Direction.DOWN;
		if (vertical == 1)
			currDir = Direction.UP;
		checkDirection ();

		if ((horizontal != 0 || vertical != 0) && !MovingObject.isMoving) 
		{
			AttemptMove(horizontal, vertical);
			Input.ResetInputAxes();
		}

		if(Input.GetButton("Ability 1"))
		{
			if(mana.UseResource(50))
			{
				GameObject temp = Instantiate(tempRock, (this.transform.position+new Vector3(0,0,0)), Quaternion.identity);
				temp.GetComponent<Projectile>().setDirection(currDir);
				temp.GetComponent<Projectile>().addIgnore(this.GetComponent<BoxCollider2D>());
				this.addIgnore(temp.GetComponent<BoxCollider2D>());
				temp.GetComponent<Projectile>().setDamage(stats.GetStatValueByName("Dexterity")*10);
				FinishPlayersTurn(true);
			}
		}

		if(Input.GetButton("Ability 2"))
		{
			if(mana.UseResource(100))
			{
				List<Projectile> ignores = new List<Projectile>();
				int range = 2;
				Vector3 t = this.transform.position;
				int counter = 1;
				for(int y = (int)(t.y-range); y <= t.y+range; y++)
				{
					GameObject temp = Instantiate(tempRock, t + new Vector3(0,0,0), Quaternion.identity);
					temp.GetComponent<Projectile>().setName(""+counter);
					counter++;
					temp.GetComponent<Projectile>().setCustomDestination( t + new Vector3(range, t.y-y, t.z) );
					temp.GetComponent<Projectile>().addIgnore(this.GetComponent<BoxCollider2D>());
					ignores.Add(temp.GetComponent<Projectile>());
					this.addIgnore(temp.GetComponent<BoxCollider2D>());
					temp.GetComponent<Projectile>().setDamage(stats.GetStatValueByName("Dexterity")*10);
				}

				for(int x = 0; x < ignores.Count; x++)
				{
					for(int y = 0; y < ignores.Count; y++)
					{
						if(x!=y)
							ignores[x].addIgnore(ignores[y].GetComponent<BoxCollider2D>());
					}
				}
				FinishPlayersTurn(true);	
			}
		}

		if(Input.GetButton("Ability 3"))
			FinishPlayersTurn(true);

	}

	void checkDirection()
	{
		if(currDir == Direction.LEFT)
			transform.localScale = new Vector3(-1,1,1);
		if(currDir == Direction.RIGHT)
			transform.localScale = Vector3.one;
	}

	protected override bool OnCantMove(GameObject objHit)
	{
		Wall hitWall = objHit.GetComponent<Wall>();
		EnemyHealth hitEnemy = objHit.GetComponent<EnemyHealth>();
		if(hitWall == null && hitEnemy == null)
			return false;

		//if(animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerChop"))
		//	animator.ResetTrigger("PlayerChop");
		animator.SetTrigger ("PlayerChop");

		if (hitWall != null)
			hitWall.DamageWall    ( (int)(stats.GetStatValueByName("Strength")/4.0) ) ;
		if(hitEnemy != null)
			hitEnemy.TakeResource ( stats.GetStatValueByName("Strength")*3.4 );
		return false;
		

	}

	private void Restart()
	{
		GameManager.instance.NextLevel();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Exit")
		{
			Invoke ("Restart", restartLevelDelay);
			enabled = false;
		}
		if (other.tag == "Food")
		{
			stats.AddAddStatBonus( "all", new BonusAddStat(1,"000") ) ;
			other.gameObject.SetActive (false);
		}
		if (other.tag == "Soda")
		{
			health.RestoreResource(pointsPerSoda);
			other.gameObject.SetActive (false);
		}
		if(other.tag == "Item")
		{
			if(inv.AddItem (other.gameObject.GetComponent<Item>()))
				other.gameObject.SetActive (false);
		}

		if(other.tag == "Debug Enemy")
		{
			TESTINGBoardCreator.spawnEnemy = true;
		}
		if(other.tag == "Debug Item")
		{
			TESTINGBoardCreator.spawnItem = true;
		}
		if(other.tag == "Debug Kill")
		{
			StartCoroutine(GameManager.instance.GameOver());
		}

	}

}
