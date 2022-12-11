using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashFireScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyBypass", 0.2f);
    }

    void DestroyBypass()
    {
        Destroy(gameObject);
    }
}
