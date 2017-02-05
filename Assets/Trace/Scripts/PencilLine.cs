using UnityEngine;
using System.Collections;

public class PencilLine : MonoBehaviour {

	//to refer to our prefab pencilline
	private string Line="Line";
	private Controller GameController;
	private float lastx = 0f;
	private Transform thisTransform;
	private Transform CameraTransform;
	private bool SpriteEnabled=false;
	//Number of lines to draw before we pause a little
	private int skipLine=150;

	void Awake()
	{
		//Cache the transform
		thisTransform=transform;
		//Hide the hand
		this.transform.parent.GetComponent<SpriteRenderer>().enabled=false;
		//Get the camera transform
		CameraTransform=GameObject.FindGameObjectWithTag("MainCamera").transform;
		//Get the GameController
		GameController=(Controller)FindObjectOfType(typeof(Controller));

	}

	void Update()
	{
		//if we have moved far enough make a new pencilline
		if (CameraTransform.position.x > 15 && SpriteEnabled==false) 
		{
			//Show the hand
			this.transform.parent.GetComponent<SpriteRenderer>().enabled=true;
			//Set SpriteEnabled to true
			SpriteEnabled=true;
			//Start drawing the line
			StartCoroutine(drawLine());
		}
	}

	IEnumerator drawLine()
	{
		yield return new WaitForSeconds(0.01f);

		//Draws the line
		if (thisTransform.position.x > (lastx+0.02f)) 
		{
			if(!GameController.isGameOver)
				skipLine -=1;
			ObjectsPool.Spawn(Line, thisTransform.position,Quaternion.identity);
			lastx = thisTransform.position.x;
		}

		//If we not drawn enough lines, keep drawing. Else, pause for a bit then start drawing again
		if(skipLine>0)
		{
			StartCoroutine(drawLine());
		}
		else
		{
			yield return new WaitForSeconds(0.45f);
			StartCoroutine(drawLine());
			skipLine=Random.Range(150,250);
		}
	}
}