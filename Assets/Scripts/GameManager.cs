﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

	using System.Collections.Generic;		//Allows us to use Lists. 
	using UnityEngine.UI;                   //Allows us to use UI.
	
	public class GameManager : MonoBehaviour
	{
		public static GameManager instance = null;				//Static instance of GameManager which allows it to be accessed by any other script.
		public bool playersTurn = true;		//Boolean to check if it's players turn, hidden in inspector but public.
		public BoardManager boardScript;                       //Store a reference to our BoardManager which will set up the level.
		public bool playerHasKey = false;
		public bool playerHasCase = false;
		public int playerHP = 100;
		public List<Enemy> enemies;
		public bool enemyIntercepting = false;
		public bool enemyCooperating = false;
		public int enemyCount = 0;
		
		
		//Awake is always called before any Start functions
		void Awake()
		{
            //Check if instance already exists
            if (instance == null)

                //if not, set instance to this
                instance = this;

            //If instance already exists and it's not this:
            else if (instance != this)
		{
			instance.playersTurn = true;
			instance.playerHasKey = false;
			instance.playerHasCase = false;
			instance.playerHP = 100;
			Destroy(gameObject);

		}
		

		//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			//Get a component reference to the attached BoardManager script
			boardScript = GetComponent<BoardManager>();
			
			//Call the InitGame function to initialize the first level 
			InitGame();
		}

        //this is called only once, and the paramter tell it to be called only after the scene was loaded
        //(otherwise, our Scene Load callback would be called the very first load, and we don't want that)
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]

		
		//Initializes the game for each level.
		public void InitGame()
		{
		enemies = new List<Enemy>();
		playersTurn = true;
		playerHasKey = false;
		playerHasCase = false;
		Debug.Log("InitGame before setup");
		boardScript.SetupScene();
		Debug.Log("InitGame after setup");
		enemyCount = enemies.ToArray().Length;




	}

	public void Restart()
	{

		SceneManager.LoadScene("Body");
	}

	public void ProgressToNextLevel()
	{

		boardScript.NextLevel();
		InitGame();
	}


	private void Update()
	{
		if (!playerHasKey&&!playerHasCase)
		{
			if (GameObject.Find("Canvas/Objective").GetComponent<Text>().text != "Objective: Fetch the Key")
			GameObject.Find("Canvas/Objective").GetComponent<Text>().text = "Objective: Fetch the Key";

		}

		if (playersTurn)
		{
			return;
		}

		playersTurn = true;


		GameObject[] paintedGrid = GameObject.FindGameObjectsWithTag("Soda");
		foreach (var item in paintedGrid)
		{
			item.SetActive(false);
			Destroy(item);

		}

		foreach (var enemy in enemies)
		{
			enemy.MoveEnemy();
		}


	}

	public void AddEnemyToList(Enemy script)
	{
		//Add Enemy to List enemies.
		enemies.Add(script);
	}



}


