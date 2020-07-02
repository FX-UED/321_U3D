using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum DriveModelVector
{
    x,
    y
}
public class DriveModeControl : MonoBehaviour
{
    [Header("选择UV动画的轴")]
    public DriveModelVector driveModelVector;
    [Header("节能的颜色")]
    public Color colorJieNeng;
    [Header("舒适的颜色")]
    public Color colorShuShi;
    [Header("运动的颜色")]
    public Color colorYunDong;
    [Header("个性化的颜色")]
    public Color colorGeXingHua;


    public static float speed;
    private float SpeedX = 0;
    private float SpeedY = 0;
    private float deltX;
    private float deltY;
    private MeshRenderer m_material;

    // Start is called before the first frame update
    void Awake()
    {
        if(driveModelVector== DriveModelVector.x)
        {
            SpeedX = 1;
            SpeedY = 0;
        }
        else
        {
            SpeedX = 0;
            SpeedY = 1;
        }
        if(GetComponent<MeshRenderer>()!=null)
        {
            m_material = GetComponent<MeshRenderer>();
            m_material.material.SetVector("_Vector",new Vector4(SpeedX, SpeedY,0,0));
        }
    }
    private void OnEnable()
    {
        AppDelegate.DriveModeChangeEvent += SetColor;
    }
    private void OnDisable()
    {
        AppDelegate.DriveModeChangeEvent -= SetColor;

    }
    public void SetColor(string s)
    {
        Debug.Log(s);
        if(s== "JieNeng")
        {
            m_material.material.SetColor("_Color", colorJieNeng);
        }
        else if(s== "ShuShi")
        {
            m_material.material.SetColor("_Color", colorShuShi);
        }
        else if (s == "YunDong")
        {
            m_material.material.SetColor("_Color", colorYunDong);
        }
        else if (s == "GeXingHua")
        {
            m_material.material.SetColor("_Color", colorGeXingHua);
        }
    }
    void Update()
    {
        if (m_material)
        {
            deltX += SpeedX * Time.deltaTime* speed;
            deltY += SpeedY * Time.deltaTime* speed;
            //_material.material.SetTextureOffset("_MainTex", new Vector2(deltX, deltY));
            m_material.material.SetFloat("_Speed", speed);
        }
    }
}
