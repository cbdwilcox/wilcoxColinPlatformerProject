using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarGremlinScript : MonoBehaviour
{
    Color baseColor;
    SpriteRenderer spriterend;

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

    // ----- Vectors -----
    Vector3 spitSpawn;

    // ----- Floats & Integers -----

    // ----- Booleans -----
    bool isGrounded;

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
        spriterend = gameObject.GetComponent<SpriteRenderer>();

        FistHit = Resources.Load("Sounds/fisthitdemo") as AudioClip;

        Player = GameObject.Find("Player");
        baseColor = spriterend.color;

        CurrentHP = HitPoints;

        Alert = false;
        Stagger = false;
    }

    void Update()
    {

        spitSpawn.x = gameObject.transform.position.x;
        spitSpawn.y = gameObject.transform.position.y + 2;
        spitSpawn.z = gameObject.transform.position.z;

        // Hit Points

        if (CurrentHP <= 0)
        {
            Die();
        }

        // ----- Ground Check -----

        if (rb2d.velocity.y == 0)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
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

        // ----- Enemy AI  -----

        Vector2 rightMove = new Vector2(40, 0);
        Vector2 leftMove = new Vector2(-40, 0);

        if (Stagger)
        {
            state = State.Idle;
            SetState();
        }

        else if (!Stagger)
        {
            if (Stagger)
            {
                return;
            }

            else if(Mathf.Abs(distance) < 4.5f && Alert)
            {
                rb2d.velocity = Vector2.zero;

                state = State.Claw;
                SetState();
            }

            else if (Mathf.Abs(distance) > 4.5 && Mathf.Abs(distance) < 14.5f && Alert && isGrounded || Mathf.Abs(distance) > 21 && Alert && isGrounded)
            {
                state = State.Run;
                SetState();

                if (state == State.Run && FacingRight && !PlayerInvuln && isGrounded)
                {
                    rb2d.velocity = Vector2.zero;
                    rb2d.AddForce(rightMove);
                }

                if (state == State.Run && !FacingRight && !PlayerInvuln && isGrounded)
                {
                    rb2d.velocity = Vector2.zero;
                    rb2d.AddForce(leftMove);
                }
            }

            // NOTE: FIX ALL THIS LATER!

            if (Mathf.Abs(distance) > 4.5f && Mathf.Abs(distance) < 21 && Alert)
            {
                state = State.Spit;
                SetState();
            }

            float idFK = rb2d.velocity.y;

            Vector2 aaAA = new Vector2(0, idFK);

            if (state == State.Spit)
            {
                rb2d.velocity = aaAA;
            }
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

    // ----- Gremlin Spit -----

    void SpitAttack()
    {
        Instantiate(Resources.Load("Prefabs/StarGremlinSun") as GameObject, spitSpawn, Quaternion.identity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "killbox")
        {
            Destroy(gameObject);
        }
    }

    IEnumerator InvulnCooldown()
    {
        yield return new WaitForSeconds(3);

        PlayerInvuln = false;
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
        Vector2 lightForceR = new Vector2(-400, 100);
        Vector2 lightForceL = new Vector2(400, 100);

        rb2d.velocity = Vector2.zero;

        RedFlash();
        AudioSource.PlayClipAtPoint(FistHit, Camera.main.transform.position, .5f);

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
        Vector2 heavyForceR = new Vector2(-600, 600);
        Vector2 heavyForceL = new Vector2(600, 600);

        RedFlash();
        AudioSource.PlayClipAtPoint(FistHit, Camera.main.transform.position, .5f);

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
