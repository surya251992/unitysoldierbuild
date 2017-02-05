using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {


	[Tooltip("The player")]
	public Transform playerTransform;
	[Tooltip("The initial camera speed")]
	public float StartSpeed = 0.035f;
	[Tooltip("The maximum camera speed")]
	public float MaxSpeed = 0.85f;
	[Tooltip("The amount we will increase the camera speed")]
	public float SpeedIncrease = 0.01f;
	[Tooltip("Increase the speed every X seconds")]
	public float Countdown = 10;

	internal bool GameStarted=false;
	private Controller GameController;
	private Transform thisTransform;
	private bool SpeedTimer=false;
	private float ydir = 0f;

	void Awake()
	{
		//Set the timescale to 1
		Time.timeScale=1;
	}

	// Use this for initialization
	void Start()
	{
		//Cache the transform of this gameobject.
		thisTransform=transform;
		//Get the GameController
		GameController=(Controller)FindObjectOfType(typeof(Controller));
		//Set the SpeedTimer bool to false
		SpeedTimer=false;
	}

	// Update is called once per frame
	void Update () 
	{
		//check that player exists and then proceed. otherwise we get an error when player dies
		if (playerTransform) 
		{
			//if player has passed the x position of 10 then start moving camera forward
			if (playerTransform.position.x > 3 && GameStarted==false) 
			{
				GameStarted=true;
				GameController.ShowGameUI();
				GameController.HideMenu();
			}

			//Check if the game is currently running
			if(GameStarted==true)
			{
				//Start the speed increase timer if we haven't already
				if(SpeedTimer==false)
				{
					IncreaseSpeed();
					SpeedTimer=true;
					//Start the score counter
					GameController.SendMessage("Score", 1);
				}

				//Make the camera move with a randomish Y position. This effect will be translated to the pencil line we are drawing 
				if(Time.timeScale>0)
				{
					//If game is running and escape/back is pressed. Pause the game.
					if(Input.GetKeyDown(KeyCode.Escape))
						GameController.Pause();

					float randy = 0f;
					randy = Random.Range (0f, 100f);
					if (randy < 20) {
						ydir = ydir + .005f;
					} else if (randy > 20 && randy < 40) {
						ydir = ydir - .005f;
					} else if (randy > 80) {
						ydir = 0f;
					}
					thisTransform.position = new Vector3 (thisTransform.position.x + StartSpeed, thisTransform.position.y + ydir, -10);
				}
				else
					//If game is paused and escape/back is pressed. Continue the game.
					if(Input.GetKeyDown(KeyCode.Escape))
						GameController.Continue();
			}
			else
				if(Input.GetKeyDown(KeyCode.Escape))
					Application.Quit();
		}
	}

	//We will use this to increase the camera speed every x seconds. 
	void IncreaseSpeed()
	{
		Invoke ("IncreaseSpeed",Countdown);

		//Check if the current speed is less than the max speed and increase it
		if(StartSpeed<MaxSpeed)
			StartSpeed +=SpeedIncrease;

		//Increase the score multiplier value on the GameController
		GameController.multiplier +=0.2f;
	}
}