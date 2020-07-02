using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum rearviewMirrorRotVector
{
    up,
    down,
    left,
    right
}
public class RearviewMirror : MonoBehaviour
{
    public Transform rearviewMirror;
    public float rotSpeed=20;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void RotateRearviewMirror(Transform rearviewMirror, rearviewMirrorRotVector vector)
    {
        switch(vector)
        {
            case rearviewMirrorRotVector.up:
                rearviewMirror.localEulerAngles = new Vector3(rearviewMirror.localEulerAngles.x + Time.deltaTime* rotSpeed, rearviewMirror.localEulerAngles.y, rearviewMirror.localEulerAngles.z);
                break;
            case rearviewMirrorRotVector.down:
                rearviewMirror.localEulerAngles = new Vector3(rearviewMirror.localEulerAngles.x - Time.deltaTime* rotSpeed, rearviewMirror.localEulerAngles.y, rearviewMirror.localEulerAngles.z);
                break;
            case rearviewMirrorRotVector.left:
                rearviewMirror.localEulerAngles = new Vector3(rearviewMirror.localEulerAngles.x , rearviewMirror.localEulerAngles.y + Time.deltaTime* rotSpeed, rearviewMirror.localEulerAngles.z);
                break;
            case rearviewMirrorRotVector.right:
                rearviewMirror.localEulerAngles = new Vector3(rearviewMirror.localEulerAngles.x , rearviewMirror.localEulerAngles.y - Time.deltaTime* rotSpeed, rearviewMirror.localEulerAngles.z);
                break;
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.UpArrow))
        {
            RotateRearviewMirror(rearviewMirror,rearviewMirrorRotVector.up);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            RotateRearviewMirror(rearviewMirror, rearviewMirrorRotVector.down);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            RotateRearviewMirror(rearviewMirror, rearviewMirrorRotVector.left);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            RotateRearviewMirror(rearviewMirror, rearviewMirrorRotVector.right);
        }
    }
}
