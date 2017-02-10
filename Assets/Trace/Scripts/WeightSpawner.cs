using UnityEngine;
using System.Collections;

public class WeightSpawner : MonoBehaviour {
	
	[Tooltip("The minimum amount to wait before spawing another weight")]
	public float spawnMin = 1f;
	[Tooltip("The maximum amount to wait before spawing another weight")]
	public float spawnMax = 4f;

	private string Weight="Weight";
	private string WeightHeavy="WeightHeavy";
	private string coin="Coin";
	private Transform thisTransform;
	private Transform CameraObj;

	void Start () {
		thisTransform=transform;
		CameraObj=GameObject.FindGameObjectWithTag("MainCamera").transform;
		//start spawn 
		Spawn ();
	}
	
	void Spawn()
	{
		//If the camera object has gone far enough start spawning different weights randomly
		if(CameraObj.position.x>22)
		{
			int randm=Random.Range(0,2);
			if(randm==0)
				ObjectsPool.Spawn(coin, thisTransform.position,Quaternion.identity);
			if(randm==1)
				ObjectsPool.Spawn(WeightHeavy, thisTransform.position,Quaternion.identity);
			if (randm == 2)
				ObjectsPool.Spawn (coin, thisTransform.position, Quaternion.identity);
		}
		//Invoke spawn at random time interval between min and max
		Invoke ("Spawn", Random.Range (spawnMin, spawnMax));
	}
	
}