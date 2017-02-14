using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class showad : MonoBehaviour {
	public GameObject thisbutton;
	// Use this for initialization
	public void OnClick () {
		//thisbutton.enabled = false;
		if (Advertisement.IsReady ("rewardedVideo")) {
			Advertisement.Show ("rewardedVideo", new ShowOptions {
				//pause = true,
				resultCallback = result => {
					switch(result)
					{
					case (ShowResult.Finished):
						//onVideoPlayed(true);
						print("finished");
						PlayerPrefs.SetInt("Coins",PlayerPrefs.GetInt("CurrentCoins")+PlayerPrefs.GetInt("Coins"));
						break;
					case (ShowResult.Failed):
						print("failed");
						//PlayerPrefs.SetInt ("DeathCount", 9);
						//Destroy (thisbutton);
						//onVideoPlayed(false);
						break;
					case(ShowResult.Skipped):
						print("skipped");
						//PlayerPrefs.SetInt ("DeathCount", 9);
						//Destroy (thisbutton);
						//onVideoPlayed(false);
						break;
					}
				}
			}
			);
		}

		SceneManager.LoadScene("game_line");

}
}