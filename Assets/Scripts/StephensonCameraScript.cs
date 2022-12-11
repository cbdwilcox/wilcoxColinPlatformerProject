using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StephensonCameraScript : MonoBehaviour
{
    public GameObject Player;
    public GameObject Scroll;

    Vector3 CamPos;

    // Update is called once per frame

    private void Start()
    {
        Player = GameObject.Find("Player");
        Scroll = GameObject.Find("scroll");

    }
    void Update()
    {
        CamPos.x = Mathf.Clamp(Player.transform.position.x, 0f, 0f);
        //CamPos.y = Mathf.Clamp(Player.transform.position.y, -26.3f, 26.3f);
        CamPos.y = Vector3.MoveTowards(gameObject.transform.position, Scroll.transform.position, 3 * Time.deltaTime).y;
        CamPos.z = -10f;

        gameObject.transform.position = CamPos;

    }
}
