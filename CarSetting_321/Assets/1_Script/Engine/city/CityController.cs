using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CityController : MonoBehaviour
{
    public GameObject ground_morning, ground_noon, ground_night, ground_cloudy;

    public Transform singleTerrain;
    public int terrainCount=15;
    /// <summary>
    /// 地形链表
    /// </summary>
    private List<Transform> m_terrainList=new List<Transform>();

    /// <summary>
    /// 地形长度
    /// </summary>
    public float m_terrainSize = 99f;

    /// <summary>
    /// 运动速度
    /// </summary>
    public float m_speed = 0.6f;

    /// <summary>
    /// 运动朝向
    /// </summary>
    public Vector3 offset = new Vector3(0, 0, -1000);
    private void OnEnable()
    {
        CloseAllTimeBG();
        AppDelegate.ChangeTimeEvent += CitySceneObjectMaterialsSetting;
        AppDelegate.ChangeWeatherEvent += CitySceneObjectMaterialsSetting;
    }

    private void OnDisable()
    {
        AppDelegate.ChangeTimeEvent -= CitySceneObjectMaterialsSetting;
        AppDelegate.ChangeWeatherEvent -= CitySceneObjectMaterialsSetting;
    }
    private void CloseAllTimeBG()
    {
        ground_morning.SetActive(false);
        ground_noon.SetActive(false);
        ground_night.SetActive(false);
        ground_cloudy.SetActive(false);
    }
    private void CitySceneObjectMaterialsSetting(string s)
    {
       
        //OnRoadLightSetting(false);
        if (s == "morning")
        {
            if (WeatherController.isSunny) {  MorningBG(); }
            else { CloudyBG(); }
        }
        else if (s == "afternoon")
        {
            if (WeatherController.isSunny) { AfternoonBG(); }
            else { CloudyBG(); }
        }
        else if (s == "night")
        {
            NightBG();
        }
        else if (s == "cloudy" || s == "rain" || s == "snow")
        {
            CloudyBG();
        }
        else if (s == "sunny")
        {
            switch (TimeController.timeType)
            {
                case "morning":
                    MorningBG();
                    break;
                case "afternoon":
                    AfternoonBG();
                    break;
                case "night":
                    NightBG();
                    break;
            }
        }
    }
    private void MorningBG()
    {
        foreach (var item in m_terrainList)
        {
            CopyManager.Instance.CopyAll(ground_morning.transform, item.transform);
            item.gameObject.SetActive(true);
        }
    }
    private void AfternoonBG()
    {
        foreach (var item in m_terrainList)
        {
            CopyManager.Instance.CopyAll(ground_noon.transform, item.transform);
            item.gameObject.SetActive(true);
        }
    }
    private void NightBG()
    {
        foreach (var item in m_terrainList)
        {
            CopyManager.Instance.CopyAll(ground_night.transform, item.transform);
            item.gameObject.SetActive(true);
        }
    }
    private void CloudyBG()
    {
        foreach (var item in m_terrainList)
        {
            CopyManager.Instance.CopyAll(ground_cloudy.transform, item.transform);
            item.gameObject.SetActive(true);
        }
    }
    #region Run
    /// <summary>
    /// 重置地形的临界点
    /// </summary>
    private float minEdge;

    void Start()
    {
        InitBuilding();
        InitTerrian();
    }


    /// <summary>
    /// 动态随机建筑方法
    /// </summary>
    private void InitBuilding()
    {
        if (!singleTerrain) { Debug.LogError("缺少地形变量"); return; }

        m_terrainList.Add(singleTerrain);

        //复制两个地形
        for (int i = 0; i < terrainCount; i++)
        {
            GameObject objTerrain0 = Instantiate(singleTerrain.gameObject, singleTerrain.position, singleTerrain.rotation,transform);
            m_terrainList.Add(objTerrain0.transform);
        }

        //初始化地形的位置
        for (int i = 0; i < m_terrainList.Count; i++)
        {
            m_terrainList[i].transform.localPosition = new Vector3(0, 0, (i - (int)(m_terrainList.Count / 2)) * m_terrainSize);
        }
    }
    private void InitTerrian()
    {
        //设置地形移动的边界
        minEdge = m_terrainList[0].localPosition.z - m_terrainSize;
    }
    /// <summary>
    /// 地形的移动
    /// </summary>
    private void TerrianMove()
    {
        if (m_terrainList.Count <= 0 || CarController.curSpeed <= 0)
        {
            return;
        }
        foreach (var item in m_terrainList)
        {
            item.localPosition = Vector3.MoveTowards(item.localPosition, offset, Time.deltaTime * CarController.curSpeed * m_speed);

        }
        //重置地形位置
        int index = m_terrainList.Count - 1;
        if (m_terrainList[0].localPosition.z <= minEdge)
        {
            m_terrainList[0].localPosition = m_terrainList[index].localPosition + new Vector3(0, 0, m_terrainSize * 1);
            Transform g = m_terrainList[0];
            m_terrainList.RemoveAt(0);
            m_terrainList.Insert(index, g);

            //建筑的随机变化
            //g.GetComponent<SingleTerrainControl>().RandomBuildings();
        }
    }
    void FixedUpdate()
    {
        //运动
        TerrianMove();
    }

#endregion
}
/*
private void CitySceneObjectMaterialsSetting(string s)
{
    OnRoadLightSetting(false);
    if (s == "morning")
    {
        if (WeatherController.isSunny) { OnRoadMatsEvent(road_grassland_morning, road_farGrassland_morning, road_shoulder_morning, road_surface_morning); }
        else { OnRoadMatsEvent(road_grassland_cloudy, road_farGrassland_cloudy, road_shoulder_cloudy, road_surface_cloudy); }
    }
    else if (s == "afternoon")
    {
        if (WeatherController.isSunny) { OnRoadMatsEvent(road_grassland_noon, road_farGrassland_noon, road_shoulder_noon, road_surface_noon); }
        else { OnRoadMatsEvent(road_grassland_cloudy, road_farGrassland_cloudy, road_shoulder_cloudy, road_surface_cloudy); }
    }
    else if (s == "night")
    {
        OnRoadMatsEvent(road_grassland_night, road_farGrassland_night, road_shoulder_night, road_surface_night);
        OnRoadLightSetting(true);
    }
    else if (s == "cloudy" || s == "rain" || s == "snow")
    {
        OnRoadMatsEvent(road_grassland_cloudy, road_farGrassland_cloudy, road_shoulder_cloudy, road_surface_cloudy);

    }
    else if (s == "sunny")
    {
        switch (TimeController.timeType)
        {
            case "morning":
                OnRoadMatsEvent(road_grassland_morning, road_farGrassland_morning, road_shoulder_morning, road_surface_morning);
                break;
            case "afternoon":
                OnRoadMatsEvent(road_grassland_noon, road_farGrassland_noon, road_shoulder_noon, road_surface_noon);
                break;
            case "night":
                OnRoadMatsEvent(road_grassland_night, road_farGrassland_night, road_shoulder_night, road_surface_night);
                break;
        }

    }
}
*/
