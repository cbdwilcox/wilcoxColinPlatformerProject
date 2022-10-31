using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VegaScript : MonoBehaviour
{

    public void SetGravZero()
    {
        rb2d.gravityScale = 0;
    }

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
    public GameObject LBlastPoint;
    public GameObject RBlastPoint;

    public GameObject BlastLeft;
    public GameObject BlastRight;

    public Image WinImage;

    public TMP_Text VegaHealth;

    // ----- Components -----
    Animator anim;
    Rigidbody2D rb2d;

    // ----- Vectors -----
    Vector3 LeftPoint;
    Vector3 RightPoint;

    // ----- Floats & Integers -----

    // ----- Booleans -----
    bool IsGrounded;
    bool IsLunging;

    bool Alert;
    private bool FacingRight = true;

    public bool Stagger = false;

    bool MoveCooldown = false;
    bool WarpCooldown = false;

    // ----- Layer Masks -----
    LayerMask PlayerLayer;

    // ----- Audio Stuff -----
    AudioClip FistHit;
    AudioClip WarpNoise;
    void Start()
    {

        PlayerLayer = LayerMask.GetMask("Player");

        VegaWarpPoint = GameObject.Find("VegaWarpPoint");

        DaggerPoint = GameObject.Find("DaggerPoint");
        LungePoint = GameObject.Find("LungePoint");
        LBlastPoint = GameObject.Find("LBlastPoint");
        RBlastPoint = GameObject.Find("RBlastPoint");

        BlastLeft = Resources.Load("Prefabs/blastleft") as GameObject;
        BlastRight = Resources.Load("Prefabs/blastright") as GameObject;

        anim = gameObject.GetComponent<Animator>();
        rb2d = gameObject.GetComponent<Rigidbody2D>();

        FistHit = Resources.Load("Sounds/fisthitdemo") as AudioClip;
        WarpNoise = Resources.Load("Sounds/betterwarpnoise") as AudioClip;

        Player = GameObject.Find("Player");

        CurrentHP = HitPoints;

        WinImage.enabled = false;
    }

    void Update()
    {
        VegaHealth.text = "Blue Sun Vega: " + CurrentHP;

        if (gameObject.transform.position.x >= 34)
        {
            rb2d.velocity = Vector2.zero;
        }

        if (IsLunging)
        {
            rb2d.gravityScale = 0;
        }

        LeftPoint = LBlastPoint.transform.position;
        RightPoint = RBlastPoint.transform.position;
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
            //AudioSource.PlayClipAtPoint(WarpNoise, Camera.main.transform.position, .5f);

            Invoke("ResetPosition", .02f);

        }

        // ------ Moves -----
        if (IsGrounded && !MoveCooldown && !IsLunging)
        {
            int moveRoll = Random.Range(1, 6);
            //Debug.Log("" + moveRoll);

            // ----- Dagger -----
            if (moveRoll == 2 && Player.transform.position.x >= -34.1f && Player.transform.position.x <= 34.1f || moveRoll == 1 && Player.transform.position.x >= -34.1f && Player.transform.position.x <= 34.1f)
            {
                MoveCooldown = true;
                Invoke("MoveReset", 3);
                state = State.Warp;
                SetState();
                AudioSource.PlayClipAtPoint(WarpNoise, Camera.main.transform.position, .5f);

                Invoke("WarpMe", .025f);

                Invoke("DaggerCall", 0.5f);

                Invoke("IdleReset", 1);
            }

            // ----- Blast -----

            if (moveRoll == 3)
            {
                MoveCooldown = true;
                rb2d.velocity = Vector2.zero;

                Invoke("BlastCall", 0.25f);

                Invoke("MoveReset", 1f);
            }

            // ----- Lunge -----

            if (moveRoll == 4)
            {
                MoveCooldown = true;
                IsLunging = true;

                state = State.Warp;
                SetState();
                AudioSource.PlayClipAtPoint(WarpNoise, Camera.main.transform.position, .5f);
                rb2d.velocity = new Vector2(0, 0);

                Invoke("LungeCall", 0.5f);
                rb2d.gravityScale = 0;

                if (Player.transform.position.y >= 14)
                {
                    Invoke("Lunge1", 0.52f);
                    rb2d.gravityScale = 0;
                }

                if (Player.transform.position.y < 14 && Player.transform.position.y >= 4.4)
                {
                    Invoke("Lunge2", 0.52f);
                    rb2d.gravityScale = 0;
                }

                if (Player.transform.position.y < 4.4 && Player.transform.position.y >= -3.4f)
                {
                    Invoke("Lunge3", 0.52f);
                    rb2d.gravityScale = 0;
                }

                if (Player.transform.position.y < -3.4f)
                {
                    Invoke("Lunge4", 0.52f);
                    rb2d.gravityScale = 0;
                }

                Invoke("MoveReset", 2.5f);
            }

            // ----- Call -----
            if (moveRoll == 5 || moveRoll == 6)
            {
                Debug.Log("CallMove is working.");
                MoveCooldown = true;

                Invoke("CallCall", 0.1f);

                Invoke("MoveReset", 0.2f);
            }
        } 


        //========================================
        // SPRITE FLIPPING
        //========================================

        if (Player.transform.position.x < gameObject.transform.position.x && FacingRight && !IsLunging)
        {
            SpriteFlip();
        }

        else if (Player.transform.position.x > gameObject.transform.position.x && !FacingRight && !IsLunging)
        {
            SpriteFlip();
        }

        else if (IsLunging)
        {
            
        }
    }

    //========================================
    // ATTACK FUNCTIONS 1
    //========================================
    public float DaggerRange = 2;
    public float LungeRange = 2;

    public void Dagger()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(DaggerPoint.transform.position, DaggerRange, PlayerLayer);

        foreach (Collider2D player in hitEnemies)
        {
            Player.GetComponent<PlayerScript>().ClawHit();
        }
    }

    //========================================
    // ATTACK FUNCTIONS 2
    //========================================

    // ----- ??? -----
    void WarpMe()
    {
        gameObject.transform.position = VegaWarpPoint.transform.position;
    }

    float LungePower = 3000;

    void Lunge1()
    {
        rb2d.gravityScale = 0;
        Physics2D.IgnoreLayerCollision(6, 7, false);
        Vector2 lungeForce = new Vector2(LungePower, 10);

        gameObject.transform.position = new Vector2(-36, 15);
        rb2d.AddForce(lungeForce);
        Invoke("ResetGravity", 2.5f);
    }
    void Lunge2()
    {
        rb2d.gravityScale = 0;
        Physics2D.IgnoreLayerCollision(6, 7, false);
        Vector2 lungeForce = new Vector2(LungePower, 10);

        gameObject.transform.position = new Vector2(-36, 6);
        rb2d.AddForce(lungeForce);
        Invoke("ResetGravity", 2.5f);
    }

    void Lunge3()
    {
        rb2d.gravityScale = 0;
        Physics2D.IgnoreLayerCollision(6, 7, false);
        Vector2 lungeForce = new Vector2(LungePower, 10);

        gameObject.transform.position = new Vector2(-36, -3.0f);
        rb2d.AddForce(lungeForce);
        Invoke("ResetGravity", 2.5f);
    }

    void Lunge4()
    {
        rb2d.gravityScale = 0;
        Physics2D.IgnoreLayerCollision(6, 7, false);
        Vector2 lungeForce = new Vector2(LungePower, 10);

        gameObject.transform.position = new Vector2(-36, -12.8f);
        rb2d.AddForce(lungeForce);
        Invoke("ResetGravity", 2.5f);
    }

    void Blasty()
    {
        if (FacingRight)
        {
            Instantiate(BlastLeft, LeftPoint, Quaternion.identity);
            Instantiate(BlastRight, RightPoint, Quaternion.identity);
        }

        else if (FacingRight)
        {
            Instantiate(BlastLeft, RightPoint, Quaternion.identity);
            Instantiate(BlastRight, LeftPoint, Quaternion.identity);
        }
    }

    void CallFire()
    {
        //Debug.Log("CallFire() Called");
        Instantiate(Resources.Load("Prefabs/StarGremlinSun") as GameObject, LungePoint.transform.position, Quaternion.identity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6 && IsLunging)
        {
            Physics2D.IgnoreLayerCollision(6, 7, true);

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(LungePoint.transform.position, LungeRange, PlayerLayer);

            foreach (Collider2D player in hitEnemies)
            {
                Player.GetComponent<PlayerScript>().ClawHit();
            }
        }

        if (collision.gameObject.layer == 8 && IsLunging)
        {
            rb2d.velocity = new Vector2(0, 0);
        }

        if (collision.gameObject.tag == "resetbox")
        {
            ResetPosition();
        }
    }

    // ----- State Calls -----

    void DaggerCall()
    {
        state = State.Dagger;
        SetState();
    }

    void BlastCall()
    {
        state = State.Blast;
        SetState();

        Invoke("Blasty", 0.25f);
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

        Invoke("CallFire", 0.25f);
    }

    //========================================
    // COOLDOWN FUNCTIONS
    //========================================
    void ResetGravity()
    {
        rb2d.gravityScale = 5;
    }
    void ResetPosition()
    {
        AudioSource.PlayClipAtPoint(WarpNoise, Camera.main.transform.position, .5f);
        Debug.Log("ResetPosition Called");
        WarpCooldown = true;
        rb2d.velocity = Vector2.zero;
        gameObject.transform.position = new Vector3(-1, 15, 0);

        state = State.Idle;
        SetState();

        Invoke("WarpReset", 1);
    }

    void MoveReset()
    {
        MoveCooldown = false;
        IsLunging = false;
        Physics2D.IgnoreLayerCollision(6, 7, true);
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
    public int HitPoints = 2000;
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

        SceneManager.LoadScene(6);

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
