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
    public TMP_Text ManaText;

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
    Vector2 JumpForce = new Vector2(0, 1700);

    // ----- Floats & Integers -----
    public float PlayerSpeed;

    public int Mana;

    // ----- Booleans -----
    private bool IsGrounded;
    public bool FacingRight = true;

    private bool Invuln;

    public bool IsDashing = false;

    bool JumpBypass = false;

    bool floatPlat;

    Vector3 SunSpawn;

    // ----- Layer Masks -----

    // ----- Audio Stuff -----
    public AudioClip HitSound;

    private void Start()
    {
        Mana = 0;

        MaxHP = 300;
        CurrentHP = MaxHP;
        anim.SetInteger("BasicState", 0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            CurrentHP = 99999;
        }
       
        // -----

        SunSpawn = GameObject.Find("PunchPoint").transform.position;

        Health.text = "" + CurrentHP;
        ManaText.text = "Mana: " + Mana;

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

        if (Input.GetButtonDown("Jump") && IsGrounded || Input.GetButtonDown("Jump") && JumpBypass)
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

        //if (!IsGrounded && state == State.Idle)
        //{
        //    state = State.Fall;
        //    SetState();
        //    anim.SetBool("GroundPound", false);
            
        //}

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

        if (rb2d.velocity.y > 0.1f && !IsGrounded)
        {
            state = State.Rise;
            SetState();
        }

        else if (rb2d.velocity.y < -0.1f && !IsGrounded)
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

        if (IsGrounded)
        {

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
            anim.SetBool("GroundPound", true);

            anim.SetInteger("BasicState", 3);
            state = State.Idle;
            SetState();
        }

        //========================================
        // PLAYER SPELLCASTING
        //========================================

        // ----- Sun Sling -----
        if (Input.GetKeyDown(KeyCode.E) && Mana >= 2)
        {
            Instantiate(Resources.Load("Prefabs/SunBall") as GameObject, SunSpawn, Quaternion.identity);

            Mana -= 2;
        }

        //========================================
        // PLAYER I-FRAMES
        //========================================

        if (Invuln)
        {
            //Physics2D.IgnoreLayerCollision(6, 7, true);
            Physics2D.IgnoreLayerCollision(6, 9, true);

            //Debug.Log("Invulnerable");
        }

        else if (!Invuln)
        {
            //Physics2D.IgnoreLayerCollision(6, 7, false);
            Physics2D.IgnoreLayerCollision(6, 9, false);
            //Debug.Log("Not Invulnerable");
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
        Vector2 lightForceR = new Vector2(-200, 0);
        Vector2 lightForceL = new Vector2(200, 0);

        rb2d.velocity = Vector2.zero;

        if (FacingRight)
        {
            rb2d.AddForce(lightForceR);
        }

        else if (!FacingRight)
        {
            rb2d.AddForce(lightForceL);
        }

        CurrentHP -= 20;
        AudioSource.PlayClipAtPoint(HitSound, Camera.main.transform.position, .5f);

        StartCoroutine(CollisionInvuln());
    }

    public void LeechHit()
    {

        CurrentHP -= 10;
        AudioSource.PlayClipAtPoint(HitSound, Camera.main.transform.position, .5f);

        StartCoroutine(CollisionInvuln());
    }

    public void CollisionInvulnBypass()
    {
        StartCoroutine(CollisionInvuln());
    }

    // ----- Vega Boss -----

    public void DaggerHit()
    {

    }

    public void LungeHit()
    {

    }    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            rb2d.velocity = Vector2.zero;

            Debug.Log("Collided with Enemy");

            StartCoroutine(CollisionInvuln());

            CurrentHP += 10;
            ClawHit();
        }

        if (collision.gameObject.tag == "ManaMote")
        {
            Mana += 1;
        }

        if (collision.gameObject.tag == "killbox")
        {
            PlayerDeath();
        }

        if (collision.gameObject.tag == "dynaplattop")
        {
            Debug.Log("this is working");
            floatPlat = true;

            //if (rb2d.velocity.y != 0)
            //{
            //    rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
            //}

        }

        //if (collision.gameObject.tag == "dynplattop")
        //{
        //    Transform m_currMovingPlatform = collision.gameObject.transform;
        //    transform.SetParent(m_currMovingPlatform);
        //}

        if (collision.gameObject.tag == "WinBox")
        {
            SceneManager.LoadScene(19);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "floatingplat")
        {
            JumpBypass = false;
        }
    }
    //========================================
    // DEATH FUNCTION
    //========================================
    public void PlayerDeath()
    {
        Debug.Log("Star has perished...");

        GameSystem.GetComponent<GameSystem>().LoadCurrentScene();
    }

    //========================================
    // MISCELLANEOUS FUNCTIONS
    //========================================

    // ----- Invincibility Frames -----
    IEnumerator CollisionInvuln()
    {
        Invuln = true;
        Debug.Log("CollisionInvuln Called");

        yield return new WaitForSeconds(3);

        Invuln = false;
        Debug.Log("CollisionInvuln Complete");

        IsDashing = false;
    }

    // ----- Sprite Flipping -----
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

    //public void PunchMomentum()
    //{
    //    if (FacingRight)
    //    {
    //        gameObject.transform.position = 
    //    }
    //}
}
