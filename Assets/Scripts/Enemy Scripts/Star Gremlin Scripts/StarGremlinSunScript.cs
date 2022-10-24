using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarGremlinSunScript : MonoBehaviour
{

    GameObject Player;

    LayerMask PlayerLayer;

    float Timer;

    Vector3 PlayerPos;

    void Start()
    {
        Player = GameObject.Find("Player");

        PlayerLayer = 6;

        Timer = 2.5f;

        PlayerPos = Player.transform.position;
    }

    void Update()
    {
        //Vector3 playerPos = Player.transform.position;
        Vector3 sunPos = gameObject.transform.position;

        float speed = 10 * Time.deltaTime;

        gameObject.transform.position = Vector3.MoveTowards(sunPos, PlayerPos, speed);

        Timer -= Time.deltaTime;
        if (Timer <= 0)
        {
            Destroy(gameObject);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            Destroy(gameObject);
            Player.GetComponent<PlayerScript>().ClawHit();
        }

        if (collision.gameObject.layer == 8)
        {
            Destroy(gameObject);
        }
    }
}
