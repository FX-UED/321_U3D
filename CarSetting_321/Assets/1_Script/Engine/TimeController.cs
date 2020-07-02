using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public GameObject morning;
    public GameObject afternoon;
    public GameObject night;
    private GameObject curTime;
    public static string timeType= "morning";
    private void OnEnable()
    {
        AppDelegate.ChangeTimeEvent += SwitchTime;
        AppDelegate.ChangeWeatherEvent += ChangeLightType;
    }
    private void OnDisable()
    {
        AppDelegate.ChangeTimeEvent -= SwitchTime;
        AppDelegate.ChangeWeatherEvent -= ChangeLightType;
    }
    public void SwitchTime(string type)
    {
        timeType = type;
        //morning.SetActive(false);
        //night.SetActive(false);
        //afternoon.SetActive(false);

        //if (type=="afternoon")
        //{
        //    curTime = afternoon;
        //}
        //else if(type=="night")
        //{
        //    curTime = night;
        //}
        //else if (type == "morning")
        //{
        //    curTime = morning;
        //}
        //curTime.SetActive(true);
    }
    private void ChangeLightType(string type)
    {
        //if(type=="sunny")
        //{
        //    curTime?.transform.Find("light").gameObject.SetActive(true);
        //    curTime?.transform.Find("dark").gameObject.SetActive(false);
        //}
        //else
        //{
        //    curTime?.transform.Find("light").gameObject.SetActive(false);
        //    curTime?.transform.Find("dark").gameObject.SetActive(true);
        //}
    }
}
