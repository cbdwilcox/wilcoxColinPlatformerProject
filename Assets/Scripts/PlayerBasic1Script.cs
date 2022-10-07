using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasic1Script : StateMachineBehaviour
{

    private enum State { Basic2 = 2 }
    private State state;

    public float Timer;
    private bool Combo = false;

    Animator anim;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Timer = 3;
        anim = GameObject.Find("Player").GetComponent<Animator>();
        Combo = false;
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
