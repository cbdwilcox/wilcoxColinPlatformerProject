using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasic1Script : StateMachineBehaviour
{
    //========================================
    // REFERENCES/VARIABLES
    //========================================

    // ----- Combo States -----
    private enum State { Basic2 = 2 }
    private State state;

    // ----- Components -----
    Animator anim;
    public Transform AttackPoint;

    Rigidbody2D rb2d;

    public Vector3 ManaPoint;

    // ----- Floats & Integers -----
    public float Timer;

    public float AttackRange;

    // ----- Booleans -----
    private bool Combo = false;

    // ----- Layer Masks -----
    public LayerMask EnemyLayers;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Timer = 3;
        Combo = false;

        anim = GameObject.Find("Player").GetComponent<Animator>();

        EnemyLayers = LayerMask.GetMask("Enemies");
        AttackPoint = GameObject.Find("PunchPoint").transform;
        ManaPoint = AttackPoint.position;
        
        // ----- Hit Detection -----
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, EnemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            Debug.Log("We hit" + enemy.name);

            // ----- Enemies -----

            if(enemy.tag == "StarGremlin")
            {
                enemy.GetComponent<StarGremlinScript>().LightHit();
            }

            // ----- Bosses -----

            if(enemy.tag == "Vega")
            {
                enemy.GetComponent<VegaScript>().LightHit();
                Debug.Log("Hit Complete");
            }

            Instantiate(Resources.Load("Prefabs/ManaMote") as GameObject, ManaPoint, Quaternion.identity);
        }

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Timer > 0)
        {
            Timer -= Time.deltaTime;
        }

        if (Timer == 0)
        {
            anim.SetInteger("BasicState", 0);
            anim.SetInteger("State", 0);
        }

        else if (Input.GetMouseButtonDown(0))
        {
            Combo = true;
            state = State.Basic2;
            SetState();
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!Combo)
        {
            anim.SetInteger("BasicState", 0);
        }
    }

    void SetState()
    {
        anim.SetInteger("BasicState", (int)state);
    }
}
