using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlareScript : MonoBehaviour
{
    bool hitThing = false;
    GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        hitThing = false;
        Player = GameObject.Find("Player");

        Invoke("DestroyBypass", 0.25f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6 && !hitThing)
        {
            hitThing = true;

            Player.GetComponent<PlayerScript>().ClawHit();
        }    
    }

    void DestroyBypass()
    {
        Destroy(gameObject);
    }
}
