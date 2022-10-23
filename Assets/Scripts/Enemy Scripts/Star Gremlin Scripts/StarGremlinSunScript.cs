using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarGremlinSunScript : MonoBehaviour
{

    GameObject Player;

    LayerMask PlayerLayer;

    void Start()
    {
        Player = GameObject.Find("Player");

        PlayerLayer = 6;
    }

    void Update()
    {
        Vector3 playerPos = Player.transform.position;
        Vector3 sunPos = gameObject.transform.position;

        float speed = 10 * Time.deltaTime;

        gameObject.transform.position = Vector3.MoveTowards(sunPos, playerPos, speed);

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
