using UnityEngine;
using System.Collections;
#if UNITY_5_3
using UnityEngine.SceneManagement;
#endif

public class Cleaner : MonoBehaviour 
{
	private Controller GameController;

	void Start()
	{
		//Get the Game Controller
		GameController=(Controller)FindObjectOfType(typeof(Controller));	
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		//If the col has the tag 'Line' despawn and put it back in the pool
		if (col.tag == "Line") 
		{
			ObjectsPool.Despawn(col.gameObject);		
		}

		//If the col has the tag 'Platform', destroy it
		if (col.tag == "Platform") 
		{
			Destroy(col.gameObject);		
		}

		//If the col has the tag 'Player', remove all lives
		if (col.tag == "Player") 
		{
			GameController.Damage(5);
		}
	}
}