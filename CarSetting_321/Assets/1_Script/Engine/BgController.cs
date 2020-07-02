using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 地理、光照环境配置
/// </summary>
public class BgController : MonoBehaviour
{
    public GameObject bg_city;
    public GameObject bg_desert;
    public GameObject bg_jungle;
    public GameObject bg_mountain;

    public ReflectionProbe firstRP;
    public ReflectionProbe secondRP;

    public Cubemap firstRef_morning;
    public Cubemap firstRef_noon;
    public Cubemap firstRef_night;
    public Cubemap firstRef_cloudy;

    public Cubemap secondRef_morning;
    public Cubemap secondRef_noon;
    public Cubemap secondRef_night;
    public Cubemap secondRef_cloudy;

    public Material mat_morning;
    public Material mat_cloudy;
    public Material mat_night;
    public Material mat_afternoon;

    private Skybox mSky;

    private Skybox[] otherSkyballs;
public List<GameObject> ppVolumList;
    private void Start()
    {
        mSky = Camera.main.gameObject.GetComponent<Skybox>();
        otherSkyballs = GameObject.Find("CameraPath").transform.GetComponentsInChildren<Skybox>(true);
    foreach (var item in ppVolumList)
        {
            item.SetActive(false);
 if(item.name.Contains("morning"))  item.SetActive(true);
        }
    }
    private void OnEnable()
    {
        AppDelegate.ChangeBackgroundEvent += SwitchBackground;
        AppDelegate.ChangeTimeEvent += ChangeReflectionProbe;
        AppDelegate.ChangeWeatherEvent += ChangeReflectionProbe;

        AppDelegate.ChangeTimeEvent += ChangeTimeSkyball;
        AppDelegate.ChangeWeatherEvent += ChangeWeatherSkyball;
    }
    private void OnDisable()
    {
        AppDelegate.ChangeBackgroundEvent -= SwitchBackground;
        AppDelegate.ChangeTimeEvent -= ChangeReflectionProbe;
        AppDelegate.ChangeWeatherEvent -= ChangeReflectionProbe;

        AppDelegate.ChangeTimeEvent -= ChangeTimeSkyball;
        AppDelegate.ChangeWeatherEvent -= ChangeWeatherSkyball;
    }
    public void SwitchBackground(string type)
    {
        if (type == "city")
        {
            bg_city?.SetActive(true);
            bg_desert?.SetActive(false);
            bg_jungle?.SetActive(false);
            bg_mountain?.SetActive(false);
        }
        else if (type == "desert")
        {
            bg_city?.SetActive(false);
            bg_desert?.SetActive(true);
            bg_jungle?.SetActive(false);
            bg_mountain?.SetActive(false);
        }
        else if (type == "jungle")
        {
            bg_city?.SetActive(false);
            bg_desert?.SetActive(false);
            bg_jungle?.SetActive(true);
            bg_mountain?.SetActive(false);
        }
        else if (type == "mountain")
        {
            bg_city?.SetActive(false);
            bg_desert?.SetActive(false);
            bg_jungle?.SetActive(false);
            bg_mountain?.SetActive(true);
        }
    }
/*
    private void ChangeReflectionProbe(string s)
    {
        if(s=="morning")
        {
            if(WeatherController.isSunny)
            {
                firstRP.customBakedTexture = firstRef_morning;
                secondRP.customBakedTexture = secondRef_morning;
            }
            else
            {
                firstRP.customBakedTexture = firstRef_cloudy;
                secondRP.customBakedTexture = secondRef_cloudy;
            }
  
        }
        else if(s=="afternoon")
        {
            if (WeatherController.isSunny)
            {
                firstRP.customBakedTexture = firstRef_noon;
                secondRP.customBakedTexture = secondRef_noon;
            }
            else
            {
                firstRP.customBakedTexture = firstRef_cloudy;
                secondRP.customBakedTexture = secondRef_cloudy;
            }
        }
        else if (s == "night")
        {
            firstRP.customBakedTexture = firstRef_night;
            secondRP.customBakedTexture = secondRef_night;
        }
        else if (s == "cloudy" ||s=="rain" ||s=="snow")
        {
            firstRP.customBakedTexture = firstRef_cloudy;
            secondRP.customBakedTexture = secondRef_cloudy;
        }
        else if (s == "sunny")
        {
            switch (TimeController.timeType)
            {
                case "morning":
                    firstRP.customBakedTexture = firstRef_morning;
                    secondRP.customBakedTexture = secondRef_morning;
                    break;
                case "afternoon":
                    firstRP.customBakedTexture = firstRef_noon;
                    secondRP.customBakedTexture = secondRef_noon;
                    break;
                case "night":
                    firstRP.customBakedTexture = firstRef_night;
                    secondRP.customBakedTexture = secondRef_night;
                    break;
            }
         
        }
    }
*/
 private void ChangeReflectionProbe(string s)
    {
        if(s=="morning")
        {
            if(WeatherController.isSunny)
            {
                firstRP.customBakedTexture = firstRef_morning;
                secondRP.customBakedTexture = secondRef_morning;
                RefProbeSetting(s);
            }
            else
            {
                firstRP.customBakedTexture = firstRef_cloudy;
                secondRP.customBakedTexture = secondRef_cloudy;
                RefProbeSetting("cloudy");
            }
  
        }
        else if(s=="afternoon")
        {
            if (WeatherController.isSunny)
            {
                firstRP.customBakedTexture = firstRef_noon;
                secondRP.customBakedTexture = secondRef_noon;
                RefProbeSetting(s);
            }
            else
            {
                firstRP.customBakedTexture = firstRef_cloudy;
                secondRP.customBakedTexture = secondRef_cloudy;
                RefProbeSetting("cloudy");
            }
        }
        else if (s == "night")
        {
            firstRP.customBakedTexture = firstRef_night;
            secondRP.customBakedTexture = secondRef_night;
            RefProbeSetting(s);
        }
        else if (s == "cloudy" ||s=="rain" ||s=="snow")
        {
            firstRP.customBakedTexture = firstRef_cloudy;
            secondRP.customBakedTexture = secondRef_cloudy;
            RefProbeSetting("cloudy");
        }
        else if (s == "sunny")
        {
            switch (TimeController.timeType)
            {
                case "morning":
                    firstRP.customBakedTexture = firstRef_morning;
                    secondRP.customBakedTexture = secondRef_morning;
                    RefProbeSetting(TimeController.timeType);
                    break;
                case "afternoon":
                    firstRP.customBakedTexture = firstRef_noon;
                    secondRP.customBakedTexture = secondRef_noon;
                    RefProbeSetting(TimeController.timeType);
                    break;
                case "night":
                    firstRP.customBakedTexture = firstRef_night;
                    secondRP.customBakedTexture = secondRef_night;
                    RefProbeSetting(TimeController.timeType);
                    break;
            }
         
        }
    }

