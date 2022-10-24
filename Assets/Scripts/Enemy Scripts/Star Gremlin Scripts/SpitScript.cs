using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitScript : StateMachineBehaviour
{
    Vector3 SpitSpawn;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SpitSpawn = GameObject.Find("SpitPoint").transform.position;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Instantiate(Resources.Load("Prefabs/StarGremlinSun") as GameObject, SpitSpawn, Quaternion.identity);
    }
}
