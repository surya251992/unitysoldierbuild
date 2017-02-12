using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CoinsCount : MonoBehaviour {
	public Text TotalCoins;
	// Use this for initialization
	void Start () {

		TotalCoins.text = "Coins Available:  "+PlayerPrefs.GetInt("Coins").ToString();
	
	}
}
