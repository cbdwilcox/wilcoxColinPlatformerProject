using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierScript : MonoBehaviour
{
    Color baseColor;
    SpriteRenderer spriterend;

    //========================================
    // REFERENCES/VARIABLES
    //========================================

    // ----- Animation States -----

    private enum State { Idle, SpinPrep, Spin }
    private State state;

    // ----- Game Objects -----
    GameObject Player;

    // ----- Components -----
    Animator anim;
    Rigidbody2D rb2d;

    // ----- Booleans -----

    bool Alert;
    private bool FacingRight = true;

    //bool PlayerInvuln = false;

    public bool Stagger = false;

    public bool HitCool = false;

    // ----- Audio Stuff -----
    AudioClip FistHit;

    void Start()
    {
        CurrentHP = HitPoints;

        FistHit = Resources.Load("Sounds/fisthitdemo") as AudioClip;

        anim = gameObject.GetComponent<Animator>();

        spriterend = gameObject.GetComponent<SpriteRenderer>();
        baseColor = spriterend.color;

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

        if (Stagger)
        {
            state = State.Idle;
            SetState();
        }

        else if (!Stagger)
        {

            //if (Mathf.Abs(distance) < 4.5f && Alert)
            //{
            //    rb2d.velocity = Vector2.zero;
            //}

            if (Mathf.Abs(distance) > 4.5 && Mathf.Abs(distance) < 14.5f && Alert && !Stagger || Mathf.Abs(distance) > 21 && Alert && !HitCool)
            {
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, Player.transform.position, 10 * Time.deltaTime);

                state = State.SpinPrep;
                SetState();

                Invoke("SpinMe", 1);
            }


            //float idFK = rb2d.velocity.y;

            //Vector2 aaAA = new Vector2(0, idFK);
        }

        if (state == State.Spin)
        {
            Physics2D.IgnoreLayerCollision(6, 7, false);
        }

        else
        {
            Physics2D.IgnoreLayerCollision(6, 7, true);
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

    private void SpinMe()
    {
        state = State.Spin;
        SetState();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6 && !HitCool && state == State.Spin)
        {
            HitCool = true;
            Debug.Log("aaaaaa");

            Player.GetComponent<PlayerScript>().ClawHit();

            state = State.Idle;

            Invoke("HitReset", 3);
        }
    }

    void HitReset()
    {
        HitCool = false;
    }

    //========================================
    // DAMAGE FUNCTIONS
    //========================================

    // ----- Hit Points -----
    public int HitPoints = 200;
    public int CurrentHP;

    // ----- Fists of Sol -----
    public void LightHit()
    {
        Vector2 lightForceR = new Vector2(-200, 100);
        Vector2 lightForceL = new Vector2(200, 100);

        RedFlash();
        AudioSource.PlayClipAtPoint(FistHit, Camera.main.transform.position, .5f);

        //rb2d.velocity = Vector2.zero;

        //if (FacingRight)
        //{
        //    rb2d.AddForce(lightForceR);
        //}

        //else if (!FacingRight)
        //{
        //    rb2d.AddForce(lightForceL);
        //}

        CurrentHP -= 30;

        Stagger = true;

        Invoke("ResetStagger", 2.5f);

    }

    public void HeavyHit()
    {
        Vector2 heavyForceR = new Vector2(-300, 200);
        Vector2 heavyForceL = new Vector2(300, 200);

        //rb2d.velocity = Vector2.zero;

        RedFlash();
        AudioSource.PlayClipAtPoint(FistHit, Camera.main.transform.position, .5f);

        //if (FacingRight)
        //{
        //    rb2d.AddForce(heavyForceR);
        //}

        //else if (!FacingRight)
        //{
        //    rb2d.AddForce(heavyForceL);
        //}

        CurrentHP -= 50;

        Stagger = true;

        Invoke("ResetStagger", 2);
    }

    //========================================
    // DEATH FUNCTION
    //========================================
    void Die()
    {
        Debug.Log("Arcturus Soldier down...");

        Destroy(gameObject);
    }

    //========================================
    // MISCELLANEOUS FUNCTIONS
    //========================================

    // ----- Enemy Hit Visual Function -----
    void RedFlash()
    {
        spriterend.color = baseColor;
        spriterend.color = Color.red;
        Invoke("ResetColor", 0.5f);
    }

    void ResetColor()
    {
        spriterend.color = baseColor;
    }

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
