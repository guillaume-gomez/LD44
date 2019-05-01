﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;	//Allows us to use UI.
using UnityEngine.SceneManagement;

	//Player inherits from MovingObject, our base class for objects that can move, Enemy also inherits from this.
	public class Player : MonoBehaviour
	{

	  public float speed = 1.0f;
    public LayerMask blockingLayer;

    private Rigidbody2D rb2D;
    private BoxCollider2D boxCollider;
    private bool IsMovingToFinish;

		private const int RIGHT = 0;
		private const int LEFT  = 1;
		private const int UP    = 2;
		private const int DOWN  = 3;

		public float restartLevelDelay = 1f;		//Delay time in seconds to restart level.
		public AudioClip moveSound1;				//1 of 2 Audio clips to play when player moves.
		public AudioClip moveSound2;				//2 of 2 Audio clips to play when player moves.
		public AudioClip gameOverSound;				//Audio clip to play when player dies.
    public GameObject waterPrefab;
		private Animator animator;					//Used to store a reference to the Player's animator component.
		private int food;                           //Used to store player food points total during level.
#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
        private Vector2 touchOrigin = -Vector2.one;	//Used to store location of screen touch origin for mobile controls.
#endif


		void Start ()
		{
			animator = GetComponent<Animator>();

		  boxCollider = GetComponent<BoxCollider2D>();
      rb2D = GetComponent<Rigidbody2D>();
      IsMovingToFinish = false;
		}


    void FixedUpdate()
    {

    }


		private void Update ()
		{
			int horizontal = 0;  	//Used to store the horizontal move direction.
			int vertical = 0;		//Used to store the vertical move direction.

			//Check if we are running either in the Unity editor or in a standalone build.
#if UNITY_STANDALONE || UNITY_WEBGL

			//Get input from the input manager, round it to an integer and store in horizontal to set x axis move direction
			horizontal = (int) (Input.GetAxisRaw ("Horizontal"));

			//Get input from the input manager, round it to an integer and store in vertical to set y axis move direction
			vertical = (int) (Input.GetAxisRaw ("Vertical"));

			//Check if moving horizontally, if so set vertical to zero.
			if(horizontal != 0)
			{
				vertical = 0;
			}

      if(Input.GetButtonDown("Jump"))
      {
      	PutOutFire();
        return;
      }

			//Check if we are running on iOS, Android, Windows Phone 8 or Unity iPhone
#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE

			//Check if Input has registered more than zero touches
			if (Input.touchCount > 0)
			{
        if(touch.tapCount == 2)
        {
          PutOutFire();
          return;
        }

				//Store the first touch detected.
				Touch myTouch = Input.touches[0];

				//Check if the phase of that touch equals Began
				if (myTouch.phase == TouchPhase.Began)
				{
					//If so, set touchOrigin to the position of that touch
					touchOrigin = myTouch.position;
				}

				//If the touch phase is not Began, and instead is equal to Ended and the x of touchOrigin is greater or equal to zero:
				else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0)
				{
					//Set touchEnd to equal the position of this touch
					Vector2 touchEnd = myTouch.position;

					//Calculate the difference between the beginning and end of the touch on the x axis.
					float x = touchEnd.x - touchOrigin.x;

					//Calculate the difference between the beginning and end of the touch on the y axis.
					float y = touchEnd.y - touchOrigin.y;

					//Set touchOrigin.x to -1 so that our else if statement will evaluate false and not repeat immediately.
					touchOrigin.x = -1;

					//Check if the difference along the x axis is greater than the difference along the y axis.
					if (Mathf.Abs(x) > Mathf.Abs(y))
					{
						//If x is greater than zero, set horizontal to 1, otherwise set it to -1
						horizontal = x > 0 ? 1 : -1;
					}
					else
					{
						//If y is greater than zero, set horizontal to 1, otherwise set it to -1
						vertical = y > 0 ? 1 : -1;
					}
				}
			}

#endif //End of mobile platform dependendent compilation section started above with #elif
			//Check if we have a non-zero value for horizontal or vertical
			if(horizontal != 0 || vertical != 0)
			{
				//Call AttemptMove passing in the generic parameter Wall, since that is what Player may interact with if they encounter one (by attacking it)
				//Pass in horizontal and vertical as parameters to specify the direction to move Player in.
				Move(horizontal, vertical);
			} else
      {
        animator.SetBool("isMoving", false);
      }
		}

		//AttemptMove overrides the AttemptMove function in the base class MovingObject
		//AttemptMove takes a generic parameter T which for Player will be of the type Wall, it also takes integers for x and y direction to move in.
		void Move(int xDir, int yDir)
		{
      RaycastHit2D hit;
      Vector2 movement = new Vector2(xDir, yDir);
      Vector2 start = transform.position;
      Vector2 end = start + (movement * speed * Time.fixedDeltaTime);
      boxCollider.enabled = false;
      hit = Physics2D.Linecast(start, end, blockingLayer);
      boxCollider.enabled = true;

			//If Move returns true, meaning Player was able to move into an empty space.
			if (hit.transform == null)
			{
        if(xDir != 0)
        {
          int direction = xDir > 0 ? RIGHT : LEFT;
          animator.SetInteger("playerDirection", direction);
        } else {
          int direction = yDir > 0 ? UP : DOWN;
          animator.SetInteger("playerDirection", direction);
        }
        animator.SetBool("isMoving", true);
        rb2D.MovePosition(end);
				//Call RandomizeSfx of SoundManager to play the move sound, passing in two audio clips to choose from.
				//SoundManager.instance.RandomizeSfx (moveSound1, moveSound2);
			}
		}

		//OnTriggerEnter2D is sent when another object enters a trigger collider attached to this object (2D physics only).
		private void OnTriggerEnter2D (Collider2D other)
		{
			//Check if the tag of the trigger collided with is Exit.
			if(other.tag == "Exit")
			{
				//Invoke the Restart function to start the next level with a delay of restartLevelDelay (default 1 second).
				Invoke ("Restart", restartLevelDelay);

				//Disable the player object since level is over.
				enabled = false;
			}
		}


		//Restart reloads the scene when called.
		private void Restart ()
		{
			//Load the last scene loaded, in this case Main, the only scene in the game. And we load it in "Single" mode so it replace the existing one
      //and not load all the scene object in the current scene.
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
		}

		public void PutOutFire()
		{
			Debug.Log("PutOutFire");
			animator.SetBool("isMoving", true);
      animator.SetInteger("playerDirection", UP);

			enabled = false;
      GameObject instance =
          Instantiate (waterPrefab, new Vector3 (gameObject.transform.position.x, gameObject.transform.position.y + 4.0f, 0f), Quaternion.identity) as GameObject;
      instance.transform.SetParent (gameObject.transform);
			Invoke("EndInteract", 2.0f);
		}


		//CheckIfGameOver checks if the player is out of food points and if so, ends the game.
		private void CheckIfGameOver()
		{
			//Check if food point total is less than or equal to zero.
				//Call the PlaySingle function of SoundManager and pass it the gameOverSound as the audio clip to play.
				SoundManager.instance.PlaySingle (gameOverSound);

				//Stop the background music.
				SoundManager.instance.musicSource.Stop();

				//Call the GameOver function of GameManager.
				GameManager.instance.GameOver ();

		}

		private void EndInteract()
		{
			enabled = true;
		}
	}
