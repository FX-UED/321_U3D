using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : DDOLSingleton<CarController>
{
    [Header("是否为测试模式")]
    public bool isTest = false;
    [Header("输入车速后，按下大键盘的数字6")]
    public float carSpeed;
    public MeshRenderer CarBodyMat;//换材质球
    public MeshRenderer CarLightMat;//开关灯
    public GameObject CarFrontLight_volumObj;//开关灯
    public Material DefaultCarPaint { get; set; }//换材质球

    public Animator CarFrontLeftDoor;
    public Animator CarFrontRightDoor;
    public Animator CarBackLeftDoor;
    public Animator CarBackRightDoor;

    public GameObject CarFrontLeftWindow;
    public GameObject CarFrontRightWindow;
    public GameObject CarBackLeftWindow;
    public GameObject CarBackRightWindow;

    public Animator CarTrunkDoor;
    public Animator CarSkylight;
    public List<GameObject> wheelList;//换模型预制体

    //轮子
    public List<Transform> wheelRotList;
    private float oldSpeed=0;//记过上一个速度，用于过度效果
    public static float curSpeed=0;//当前的速度
    public static float carSpeed_finall=0;//最终需要实现的速度

    //public GameObject front_chargingOn;
    //public GameObject front_chargingOff;
    //public GameObject back_chargingOn;
    //public GameObject back_chargingOff;
    public GameObject chargingDoor;
    public MeshRenderer roadLineMat;
    public static bool isFront;

    [Header("近光灯")]
    public MeshRenderer LigthNear;
    [Header("近光灯—亮灯的材质")]
    public Material LigthNearMat_on;
    [Header("近光灯—灭灯的材质")]
    public Material LigthNearMat_off;
    [Header("近光灯—体积光")]
    public GameObject LigthNear_obj;

    [Header("logo灯")]
    public MeshRenderer Light_Logo;
    [Header("logo灯—亮灯的材质")]
    public Material Light_Logo_on;
    [Header("logo灯—灭灯的材质")]
    public Material Light_Logo_off;
    [Header("logo灯—体积光")]
    public GameObject Light_Logo_obj;

    [Header("刹车灯")]
    public MeshRenderer LigthBrake;
    [Header("刹车灯—亮灯的材质")]
    public Material LigthBrakeMat_on;
    [Header("刹车灯—灭灯的材质")]
    public Material LigthBrakeMat_off;
    [Header("刹车灯—体积光")]
    public GameObject LigthBrake_obj;


    [Header("左转向灯")]
    public MeshRenderer LigthTurnLeft;
    [Header("左转向灯—亮灯的材质")]
    public Material LigthTurnLeftMat_on;
    [Header("左转向灯—灭灯的材质")]
    public Material LigthTurnLeftMat_off;
    [Header("左转向灯—体积光")]
    public GameObject LigthTurnLeft_obj;

    [Header("右转向灯")]
    public MeshRenderer LigthTurnRight;
    [Header("右转向灯—亮灯的材质")]
    public Material LigthTurnRightMat_on;
    [Header("右转向灯—灭灯的材质")]
    public Material LigthTurnRightMat_off;
    [Header("右转向灯—体积光")]
    public GameObject LigthTurnRight_obj;

    public GameObject RadaFront;
    public GameObject RadaBack;

    private void Start()
    {
        Init();
        SetEvent();
    }

    private void Init()
    {
        ChangeCarLigth("0"); //默认关灯

        ChangeWheels("20");//默认轮毂
        //默认前后都不充电
        //front_chargingOn.SetActive(false);
        //front_chargingOff.SetActive(true);
        //back_chargingOn.SetActive(false);
        //back_chargingOff.SetActive(true);
        // 车辆初始化结束，可以接收任何指令。
        AppStart.IsGameStart = true;
    }
    /// <summary>
    /// 找到所有的车漆模型，重新付给车漆材质
    /// </summary>
    private void FindAllCarpaint()
    {
        object[] gameObjects;
        gameObjects = GameObject.FindObjectsOfType(typeof(MeshRenderer));
        foreach (MeshRenderer go in gameObjects)
        {
            if (go.material.shader.name== CarBodyMat.material.shader.name)
            {
                go.material = CarBodyMat.material;
            }
        }
    }
    private void SetEvent()
    {
        AppDelegate.ChangeColorEvent += ChangeColor;
        AppDelegate.ChangeCarLigthEvent += ChangeCarLigth;
        AppDelegate.CarAnimationEvent += CarAnimationPlay;
        AppDelegate.ChangeWheelsEvent += ChangeWheels;
        AppDelegate.ChargingEvent += Charging;
    }

    public void ChangeColor(string color)
    {
 
        Material targetMaterial = Resources.Load("Materials/CarPaint/" + color.ToString()) as Material;
        Debug.Log(CarBodyMat.sharedMaterial.shader);

        CarBodyMat.sharedMaterial.shader = targetMaterial.shader;
        CarBodyMat.sharedMaterial.CopyPropertiesFromMaterial(targetMaterial);
    }

    private void ChangeCarLigth(string s)
    {
        if(s== "nearFront")
        {
            bool isOn = LigthNear.material.name == LigthNearMat_on.name;

            LigthNear.material = isOn? LigthNearMat_off: LigthNearMat_on;
            LigthNear_obj.SetActive(!isOn);
        }
        else if(s== "Light_Logo")
        {
            bool isOn = Light_Logo.material.name == Light_Logo_on.name;
            Light_Logo.material = isOn ? Light_Logo_off : Light_Logo_on;
            Light_Logo_obj.SetActive(!isOn);
        }
        else if (s == "brake")
        {
            bool isOn = LigthBrake.material.name == LigthBrakeMat_on.name;

            LigthBrake.material = isOn ? LigthBrakeMat_off : LigthBrakeMat_on;
            LigthBrake_obj.SetActive(!isOn);
        }
        else if (s == "turnLeft")
        {

            bool isOn = LigthTurnLeft.material.name == LigthTurnLeftMat_on.name;

            LigthTurnLeft.material = isOn? LigthTurnLeftMat_off: LigthTurnLeftMat_on;
            LigthTurnLeft_obj.SetActive(!isOn);

            LigthTurnRight.material = LigthTurnRightMat_off;
            LigthTurnRight_obj.SetActive(false);
        }
        else if (s == "turnRight")
        {
            bool isOn = LigthTurnRight.material.name == LigthTurnRightMat_on.name;

            LigthTurnRight.material = isOn ? LigthTurnRightMat_off : LigthTurnRightMat_on;
            LigthTurnRight_obj.SetActive(!isOn);

            LigthTurnLeft.material = LigthTurnLeftMat_off;
            LigthTurnLeft_obj.SetActive(false);
        }
    }
    
    private void CarAnimationState(CarAniEnum aniType)
    {
        Debug.Log(aniType);
        switch(aniType)
        {
            // door
            case CarAniEnum.doorFrontLeft_on:
                { CarFrontLeftDoor?.SetInteger("changeState", 0);
                    CarFrontLeftDoor.Play("Door_front_L_control_ON", 0);
                    break; }
            case CarAniEnum.doorFrontLeft_off: CarFrontLeftDoor?.SetInteger("changeState", 1 ); break;
            case CarAniEnum.doorFrontRight_on: CarFrontRightDoor?.SetInteger("changeState",  0); break;
            case CarAniEnum.doorFrontRight_off: CarFrontRightDoor?.SetInteger("changeState", 1); break;
            case CarAniEnum.doorBackLeft_on:   CarBackLeftDoor?.SetInteger("changeState", 0 ); break;
            case CarAniEnum.doorBackLeft_off:  CarBackLeftDoor?.SetInteger("changeState",  1); break;
            case CarAniEnum.doorBackRight_on:   CarBackRightDoor?.SetInteger("changeState",  0); break;
            case CarAniEnum.doorBackRight_off:  CarBackRightDoor?.SetInteger("changeState", 1); break;

            // mirror
            case CarAniEnum.mirrorLeft_on: CarFrontLeftDoor?.SetInteger("changeState", 2); break;
            case CarAniEnum.mirrorLeft_off: CarFrontLeftDoor?.SetInteger("changeState", 3); break;
            case CarAniEnum.mirrorRight_on: CarFrontRightDoor?.SetInteger("changeState", 2); break;
            case CarAniEnum.mirrorRight_off: CarFrontRightDoor?.SetInteger("changeState", 3); break;

            // window
            case CarAniEnum.windowFrontLeft_on: CarFrontLeftDoor?.SetInteger("changeState", 4); break;
            case CarAniEnum.windowFrontLeft_off: CarFrontLeftDoor?.SetInteger("changeState", 5); break;
            case CarAniEnum.windowFrontRight_on: CarFrontRightDoor?.SetInteger("changeState", 4); break;
            case CarAniEnum.windowFrontRight_off: CarFrontRightDoor?.SetInteger("changeState", 5); break;
            case CarAniEnum.windowBackLeft_on: CarBackLeftDoor?.SetInteger("changeState", 2); break;
            case CarAniEnum.windowBackLeft_off: CarBackLeftDoor?.SetInteger("changeState", 3); break;
            case CarAniEnum.windowBackRight_on: CarBackRightDoor?.SetInteger("changeState", 2); break;
            case CarAniEnum.windowBackRight_off: CarBackRightDoor?.SetInteger("changeState", 3); break;

            // trunk
            case CarAniEnum.doorTrunk_on:  CarTrunkDoor?.SetInteger("changeState",0 ); break;
            case CarAniEnum.doorTrunk_off: CarTrunkDoor?.SetInteger("changeState",1); break;
        }
    }
    private void CarAnimationPlay(CarAniEnum aniType)
    {
        Debug.Log(aniType);
        switch (aniType)
        {
            // door
            case CarAniEnum.doorFrontLeft_on:{ CarFrontLeftDoor.Play("Door_front_L_control_ON", 0);break; }
            case CarAniEnum.doorFrontLeft_off: CarFrontLeftDoor?.Play("Door_front_L_control_OFF", 0); break;
            case CarAniEnum.doorFrontRight_on: CarFrontRightDoor?.Play("Door_front_R_control_ON", 0); break;
            case CarAniEnum.doorFrontRight_off: CarFrontRightDoor?.Play("Door_front_R_control__OFF", 0); break;
            case CarAniEnum.doorBackLeft_on: CarBackLeftDoor?.Play("Door_back_L_control_ON", 0); break;
            case CarAniEnum.doorBackLeft_off: CarBackLeftDoor?.Play("Door_back_L_control_OFF", 0); break;
            case CarAniEnum.doorBackRight_on: CarBackRightDoor?.Play("Door_back_R_control)_ON", 0); break;
            case CarAniEnum.doorBackRight_off: CarBackRightDoor?.Play("Door_back_R_control_OFF", 0); break;

            // mirror
            case CarAniEnum.mirrorLeft_on: CarFrontLeftDoor?.Play("Door_front_L_RearviewMirror_control_ON", 0); break;
            case CarAniEnum.mirrorLeft_off: CarFrontLeftDoor?.Play("Door_front_L_RearviewMirror_control_OFF", 0); break;
            case CarAniEnum.mirrorRight_on: CarFrontRightDoor?.Play("Door_front_R_RearviewMirror_control_ON", 0); break;
            case CarAniEnum.mirrorRight_off: CarFrontRightDoor?.Play("Door_front_R_RearviewMirror_control_OFF", 0); break;

            // window
            case CarAniEnum.windowFrontLeft_on: CarFrontLeftDoor?.Play("Door_front_L_WindowGlass_control_ON", 0); break;
            case CarAniEnum.windowFrontLeft_off: CarFrontLeftDoor?.Play("Door_front_L_WindowGlass_control_OFF", 0); break;
            case CarAniEnum.windowFrontRight_on: CarFrontRightDoor?.Play("Door_front_R_WindowGlass_control_ON", 0); break;
            case CarAniEnum.windowFrontRight_off: CarFrontRightDoor?.Play("Door_front_R_WindowGlass_control_OFF", 0); break;
            case CarAniEnum.windowBackLeft_on: CarBackLeftDoor?.Play("Door_back_L_WindowGlass_control_ON 0", 0); break;
            case CarAniEnum.windowBackLeft_off: CarBackLeftDoor?.Play("Door_back_L_WindowGlass_control_OFF 0", 0); break;
            case CarAniEnum.windowBackRight_on: CarBackRightDoor?.Play("Door_back_R_WindowGlass_control_ON", 0); break;
            case CarAniEnum.windowBackRight_off: CarBackRightDoor?.Play("Door_back_R_WindowGlass_control_OFF", 0); break;

            // trunk
            case CarAniEnum.doorTrunk_on: CarTrunkDoor?.SetInteger("changeState", 0); break;
            case CarAniEnum.doorTrunk_off: CarTrunkDoor?.SetInteger("changeState", 1); break;
        }
    }
    private void ChangeWheels(string wheel)
    {
        foreach (var item in wheelList)
        {
            if( item.name.Contains(wheel))
            {
                item.SetActive(true);
            }
            else
            {
                item.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 更改后备箱角度、高度
    /// </summary>
    /// <param name="height"></param>
    public void TrunkHeight(float height)
    {
        Vector3 rotAngle = Vector3.zero;
        if(height==160)
        {
            rotAngle = new Vector3(-60, CarTrunkDoor.transform.localEulerAngles.y, CarTrunkDoor.transform.localEulerAngles.z);
        }
        else if(height == 170)
        {
            rotAngle = new Vector3(-70, CarTrunkDoor.transform.localEulerAngles.y, CarTrunkDoor.transform.localEulerAngles.z);
        }
        else if (height == 180)
        {
            rotAngle = new Vector3(-80, CarTrunkDoor.transform.localEulerAngles.y, CarTrunkDoor.transform.localEulerAngles.z);
        }
        TweenControl.Instance.RotateTo(CarTrunkDoor.transform, rotAngle, 1, 0, 1, DG.Tweening.LoopType.Incremental, DG.Tweening.Ease.InQuad);
    }    /// <summary>
         /// 天窗分段打开
         /// </summary>
         /// <param name="height"></param>
    public void SkylightOpen(float wide)
    {
        float distance = 0;
        if (wide == 160)
        {
            distance = 10;
        }
        else if (wide == 170)
        {
            distance = 20;
        }
        else if (wide == 180)
        {
            distance = 30;
        }
        TweenControl.Instance.MoveTo(CarSkylight.transform, new Vector3(CarSkylight.transform.localPosition.x, CarSkylight.transform.localPosition.y,distance), 1, 0, 1, DG.Tweening.LoopType.Incremental, DG.Tweening.Ease.InQuad);
    }
    #region 车辆 行驶
    /// <summary>
    /// 车辆逐步变速
    /// </summary>
    public void Run()
    {
        if (carSpeed_finall == 0 && curSpeed==0)
        {
            speedLerpTime = 0;
            return;
        }

        curSpeed = SpeedLerp(carSpeed_finall);

        foreach (var item in wheelRotList)
        {
            if( Mathf.Abs(curSpeed - carSpeed_finall) > 2)
            {
                item.Rotate(Vector3.back* curSpeed * 0.3f);
                roadLineMat.material.SetFloat("_speed", curSpeed * 0.01f);
            }
            else
            {
                speedLerpTime = 0;
                oldSpeed = carSpeed_finall;
                item.Rotate(Vector3.back * carSpeed_finall * 0.3f);
                roadLineMat.material.SetFloat("_speed", carSpeed_finall * 0.01f);
            }
           // Debug.Log(roadLineMat.material.GetFloat("_speed"));
        }
       
    }

    float speedLerpTime = 0;//过度时间
    private float SpeedLerp(float toSpeed)
    {
        speedLerpTime += 0.005f;//0.3 是提速的力量，数值越大，提速越快

        return Mathf.Lerp(oldSpeed, toSpeed, speedLerpTime);
    }

    #endregion

    /// <summary>
    /// 汽车充电
    /// </summary>
    private void Charging(string s)
    {
        //if (s=="front_on")
        //{
        //    front_chargingOn.SetActive(true);
        //    front_chargingOff.SetActive(false);
        //}
        //else if (s == "back_on")
        //{
        //    back_chargingOn.SetActive(true);
        //    back_chargingOff.SetActive(false);
        //}
        //else if (s == "front_off")
        //{
        //    front_chargingOn.SetActive(false);
        //    front_chargingOff.SetActive(true);
        //}
        //else if (s == "back_off")
        //{
        //    back_chargingOn.SetActive(false);
        //    back_chargingOff.SetActive(true);
        //}
    }
    public void SpecialFuncTrigger()
    {

    }
    private void FixedUpdate()
    {
        if (isTest==false) { return; }
        Run();
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            carSpeed_finall = carSpeed;
        }
    }
}
