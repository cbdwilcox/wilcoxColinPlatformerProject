using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandScript : MonoBehaviour
{
    public GameObject Empty;
    GameObject Player;

    bool startMoving = false;

    float distance;

    public float TriggerDistance;

    void Start()
    {
        Player = GameObject.Find("Player");

        startMoving = false;
    }

    void Update()
    {
        distance = Vector3.Distance(Player.transform.position, gameObject.transform.position);
        Debug.Log(distance);

        if (distance <= TriggerDistance)
        {
            startMoving = true;
        }

        if (startMoving)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, Empty.transform.position, 20 * Time.deltaTime);
        }
        
        else
        {
            gameObject.transform.position = gameObject.transform.position;
        }

        if (gameObject.transform.position == Empty.transform.position)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = false;

            Player.GetComponent<PlayerScript>().ClawHit();
        }
    }
}
