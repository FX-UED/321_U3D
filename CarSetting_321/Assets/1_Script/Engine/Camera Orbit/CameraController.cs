using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ProjectName
{

}
public class CameraController : MonoBehaviour
{
    public Transform tar_front;
    public Transform tar_left;
    public Transform tar_right;
    public Transform tar_back;
    public Transform tar_top;
    public Transform lookTarget;

    private UltimateOrbitCamera mOrbitCamera;
    public Transform camTrans;

    public Dictionary<Transform, Transform> cameraPosAndTargetPos;
    void Awake()
    {
        mOrbitCamera = GetComponent<UltimateOrbitCamera>();
       // mSky = GetComponent<Skybox>();
    }
    private void OnEnable()
    {
        AppDelegate.ChangeCameraVectorEvent += ChangeCamPos;
        //AppDelegate.ChangeTimeEvent += ChangeTimeSkyball;
        //AppDelegate.ChangeWeatherEvent+= ChangeWeatherSkyball;
    }
    private void OnDisable()
    {
        AppDelegate.ChangeCameraVectorEvent -= ChangeCamPos;
        //AppDelegate.ChangeTimeEvent -= ChangeTimeSkyball;
        //AppDelegate.ChangeWeatherEvent -= ChangeWeatherSkyball;
    }

    public void ChangeCamPos(string vector)
    {
        mOrbitCamera.enabled = false;
        if (vector=="front")
        { camTrans.position = tar_front.position;
            mOrbitCamera.yMinLimit = 0;
        }
        else if(vector=="left")
        { camTrans.position = tar_left.position;
            mOrbitCamera.yMinLimit = 0;
        }
        else if (vector == "right")
        { camTrans.position = tar_right.position;
            mOrbitCamera.yMinLimit = 0;
        }
        else if (vector == "back")
        { camTrans.position = tar_back.position;
            mOrbitCamera.yMinLimit = 0;
        }
        else if (vector == "top")
        { camTrans.position = tar_top.position;
            mOrbitCamera.yMinLimit = -90;
        }

        //if (vector == "free")
        //{
        //    camTrans.localEulerAngles = new Vector3(0, 90, 0);
        //    camTrans.position = new Vector3(-6, 0.9f, 0);
        //    mOrbitCamera.yMinLimit = 0;

        //}
        camTrans.LookAt(lookTarget);
        mOrbitCamera.enabled = true;
    }

    private void CamTransToTarget()
    {
        mOrbitCamera.enabled = false;
    }
    private void FixedUpdate()
    {
        
    }
    /*
    private void ChangeTimeSkyball(string type)
    {
        if (type == "afternoon")
        {
            if (WeatherController.isSunny) { mSky.material =BgController.Instance.mat_afternoon; }
            else {  mSky.material = BgController.Instance.mat_cloudy; }
        }
        else if (type == "night")
        {
            mSky.material = BgController.Instance.mat_night;
        }
        else if (type == "morning")
        {
            if (WeatherController.isSunny)  { mSky.material = BgController.Instance.mat_morning; }
            else {  mSky.material = BgController.Instance.mat_cloudy; }
        }
    }
    private void ChangeWeatherSkyball(string type)
    {
        if (type == "sunny")
        {
            switch(TimeController.timeType)
            {
                case "morning": mSky.material = BgController.Instance.mat_morning; break;
                case "afternoon":  mSky.material = BgController.Instance.mat_afternoon; break;
            }
        }
        else if (type == "cloudy" || type == "snow" || type == "rain")
        {
            if (mSky.material == BgController.Instance.mat_night)  return;
            mSky.material = BgController.Instance.mat_cloudy;
        }
    }
    */
}
