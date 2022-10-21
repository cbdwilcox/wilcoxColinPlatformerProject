using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GremlinClawScript : StateMachineBehaviour
{
    //========================================
    // REFERENCES/VARIABLES
    //========================================

    // ----- Components -----
    Transform ClawPoint;

    // ----- Floats & Integers -----
    public float HitTime;

    public float ClawRange;

    // ----- Booleans -----
    bool HasHit;

    // ----- Layer Masks -----
    LayerMask PlayerLayer;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ClawPoint = GameObject.Find("ClawPoint").transform;
        PlayerLayer = LayerMask.GetMask("Player");

        HitTime = 0.5f;
        HasHit = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        HitTime -= Time.deltaTime;
        
        if (HitTime < 0)
        {
            HitTime = 0;
        }

        if (HitTime == 0 && !HasHit)
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(ClawPoint.position, ClawRange, PlayerLayer);

            foreach (Collider2D player in hitEnemies)
            {
                Debug.Log("We hit" + player.name);

                player.GetComponent<PlayerScript>().ClawHit();
            }

            HasHit = true;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
