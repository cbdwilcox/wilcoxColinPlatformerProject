using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasic2Script : StateMachineBehaviour
{
    enum State { Basic3 = 3}
    State state;

    // ----- Components -----
    Animator anim;
    public Transform AttackPoint;

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
        anim = GameObject.Find("Player").GetComponent<Animator>();

        EnemyLayers = LayerMask.GetMask("Enemies");
        AttackPoint = GameObject.Find("PunchPoint").transform;

        // ----- Hit Detection -----
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, EnemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("We hit" + enemy.name);

            enemy.GetComponent<StarGremlinScript>().LightHit();
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Input.GetMouseButtonDown(0))
        {
            state = State.Basic3;
            SetState();
        }

        if (Timer > 0)
        {
            Timer -= Time.deltaTime;
        }

        if (Timer == 0)
        {
            anim.SetInteger("BasicState", 0);
            anim.SetInteger("State", 0);
        }
    }

    void SetState()
    {
        anim.SetInteger("BasicState", (int)state);
    }
}