using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlareJumpScript : MonoBehaviour
{
    bool isGrounded = false;
    Vector2 jumpForce = new Vector2(0, 2400);

    Rigidbody2D rb2d;

    public GameObject VoidWormA;
    public GameObject VoidWormB;

    GameObject flareJumpParticle;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();

        flareJumpParticle = Resources.Load("Prefabs/FlareJump") as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceA = Vector3.Distance(VoidWormA.transform.position, gameObject.transform.position);

        float distanceB = Vector3.Distance(VoidWormB.transform.position, gameObject.transform.position);

        if (distanceA <= 21.5f)
        {
            rb2d.gravityScale = 0;

            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, VoidWormA.transform.position, 5 * Time.deltaTime);
        }

        else
        {
            rb2d.gravityScale = 5;
        }

        if (distanceB <= 21.5f)
        {
            rb2d.gravityScale = 0;

            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, VoidWormB.transform.position, 5 * Time.deltaTime);
        }

        else
        {
            rb2d.gravityScale = 5;
        }

        if (Input.GetKeyUp(KeyCode.X) && isGrounded)
        {
            rb2d.velocity = Vector2.zero;
            rb2d.AddForce(jumpForce);

            Instantiate(flareJumpParticle, gameObject.transform.position, Quaternion.identity);
        }

        // ----- Ground Check -----

        if (rb2d.velocity.y == 0)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "VoidWormA")
        {
            gameObject.GetComponent<PlayerScript>().PlayerDeath();
        }

        if (collision.gameObject.tag == "VoidWormB")
        {
            gameObject.GetComponent<PlayerScript>().PlayerDeath();
        }
    }
}
