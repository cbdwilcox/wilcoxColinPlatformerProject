using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    //GIZMO SHIT

    public Transform PunchPoint;
    public float AttackRange = 0.5f;
    public int ForceDirSource = 1;

    private void OnDrawGizmosSelected()
    {
        if (PunchPoint == null)
            return;

        Gizmos.DrawWireSphere(PunchPoint.position, AttackRange);
    }

    //========================================
    // REFERENCES/VARIABLES
    //========================================

    public TMP_Text Health;

    // ----- Animation States -----
    private enum State { Idle, Run, Rise, Fall, Crouch, CrouchWalk, Slide, Basic1 }
    private State state;

    // ----- Game Objects -----
    public GameObject StarGremlin;
    public GameObject GameSystem;

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
        MaxHP = 100;
        CurrentHP = MaxHP;
        anim.SetInteger("BasicState", 0);
    }

    private void Update()
    {
        Health.text = "" + CurrentHP;

        if (CurrentHP <= 0)
        {
            PlayerDeath();
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
        // PLAYER BASIC COMBO
        //========================================

        if (Input.GetMouseButtonDown(0) && IsGrounded)
        {
            anim.SetInteger("BasicState", 1);
            state = State.Basic1;
            SetState();
        }

        if (Input.GetMouseButtonDown(0) && !IsGrounded)
        {
            anim.SetInteger("BasicState", 3);
            state = State.Idle;
            anim.SetBool("GroundPound", true);
            SetState();
        }
    }

    //========================================
    // DAMAGE FUNCTIONS
    //========================================

    // ----- Hit Points ------
    public int MaxHP = 100;
    public int CurrentHP;

    // ----- Star Gremlin -----

    public void ClawHit()
    {
        CurrentHP -= 20;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            CollisionInvuln();
        }
    }

    //========================================
    // DEATH FUNCTION
    //========================================
    void PlayerDeath()
    {
        Debug.Log("Star has perished...");

        SceneManager.LoadScene(0);
    }

    //========================================
    // MISCELLANEOUS FUNCTIONS
    //========================================

    // ----- Invincibility Frames -----
    IEnumerator CollisionInvuln()
    {
        Physics2D.IgnoreLayerCollision(6, 7);

        yield return new WaitForSeconds(3);

        Physics2D.IgnoreLayerCollision(6, 7, false);
    }

    // ----- Sprite Flipping -----
    void SpriteFlip()
    {
        FacingRight = !FacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

        ForceDirSource *= -1;
    }

    // ----- Setting Animation States -----
    void SetState()
    {
        anim.SetInteger("State", (int)state);
    }    
}
