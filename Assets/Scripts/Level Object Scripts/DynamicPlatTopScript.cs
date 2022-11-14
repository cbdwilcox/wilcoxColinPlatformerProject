using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicPlatTopScript : MonoBehaviour
{
    public GameObject ParentPlat;
    public GameObject Player;
    bool movingBool = false;

    Vector2 moveVector;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveVector = new Vector2(ParentPlat.transform.position.x, Player.transform.position.y);

        if (movingBool)
        {
            Player.transform.position = moveVector;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            collision.collider.transform.SetParent(ParentPlat.transform);
            movingBool = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            movingBool = false;
        }
    }
}
