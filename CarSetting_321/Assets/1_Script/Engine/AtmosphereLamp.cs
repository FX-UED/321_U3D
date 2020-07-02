using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtmosphereLamp : MonoBehaviour
{
    private MeshRenderer atmosphereBoardMeshrender;

    public static bool isTwinkle=false;
    public static float twinkleSpeed =1;
    public static float lightBrightness=1;
    public static float colorType=0;
    private Color curColor=Color.white;
    private Color lastColor;
    private float colorLerpOffset=0;
    private int colorLerpVector = 1;
    private void OnEnable()
    {
        AppDelegate.AtmosphereColorEvent += ChangeSingleColor;
    }
    private void OnDisable()
    {
        AppDelegate.AtmosphereColorEvent -= ChangeSingleColor;
    }
    // Start is called before the first frame update
    void Start()
    {
       
        atmosphereBoardMeshrender = GetComponent<MeshRenderer>();
    }
    /// <summary>
    /// 变色
    /// </summary>
    /// <param name="Hex"></param>
    public void ChangeSingleColor(string Hex)
    {
       
        curColor = HexToColor(Hex);
        lastColor = curColor;

    }
    // Update is called once per frame
    void Update()
    {
        if(isTwinkle)//动态
        {
            if(colorType==0)// 单色
            {
                colorLerpOffset += Time.deltaTime * twinkleSpeed * colorLerpVector;

               
                if (colorLerpOffset >= 1) { colorLerpVector = -1; }
                else if (colorLerpOffset <= 0) { colorLerpVector = 1; }
                curColor = Color.Lerp(lastColor, Color.black, colorLerpOffset);
            }
            else if(colorType==1)// 变色
            {
                colorLerpOffset += Time.deltaTime * twinkleSpeed * colorLerpVector;

              
                if (colorLerpOffset >= 1) { lastColor = RandomColorRange(); colorLerpVector = -1; }
                else if (colorLerpOffset <= 0) {  colorLerpVector = 1;  }
                atmosphereBoardMeshrender.material.SetFloat("_LowBrightness", 0.1f);
                curColor = Color.Lerp(lastColor, Color.black, colorLerpOffset);
            }
            else if(colorType==2)// 全色
            {
                colorLerpOffset += Time.deltaTime * twinkleSpeed * colorLerpVector;

              
                if (colorLerpOffset >= 1) { lastColor = RandomColorFull();
                    Debug.Log(lastColor+ ",lightBrightness = " + lightBrightness);
                    colorLerpVector = -1; }
                else if (colorLerpOffset <= 0) { colorLerpVector = 1;  }
                atmosphereBoardMeshrender.material.SetFloat("_LowBrightness", 0.1f);
                curColor = Color.Lerp(lastColor, Color.black, colorLerpOffset);
            }

        }
        else //静态
        {
            atmosphereBoardMeshrender.material.SetFloat("_LowBrightness", 1f);
            curColor = lastColor;
        }
        atmosphereBoardMeshrender.material.SetColor("_ColorLamp", curColor);
        atmosphereBoardMeshrender.material.SetFloat("_Brightness", lightBrightness);

    }

    public string ColorToHex(Color color)
    {
        int r = Mathf.RoundToInt(color.r * 255.0f);
        int g = Mathf.RoundToInt(color.g * 255.0f);
        int b = Mathf.RoundToInt(color.b * 255.0f);
        int a = Mathf.RoundToInt(color.a * 255.0f);
        string hex = string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", r, g, b, a);
        return hex;
    }
    /// <summary>
    /// hex转换到color
    /// </summary>
    /// <param name="hex"></param>
    /// <returns></returns>
    public Color HexToColor(string hex)
    {

        byte br = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte bg = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte bb = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        //byte cc = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
        float r = br / 255f;
        float g = bg / 255f;
        float b = bb / 255f;
        float a = 255 / 255f;
        return new Color(r, g, b, a);
    }
    public Color RandomColorFull()
    {
        byte r = byte.Parse(Random.Range(10,200).ToString());
        byte g = byte.Parse(Random.Range(10, 200).ToString());
        byte b = byte.Parse(Random.Range(10, 200).ToString());
        byte a = 255;
        return new Color(r, g, b, a);
    }
    public Color RandomColorRange()
    {
    
        byte r = byte.Parse(ClampMathf(Random.Range(lastColor.r-50,lastColor.r + 50)));
        byte g = byte.Parse(ClampMathf(Random.Range(lastColor.g - 50, lastColor.g + 50)));
        byte b = byte.Parse(ClampMathf(Random.Range(lastColor.b - 50, lastColor.b + 50)));
        byte a = 255;

        return new Color(r, g, b, a);
    }
    private string ClampMathf(float f)
    {
        return Mathf.Round( Mathf.Clamp(f, 10, 150)).ToString();
    }
}
