using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Pickup : MonoBehaviour {


	private Controller GameController;
	private Transform thisTransform;
	public Text coins;

	void Start()
	{
		//Cache the transform
		thisTransform=transform;
		//Get the Game Controller
		GameController=(Controller)FindObjectOfType(typeof(Controller));
		//coins=
	}

	void Update () 
	{
		//Enable if this weight is spawned after the spawner reaches a position past x 
		if (thisTransform.position.x > 49) 
			this.gameObject.SetActive (true);
		else
			this.gameObject.SetActive (false);

		//Get all the objects in a radius tagged Line
		Collider2D[] colliders = Physics2D.OverlapCircleAll (thisTransform.position,0.7f);

		//Dispawn the lines hit
		//foreach(Collider2D col in colliders){
			/*if (col.tag == "Line")
				ObjectsPool.Despawn(col.gameObject);*/
		//}
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		//If the weight hits the player, despawn it and remove one life
		if (coll.gameObject.tag == "Player")
		{
			ObjectsPool.Despawn(this.gameObject);
			//PlayerPrefs.SetInt ("Coins",PlayerPrefs.GetInt("Coins")+1);
			//print (PlayerPrefs.GetInt("Coins"));
			//GameController.Damage(1);
		}
	}
}
