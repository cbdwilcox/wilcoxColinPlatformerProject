using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarSlideScript : MonoBehaviour
{
    bool isDashing = false;

    bool facingRight;

    Rigidbody2D rb2d;

    GameObject dashLeftParticle;

    GameObject dashRightParticle;

    GameObject WarpPoint;

    // Start is called before the first frame update
    void Start()
    {
        facingRight = gameObject.GetComponent<PlayerScript>().FacingRight;
        isDashing = gameObject.GetComponent<PlayerScript>().IsDashing;

        WarpPoint = GameObject.Find("VegaWarpPoint");

        rb2d = gameObject.GetComponent<Rigidbody2D>();

        dashLeftParticle = Resources.Load("Prefabs/DashRight") as GameObject;
        dashRightParticle = Resources.Load("Prefabs/DashLeft") as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        facingRight = gameObject.GetComponent<PlayerScript>().FacingRight;
        isDashing = gameObject.GetComponent<PlayerScript>().IsDashing;

        // ----- Solar Slide Ability -----
        if (Input.GetMouseButtonDown(1) && !isDashing)
        {
            gameObject.GetComponent<PlayerScript>().CollisionInvulnBypass();

            Vector2 slideLeft = new Vector2(-1500, 0);
            Vector2 slideRight = new Vector2(1500, 0);

            if (facingRight)
            {
                rb2d.AddForce(slideRight);

                Instantiate(dashRightParticle, WarpPoint.transform.position, Quaternion.identity);
            }

            if (!facingRight)
            {
                rb2d.AddForce(slideLeft);

                Instantiate(dashLeftParticle, WarpPoint.transform.position, Quaternion.identity);
            }

            isDashing = true;
        }
    }
}
