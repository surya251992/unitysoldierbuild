using System;
using UnityEngine;
using CnControls;

public class Platformer2DUserControl : MonoBehaviour
{
	private Controller GameController;
	private PlatformerCharacter2D Character;
    private bool Jump;
	private bool Crouch;
	private float h;

    private void Awake()
    {
		//Get the PlatformerCharacter2D script
        Character = GetComponent<PlatformerCharacter2D>();
		//Get the Game Controller
		GameController=(Controller)FindObjectOfType(typeof(Controller));
    }
		
    private void Update()
    {
        if (!Jump)
        {
			Jump = CnInputManager.GetButtonDown("Jump");
        }
    }
		
    private void FixedUpdate()
    {
		Crouch = CnInputManager.GetButton("Crouch");
		h = CnInputManager.GetAxis("Horizontal");
		// Pass all parameters to the character control script.
		if(!GameController.isGameOver)
			Character.Move(h, Crouch, Jump);
        Jump = false;
    }
}