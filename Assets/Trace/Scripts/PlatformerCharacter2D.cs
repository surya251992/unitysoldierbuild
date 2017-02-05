using System;
using UnityEngine;

public class PlatformerCharacter2D : MonoBehaviour
{
	[Tooltip("The fastest the player can travel in the x axis.")]
	public float MaxSpeed = 10f;
	[Tooltip("Amount of force added when the player jumps.")]
	public float JumpForce = 400f;
	[Range(0, 1)] 
	[Tooltip("Amount of maxSpeed applied to crouching movement. 1 = 100%")]
	public float CrouchSpeed = 0.36f;
	[Tooltip("Whether or not a player can steer while jumping")]
	public bool AirControl = false;
	[Tooltip("A mask determining what is ground to the character")]
	public LayerMask WhatIsGround;

	// A position marking where to check if the player is grounded.
    private Transform GroundCheck;
	// Radius of the overlap circle to determine if grounded
    const float GroundedRadius = 0.2f;
	// Whether or not the player is grounded.
    private bool Grounded;
	// A position marking where to check for ceilings
    private Transform CeilingCheck;
	// Radius of the overlap circle to determine if the player can stand up
    const float CeilingRadius = 0.01f;
	// Reference to the player's animator component.
    private Animator Anim;
	// Reference to the player's Rigidbody2D component.
    private Rigidbody2D Rigidbody2D;
	// For determining which way the player is currently facing.
    private bool FacingRight = true;  
	//The UISounds component
	internal SoundManager SoundMgr;

	private void Start()
	{
		SoundMgr=(SoundManager)FindObjectOfType(typeof(SoundManager));
	}

    private void Awake()
    {
        // Setting up references.
        GroundCheck = transform.Find("GroundCheck");
        CeilingCheck = transform.Find("CeilingCheck");
        Anim = GetComponent<Animator>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(GroundCheck.position, GroundedRadius, WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
                Grounded = true;
        }
        Anim.SetBool("Ground", Grounded);

        // Set the vertical animation
        Anim.SetFloat("vSpeed", Rigidbody2D.velocity.y);
    }

    public void Move(float move, bool crouch, bool jump)
    {
        // If crouching, check to see if the character can stand up
        if (!crouch && Anim.GetBool("Crouch"))
        {
            // If the character has a ceiling preventing them from standing up, keep them crouching
            if (Physics2D.OverlapCircle(CeilingCheck.position, CeilingRadius, WhatIsGround))
            {
                crouch = true;
            }
        }

        // Set whether or not the character is crouching in the animator
        Anim.SetBool("Crouch", crouch);

        //only control the player if grounded or airControl is turned on
        if (Grounded || AirControl)
        {
            // Reduce the speed if crouching by the crouchSpeed multiplier
            move = (crouch ? move*CrouchSpeed : move);

            // The Speed animator parameter is set to the absolute value of the horizontal input.
            Anim.SetFloat("Speed", Mathf.Abs(move));

          	Rigidbody2D.velocity = new Vector2(move*MaxSpeed, Rigidbody2D.velocity.y);


            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !FacingRight)
            {
                // ... flip the player.
                Flip();
            }
                // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && FacingRight)
            {
                // ... flip the player.
                Flip();
            }
        }
        // If the player should jump...
        if (Grounded==true && jump && Anim.GetBool("Ground"))
        {
            // Add a vertical force to the player.
            Grounded = false;
            Anim.SetBool("Ground", false);
            Rigidbody2D.AddForce(new Vector2(0f, JumpForce));
			if(SoundMgr) SoundMgr.PlaySound(0,0.55f);
			jump=false;
        }
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        FacingRight = !FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}