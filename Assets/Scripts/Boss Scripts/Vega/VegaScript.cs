using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegaScript : MonoBehaviour
{

    //========================================
    // REFERENCES/VARIABLES
    //========================================

    // ----- Animation States -----
    private enum State { Idle, Warp, Dagger, Blast, Lunge, Call, Shoot, Rise }
    private State state;

    // ----- Game Objects -----
    public GameObject Player;
    public GameObject VegaWarpPoint;

    public GameObject DaggerPoint;
    public GameObject LungePoint;

    // ----- Components -----
    Animator anim;
    Rigidbody2D rb2d;

    // ----- Vectors -----
    Vector3 SpitSpawn;

    // ----- Floats & Integers -----

    // ----- Booleans -----
    bool IsGrounded;
    bool IsLunging;

    bool Alert;
    private bool FacingRight = true;

    bool PlayerInvuln = false;

    public bool Stagger = false;

    bool MoveCooldown = false;
    bool WarpCooldown = false;

    bool StageTwo = false;

    // ----- Layer Masks -----
    LayerMask PlayerLayer;

    // ----- Audio Stuff -----

    AudioClip FistHit;
    void Start()
    {
        PlayerLayer = LayerMask.GetMask("Player");

        VegaWarpPoint = GameObject.Find("VegaWarpPoint");

        DaggerPoint = GameObject.Find("DaggerPoint");
        LungePoint = GameObject.Find("LungePoint");

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

        // ----- Player Distance Calculation -----
        float distance = Vector3.Distance(Player.transform.position, gameObject.transform.position);

        // ----- Ground Check -----

        if (rb2d.velocity.y <= 0.1)
        {
            IsGrounded = true;
        }
        else
        {
            IsGrounded = false;
        }

        //========================================
        // VEGA MOVES
        //========================================

        // ----- Warp Reset -----

        if (!IsGrounded && !IsLunging && !WarpCooldown)
        {
            state = State.Warp;
            SetState();

            Invoke("ResetPosition", .02f);

        }

        // ------ Moves -----
        if (IsGrounded && !MoveCooldown)
        {
            int moveRoll = Random.Range(2, 5);

            // ----- Dagger -----
            if (moveRoll == 2 && Player.transform.position.x >= -34.1f && Player.transform.position.x <= 34.1f)
            {
                MoveCooldown = true;
                Invoke("MoveReset", 3);
                state = State.Warp;
                SetState();

                Invoke("WarpMe", .025f);

                Invoke("DaggerCall", 0.5f);

                Invoke("IdleReset", 1);
            }

            //else
            //{
            //    MoveCooldown = true;
            //    state = State.Idle;
            //    Invoke("MoveReset", 0.2f);
            //}

            // ----- Blast Random -----
            if (moveRoll == 3)
            {
                MoveCooldown = true;
                state = State.Idle;
                Invoke("MoveReset", 0.2f);
            }

            // ----- Lunge -----
            if (moveRoll == 4)
            {
                MoveCooldown = true;
                state = State.Idle;
                Invoke("MoveReset", 0.2f);
            }

            // ----- Call -----
            if (moveRoll == 5)
            {
                MoveCooldown = true;
                state = State.Idle;
                Invoke("MoveReset", 0.2f);
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

    //========================================
    // ATTACK FUNCTIONS
    //========================================
    public float DaggerRange = 2;

    public void Dagger()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(DaggerPoint.transform.position, DaggerRange, PlayerLayer);

        foreach (Collider2D player in hitEnemies)
        {

            Debug.Log("We hit" + player.name);
            Player.GetComponent<PlayerScript>().ClawHit();
        }
    }

    //========================================
    // ATTACK STATE FUNCTIONS
    //========================================
    void WarpMe()
    {
        gameObject.transform.position = VegaWarpPoint.transform.position;
    }

    void DaggerCall()
    {
        state = State.Dagger;
        SetState();
    }

    void BlastCall()
    {
        state = State.Blast;
        SetState();
    }

    void LungeCall()
    {
        state = State.Lunge;
        SetState();
    }

    void CallCall()
    {
        state = State.Call;
        SetState();
    }

    //========================================
    // COOLDOWN FUNCTIONS
    //========================================
    void ResetPosition()
    {
        WarpCooldown = true;
        gameObject.transform.position = new Vector3(-1, 15, 0);

        state = State.Idle;
        SetState();

        Invoke("WarpReset", 1);
    }

    void MoveReset()
    {
        MoveCooldown = false;
    }

    void WarpReset()
    {
        WarpCooldown = false;
    }

    void IdleReset()
    {
        state = State.Idle;
        SetState();
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
