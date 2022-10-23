using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GremlinClawScript : StateMachineBehaviour
{
    //========================================
    // REFERENCES/VARIABLES
    //========================================

    // ----- Game Objects -----
    GameObject Player;

    // ----- Components -----
    Transform ClawPoint;

    // ----- Floats & Integers -----
    public float HitTime;

    public float ClawRange;

    // ----- Booleans -----
    bool HasHit;

    // ----- Layer Masks -----
    LayerMask PlayerLayer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ClawPoint = GameObject.Find("ClawPoint").transform;
        PlayerLayer = LayerMask.GetMask("Player");

        Player = GameObject.Find("Player");

        HitTime = 0.5f;
        HasHit = false;
    }
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
                Player.GetComponent<PlayerScript>().ClawHit();
            }

            HasHit = true;
        }
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
