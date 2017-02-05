using UnityEngine;
using System.Collections;
using UnityEngine.UI;
#if (UNITY_5_3_OR_NEWER || UNITY_5_3)
using UnityEngine.SceneManagement;
#endif

public class Controller : MonoBehaviour 
{
	[Space(10)]

	[Tooltip("The Player")]
	public GameObject Player;
	[Tooltip("The Game UI")]
	public GameObject GameUI;
	[Tooltip("The Pause UI")]
	public GameObject PauseUI;
	[Tooltip("The Main Menu UI")]
	public GameObject MenuUI;
	[Tooltip("The Game Over UI")]
	public GameObject GameOverUI;
	[Tooltip("The Settings Panel")]
	public GameObject SettingsPanel;

	[Space(10)]

	[Tooltip("The sound text on the settings panel")]
	public Text SoundText;
	[Tooltip("The control mode text on the settings panel")]
	public Text ControlText;
	[Tooltip("The score display text")]
	public Text ScoreText;
	[Tooltip("The final score text. Displays the score for that level.")]
	public Text FinalScore;
	[Tooltip("The highscore text")]
	public Text CurHighscore;

	[Space(10)]

	[Tooltip("The sound button on the settings panel")]
	public GameObject SoundButton;
	[Tooltip("The scroll view on the settings panel")]
	public GameObject InfoView;
	[Tooltip("The touch controls i.e Joystick + Buttons")]
	public GameObject TouchControls;

	[Space(10)]

	[Tooltip("The health bar")]
	public Slider HealthBar;


	internal float multiplier=1;
	internal bool isGameOver=false;
	private SoundManager SoundMgr;
	private int healthLevel=5;
	private Animator Anim;
	private int soundToggle;
	private int controlToggle;
	private int highscore;
	private int curScore;
	private float score;


	// Use this for initialization
	void Start () 
	{
		//Get the animator component
		Anim=Player.GetComponent<Animator>();
		//Reset the Health Bar level
		HealthBar.value=healthLevel;
		//Get the UI Sound reference
		SoundMgr=(SoundManager)FindObjectOfType(typeof(SoundManager));
		//Get the previous saved control mode
		controlToggle = PlayerPrefs.GetInt ("Controls",0);
		//Update UI Objects
		Updateitems();
		//Show the start ui
		if(GameOverUI) GameOverUI.SetActive(false);
		if(GameUI) GameUI.SetActive(false);
		if(PauseUI) PauseUI.SetActive(false);
		if(SettingsPanel) SettingsPanel.SetActive(false);
		if(MenuUI) MenuUI.SetActive(true);
	}
		
	//We will use this to update the sound button graphics, various texts etc.
	void Updateitems()
	{
		Color soundColor = SoundButton.GetComponent<Image>().material.color;

		soundToggle = PlayerPrefs.GetInt ("Sound",1);

		if (soundToggle == 1) 
		{
			if(SoundText) SoundText.text = "Sound ON";
			AudioListener.pause=false;
			soundColor.a = 1;
			
		} 
		else if(soundToggle == 0){
			if(SoundText) SoundText.text = "Sound OFF";
			AudioListener.pause=true;
			soundColor.a = 0.5f;			
		}

		SoundButton.GetComponent<Image>().color = soundColor;

		if (controlToggle == 0) 
		{
			if(ControlText) ControlText.text = "Touch";
			PlayerPrefs.SetInt ("Controls",0);
			TouchControls.SetActive(true);
		} 

		else if(controlToggle == 1) 
		{
			if(ControlText) ControlText.text = "Hardware";
			PlayerPrefs.SetInt ("Controls",1);
			TouchControls.SetActive(false);
		}
	}

	//Used to toggle the sound on or off
	public void Sound()
	{
		if (soundToggle == 1)
			PlayerPrefs.SetInt("Sound", 0);
		else 
			PlayerPrefs.SetInt("Sound", 1);
		
		Updateitems ();
	}

	//Used to toggle the control type(Touch or Hardware)
	public void Controls()
	{
		controlToggle = PlayerPrefs.GetInt ("Controls",0);
		if (controlToggle == 0)
			controlToggle = 1;
		else 
			controlToggle = 0;

		Updateitems ();
	}

	//Show the settings panel
	public void ShowSettings()
	{
		InfoView.SetActive(false);
		SettingsPanel.SetActive(true);
	}

	//Hide the settings panel
	public void HideSettings()
	{
		SettingsPanel.SetActive(false);
	}

	//Show info view
	public void ShowInfo()
	{
		SettingsPanel.SetActive(false);
		InfoView.SetActive(true);
	}

	//Hide info view
	public void HideInfo()
	{
		SettingsPanel.SetActive(true);
		InfoView.SetActive(false);
	}

	//Hide the menu
	public void HideMenu()
	{
		MenuUI.SetActive(false);
	}

	//Show gameUI if the game is still running
	public void ShowGameUI()
	{
		if(isGameOver==false)
			GameUI.SetActive(true);
	}

	//Pause the game
	public void Pause()
	{
		if(isGameOver==false)
		{
			Time.timeScale=0;
			PauseUI.SetActive(true);
		}
	}

	//Unpause the game
	public void Continue()
	{
		PauseUI.SetActive(false);
		Time.timeScale=1;
	}

	//Increment the score every x seconds
	public IEnumerator Score(int increase)
	{
		if(isGameOver==false)
		{
			yield return new WaitForSeconds(0.5f);
			score +=(increase*multiplier);
			ScoreText.text="Score: " + score.ToString("#,0");
			StartCoroutine(Score(1));
		}
		else
			yield break;
	}

	//Receives damage dealt from enemies
	public void Damage(int damage)
	{
		//Subtract the damage dealt from the health level
		healthLevel -=damage;
		//Display remaining life on the health bar
		HealthBar.value=healthLevel;

		//Check if the health level is zero and game is still running.
		if(healthLevel<=0 && isGameOver==false)
		{
			//Stop the score counter
			StopCoroutine(Score(0));
			//Get the highscore value
			highscore = PlayerPrefs.GetInt("Highscore", 0);
			//Set the animator bool 'isDead' to true
			Anim.SetBool("isDead", true);
			//Play the die animation
			Player.GetComponent<Animator>().Play("RobotBoyDie");

			//Set the gameOver bool to true, compare the scores and play the game over sound
			if(isGameOver==false)
			{
				isGameOver=true;
				curScore = ((int)(score * 1));
				if(curScore>highscore)
				{
					PlayerPrefs.SetInt("Highscore",curScore);
					highscore=curScore;
				}

				#if TraceAds
				AdManager.instance.ShowInterstitial();
				#endif

				if(SoundMgr) SoundMgr.PlaySound(1,0.5f);
			}
		
			//Disable the health bar fill
			HealthBar.fillRect.gameObject.SetActive(false);
			//Hide touch controls
			if(TouchControls.activeInHierarchy) 
				TouchControls.SetActive(false);
			//Hide Game UI
			GameUI.SetActive(false);
			//Show GameOver UI
			GameOverUI.SetActive(true);
			//Display the score for that session
			FinalScore.text = curScore.ToString("#,0");
			//Display the highscore
			CurHighscore.text = "Best Score: " + highscore.ToString("#,0");
		}
	}

	//Reload the game
	public void Restart()
	{
#if (UNITY_5_3_OR_NEWER || UNITY_5_3)
		SceneManager.LoadScene("Game");
#elif UNITY_5
		Application.LoadLevel(Application.loadedLevelName);
#endif
	}
}