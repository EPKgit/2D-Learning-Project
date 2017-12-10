using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour 
{

	public enum Direction{UP=1,RIGHT,DOWN,LEFT};

	public float moveTime= 0.05f;
	public LayerMask blockingLayer;

	public static bool isMoving;
	public static List<Vector2> potentialMoves = new List<Vector2>();

	protected BoxCollider2D boxCollider;
	protected Rigidbody2D rb2D;
	private float inverseMoveTime;

	[SerializeField]protected List<BoxCollider2D> toIgnore;

	[SerializeField]protected Direction currDir;

	// Use this for initialization
	protected virtual void Awake ()
	{
		boxCollider = GetComponent<BoxCollider2D> ();
		rb2D = GetComponent<Rigidbody2D>();
		inverseMoveTime = 1f / moveTime;
		currDir = Direction.UP;
		toIgnore =  new List<BoxCollider2D>();
		toIgnore.Add(boxCollider);
	}

	protected virtual bool Move(int xDir, int yDir, out RaycastHit2D hit)
	{
		//toIgnore.ForEach(x=>Debug.Log(x.gameObject));
		Vector2 start = gameObject.transform.position;
		Vector2 end = start + new Vector2 (xDir, yDir);

		for(int x = 0; x < toIgnore.Count; x++)
			if(toIgnore[x] == null)
				toIgnore.Remove(toIgnore[x]);
			else
				toIgnore[x].enabled = false;
		//toIgnore.ForEach(x => (x == null) ? toIgnore.Remove(x) : x.enabled = false);
		//boxCollider.enabled = false;
		hit = Physics2D.Linecast (start, end, blockingLayer);
		//boxCollider.enabled = true;
		for(int x = 0; x < toIgnore.Count; x++)
			if(toIgnore[x] == null)
				toIgnore.Remove(toIgnore[x]);
			else
				toIgnore[x].enabled = true;

		if (hit.transform == null)
		{
			//StartCoroutine (SmoothMovement (end));
			return true;
		}
		return false;
	}

	protected IEnumerator SmoothMovement(Vector3 end)
	{
		potentialMoves.Add (end);
		isMoving = true;
		float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
		while(sqrRemainingDistance > float.Epsilon)
		{
			Vector3 newPosition = Vector3.MoveTowards (rb2D.position, end, inverseMoveTime * Time.deltaTime);
			rb2D.MovePosition (newPosition);
			sqrRemainingDistance = (transform.position - end).sqrMagnitude;
			yield return null;
		}
		isMoving = false;
	}

	protected virtual bool AttemptMove (int xDir, int yDir)
	{
		RaycastHit2D hit;
		bool canMove = Move(xDir, yDir, out hit);
		//Debug.Log(hit);
		Vector2 pMove = (Vector2)transform.position + new Vector2 (xDir, yDir);
		if (hit.transform == null && !potentialMoves.Contains(pMove) ){
			StartCoroutine (SmoothMovement (pMove) );
			return false;
		}

		if (!canMove && hit.transform != null) {
			GameObject hitObject = hit.transform.gameObject;
			return OnCantMove (hitObject);
		}
		return false;
	}

	protected virtual bool OnCantMove (GameObject objHit){ return false; }

	public void addIgnore(BoxCollider2D i)
	{
		toIgnore.Add(i);
	}
	public void removeIgnore(BoxCollider2D i)
	{
		toIgnore.Remove(i);
	}

}
