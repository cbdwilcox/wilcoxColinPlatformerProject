using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasic3Script : StateMachineBehaviour
{
    Animator anim;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        anim = GameObject.Find("Player").GetComponent<Animator>();

        anim.SetInteger("State", 0);
        anim.SetInteger("BasicState", 0);
    }
}
