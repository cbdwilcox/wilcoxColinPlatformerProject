using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LMetScript : MonoBehaviour
{
    Rigidbody2D rb2d;
    void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();

        rb2d.AddForce(new Vector2(-500, 0));

        Invoke("DestroyBypass", 9);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.layer == 6)
        {
            Destroy(gameObject);
            GameObject.Find("Player").GetComponent<PlayerScript>().ClawHit();
        }
    }

    void DestroyBypass()
    {
        Destroy(gameObject);
    }    
}
