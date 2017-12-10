using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {

	public bool createenemy;
	public bool createmap;

	[SerializeField]
	public class Count
	{
		public int min;
		public int max;
		public Count(int min, int max)
		{
			this.min = min;
			this.max = max;
		}
	}

	public int cols;
	public int rows;

	public Count wallCount = new Count(5,9);
	public Count sodaCount = new Count(5,9);

	public GameObject exit;

	public GameObject food;
	public GameObject soda;
	public GameObject[] floorTiles;
	public GameObject[] wallTiles;
	public GameObject[] enemyTiles;
	public GameObject[] outerWallTiles;
	public GameObject[] items;

	private Transform boardHolder;
	private List <Vector3> gridPositions = new List<Vector3> ();

	void InitialiseList()
	{
		gridPositions.Clear ();
		for (int x = 0; x < cols ; x++)
		{
			for (int y = 0; y < rows ; y++) 
			{
				gridPositions.Add (new Vector3 (x, y, 0f));
			}
		}
		gridPositions.Remove (new Vector3 (0, 0, 0f));
		gridPositions.Remove (new Vector3 (cols - 1, rows - 1, 0f));
	}

	void BoardSetup(int level)
	{
		boardHolder = new GameObject ("Board").transform;

		rows = (int)Mathf.Pow((float)(level + 3), 1.5f);
		cols = rows;

		wallCount = new Count((int)((5.0 / 49) * (rows * cols)), (int)((9.0 / 49) * (rows * cols)));
		sodaCount = new Count((int)((1.0 / 49) * (rows * cols)), (int)((5.0 / 49) * (rows * cols)));

		for (int x = -1; x < cols + 1; x++)
		{
			for (int y = -1; y < rows + 1; y++) 
			{
				GameObject toInstantiate = floorTiles [Random.Range (0, floorTiles.Length)];
				if(x==-1 || x == cols || y == -1|| y == rows)
					toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];

				GameObject instance = Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;

				instance.transform.SetParent (boardHolder);
			}
		}
	}

	Vector3 RandomPosition()
	{
		int randomIndex = Random.Range (0, gridPositions.Count);
		Vector3 randomPosition = gridPositions [randomIndex];
		gridPositions.RemoveAt (randomIndex);
		return randomPosition;
	}

	void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
	{
		int objectCount = Random.Range (minimum, maximum + 1);
		for (int x = 0; x < objectCount; x++) 
		{
			Vector3 randomPosition = RandomPosition ();
			GameObject tileChoice = tileArray [Random.Range (0, tileArray.Length)];
			Instantiate (tileChoice, randomPosition, Quaternion.identity);
		}
	}

	void LayoutObjectAtRandom(GameObject tileChoice, int minimum, int maximum)
	{
		int objectCount = Random.Range (minimum, maximum + 1);
		for (int x = 0; x < objectCount; x++) 
		{
			Vector3 randomPosition = RandomPosition ();
			Instantiate (tileChoice, randomPosition, Quaternion.identity);
		}
	}

	public void SetupScene(int level)
	{
		BoardSetup (level);
		InitialiseList ();
		LayoutObjectAtRandom (wallTiles, wallCount.min, wallCount.max);
		LayoutObjectAtRandom (soda, sodaCount.min, sodaCount.max);
		LayoutObjectAtRandom (items, 0, 2);
		int enemyCount = level;/*(int)Mathf.Log (level, 2f);*/
		LayoutObjectAtRandom (enemyTiles, enemyCount, enemyCount);
		Instantiate (food, RandomPosition(), Quaternion.identity);

		Instantiate (exit, new Vector3 (cols - 1, rows - 1, 0f), Quaternion.identity);

	}
		
	
	// Update is called once per frame
	void Update () {
		if (createenemy) {
			LayoutObjectAtRandom (enemyTiles, 1, 1);
			createenemy = false;
		}
		if (createmap) {
			SetupScene(1);
			createmap = false;
		}
	}
}
