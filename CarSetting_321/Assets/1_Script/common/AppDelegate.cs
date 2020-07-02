using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public delegate void CarSettingDelegate();
public delegate void ChangeColorDele(string value);
public delegate void ChangeCarLigthDele(string s);
public delegate void ChargingDele(string s);
public delegate void CarAnimationDele(CarAniEnum value);
public delegate void ChangeWheelsDele(string value);
public delegate void ChangeWeatherDele(string value);
public delegate void ChangeTimeDele(string value);
public delegate void ChangeCameraVectorDele(string value);
public delegate void ChangeBackgroundDele(string value);
public delegate void WarningDele(string value);
public delegate void RestartDele(string value);
public delegate void ReleaseMemoryDele(string value);
public delegate void CameraPathAniDele(string value);
public delegate void RadarDele(bool value);
public delegate void FrontLightrotateByRoadDele(bool value);
public delegate void FrontLightfarNearAutoControlDele(bool value);
public delegate void DriveModeChangeDele(string value);
public delegate void AtmosphereColorDele(string value);
public delegate void ReadingLightDele(string value);

public class AppDelegate :Singleton<AppDelegate>
{
    public static event ReadingLightDele ReadingLightEvent;
    public void OnReadingLight(string value)
    {
        if (ReadingLightEvent != null)
            ReadingLightEvent(value);
    }




    public static event AtmosphereColorDele AtmosphereColorEvent;
    public void OnAtmosphereColor(string value)
    {
        if(AtmosphereColorEvent!=null)
            AtmosphereColorEvent(value);
    }

    public static event CarSettingDelegate CarSettingEvent;
    public void OnCarSetting()
    {
        if(CarSettingEvent!=null)
        CarSettingEvent(); 
    }

    public static event ChangeColorDele ChangeColorEvent;
    public void OnChangeColor(string value)
    {
        if(ChangeColorEvent!=null)
        ChangeColorEvent(value);
    }
    public static event ChangeCarLigthDele ChangeCarLigthEvent;
    public void OnChangeCarLigth(string s)
    {
        if(ChangeCarLigthEvent!=null)
        ChangeCarLigthEvent(s);

    }
    public static event CarAnimationDele CarAnimationEvent;
    public void OnCarAnimation(CarAniEnum aniType)
    {
        if(CarAnimationEvent!=null)
        CarAnimationEvent(aniType);

    }
    public static event ChangeWheelsDele ChangeWheelsEvent;
    public void OnChangeWheels(string wheel)
    {
        if(ChangeWheelsEvent!=null)
        ChangeWheelsEvent(wheel);

    }
    public static event ChargingDele ChargingEvent;
    public void OnCharging( string s)
    {
        if (ChargingEvent == null) return;
        ChargingEvent(s);
    }

    public static event ChangeWeatherDele ChangeWeatherEvent;
    public void OnChangeWeather(string s)
    {
        if (ChangeWeatherEvent == null) return;
        ChangeWeatherEvent(s);
    }

    public static event ChangeTimeDele ChangeTimeEvent;
    public void OnChangeTime(string s)
    {
        if (ChangeTimeEvent == null) return;
        ChangeTimeEvent(s);
    }

    public static event ChangeCameraVectorDele ChangeCameraVectorEvent;
    public void OnchangeCameraVecotr(string s)
    {
        if(ChangeCameraVectorEvent!=null)
        {
            ChangeCameraVectorEvent(s);
        }
    }

    public static event ChangeBackgroundDele ChangeBackgroundEvent;
    public void OnChangeBackground(string s)
    {
        if (ChangeBackgroundEvent != null)
        {
            ChangeBackgroundEvent(s);
        }
    }

    public static event WarningDele WarningEvent;
    public void OnWarning(string s)
    {
        if (WarningEvent != null)
        {
            WarningEvent(s);
        }
    }

    public static event RestartDele Restart;
    public void OnRestart(string s)
    {
        if (Restart != null)
        {
            Restart(s);
        }
    }

    public static event ReleaseMemoryDele ReleaseMemory;
    public void OnReleaseMemory(string s)
    {
        if (ReleaseMemory != null)
        {
            ReleaseMemory(s);
        }
    }

    public static event CameraPathAniDele CameraPathAni;
    public void OnCameraPathAni(string s)
    {
        if(CameraPathAni!=null)
        {
            CameraPathAni(s);
        }
    }

    public static event RadarDele RadarEvent;
    public void OnRadarOpenCloseEvent(bool isOpen)
    {
        if(RadarEvent!=null)
        {
            RadarEvent(isOpen);
        }
    }

    public static event FrontLightrotateByRoadDele FrontLightrotateByRoadEvent;
    public void OnFrontLightrotateByRoadEvent(bool isOpen)
    {
        if (FrontLightrotateByRoadEvent != null)
        {
            FrontLightrotateByRoadEvent(isOpen);
        }
    }

    public static event FrontLightfarNearAutoControlDele FrontLightfarNearAutoControlEvent;
    public void OnFrontLightfarNearAutoControlDeleEvent(bool isOpen)
    {
        if (FrontLightfarNearAutoControlEvent != null)
        {
            FrontLightfarNearAutoControlEvent(isOpen);
        }
    }
    public static event DriveModeChangeDele DriveModeChangeEvent;
    public void OnDriveModeChangeEvent(string s)
    {
        if(DriveModeChangeEvent!=null)
        {
            DriveModeChangeEvent(s);
        }
    }
}
