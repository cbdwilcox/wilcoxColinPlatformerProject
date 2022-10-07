using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerScript : MonoBehaviour
{

    //========================================
    // REFERENCES/VARIABLES
    //========================================

    public TMP_Text MoveKeys;

    // ----- Animation States -----
    private enum State { Idle, Run, Rise, Fall, Crouch, CrouchWalk, Slide, Basic1 }
    private State state;

    // ----- Components -----
    public Animator anim;
    public Rigidbody2D rb2d;

    // ----- Vectors -----
    public Vector2 JumpForce = new Vector2(0, 500);

    // ----- Floats & Integers -----
    public float PlayerSpeed;

    // ----- Booleans -----
    private bool IsGrounded;
    private bool FacingRight = true;

    private void Start()
    {
        anim.SetInteger("BasicState", 0);
    }

    private void Update()
    {
        // this is only here for now ill make a gamesystem script later
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            MoveKeys.enabled = false;
        }

        //========================================
        // PLAYER MOVEMENT CALCULATIONS/INPUT
        //========================================

        // ----- X-Axis Movement -----

        float xMove = Input.GetAxis("Horizontal");
        Vector2 newPos = gameObject.transform.position;
        newPos.x += xMove * PlayerSpeed * Time.deltaTime;
        gameObject.transform.position = newPos;
        
        // ----- Y-Axis Movement -----

        if (Input.GetButtonDown("Jump") && IsGrounded)
        {
            rb2d.velocity = Vector2.zero;
            rb2d.AddForce(JumpForce);
        }

        // ----- Ground Check -----

        if (rb2d.velocity.y == 0)
        {
            IsGrounded = true;
        }
        else
        {
            IsGrounded = false;
        }

        //========================================
        // PLAYER MOVEMENT ANIMATIONS
        //========================================

        // ----- Basic Movement States -----

        if (Input.GetButton("Horizontal") && IsGrounded)
        {
            state = State.Run;
            SetState();
        }

        else if (IsGrounded)
        {
            state = State.Idle;
            SetState();
        }

        if (rb2d.velocity.y > 0 && !IsGrounded)
        {
            state = State.Rise;
            SetState();
        }

        else if (rb2d.velocity.y < 0 && !IsGrounded)
        {
            state = State.Fall;
            SetState();
        }
        
        // ----- Sprite & Collider Flipping -----

        if (xMove > 0 && !FacingRight)
        {
            SpriteFlip();
        }

        else if (xMove < 0 && FacingRight)
        {
            SpriteFlip();
        }

        //========================================
        // PLAYER BASIC ATTACK
        //========================================

        if (Input.GetMouseButtonDown(0) && IsGrounded == true)
        {
            anim.SetInteger("BasicState", 1);
            state = State.Basic1;
            SetState();
        }

    }

    //========================================
    // MISCELLANEOUS FUNCTIONS
    //========================================

    void SpriteFlip() // For flipping sprites
    {
        FacingRight = !FacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void SetState() // Abbreviates setting an Animation State
    {
        anim.SetInteger("State", (int)state);
    }    
}
