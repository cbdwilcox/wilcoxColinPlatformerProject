using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasic3Script : StateMachineBehaviour
{
    // ----- Components -----
    Animator anim;
    public Transform AttackPoint;

    public Vector3 ManaPoint;

    // ----- Floats & Integers -----
    public float HitTime;

    public float AttackRange = 2.0f;

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
        ManaPoint = AttackPoint.position;
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

                // ----- Enemies -----

                if (enemy.tag == "StarGremlin")
                {
                    enemy.GetComponent<StarGremlinScript>().HeavyHit();
                }

                if (enemy.tag == "Meteormite")
                {
                    enemy.GetComponent<MeteormiteScript>().HeavyHit();
                }

                if (enemy.tag == "ArcturusSoldier")
                {
                    enemy.GetComponent<SoldierScript>().HeavyHit();
                }

                if (enemy.tag == "NovaGremlin")
                {
                    enemy.GetComponent<NovaGremlinScript>().LightHit();
                }

                // ----- Bosses -----

                if (enemy.tag == "Vega")
                {
                    enemy.GetComponent<VegaScript>().HeavyHit();
                    Debug.Log("Hit Complete");
                }

                if (enemy.tag == "Arcturus")
                {
                    enemy.GetComponent<ArcturusScript>().LightHit();
                    Debug.Log("Hit Complete");
                }

                HasHit = true;

                Instantiate(Resources.Load("Prefabs/ManaMote") as GameObject, ManaPoint, Quaternion.identity);
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        anim.SetBool("GroundPound", false);
    }
}
