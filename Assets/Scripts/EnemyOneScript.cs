using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOneScript : MonoBehaviour
{
    public int MaxHealth = 3;
    int CurrentHealth;

    Vector2 HitForce = new Vector2(200, 0);

    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
    }

    public void GremlinDamaged()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        gameObject.GetComponent<Rigidbody2D>().AddForce(HitForce);

        CurrentHealth -= 1;

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy has passed away! Truly a tragedy...");
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
