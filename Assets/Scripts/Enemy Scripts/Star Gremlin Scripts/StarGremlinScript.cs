using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarGremlinScript : MonoBehaviour
{
    //========================================
    // REFERENCES/VARIABLES
    //========================================

    // ----- Animation States -----

    private enum State { Idle, Run, Rise, Fall, Claw, Spit }
    private State state;

    // ----- Game Objects -----
    public GameObject Player;

    // ----- Components -----
    Animator anim;
    Rigidbody2D rb2d;

    // ----- Floats & Integers -----
    int ForceDir;

    // ----- Booleans -----
    bool Alert;
    private bool FacingRight = true;

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        rb2d = gameObject.GetComponent<Rigidbody2D>();

        CurrentHP = HitPoints;
        ForceDir = GameObject.Find("Player").GetComponent<PlayerScript>().ForceDirSource;

        Alert = false;
    }

    void Update()
    {
        // Hit Points

        if (CurrentHP <= 0)
        {
            Die();
        }

        //========================================
        // ???
        //========================================

        // ----- Player Distance Calculation -----
        float distance = Vector3.Distance(Player.transform.position, gameObject.transform.position);

        //Debug.Log(distance);

        // ----- Enemy Alert -----
        if (Mathf.Abs(distance) < 17f && !Alert)
        {
            Alert = true;
            state = State.Run;
            SetState();
        }

        // ----- Enemy Attack AI -----

        if (Mathf.Abs(distance) < 10f && Alert)
        {

            state = State.Claw;
            SetState();
        }

        if (Mathf.Abs(distance) > 10f && Alert)
        {
            state = State.Spit;
            SetState();
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
        Debug.Log("Hit Star Gremlin with Light Attack!");

        Vector2 lightForce = new Vector2(200, 100);
        // ForceDir not working????

        rb2d.velocity = Vector2.zero;
        rb2d.AddForce(lightForce);

        CurrentHP -= 30;
    }

    public void HeavyHit()
    {
        Vector2 heavyForce = new Vector2(200 * ForceDir, 0);

        rb2d.velocity = Vector2.zero;
        rb2d.AddForce(heavyForce);

        CurrentHP -= 50;
    }

    // ----- Sunsling -----
    public void SunSlingHit()
    {
        CurrentHP -= 70;
    }

    //========================================
    // DEATH FUNCTION
    //========================================
    void Die()
    {
        Debug.Log("Star Gremlin down...");

        Destroy(gameObject);
    }

    //========================================
    // MISCELLANEOUS FUNCTIONS
    //========================================

    // ----- Sprite Flipping Function -----
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
