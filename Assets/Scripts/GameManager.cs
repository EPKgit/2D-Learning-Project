using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour 
{

	public enum Turn{ doingSetup = 0, playersTurn, enemiesMoving, projectilesMoving };

	public static GameManager instance = null;

	public float levelStartDelay = 1f;

	//public BoardManager boardScript;
	public BoardCreator boardScript;

	public GameObject player;
	public float turnDelay;

	public double playerCurrentHealthPoints = 100;
	public double playerMaxHealthPoints = 100;

	public double playerCurrentManaPoints = 100;
	public double playerMaxManaPoints = 100;

	public List<Item> playerCurrentInventory;

	private int level = 0;
	[SerializeField]private List<Enemy> enemies;
	[SerializeField]private List<Projectile> projectiles;
	[SerializeField]private Turn turn;
	[SerializeField]private bool takingTurn;

	private Text levelText;
	[SerializeField]private GameObject levelImage;


	void Awake ()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
		DontDestroyOnLoad(gameObject);
		enemies = new List<Enemy>();

		//boardScript = GetComponent<BoardManager> ();
		boardScript = GetComponent<BoardCreator> ();
		if(boardScript == null)
		{
			boardScript = GetComponent<TESTINGBoardCreator>();
		}
		turn = Turn.projectilesMoving;
		NextLevel();
	}

	public int GetTurn()
	{
		return (int)turn;
	}

	public void AdvanceTurn()
	{
		if(turn > Turn.enemiesMoving)
		{
			turn = Turn.playersTurn;
			takingTurn = false;
		}
		else
			turn++;
	}
		
	public IEnumerator GameOver()
	{
		levelText.text = "After " + level + " days you died.";
		levelImage.SetActive (true);
		Destroy(this.gameObject);
		yield return new WaitForSeconds(3f);
		SceneManager.LoadSceneAsync("Menu");
	}


	public void NextLevel()
	{
		takingTurn = true;
		turn = Turn.doingSetup;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

	}

	void InitGame()
	{
		levelImage = GameObject.Find("InGameCanvas").transform.Find("LevelImage").gameObject;
		levelText = levelImage.GetComponentInChildren<Text>();
		levelText.text = "Day " + level;
		levelImage.SetActive(true);
		enemies.Clear ();

		boardScript.SetupScene(/*level*/);

		Camera.main.GetComponent<CameraController>().FindPlayer();
		Invoke("HideLevelImage",levelStartDelay);
	}

	private void HideLevelImage()
	{
		if(levelImage != null)
			levelImage.SetActive (false);
		takingTurn = false;
	}
		

	// Update is called once per frame
	void Update () 
	{
		if(turn == Turn.doingSetup)
		{
			turn++;
			level++;
			InitGame();
		}
		if (turn == Turn.playersTurn || MovingObject.isMoving || takingTurn)
			return;
		if(turn == Turn.projectilesMoving && !takingTurn)
			StartCoroutine(MoveProjectiles());
		if(turn == Turn.enemiesMoving && !takingTurn)
			StartCoroutine (MoveEnemies ());
		if(turn > Turn.enemiesMoving && !takingTurn)
		{
			takingTurn = true;
			AdvanceTurn();
			//Invoke("AdvanceTurn",turnDelay);
		}
	}

	public void AddEnemyToList (Enemy script)
	{
		enemies.Add (script);
	}
	public void RemoveEnemyFromList (Enemy script)
	{
		enemies.Remove (script);
	}
	public void AddProjectileToList (Projectile script)
	{
		projectiles.Add (script);
	}
	public void RemoveProjectileFromList (Projectile script)
	{
		projectiles.Remove (script);
	}

	IEnumerator MoveEnemies()
	{
		takingTurn = true;
		yield return new WaitForSeconds(turnDelay);
		MovingObject.potentialMoves.Clear ();
		for(int x = 0; x < enemies.Count; x++)
		{
			enemies [x].MoveEnemy ();
			yield return null;
		}
		takingTurn = false;
		turn++;

	}

	IEnumerator MoveProjectiles()
	{
		takingTurn = true;
		yield return new WaitForSeconds(turnDelay);
		MovingObject.potentialMoves.Clear ();
		for(int x = 0; x < projectiles.Count; x++)
		{
			if(projectiles[x].MoveProjectile())
				x--;
			yield return null;
		}
		takingTurn = false;
		turn++;
	}

}