    private void RefProbeSetting(string s)
    {
       foreach (var item in ppVolumList)
        {
            item.SetActive(false);
            if (item.name.Contains(s))
            {
                item.SetActive(true);
            }
        }
       switch( s)
        {
            case "morning":
                firstRP.intensity = 1;
                secondRP.intensity =2;
                break;
            case "afternoon":
                firstRP.intensity = 1f;
                secondRP.intensity = 2.2f;
                break;
            case "night":
                firstRP.intensity = 4f;
                secondRP.intensity = 0.6f;
                break;
            case "cloudy":
                firstRP.intensity = 2f;
                secondRP.intensity = 1f;
                break;

        }
    }
    private void ChangeTimeSkyball(string type)
    {
        if (type == "afternoon")
        {
            if (WeatherController.isSunny) { mSky.material = mat_afternoon; }
            else { mSky.material = mat_cloudy; }
        }
        else if (type == "night")
        {
            mSky.material = mat_night;
        }
        else if (type == "morning")
        {
            if (WeatherController.isSunny) { mSky.material = mat_morning; }
            else { mSky.material = mat_cloudy; }
        }
        foreach (var item in otherSkyballs)
        {
            item.material = mSky.material;
        }
    }
    private void ChangeWeatherSkyball(string type)
    {
        if (type == "sunny")
        {
            Debug.Log(TimeController.timeType);
            switch (TimeController.timeType)
            {
                case "morning": mSky.material = mat_morning; break;
                case "afternoon": mSky.material =mat_afternoon; break;
                case "night": mSky.material = mat_night; break;
            }
        }
        else if (type == "cloudy" || type == "snow" || type == "rain")
        {
            if (mSky.material == mat_night) return;
            mSky.material = mat_cloudy;
        }
        foreach (var item in otherSkyballs)
        {
            item.material = mSky.material;
        }
    }
}
