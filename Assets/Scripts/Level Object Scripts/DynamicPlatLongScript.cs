using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicPlatLongScript : MonoBehaviour
{
    // Start is called before the first frame update

    Rigidbody2D rb2d;

    float ForceFloat = 9000;

    void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();

        ForceAdd();
    }

    void ForceAdd()
    {
        rb2d.AddForce(new Vector2(ForceFloat, 0));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            ForceFloat *= -1;
            rb2d.velocity = Vector2.zero;
            ForceAdd();
        }
    }
}
