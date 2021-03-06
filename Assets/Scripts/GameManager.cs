﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

	using System.Collections.Generic;		//Allows us to use Lists.
	using UnityEngine.UI;					//Allows us to use UI.

	public class GameManager : MonoBehaviour
	{
    public static float noSaveVictim = 0.5f;
    public static float letBuildingBurn = 0.0001f;
		public float levelStartDelay = 0.2f;						//Time to wait before starting level, in seconds.
		public float turnDelay = 0.1f;							//Delay between each Player turn.
		public static GameManager instance = null;				//Static instance of GameManager which allows it to be accessed by any other script.
    [HideInInspector] public bool playersTurn = true;		//Boolean to check if it's players turn, hidden in inspector but public.
    public AudioClip endGameAudio;

		private Timer timerInGame;
		private Text levelText;									//Text to display current level number.
		private Text moneyText;
    private Text karmaText;
    private GameObject levelImage;							//Image to block out level as levels are being set up, background for levelText.
		private BoardManager boardScript;						//Store a reference to our BoardManager which will set up the level.
		private FireSpawner fireSpawnerScript;
    private int level = 0;									//Current level number, expressed in game as "Day 1".
		private List<Enemy> enemies;							//List of all Enemy units, used to issue them move commands.
		private List<Housing> housings;
    private bool doingSetup = true;							//Boolean to check if we're setting up board, prevent Player from moving during setup.
    private int money = 0;
    private float karma = 1.0f;
		//Awake is always called before any Start functions
		void Awake()
		{
            //Check if instance already exists
            if (instance == null){

              //if not, set instance to this
              instance = this;
            }
            //If instance already exists and it's not this:
            else if (instance != this) {
               //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                Destroy(gameObject);
            }


			//Sets this to not be destroyed when reloading scene
			DontDestroyOnLoad(gameObject);

			//Assign enemies to a new List of Enemy objects.
			enemies = new List<Enemy>();
      housings = new List<Housing>();

			//Get a component reference to the attached BoardManager script
			boardScript = GetComponent<BoardManager>();

      // Get a component reference to the attached FireSpawner script
      fireSpawnerScript = GetComponent<FireSpawner>();

      GameObject MenuUI = GameObject.Find("Menu UI");
      if(MenuUI)
      {
        MenuUI.SetActive(false);
      }
      //uncomment this on standalone
      level = 1;
      InitGame();
      /////
		}

    //this is called only once, and the parameter tell it to be called only after the scene was loaded
    //(otherwise, our Scene Load callback would be called the very first load, and we don't want that)
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static public void CallbackInitialization()
    {
        //register the callback to be called everytime the scene is loaded
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //This is called each time a scene is loaded.
    static private void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
    {
        // the game scene
        if(scene.buildIndex == 3)
        {
          instance.level++;
          instance.InitGame();
        }
    }


		//Initializes the game for each level.
		void InitGame()
		{
      Debug.Log("InitGame");
			//While doingSetup is true the player can't move, prevent player from moving while title card is up.
			doingSetup = true;
      money = 0;
      karma = 1.00f;

			//Get a reference to our image LevelImage by finding it by name.
			levelImage = GameObject.Find("LevelImage");

			//Get a reference to our text LevelText's text component by finding it by name and calling GetComponent.
			levelText = GameObject.Find("LevelText").GetComponent<Text>();
			//Set the text of levelText to the string "Day" and append the current level number.
			levelText.text = "Get Ready !!";

      moneyText = GameObject.Find("MoneyText").GetComponent<Text>();
      moneyText.text = "Money: " + money + "$";

      karmaText = GameObject.Find("KarmaText").GetComponent<Text>();
      karmaText.text = "Karma: " + karma.ToString("f2") + "%";

			//Set levelImage to active blocking player's view of the game board during setup.
			levelImage.SetActive(true);

			//Call the HideLevelImage function with a delay in seconds of levelStartDelay.
			Invoke("HideLevelImage", levelStartDelay);

			//Clear any Enemy objects in our List to prepare for next level.
			enemies.Clear();
      housings.Clear();

			//Call the SetupScene function of the BoardManager script, pass it current level number.
			boardScript.SetupScene(level);
      fireSpawnerScript.StartFires();
		}

		//Hides black image used between levels
		void HideLevelImage()
		{
      gameObject.SetActive(true);
			//Disable the levelImage gameObject.
			levelImage.SetActive(false);

			//Set doingSetup to false allowing player to move again.
			doingSetup = false;
      playersTurn = true;
      timerInGame = GameObject.Find("MyTimer").GetComponent<Timer>();
      timerInGame.StartTimer();
		}

		//Update is called every frame.
		void Update()
		{
			//Check that playersTurn or enemiesMoving or doingSetup are not currently true.
			if(doingSetup)//if(playersTurn || enemiesMoving || doingSetup)
			{
        //If any of these are true, return and do not start MoveEnemies.
				return;
      }

      if (Input.GetKey ("escape"))
      {
        Application.Quit();
      }
		}
		//Call this to add the passed in Enemy to the List of Enemy objects.
		public void AddEnemyToList(Enemy script)
		{
			//Add Enemy to List enemies.
			enemies.Add(script);
		}

    public void AddHousingToList(Housing script)
    {
      housings.Add(script);
    }


		//GameOver is called when the player reaches 0 food points
		public void GameOver()
		{
      SoundManager.instance.PlaySingle(endGameAudio);
      float score = karma > 0.0f ? karma * money : 0.0f;
			//Set levelText to display number of levels passed and game over message
			levelText.text = "You Score\n score = karma * money = " + karma + " x " + money + "$ =" + score + "\n\n\n Thanks for playing \n\n\nYou can quit or leave the game restart.";

      timerInGame.StopTimer();
      timerInGame.SetAsZeroText();

			//Enable black background image gameObject.
			levelImage.SetActive(true);

			//Disable this GameManager.
			gameObject.SetActive(false);

      Invoke("Restart", 10f);
		}

    private void Restart()
    {
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    public void AddMoney(int collectedMoney)
    {
      money += collectedMoney;
      moneyText.text = "Money: "+ money +"$";
    }

    public void EditKarma(float amountKarma)
    {
      karma += amountKarma;
      karmaText.text = "Karma :" + karma.ToString("f2") + "%";
    }

    public Housing GetRandomHousing()
    {
      if(housings.Count == 0)
      {
        return null;
      }
      return housings[Random.Range(0, housings.Count)];
    }
	}
