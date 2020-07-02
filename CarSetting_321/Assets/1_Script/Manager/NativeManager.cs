using UnityEngine;
using LitJson;
public class NativeManager : DDOLSingleton<NativeManager>
{
    public void SwitchScene(string xmlName)
    {
        DataManager.Instance.LoadSceneXML(xmlName);//测试
    }
    /// <summary>
    /// js向unity发送消息
    /// </summary>
    /// <param name="pstr">消息内容:json</param>
    public void VehicleControl(string pstr)
    {
        if (!AppStart.IsGameStart) { return; }

        Debug.Log(pstr);
        JsMessage jm = JsonMapper.ToObject<JsMessage>(pstr);
        if (jm == null) { return; }

        if (!string.IsNullOrEmpty(jm.FLDoorStatus))//左前门
        {

            if (jm.FLDoorStatus == "1")
                AppDelegate.Instance.OnCarAnimation(CarAniEnum.doorFrontLeft_on);
            else
                AppDelegate.Instance.OnCarAnimation(CarAniEnum.doorFrontLeft_off);
        }
        if (!string.IsNullOrEmpty(jm.FRDoorStatus))//右前门
        {

            if (jm.FRDoorStatus == "1")
                AppDelegate.Instance.OnCarAnimation(CarAniEnum.doorFrontRight_on);
            else
                AppDelegate.Instance.OnCarAnimation(CarAniEnum.doorFrontRight_off);
        }
        if (!string.IsNullOrEmpty(jm.RLDoorStatus))//左后门
        {

            if (jm.RLDoorStatus == "1")
                AppDelegate.Instance.OnCarAnimation(CarAniEnum.doorBackLeft_on);
            else
                AppDelegate.Instance.OnCarAnimation(CarAniEnum.doorBackLeft_off);
        }
        if (!string.IsNullOrEmpty(jm.RRDoorStatus))//右后门
        {

            if (jm.RRDoorStatus == "1")
                AppDelegate.Instance.OnCarAnimation(CarAniEnum.doorBackRight_on);
            else
                AppDelegate.Instance.OnCarAnimation(CarAniEnum.doorBackRight_off);
        }
        if (!string.IsNullOrEmpty(jm.doorTrunk))//右后门
        {
            if (jm.doorTrunk == "1")
                AppDelegate.Instance.OnCarAnimation(CarAniEnum.doorTrunk_on);
            else
                AppDelegate.Instance.OnCarAnimation(CarAniEnum.doorTrunk_off);
        }
        if (!string.IsNullOrEmpty(jm.skylight))//右后门
        {
            if (jm.skylight == "1")
                AppDelegate.Instance.OnCarAnimation(CarAniEnum.skylight_on);
            else
                AppDelegate.Instance.OnCarAnimation(CarAniEnum.skylight_off);
        }

        if (!string.IsNullOrEmpty(jm.windowFrontLeft))//左前门-窗
        {

            if (jm.windowFrontLeft == "1")
                AppDelegate.Instance.OnCarAnimation(CarAniEnum.windowFrontLeft_on);
            else
                AppDelegate.Instance.OnCarAnimation(CarAniEnum.windowFrontLeft_off);
        }
        if (!string.IsNullOrEmpty(jm.windowFrontRight))//右前门-窗
        {

            if (jm.windowFrontRight == "1")
                AppDelegate.Instance.OnCarAnimation(CarAniEnum.windowFrontRight_on);
            else
                AppDelegate.Instance.OnCarAnimation(CarAniEnum.windowFrontRight_off);
        }
        if (!string.IsNullOrEmpty(jm.windowBackLeft))//左后门-窗
        {

            if (jm.windowBackLeft == "1")
                AppDelegate.Instance.OnCarAnimation(CarAniEnum.windowBackLeft_on);
            else
                AppDelegate.Instance.OnCarAnimation(CarAniEnum.windowBackLeft_off);
        }
        if (!string.IsNullOrEmpty(jm.windowBackRight))//右后门-窗
        {

            if (jm.windowBackRight == "1")
                AppDelegate.Instance.OnCarAnimation(CarAniEnum.windowBackRight_on);
            else
                AppDelegate.Instance.OnCarAnimation(CarAniEnum.windowBackRight_off);
        }

        if (!string.IsNullOrEmpty(jm.LMirror))//左边后视镜
        {

            if (jm.LMirror == "1")
                AppDelegate.Instance.OnCarAnimation(CarAniEnum.mirrorLeft_on);
            else
                AppDelegate.Instance.OnCarAnimation(CarAniEnum.mirrorLeft_off);
        }
        if (!string.IsNullOrEmpty(jm.RMirror))//左边后视镜
        {

            if (jm.RMirror == "1")
                AppDelegate.Instance.OnCarAnimation(CarAniEnum.mirrorRight_on);
            else
                AppDelegate.Instance.OnCarAnimation(CarAniEnum.mirrorRight_off);
        }
        if (!string.IsNullOrEmpty(jm.runSpeed))//车速
        {
            CarController.carSpeed_finall = float.Parse(jm.runSpeed);
        }

        if (!string.IsNullOrEmpty(jm.wheel))// 轮毂 20/21
        {
            AppDelegate.Instance.OnChangeWheels(jm.wheel);
        }
        if (!string.IsNullOrEmpty(jm.cameraVector))// 相机位置 0,1,2
        {
            AppDelegate.Instance.OnchangeCameraVecotr(jm.cameraVector);
        }
        if (!string.IsNullOrEmpty(jm.carPaint))// 车漆 0,1,2
        {
            AppDelegate.Instance.OnChangeColor(jm.carPaint);
        }
        if (!string.IsNullOrEmpty(jm.background))// 环境 0,1,2
        {
            AppDelegate.Instance.OnChangeBackground(jm.background);
        }
        if (!string.IsNullOrEmpty(jm.carLight))// 车灯
        {
            AppDelegate.Instance.OnChangeCarLigth(jm.carLight);
        }
        if (!string.IsNullOrEmpty(jm.warning))// 警告
        {
            AppDelegate.Instance.OnWarning(jm.warning);
        }
        if (!string.IsNullOrEmpty(jm.charge))// 充电显示
        {
            AppDelegate.Instance.OnCharging(jm.charge);
        }
        if (!string.IsNullOrEmpty(jm.restart))// 重启程序
        {
            AppDelegate.Instance.OnRestart(jm.restart);
        }
        if (!string.IsNullOrEmpty(jm.releaseMemory))// 释放内存
        {
            AppDelegate.Instance.OnReleaseMemory(jm.releaseMemory);
        }

        if (!string.IsNullOrEmpty(jm.cameraPathAnimation))// 相机路径
        {
            AppDelegate.Instance.OnCameraPathAni(jm.cameraPathAnimation);
        }
        if (!string.IsNullOrEmpty(jm.driveMode))// 驾驶模式
        {
            AppDelegate.Instance.OnDriveModeChangeEvent(jm.driveMode);
        }
        if (!string.IsNullOrEmpty(jm.driveModeCustomSpeedSetting))// 速度设置
        {
            DriveModeControl.speed = float.Parse(jm.driveModeCustomSpeedSetting);
            CarController.carSpeed_finall= float.Parse(jm.driveModeCustomSpeedSetting)*100;
        }
        if (!string.IsNullOrEmpty(jm.driveModeCustomEnergyRecovery))// 能量回收
        {
            DriveModeControl.speed = float.Parse(jm.driveModeCustomEnergyRecovery) * -1 ;
            CarController.carSpeed_finall =(1- float.Parse(jm.driveModeCustomEnergyRecovery))*100;
        }

        if (!string.IsNullOrEmpty(jm.atmosphereLightMode))// 氛围灯 模式
        {
            AtmosphereLamp.isTwinkle = jm.atmosphereLightMode != "1";
        }
        if (!string.IsNullOrEmpty(jm.atmosphereLightColor))// 氛围灯 颜色
        {
            AppDelegate.Instance.OnAtmosphereColor(jm.atmosphereLightColor);
        }
        if (!string.IsNullOrEmpty(jm.atmosphereLightBrightness))// 氛围灯 亮度
        {
            AtmosphereLamp.lightBrightness = float.Parse(jm.atmosphereLightBrightness);
        }
        if (!string.IsNullOrEmpty(jm.atmosphereLightSpeed))// 氛围灯 速度
        {
            AtmosphereLamp.twinkleSpeed = float.Parse(jm.atmosphereLightSpeed);
        }
        if (!string.IsNullOrEmpty(jm.atmosphereLightColorType))// 氛围灯 类型； 单色、变色、全色
        {
            AtmosphereLamp.colorType = int.Parse(jm.atmosphereLightColorType);
        }

        if (!string.IsNullOrEmpty(jm.leftReadLight))// 左侧阅读灯
        {
            if (jm.leftReadLight == "0")
            { AppDelegate.Instance.OnReadingLight("left0"); }
            else { AppDelegate.Instance.OnReadingLight("left1"); }
        }
        if (!string.IsNullOrEmpty(jm.rightReadLight))// 右侧阅读灯
        {
            if (jm.rightReadLight == "0")
            { AppDelegate.Instance.OnReadingLight("right0"); }
            else { AppDelegate.Instance.OnReadingLight("right1"); }
        }
    }

