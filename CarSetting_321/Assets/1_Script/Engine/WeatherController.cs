using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherController : MonoBehaviour
{
    public GameObject sunny;
    public GameObject rain;
    public GameObject snow;
    public GameObject cloudy;
    public static bool isSunny = true;
    private void OnEnable()
    {
        AppDelegate.ChangeWeatherEvent += SwitchWeather;
    }
    private void OnDisable()
    {
        AppDelegate.ChangeWeatherEvent -= SwitchWeather;
    }
    public void SwitchWeather(string type)
    {
        if (type == "sunny")
        {
            isSunny = true;
            sunny.SetActive(true);
            rain.SetActive(false);
            snow.SetActive(false);
            cloudy.SetActive(false);
        }
        else if(type=="rain")
        {
            isSunny = false;
            sunny.SetActive(false);
            rain.SetActive(true);
            snow.SetActive(false);
            cloudy.SetActive(false);
        }
        else if (type == "snow")
        {
            isSunny = false;
            sunny.SetActive(false);
            rain.SetActive(false);
            snow.SetActive(true);
            cloudy.SetActive(false);
        }
        else if (type == "cloudy")
        {
            isSunny = false;
            sunny.SetActive(false);
            rain.SetActive(false);
            snow.SetActive(false);
            cloudy.SetActive(true);
        }
    }
}
