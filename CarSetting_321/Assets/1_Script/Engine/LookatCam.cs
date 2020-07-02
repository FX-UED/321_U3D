using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookatCam : MonoBehaviour
{
    private Transform camTarget;
    // Start is called before the first frame update
    void Start()
    {
        camTarget = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (camTarget == null) return;

        transform.LookAt(camTarget);
    }
}
