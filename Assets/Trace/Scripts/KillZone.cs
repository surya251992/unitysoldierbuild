using System;
using UnityEngine;

public class KillZone : MonoBehaviour
{
	internal Controller GameController;

	void Start()
	{
		//Get the GameController
		GameController=(Controller)FindObjectOfType(typeof(Controller));
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
		//Kill and deactivate the player
        if (other.tag == "Player")
        {
			GameController.Damage(5);
			other.gameObject.SetActive(false);
        }

		//Despawn the weight
		if (other.tag == "Weight")
		{
			ObjectsPool.Despawn(other.gameObject);
		}
	}
}