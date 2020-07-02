using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTest : MonoBehaviour
{
    public float speed = 1;
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(CarController.isFront)
        transform.Rotate(Vector3.right * speed);
        else
            transform.Rotate(Vector3.left * speed);
    }
}
