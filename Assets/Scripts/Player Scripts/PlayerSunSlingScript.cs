using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSunSlingScript : MonoBehaviour
{
    // Start is called before the first frame update

    Vector3 Trajectory;

    LayerMask EnemyLayers;

    Transform SunSlingPoint;

    public float SunSlingRange = 1.2f;

    Camera MainCam;

    // ==========

    public GameObject SunSlingPointB;

    void Start()
    {
        //SunSlingPointB = GameObject.Find("SunSlingPoint");

        // =======

        SunSlingRange = 1.2f;

        EnemyLayers = LayerMask.GetMask("Enemies");

        MainCam = GameObject.Find("Main Camera").GetComponent<Camera>();

        Trajectory = MainCam.ScreenToWorldPoint(Input.mousePosition);

        Invoke("DestroyBypass", 7);
    }

    private void OnDrawGizmosSelected()
    {
        if (SunSlingPoint == null)
            return;

        Gizmos.DrawWireSphere(SunSlingPoint.position, SunSlingRange);
    }

   // this code is a mess
   // but ill fix it later
    
    void Update()
    {
        Vector3 sunPos = gameObject.transform.position;

        //Vector3 difference = Trajectory - SunSlingPointB.transform.position;
        //float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        //SunSlingPointB.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);

        //SunSlingPoint = GameObject.Find("SunSlingPoint").transform;

        SunSlingPoint = gameObject.transform;

        gameObject.transform.position = Vector3.MoveTowards(sunPos, Trajectory, 15 * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            Destroy(gameObject);
        }

        else if (collision.gameObject.layer == 7)
        {
            Debug.Log("SUN TEST WORKING");

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(SunSlingPoint.position, SunSlingRange, EnemyLayers);

            foreach (Collider2D enemy in hitEnemies)
            {
                Debug.Log("We burnt " + enemy.name);

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
                    enemy.GetComponent<NovaGremlinScript>().HeavyHit();
                }

                // ----- Bosses -----

                if (enemy.tag == "Vega")
                {
                    enemy.GetComponent<VegaScript>().HeavyHit();
                    Debug.Log("Hit Complete");
                }

                if (enemy.tag == "Arcturus")
                {
                    enemy.GetComponent<ArcturusScript>().HeavyHit();
                    Debug.Log("Hit Complete");
                }

                Physics2D.IgnoreCollision(gameObject.GetComponent<CircleCollider2D>(), enemy.GetComponent<CapsuleCollider2D>());
            }

            //Invoke("ResetCooldown", 1);
        }
    }

    void DestroyBypass()
    {
        Destroy(gameObject);
    }    
}
