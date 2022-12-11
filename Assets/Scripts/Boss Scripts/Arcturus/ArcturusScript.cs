using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ArcturusScript : MonoBehaviour
{
    Color baseColor;
    SpriteRenderer spriterend;

    public TMP_Text ArcturusHealth;

    LayerMask PlayerLayer;

    //========================================
    // REFERENCES/VARIABLES
    //========================================

    // ----- Animation States -----

    private enum State { Idle, Slam, Slamback, Flare, Punch }
    private State state;

    // ----- Game Objects -----
    GameObject Player;
    GameObject Flare;

    Transform HammerPoint;
    Transform BackPoint;

    // ----- Floats & Integers
    public float HammerRange;
    public float PunchRange;

    private float backDistance;

    // ----- Components -----
    Animator anim;
    Rigidbody2D rb2d;

    // ----- Booleans -----
    private bool FacingRight = true;
    private bool isAttacking = false;
    public bool flareCool = false;

    //bool PlayerInvuln = false;

    public bool Stagger = false;

    public bool HitCool = false;

    // ----- Audio Stuff -----
    AudioClip FistHit;

    void Start()
    {
        flareCool = false;

        CurrentHP = HitPoints;

        HammerPoint = GameObject.Find("HammerPoint").transform;
        BackPoint = GameObject.Find("BackPoint").transform;

        Flare = Resources.Load("Prefabs/RedFlare") as GameObject;

        PlayerLayer = LayerMask.GetMask("Player");

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

        ArcturusHealth.text = "Arcturus, the Red General: " + CurrentHP;

        if (CurrentHP <= 0)
        {
            Die();
        }

        //========================================
        // ???
        //========================================

        if (state != State.Idle)
        {
            isAttacking = true;
        }

        else
        {
            isAttacking = false;
        }

        // ----- Player Distance Calculation -----
        float distance = Vector3.Distance(Player.transform.position, gameObject.transform.position);
        backDistance = Vector3.Distance(Player.transform.position, BackPoint.transform.position);

        //Debug.Log(distance);

        // ----- Boss AI  -----

        if (Mathf.Abs(distance) > 6 || Mathf.Abs(distance) > 21)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, Player.transform.position, 10 * Time.deltaTime);
        }

        if (Mathf.Abs(distance) <= 6 && !HitCool)
        {
            int moveRoll = Random.Range(1, 4);

            if (moveRoll == 1)
            {
                HitCool = true;
                state = State.Slam;
                SetState();
            }

            if (moveRoll == 2 && !flareCool)
            {
                Invoke("FlareOff", 1.5f);

                HitCool = true;
                state = State.Flare;
                SetState();
            }

            else if (moveRoll == 2 && flareCool)
            {
                HitCool = true;
                state = State.Punch;
                SetState();
            }

            if (moveRoll == 3)
            {
                HitCool = true;
                state = State.Idle;
                SetState();
            }

            if (moveRoll == 4)
            {
                HitCool = true;
                state = State.Punch;
                SetState();
            }

        }



        //========================================
        // SPRITE FLIPPING
        //========================================

        if (Player.transform.position.x < gameObject.transform.position.x && FacingRight && !isAttacking)
        {
            SpriteFlip();
        }

        else if (Player.transform.position.x > gameObject.transform.position.x && !FacingRight && !isAttacking)
        {
            SpriteFlip();
        }
    }

    public void BackSwing()
    {
            Debug.Log("Checked Backswing");

            int backRoll = Random.Range(1, 3);

            if (backRoll == 1)
            {
                ResetIdle();
            }

            if (backRoll == 2 || backRoll == 3)
            {
                state = State.Slamback;
                SetState();
            }
    }

    public void ResetIdle()
    {
        state = State.Idle;
        SetState();

        Invoke("ResetHit", 2);
    }

    void ResetHit()
    {
        HitCool = false;
    }

    void FlareOff()
    {
        flareCool = true;
    }

    //========================================
    // DAMAGE FUNCTIONS
    //========================================

    // ----- Hit Points -----
    public int HitPoints = 2500;
    public int CurrentHP;

    // ----- Fists of Sol -----
    public void LightHit()
    {
        flareCool = false;

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
        flareCool = false;

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
        Debug.Log("Arcturus has been slain...");

        SceneManager.LoadScene(12);

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
    public void HammerHit()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(HammerPoint.position, HammerRange, PlayerLayer);

        foreach (Collider2D player in hitEnemies)
        {

            Debug.Log("We hit" + player.name);
            Player.GetComponent<PlayerScript>().ClawHit();
        }
    }

    public void PunchHit()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(HammerPoint.position, PunchRange, PlayerLayer);

        foreach (Collider2D player in hitEnemies)
        {

            Debug.Log("We hit" + player.name);
            Player.GetComponent<PlayerScript>().ClawHit();
        }
    }

    public void BackHit()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(BackPoint.position, HammerRange, PlayerLayer);

        foreach (Collider2D player in hitEnemies)
        {

            Debug.Log("We hit" + player.name);
            Player.GetComponent<PlayerScript>().ClawHit();
        }
    }

    public void RedFlare()
    {
        Vector3 flarePos = new Vector3(0, 0, 0);

        Instantiate(Flare, flarePos, Quaternion.identity);
    }
}
