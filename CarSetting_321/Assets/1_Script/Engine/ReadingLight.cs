using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadingLight : MonoBehaviour
{
    public GameObject lightLeft;
    public GameObject lightRight;
    public MeshRenderer Seat;

    public Material mat_leftLight_on;
    public Material mat_rightLight_on;
    public Material mat_DoubleLight_on;
   // public Material mat_leftLight_off;
   // public Material mat_rightLight_off;
    public Material mat_DoubleLight_off;

    private bool isLeftOn = false;
    private bool isRightOn = false;
    // Start is called before the first frame update
    void Start()
    {

        ReadingLightControl("left0");
        ReadingLightControl("right0");
    }
    private void OnEnable()
    {
        AppDelegate.ReadingLightEvent += ReadingLightControl;
    }
    private void OnDisable()
    {
        
        AppDelegate.ReadingLightEvent -= ReadingLightControl;
    }

    private void ReadingLightControl(string s)
    {
      
        if (s=="left0")// 关闭 左
        {
          
            lightLeft.SetActive(false);
            if (isRightOn==false)
            {
                Seat.material = mat_DoubleLight_off;
            }
            else
            {
                Seat.material = mat_rightLight_on;
            }
                isLeftOn = false;
        }
        else if(s== "left1")// 打开 左
        {
     
            lightLeft.SetActive(true);

            if (isRightOn == true)
            {
                Seat.material = mat_DoubleLight_on;
            }
            else
            {
                Seat.material = mat_leftLight_on;
            }

            isLeftOn = true;
        }
        else if (s == "right0")// 关闭 右
        {

            lightRight.SetActive(false);
            if (isLeftOn == false)
            {
                Seat.material = mat_DoubleLight_off;
            }
            else
            {
                Seat.material = mat_leftLight_on;
            }

            isRightOn = false;
        }
        else if (s == "right1")// 打开 右
        {
   
            lightRight.SetActive(true);
            if (isLeftOn == true)
            {
                Seat.material = mat_DoubleLight_on;
            }
            else
            {
                Seat.material = mat_rightLight_on;
            }
            isRightOn = true;
        }

      

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
