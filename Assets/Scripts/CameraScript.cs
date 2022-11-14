using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public GameObject Player;

    Vector3 CamPos;

    // Update is called once per frame

    private void Start()
    {
        Player = GameObject.Find("Player");
    }
    void Update()
    {
        CamPos.x = Mathf.Clamp(Player.transform.position.x, -29f, 29f);
        CamPos.y = Mathf.Clamp(Player.transform.position.y, -16.4f, 16.4f);
        CamPos.z = -10f;

        gameObject.transform.position = CamPos;

    }
}
