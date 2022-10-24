using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaMoteScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int manaSplodeX = Random.Range(100, 250);
        int manaSplodeY = Random.Range(100, 250);

        Vector2 manaSplode;
        manaSplode.x = manaSplodeX;
        manaSplode.y = manaSplodeY;

        gameObject.GetComponent<Rigidbody2D>().AddForce(manaSplode);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            Destroy(gameObject);
        }
    }
}