    Camera mainCam;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="data">0/1</param>
    public void ControlCamera(string data)
    {
        if (mainCam == null)
        { mainCam = Camera.main; }

        if (data == "0")
        {
            mainCam.gameObject.SetActive(false);
        }
        else if(data=="1")
        {
            mainCam.gameObject.SetActive(true);
        }
    }
}
/// <summary>
/// 字段在json里是key，value是传入的数值
/// </summary>
public class JsMessage
{
    public string leftReadLight;// open 1, close 0
    public string rightReadLight;// open 1, close 0

    public string atmosphereLightMode;// 0 智能。 1 静态， 2 动态
    public string atmosphereLightColor;// 传递颜色的hex值
    public string atmosphereLightBrightness;// 0-2
    public string atmosphereLightSpeed;//0-2
    public string atmosphereLightColorType;// 单色0  变色1  全色2

    public string windowFrontLeft;// open 1, close 0
    public string windowFrontRight;// open 1, close 0
    public string windowBackLeft;// open 1, close 0
    public string windowBackRight;// open 1, close 0
    public string doorTrunk;// open 1, close 0
    public string skylight;//天窗
    public string carLight;//车灯

    public string runSpeed;//直接传递速度数值
    public string carPaint;//车漆
    public string wheel;// 轮毂切换：轮毂 20/21
    public string cameraVector;//相机位置
    public string background;//背景环境
    public string warning;//警告
    public string charge;//充电

