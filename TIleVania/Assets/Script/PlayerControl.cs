using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerControl : MonoBehaviour {

    public int life;
    public int score;

    public float RunSpeed = 1f;
    public float JumpForce = 1f;
    public float ClimbSpeed = 1f;
    private float Gravity;

    private Rigidbody2D rb2d;
    private Animator anim;

    private BoxCollider2D myFeet;
    private CapsuleCollider2D myBody;
    

    private void Start()
    {
        
        
        //Component
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        myFeet = GetComponent<BoxCollider2D>();
        myBody = GetComponent<CapsuleCollider2D>();
        //Variable value
        score = 0;
        Gravity = rb2d.gravityScale;

    }

    // Update is called once per frame
    void Update () {

        Run();
        Jump();
        ClimbLadder();
	}

    private void Run()
    {
        float ThrowDirection = CrossPlatformInputManager.GetAxis( "Horizontal" );
        FlipSprite(ThrowDirection);

        Vector2 PlayerVelocity = new Vector2( ThrowDirection * RunSpeed, rb2d.velocity.y );
        rb2d.velocity = PlayerVelocity;

        bool PlayerAsVelocity = ( Mathf.Abs( ThrowDirection ) > Mathf.Epsilon );
        anim.SetBool( "isRunning", PlayerAsVelocity );
    }

    private void Jump()
    {
        //bool PlayerAsVelocity = ( Mathf.Abs( rb2d.velocity.y ) > Mathf.Epsilon );
        bool TouchingGround = myFeet.IsTouchingLayers( LayerMask.GetMask( "Ground" ) );
        if (CrossPlatformInputManager.GetButtonDown( "Jump" ) && TouchingGround)
        {
            Vector2 PlayerVelocity = new Vector2( 0f, JumpForce );
            rb2d.velocity += PlayerVelocity;
        }
    }

    private void ClimbLadder()
    {
        bool TouchingLadder = myBody.IsTouchingLayers( LayerMask.GetMask( "Ladder" ) );
        
        if (!TouchingLadder)
        {
            rb2d.gravityScale = Gravity;
            anim.SetBool( "isClimbing", false );
            return;
        }
        rb2d.gravityScale = 0f;
        bool PlayerHasVelocitySpeed = Mathf.Abs( rb2d.velocity.y ) > Mathf.Epsilon;
        anim.SetBool( "isClimbing", PlayerHasVelocitySpeed );
        float ThrowClimb = CrossPlatformInputManager.GetAxis( "Vertical" );
        Vector2 ClimbVelocity = new Vector2( rb2d.velocity.x, ClimbSpeed * ThrowClimb );
        rb2d.velocity = ClimbVelocity;
    }

    private void FlipSprite(float value) // Flip player as of direction he run
    {
        SpriteRenderer SpriteRen = GetComponent<SpriteRenderer>();
        if (value < 0 && SpriteRen.flipX == false)
        {
            SpriteRen.flipX = true;
        }
        else if (value > 0 && SpriteRen.flipX == true)
        {
            SpriteRen.flipX = false;
        }
    }
}
