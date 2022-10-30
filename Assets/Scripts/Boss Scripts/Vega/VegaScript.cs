using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegaScript : MonoBehaviour
{

    //========================================
    // REFERENCES/VARIABLES
    //========================================

    // ----- Animation States -----
    private enum State { Idle, Lunge, Rise, Call }
    private State state;

    // ----- Game Objects -----
    public GameObject Player;

    // ----- Components -----
    Animator anim;
    Rigidbody2D rb2d;

    // ----- Vectors -----
    Vector3 SpitSpawn;

    // ----- Floats & Integers -----

    // ----- Booleans -----
    bool IsGrounded;

    bool Alert;
    private bool FacingRight = true;

    bool PlayerInvuln = false;

    public bool Stagger = false;

    // ----- Audio Stuff -----

    AudioClip FistHit;
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        rb2d = gameObject.GetComponent<Rigidbody2D>();

        FistHit = Resources.Load("Sounds/fisthitdemo") as AudioClip;

        Player = GameObject.Find("Player");

        

        CurrentHP = HitPoints;
    }

    void Update()
    {
        // Hit Points

        if (CurrentHP <= 0)
        {
            Die();
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
        // SPRITE FLIPPING
        //========================================

        if (Player.transform.position.x < gameObject.transform.position.x && FacingRight)
        {
            SpriteFlip();
        }

        else if (Player.transform.position.x > gameObject.transform.position.x && !FacingRight)
        {
            SpriteFlip();
        }
    }

    //========================================
    // DAMAGE FUNCTIONS
    //========================================

    // ----- Hit Points -----
    public int HitPoints = 100;
    public int CurrentHP;

    // ----- Fists of Sol -----
    public void LightHit()
    {
        CurrentHP -= 20;
        AudioSource.PlayClipAtPoint(FistHit, Camera.main.transform.position, .5f);
    }

    public void HeavyHit()
    {
        CurrentHP -= 50;
        AudioSource.PlayClipAtPoint(FistHit, Camera.main.transform.position, .5f);
    }

    //========================================
    // DEATH FUNCTION
    //========================================
    void Die()
    {
        Debug.Log("Vega has been slain...");

        Destroy(gameObject);
    }

    //========================================
    // MISCELLANEOUS FUNCTIONS
    //========================================

    // ----- Sprite Flipping Function -----

    void ResetStagger()
    {
        Stagger = false;
    }
    void SpriteFlip()
    {
        FacingRight = !FacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    // ----- Setting Animation States -----
    void SetState()
    {
        anim.SetInteger("State", (int)state);
    }
}