    public string restart;//重启
    public string releaseMemory;//释放内存
    public string cameraPathAnimation;//相机路径动画
    public string FLDoorStatus;// open 1, close 0
    public string FRDoorStatus;// open 1, close 0
    public string RLDoorStatus;// open 1, close 0
    public string RRDoorStatus;// open 1, close 0
    public string LMirror;// open 1, close 0
    public string RMirror;// open 1, close 0
    public string driveMode;//JieNeng/ShuShi/YunDong/GeXingHua
    public string driveModeCustomSpeedSetting;// 0-1
    public string driveModeCustomEnergyRecovery;//0-1
    //----------------------------------------------

    public string cameraLens                 ;
    public string foldState                  ;
    public string lowBeamLight               ;
    public string lowBeamLightHeight         ;
    public string highBeamLight              ;
    public string positionLight              ;
    public string fogLight                   ;
    public string logoLight                  ;
    public string carpetLight                ;

    public string frontDecorateLight         ;

    public string driverSeatTemperature      ;
    public string codriverSeatTemperature    ;
    public string autoLockOn                 ;
    public string autoCloseAfterLock         ;
    public string tailgatePos                ;
    public string leftFrontWindowPos         ;
    public string leftBackWindowPos          ;
    public string rightFrontWindowPos        ;
    public string rightBackWindowPos         ;
    public string roofPos                    ;
    public string ventPos                    ;
    public string hvacPower                  ;
    public string fanDirection               ;
    public string fanSpeed                   ;
    public string acStatus                   ;
    public string hvacAutoStatus             ;
    public string envCarbinTemperature       ;
    public string hvacTemperature            ;
    public string energyRecoveryPercent      ;
    public string energyRecoveryMode         ;
    public string vcuOnePedal                ;
    public string vcuAccelerationMode        ;
    public string vcuAccelerationModePer     ;
    public string domeLightAutoState         ;
    public string adaptiveFrontState         ;
    public string hdcOn                      ;
    public string dayRunningLight            ;
    public string batteryLevel               ;
    public string lateralAcceleration        ;
    public string longitudinalAcceleration   ;
    public string steerWheelAngle            ;
    public string leftFrontWheelSpeed        ;
    public string rightFrontWheelSpeed       ;
    public string leftBackWheelSpeed         ;
    public string rightBackWheelSpeed        ;
    public string gearLeverPos               ;
    public string controlmodel               ;
    public string carappreciationmodel       ;
    public string renderingmodel             ;
    public string Carpaintswitching          ;
    public string Hubswitching               ;
    public string interiorswitching          ;
    public string HoodStatus                 ;
    public string RearwiperStatus            ;
    public string frontwiperStatus           ;
    public string ACchargingportstatus       ;
    public string DCchargingportstatus       ;
    public string environmentTime            ;
    public string SeatHeat                   ;
    public string WetherType                 ;
    public string interiorColor              ;
    public string leftFrontWheelCorner       ;
    public string leftBackWheelCorner        ;
    public string rightFrontWheelCorner      ;
    public string rightBackWheelCorner       ;
    public string brakeLight                 ;
    public string TurnLeftLight              ;
    public string TurnRightLight             ;
    public string AlermLight                 ;
    public string FrontULight                ;
    public string SpeedLimit                 ;
    public string HeatLevelDriver            ;
    public string HeatLevelCopilot           ;
    public string ClusterBrightness          ;
    public string HDCSlideResource           ;
    public string IdlingSlideResource        ;
    public string AutoHoldSlideResource      ;
    public string SinglePadelResource        ;
    public string KanziMenu                  ;
    public string ReStartAnimation           ;
    public string ACCautolamp                ;
    public string ACCreminderlamp            ;
    public string AEBaccident                ;
    public string AEBaccidentoff             ;
    public string BSDlefttip                 ;
    public string DCAwarning                 ;
    public string DFMSwarning                ;
    public string DOWlefttip                 ;
    public string FCWaccident                ;
    public string FCWaccidentoff             ;
    public string LDWSwarning                ;
    public string LKSwarning                 ;
    public string PCWaccident                ;
    public string PCWaccidentoff             ;
    public string RCTAlefttip                ;
    public string TrunkHeight                ;
    public string EPBBrake                   ;
    public string RearScreens                ;
    public string SceneMode                  ;
    public string RoofMoving                 ;
    public string AcceleratePercentage       ;
    public string RecyclePercentage          ;
    public string CarSpeed;
}

