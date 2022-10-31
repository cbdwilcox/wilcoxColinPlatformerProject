using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteormiteScript : MonoBehaviour
{
    //========================================
    // REFERENCES/VARIABLES
    //========================================

    // ----- Game Objects -----
    GameObject Player;

    // ----- Components -----
    Rigidbody2D rb2d;

    // ----- Booleans -----

    bool Alert;
    private bool FacingRight = true;

    //bool PlayerInvuln = false;

    public bool Stagger = false;


    void Start()
    {
        CurrentHP = HitPoints;

        Player = GameObject.Find("Player");
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
        }

        // ----- Enemy AI  -----

        Vector2 rightMove = new Vector2(40, 0);
        Vector2 leftMove = new Vector2(-40, 0);

        if (!Stagger)
        {

            if (Mathf.Abs(distance) < 4.5f && Alert)
            {
                rb2d.velocity = Vector2.zero;
            }

            if (Mathf.Abs(distance) > 4.5 && Mathf.Abs(distance) < 14.5f && Alert && !Stagger || Mathf.Abs(distance) > 21 && Alert)
            {
                if (FacingRight)
                {
                    rb2d.velocity = Vector2.zero;
                    rb2d.AddForce(rightMove);
                }

                if (!FacingRight)
                {
                    rb2d.velocity = Vector2.zero;
                    rb2d.AddForce(leftMove);
                }
            }

            if (Mathf.Abs(distance) > 4.5f && Mathf.Abs(distance) < 21 && Alert && !Stagger)
            {
                // SHOOT METEOROID HERE
            }

            //float idFK = rb2d.velocity.y;

            //Vector2 aaAA = new Vector2(0, idFK);
        }

        else if (Stagger)
        {
            rb2d.velocity = Vector2.zero;
        }
    }

    //========================================
    // DAMAGE FUNCTIONS
    //========================================

        // ----- Hit Points -----
    public int HitPoints = 120;
    public int CurrentHP;

    // ----- Fists of Sol -----
    public void LightHit()
    {
        Vector2 lightForceR = new Vector2(200, 100);
        Vector2 lightForceL = new Vector2(-200, 100);

        rb2d.velocity = Vector2.zero;

        if (FacingRight)
        {
            rb2d.AddForce(lightForceR);
        }

        else if (!FacingRight)
        {
            rb2d.AddForce(lightForceL);
        }

        CurrentHP -= 30;

        Stagger = true;

        Invoke("ResetStagger", 2.5f);

    }

    public void HeavyHit()
    {
        Vector2 heavyForceR = new Vector2(400, 400);
        Vector2 heavyForceL = new Vector2(-400, 400);

        rb2d.velocity = Vector2.zero;

        if (FacingRight)
        {
            rb2d.AddForce(heavyForceR);
        }

        else if (!FacingRight)
        {
            rb2d.AddForce(heavyForceL);
        }

        CurrentHP -= 50;

        Stagger = true;

        Invoke("ResetStagger", 2);
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
}
