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
    GameObject LeftMet;
    GameObject RightMet;

    // ----- Components -----
    Rigidbody2D rb2d;

    // ----- Booleans -----

    bool Alert;
    private bool FacingRight = true;

    //bool PlayerInvuln = false;

    public bool Stagger = false;

    bool ShootCool = false;


    void Start()
    {
        CurrentHP = HitPoints;

        LeftMet = Resources.Load("Prefabs/LMet") as GameObject;
        RightMet = Resources.Load("Prefabs/RMet") as GameObject;

        rb2d = gameObject.GetComponent<Rigidbody2D>();

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

        if (!Stagger)
        {

            if (Mathf.Abs(distance) < 4.5f && Alert)
            {
                rb2d.velocity = Vector2.zero;
            }

            if (Mathf.Abs(distance) > 4.5 && Mathf.Abs(distance) < 14.5f && Alert && !Stagger || Mathf.Abs(distance) > 21 && Alert)
            {
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, Player.transform.position, 5 * Time.deltaTime);
            }

            if (Mathf.Abs(distance) > 4.5f && Mathf.Abs(distance) < 21 && Alert && !Stagger && FacingRight && !ShootCool)
            {
                ShootCool = true;
                Instantiate(RightMet, gameObject.transform.position, Quaternion.identity);

                Invoke("ShootReset", 4);
            }

            else if (Mathf.Abs(distance) > 4.5f && Mathf.Abs(distance) < 21 && Alert && !Stagger && !FacingRight && !ShootCool)
            {
                ShootCool = true;
                Instantiate(LeftMet, gameObject.transform.position, Quaternion.identity);

                Invoke("ShootReset", 4);
            }

            //float idFK = rb2d.velocity.y;

            //Vector2 aaAA = new Vector2(0, idFK);
        }

        else if (Stagger)
        {
            rb2d.velocity = Vector2.zero;
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

    void ShootReset()
    {
        ShootCool = false;
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