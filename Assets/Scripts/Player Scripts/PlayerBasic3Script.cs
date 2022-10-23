using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasic3Script : StateMachineBehaviour
{
    // ----- Components -----
    Animator anim;
    public Transform AttackPoint;

    // ----- Floats & Integers -----
    public float HitTime;

    public float AttackRange;

    // ----- Booleans -----
    private bool HasHit = false;

    // ----- Layer Masks -----
    public LayerMask EnemyLayers;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        HitTime = 1.2f;
        
        anim = GameObject.Find("Player").GetComponent<Animator>();

        anim.SetInteger("State", 0);
        anim.SetInteger("BasicState", 0);

        EnemyLayers = LayerMask.GetMask("Enemies");
        AttackPoint = GameObject.Find("PunchPoint").transform;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        HitTime -= 1 * Time.deltaTime;

        if (HitTime < 0 && !HasHit)
        {
            HitTime = 0;
        }

        if (HitTime == 0)
        {
            // ----- Hit Detection -----
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, EnemyLayers);

            foreach (Collider2D enemy in hitEnemies)
            {
                Debug.Log("We hit " + enemy.name + " with a Ground Pound!");

                enemy.GetComponent<StarGremlinScript>().HeavyHit();
                anim.SetBool("GroundPound", false);
                HasHit = true;
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        anim.SetBool("GroundPound", false);
    }
}
