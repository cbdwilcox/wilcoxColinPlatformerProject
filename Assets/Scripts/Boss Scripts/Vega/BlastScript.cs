using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastScript : MonoBehaviour
{
    LayerMask PlayerLayer;
    float BlastRange = 2;
    GameObject Player;

    bool HasHit = false;

    // Start is called before the first frame update
    void Start()
    {
        PlayerLayer = LayerMask.GetMask("Player");
        Player = GameObject.Find("Player");

        Invoke("DestroyBypass", 1);
    }

    void Update()
    {
        if (!HasHit)
        {
            HasHit = true;
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(gameObject.transform.position, BlastRange, PlayerLayer);

            foreach (Collider2D player in hitEnemies)
            {

                Debug.Log("We hit" + player.name);
                Player.GetComponent<PlayerScript>().ClawHit();
            }
        }
    }

    void DestroyBypass()
    {
        Destroy(gameObject);
    }
}
