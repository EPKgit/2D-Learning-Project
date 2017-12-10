using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class BoardCreator : MonoBehaviour
{
	// The type of tile that will be laid in a specific position.
	public enum TileType
	{
		Wall, Floor,
	}


	public int columns = 100;                                 // The number of columns on the board (how wide it will be).
	public int rows = 100;                                    // The number of rows on the board (how tall it will be).
	public IntRange numRooms = new IntRange (15, 20);         // The range of the number of rooms there can be.
	public IntRange roomWidth = new IntRange (3, 10);         // The range of widths rooms can have.
	public IntRange roomHeight = new IntRange (3, 10);        // The range of heights rooms can have.
	public IntRange corridorLength = new IntRange (6, 10);    // The range of lengths corridors between rooms can have.
	public IntRange sodaCount = new IntRange(0,0);
	public IntRange wallCount = new IntRange(0,0);


	public GameObject[] floorTiles;                           // An array of floor tile prefabs.
	public GameObject[] wallTiles;                            // An array of wall tile prefabs.
	public GameObject[] outerWallTiles;                       // An array of outer wall tile prefabs.
	public GameObject player;
	public GameObject[] enemies;
	public GameObject soda;
	public GameObject food;
	public GameObject exit;
	public GameObject[] items;
	//public GameObject[] items;

	protected TileType[][] tiles;                               // A jagged array of tile types representing the board, like a grid.
	protected Room[] rooms;                                     // All the rooms that are created for this board.
	protected Corridor[] corridors;                             // All the corridors that connect the rooms.
	protected GameObject boardHolder;                           // GameObject that acts as a container for all other tiles.
	protected Vector3 playerPos; 

	protected List<Vector3> gridPositions;

	private void Start ()
	{
		gridPositions = new List<Vector3>();
		//SetupScene();
	}

	public void SetupScene()
	{
		gridPositions.Clear();
		// Create the board holder.
		boardHolder = new GameObject("BoardHolder");

		SetupTilesArray ();

		CreateRoomsAndCorridors ();

		SetTilesValuesForRooms ();
		SetTilesValuesForCorridors ();

		InstantiateTiles ();
		InstantiateOuterWalls ();

		DetermineGridPositions();
		HashSet<Vector3> temp = new HashSet<Vector3>(gridPositions);
		gridPositions = new List<Vector3>(temp);

		LayoutOutEntities();

		Camera[] allCams = new Camera[Camera.allCamerasCount];
		Camera.GetAllCameras(allCams);
		for(int x = 0; x < allCams.Length; x++)
		{
			allCams[x].GetComponent<CameraController>().FindPlayer();
		}
	}

	protected virtual void LayoutOutEntities()
	{
		Vector3 exitPos = RandomPositionFurthest(playerPos);
		gridPositions.Remove(exitPos);
		gridPositions.Remove(playerPos);
		Instantiate(exit,exitPos,Quaternion.identity);
		Instantiate(player, playerPos, Quaternion.identity);

		wallCount.m_Min = (int)(gridPositions.Count*0.1);
		wallCount.m_Max = (int)(gridPositions.Count*0.15);
		sodaCount.m_Min = (int)(gridPositions.Count*0.01);
		sodaCount.m_Min = (int)(gridPositions.Count*0.03);

		LayoutObjectAtRandom ("Walls",wallTiles, wallCount.m_Min, wallCount.m_Max);
		LayoutObjectAtRandom ("Sodas",soda, sodaCount.m_Min, sodaCount.m_Max);
		LayoutObjectAtRandom ("Items",items, 20, 30);
		int enemyCount = numRooms.m_Min-3;
		double enemyRatio = Random.value/4.0;
		int enemy1Count = (int)(enemyCount*(1-enemyRatio));
		int enemy2Count = (int)(enemyRatio*enemyCount);
		LayoutObjectAtRandom ("Enemies1", enemies[0], enemy1Count,enemy1Count);
		LayoutObjectAtRandom ("Enemies2", enemies[1], enemy2Count,enemy2Count);
		LayoutObjectAtRandom ("Enemies3", enemies[2], 0, 3);
		//LayoutObjectAtRandom ("Enemies", enemies[2], 1, 1);
		LayoutObjectAtRandom ("Food", food, 2, 3);


	}

	protected void LayoutObjectAtRandom(string name, GameObject[] tileArray, int minimum, int maximum)
	{
		GameObject tileHolder = new GameObject(name);
		int objectCount = Random.Range (minimum, maximum + 1);
		for (int x = 0; x < objectCount; x++) 
		{
			Vector3 randomPosition = RandomPosition ();
			GameObject tileChoice = tileArray [Random.Range (0, tileArray.Length)];
			GameObject instance = Instantiate (tileChoice, randomPosition, Quaternion.identity) as GameObject;
			if(name != "false")
				instance.transform.SetParent (tileHolder.transform);
		}
	}

	protected void LayoutObjectAtRandom(string name, GameObject tile, int minimum, int maximum)
	{
		GameObject tileHolder = new GameObject(name);
		int objectCount = Random.Range (minimum, maximum + 1);
		for (int x = 0; x < objectCount; x++) 
		{
			Vector3 randomPosition = RandomPosition ();
			GameObject tileChoice = tile;
			GameObject instance = Instantiate (tileChoice, randomPosition, Quaternion.identity) as GameObject;
			if(name != "false")
				instance.transform.SetParent (tileHolder.transform);
		}
	}


	protected Vector3 RandomPosition()
	{
		int randomIndex = Random.Range (0, gridPositions.Count);
		Vector3 randomPosition = gridPositions [randomIndex];
		gridPositions.RemoveAt (randomIndex);
		return randomPosition;
	}

	protected Vector3 RandomPositionMinDistance(int minDist, Vector3 awayFrom)
	{
		List<Vector3> potentials = new List<Vector3>();
		for(int x = 0; x < gridPositions.Count; x++)
			if(Vector3.Distance(gridPositions[x],awayFrom) > minDist)
				potentials.Add(gridPositions[x]);
		if(potentials.Count == 0)
		{
			return new Vector3(0,0,0);
			//return RandomPositionFurthest(1, awayFrom);
		}
		int randomIndex = Random.Range (0, potentials.Count);
		Vector3 randomPosition = potentials [randomIndex];
		gridPositions.Remove (randomPosition);
		return randomPosition;
	}

	protected Vector3 RandomPositionFurthest(Vector3 awayFrom)
	{
		double maxDist = 0;
		Vector3 furthest = new Vector3(0,0,0);
		for(int x = 0; x < gridPositions.Count; x++)
		{
			double distance = Vector3.Distance(gridPositions[x],awayFrom);
			if(distance > maxDist )
			{
				furthest = gridPositions[x];
				maxDist = distance;
			}
		}
		gridPositions.Remove (furthest);
		return furthest;
	}

	protected Vector3 RandomPositionFurthest(int num, Vector3 awayFrom)
	{
		int counter = 0;
		double lesserMaxDist = 0;
		List<Vector3> potentials = new List<Vector3>();
		for(int x = 0; x < gridPositions.Count; x++)
		{
			double distance = Vector3.Distance(gridPositions[x],awayFrom);
			if(distance > lesserMaxDist )
			{
				potentials.Add(gridPositions[x]);
			}
			if(potentials.Count > num)
			{
				Vector3 toDel = potentials[0];
				float lowestDist =  Vector3.Distance(potentials[0], awayFrom);
				for(int y = 1; x < potentials.Count; x++)
				{
					if(Vector3.Distance(potentials[y],awayFrom) < lowestDist)
					{
						lowestDist = Vector3.Distance(potentials[y],awayFrom);
						toDel = potentials[y];
					}
				}
				potentials.Remove(toDel);
				lesserMaxDist = lowestDist;
			}
		}
		int randomIndex = Random.Range (0, potentials.Count);
		Vector3 randomPosition = potentials [randomIndex];
		gridPositions.Remove (randomPosition);
		return randomPosition;
	}

	protected void DetermineGridPositions()
	{
		for(int x = 0; x < rooms.Length; x++)
			gridPositions.AddRange(rooms[x].GeneratePositions());
	}

	protected void SetupTilesArray ()
	{
		// Set the tiles jagged array to the correct width.
		tiles = new TileType[columns][];

		// Go through all the tile arrays...
		for (int i = 0; i < tiles.Length; i++)
		{
			// ... and set each tile array is the correct height.
			tiles[i] = new TileType[rows];
		}
	}


	protected virtual void CreateRoomsAndCorridors ()
	{
		// Create the rooms array with a random size.
		rooms = new Room[numRooms.Random];

		// There should be one less corridor than there is rooms.
		corridors = new Corridor[rooms.Length - 1];

		// Create the first room and corridor.
		rooms[0] = new Room ();
		corridors[0] = new Corridor ();

		// Setup the first room, there is no previous corridor so we do not use one.
		rooms[0].SetupRoom(roomWidth, roomHeight, columns, rows);

		// Setup the first corridor using the first room.
		corridors[0].SetupCorridor(rooms[0], corridorLength, roomWidth, roomHeight, columns, rows, true);

		for (int i = 1; i < rooms.Length; i++)
		{
			// Create a room.
			rooms[i] = new Room ();

			// Setup the room based on the previous corridor.
			rooms[i].SetupRoom (roomWidth, roomHeight, columns, rows, corridors[i - 1]);

			// If we haven't reached the end of the corridors array...
			if (i < corridors.Length)
			{
				// ... create a corridor.
				corridors[i] = new Corridor ();

				// Setup the corridor based on the room that was just created.
				corridors[i].SetupCorridor(rooms[i], corridorLength, roomWidth, roomHeight, columns, rows, false);
			}

			if (i == (int)(rooms.Length *.5f))
			{
				playerPos = new Vector3 (rooms[i].xPos, rooms[i].yPos, 0);
			}
		}

	}


	protected void SetTilesValuesForRooms ()
	{
		// Go through all the rooms...
		for (int i = 0; i < rooms.Length; i++)
		{
			Room currentRoom = rooms[i];

			// ... and for each room go through it's width.
			for (int j = 0; j < currentRoom.roomWidth; j++)
			{
				int xCoord = currentRoom.xPos + j;

				// For each horizontal tile, go up vertically through the room's height.
				for (int k = 0; k < currentRoom.roomHeight; k++)
				{
					int yCoord = currentRoom.yPos + k;

					// The coordinates in the jagged array are based on the room's position and it's width and height.
					tiles[xCoord][yCoord] = TileType.Floor;
				}
			}
		}
	}


	protected virtual void SetTilesValuesForCorridors ()
	{
		// Go through every corridor...
		for (int i = 0; i < corridors.Length; i++)
		{
			Corridor currentCorridor = corridors[i];

			// and go through it's length.
			for (int j = 0; j < currentCorridor.corridorLength; j++)
			{
				// Start the coordinates at the start of the corridor.
				int xCoord = currentCorridor.startXPos;
				int yCoord = currentCorridor.startYPos;

				// Depending on the direction, add or subtract from the appropriate
				// coordinate based on how far through the length the loop is.
				switch (currentCorridor.direction)
				{
				case Direction.North:
					yCoord += j;
					break;
				case Direction.East:
					xCoord += j;
					break;
				case Direction.South:
					yCoord -= j;
					break;
				case Direction.West:
					xCoord -= j;
					break;
				}

				// Set the tile at these coordinates to Floor.
				tiles[xCoord][yCoord] = TileType.Floor;
			}
		}
	}


	protected void InstantiateTiles ()
	{
		// Go through all the tiles in the jagged array...
		for (int i = 0; i < tiles.Length; i++)
		{
			for (int j = 0; j < tiles[i].Length; j++)
			{
				// ... and instantiate a floor tile for it.
				InstantiateFromArray (floorTiles, i, j);

				// If the tile type is Wall...
				if (tiles[i][j] == TileType.Wall)
				{
					// ... instantiate a wall over the top.
					InstantiateFromArray (wallTiles, i, j);
				}
			}
		}
	}


	protected void InstantiateOuterWalls ()
	{
		// The outer walls are one unit left, right, up and down from the board.
		float leftEdgeX = -1f;
		float rightEdgeX = columns + 0f;
		float bottomEdgeY = -1f;
		float topEdgeY = rows + 0f;

		// Instantiate both vertical walls (one on each side).
		InstantiateVerticalOuterWall (leftEdgeX, bottomEdgeY, topEdgeY);
		InstantiateVerticalOuterWall(rightEdgeX, bottomEdgeY, topEdgeY);

		// Instantiate both horizontal walls, these are one in left and right from the outer walls.
		InstantiateHorizontalOuterWall(leftEdgeX + 1f, rightEdgeX - 1f, bottomEdgeY);
		InstantiateHorizontalOuterWall(leftEdgeX + 1f, rightEdgeX - 1f, topEdgeY);
	}


	protected void InstantiateVerticalOuterWall (float xCoord, float startingY, float endingY)
	{
		// Start the loop at the starting value for Y.
		float currentY = startingY;

		// While the value for Y is less than the end value...
		while (currentY <= endingY)
		{
			// ... instantiate an outer wall tile at the x coordinate and the current y coordinate.
			InstantiateFromArray(outerWallTiles, xCoord, currentY);

			currentY++;
		}
	}


	protected void InstantiateHorizontalOuterWall (float startingX, float endingX, float yCoord)
	{
		// Start the loop at the starting value for X.
		float currentX = startingX;

		// While the value for X is less than the end value...
		while (currentX <= endingX)
		{
			// ... instantiate an outer wall tile at the y coordinate and the current x coordinate.
			InstantiateFromArray (outerWallTiles, currentX, yCoord);

			currentX++;
		}
	}


	protected void InstantiateFromArray (GameObject[] prefabs, float xCoord, float yCoord)
	{
		// Create a random index for the array.
		int randomIndex = Random.Range(0, prefabs.Length);

		// The position to be instantiated at is based on the coordinates.
		Vector3 position = new Vector3(xCoord, yCoord, 0f);

		// Create an instance of the prefab from the random index of the array.
		GameObject tileInstance = Instantiate(prefabs[randomIndex], position, Quaternion.identity) as GameObject;

		// Set the tile's parent to the board holder.
		tileInstance.transform.parent = boardHolder.transform;
	}
}