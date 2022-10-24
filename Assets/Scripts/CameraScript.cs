using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public GameObject Player;

    Vector3 CamPos;

    // Update is called once per frame
    void Update()
    {
        CamPos.x = Mathf.Clamp(Player.transform.position.x, -38f, 38f);
        CamPos.y = Mathf.Clamp(Player.transform.position.y, -21.4f, 21.4f);
        CamPos.z = -10f;

        gameObject.transform.position = CamPos;

    }
}
