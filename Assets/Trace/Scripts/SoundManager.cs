using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
	
	[Tooltip("The sound effects")]
	public AudioClip[] SoundFX;

	internal AudioSource SourceFX;

	void Start()
	{
		//Get the AudioSource
		SourceFX=GetComponent<AudioSource>();
	}

	public void PlaySound(int i, float v)
	{
		//Play a sound from the SoundFX array at x volume
		if(SoundFX[i]) SourceFX.PlayOneShot(SoundFX[i],v);
	}
}