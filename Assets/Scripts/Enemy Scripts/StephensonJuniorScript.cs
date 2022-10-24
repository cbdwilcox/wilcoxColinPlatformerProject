using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StephensonJuniorScript : MonoBehaviour
{
    public TMP_Text WinText;

    void Start()
    {
        WinText.enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            Destroy(gameObject);

            WinText.enabled = true;
        }
    }
}
