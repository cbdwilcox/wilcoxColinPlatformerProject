using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidLeechScript : MonoBehaviour
{
    Color baseColor;
    SpriteRenderer spriterend;

    //========================================
    // REFERENCES/VARIABLES
    //========================================

    // ----- Animation States -----

    private enum State { Idle, Latch }
    private State state;

    // ----- Game Objects -----
    GameObject Player;

    GameObject UnlatchPoint;

    // ----- Components -----
    Animator anim;
    Rigidbody2D rb2d;

    // ----- Booleans -----

    bool Alert;
    private bool FacingRight = true;

    bool isLatched = false;
    bool canLatch = true;

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
        UnlatchPoint = GameObject.Find("UnlatchPoint");
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

            if (Mathf.Abs(distance) > 0 && Mathf.Abs(distance) < 14.5f && Alert && !Stagger && !isLatched || Mathf.Abs(distance) > 21 && Alert && !isLatched)
            {
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, Player.transform.position, 15 * Time.deltaTime);

            }

            else if (isLatched)
            {
                gameObject.transform.position = Player.transform.position;
            }


            //float idFK = rb2d.velocity.y;

            //Vector2 aaAA = new Vector2(0, idFK);
        }

        if (state == State.Latch)
        {
            Physics2D.IgnoreLayerCollision(6, 7, true);
        }

        if (state == State.Idle)
        {
            Physics2D.IgnoreLayerCollision(6, 7, false);
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

        if (Stagger)
        {
            canLatch = false;
        }    
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6 && !HitCool && state == State.Idle && canLatch)
        {

            gameObject.GetComponent<CapsuleCollider2D>().enabled = false;

            canLatch = false;
            HitCool = true;
            Debug.Log("aaaaaa");

            isLatched = true;
            state = State.Latch;
            SetState();

            Invoke("HitReset", 3);
        }
    }

    public void Chomp()
    {
        Player.GetComponent<PlayerScript>().LeechHit();

        state = State.Latch;
        SetState();
    }

    public void LetGo()
    {
        //Vector2 unlatchMe = new Vector2 (Player.transform.position.x + 1f, Player.transform.position.y + 1f);
        //gameObject.transform.position = unlatchMe;

        //canLatch = true;

        //state = State.Idle;
        //SetState();

        //Invoke("EnableCollision", 2);

        Destroy(gameObject);
    }

    void EnableCollision()
    {
        Physics2D.IgnoreLayerCollision(6, 7, false);
    }

    void HitReset()
    {
        HitCool = false;
    }

    //========================================
    // DAMAGE FUNCTIONS
    //========================================

    // ----- Hit Points -----
    public int HitPoints = 10;
    public int CurrentHP;

    // ----- Fists of Sol -----
    public void LightHit()
    {
        gameObject.GetComponent<CapsuleCollider2D>().enabled = false;

        state = State.Idle;
        SetState();
        Vector2 unlatchMe = new Vector2(Player.transform.position.x + 1.5f, Player.transform.position.y + 1.5f);
        gameObject.transform.position = unlatchMe;

        Vector2 lightForceR = new Vector2(-200, 100);
        Vector2 lightForceL = new Vector2(200, 100);

        RedFlash();
        AudioSource.PlayClipAtPoint(FistHit, Camera.main.transform.position, .5f);

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

        Invoke("ResetStagger", 2.5f);

    }

    public void HeavyHit()
    {
        Vector2 unlatchMe = new Vector2(Player.transform.position.x + 1.5f, Player.transform.position.y + 1.5f);
        gameObject.transform.position = unlatchMe;
        state = State.Idle;
        SetState();

        Vector2 heavyForceR = new Vector2(-300, 200);
        Vector2 heavyForceL = new Vector2(300, 200);

        rb2d.velocity = Vector2.zero;

        RedFlash();
        AudioSource.PlayClipAtPoint(FistHit, Camera.main.transform.position, .5f);

        if (FacingRight)
        {
            rb2d.AddForce(heavyForceR);
        }

        else if (!FacingRight)
        {
            rb2d.AddForce(heavyForceL);
        }

        CurrentHP -= 50;

        Invoke("ResetStagger", 2);
    }

    //========================================
    // DEATH FUNCTION
    //========================================
    void Die()
    {
        Debug.Log("Void Leech down...");

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
        rb2d.velocity = Vector3.zero;
        gameObject.GetComponent<CapsuleCollider2D>().enabled = true;
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
