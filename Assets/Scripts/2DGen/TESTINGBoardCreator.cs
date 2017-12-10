using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class TESTINGBoardCreator : BoardCreator
{
	public GameObject debugItem;
	public GameObject debugEnemy;

	public static bool spawnEnemy;
	public static bool spawnItem;

	protected override void LayoutOutEntities()
	{
		playerPos = new Vector3(0,0,0);
		Vector3 exitPos = new Vector3(9,9,9);
		gridPositions.Remove(exitPos);
		gridPositions.Remove(playerPos);
		Instantiate(exit,exitPos,Quaternion.identity);
		Instantiate(player, playerPos, Quaternion.identity);
		Instantiate(debugItem, new Vector3(0,3,0), Quaternion.identity);
		Instantiate(debugEnemy, new Vector3(0,4,0), Quaternion.identity);
	}
	void Update()
	{
		if(spawnEnemy)
		{
			LayoutObjectAtRandom("false",enemies,1,1);
			spawnEnemy = false;
		}
		if(spawnItem)
		{
			LayoutObjectAtRandom("false",items,1,1);
			spawnItem = false;
		}
	}




	protected override void CreateRoomsAndCorridors ()
	{
		rooms = new Room[numRooms.Random];
		rooms[0] = new Room ();
		rooms[0].SetupRoom(roomWidth, roomHeight, columns, rows);
	}



	protected override void SetTilesValuesForCorridors (){}




}