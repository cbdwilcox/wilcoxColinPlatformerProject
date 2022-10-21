using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasic3Script : StateMachineBehaviour
{
    Animator anim;

    public float HitTime;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        HitTime = 1.2f;
        
        anim = GameObject.Find("Player").GetComponent<Animator>();

        anim.SetInteger("State", 0);
        anim.SetInteger("BasicState", 0);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        HitTime -= 1 * Time.deltaTime;

        if (HitTime == 0)
        {
            // insert attack thing
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        anim.SetBool("GroundPound", false);
    }
}
