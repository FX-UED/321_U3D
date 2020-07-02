using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelRotateControl : MonoBehaviour
{
    public Transform carBody;

    private float angleY;
    private float old_angleY=0;

    private float old_carTransZ=0;
    private float old_carRotY=0;
    // Start is called before the first frame update
    void Start()
    {
        if(carBody==null)
        {
            Debug.LogError("car body is null");
            enabled = false;
            return;
        }
        old_angleY = ClambAngle(carBody.localEulerAngles.y * -1 + 180); 
    }
    public void CarVector()
    {
        float cur_carTransZ = carBody.localPosition.z;
        float cur_carRotY = carBody.localEulerAngles.y;
        if(cur_carTransZ>old_carTransZ && cur_carRotY> old_carRotY)//向前走,向右转
        {
            angleY = ClambAngle( carBody.localEulerAngles.y * -1 + 180);
            CarController.isFront = true;
        }
        else if(cur_carTransZ > old_carTransZ && cur_carRotY < old_carRotY)//向前走,向左转
        {
            angleY = ClambAngle(carBody.localEulerAngles.y * 1 + 180);
            CarController.isFront = true;
        }
        else if (cur_carTransZ < old_carTransZ && cur_carRotY > old_carRotY)//向后走,向右转
        {
            angleY = ClambAngle(carBody.localEulerAngles.y * 1 + 180);
            CarController.isFront = false;
        }
        else if (cur_carTransZ < old_carTransZ && cur_carRotY < old_carRotY)//向后走,向左转
        {
            angleY = ClambAngle(carBody.localEulerAngles.y * -1 + 180);
            CarController.isFront = false;
        }
        old_carTransZ = cur_carTransZ;
        old_carRotY = cur_carRotY;
       // old_angleY = angleY;
    }
    private float cur_AngleY;

    // Update is called once per frame  Mathf.Clamp(angleY,150,210)
    void Update()
    {
        CarVector();

       // transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, angleY, transform.localEulerAngles.z);
        RotateMethod();
    }


    public void RotateMethod()
    {
        cur_AngleY = SpeedLerp(angleY);

            if (Mathf.Abs(cur_AngleY - angleY) > 2)
            {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, cur_AngleY, transform.localEulerAngles.z);
            }
            else
            {
                speedLerpTime = 0;
                old_angleY = angleY;
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, angleY, transform.localEulerAngles.z);

            }

    }

    float speedLerpTime = 0;//过度时间
    private float SpeedLerp(float toSpeed)
    {
        speedLerpTime += Time.deltaTime;//0.3 是提速的力量，数值越大，提速越快

        return Mathf.Lerp(old_angleY, toSpeed, speedLerpTime);
    }

    private float ClambAngle(float angle)
    {
        if(angle<180)
        {
            return angle + 360;
        }
        else
        {
            return angle;
        }
    }
}
