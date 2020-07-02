using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 6点钟方向是0,12点钟方向是100
/// </summary>
public class Speedometer : MonoBehaviour
{
    public Image fillSpeedometer;
    public Image pointer;
    public Text speedText;
    // Update is called once per frame
    void Update()
    {
           
        if (speedText!=null && pointer!=null && fillSpeedometer!=null)
        {
            CalculateFill(CarController.curSpeed);
        }
    }

    private void CalculateFill(float speed)
    {
        fillSpeedometer.fillAmount = speed/200;
        float pointRot = -1 * speed * 360 / 200;
        pointer.transform.localEulerAngles = new Vector3(0,0, pointRot);
        speedText.text = speed.ToString("F0");
    }
}
