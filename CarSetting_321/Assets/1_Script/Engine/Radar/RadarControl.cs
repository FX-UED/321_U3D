using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RadarType
{
    floorPlane,
    boxNearMiddleFar
}
public class RadarControl : MonoBehaviour
{
    public RadarType radarType;
    private void OnEnable()
    {
        AppDelegate.RadarEvent += SetRadar;
    }
    private void OnDisable()
    {
        AppDelegate.RadarEvent -= SetRadar;
    }
    public void SetRadar(bool isOpenRadar)
    {
        if (radarType == RadarType.boxNearMiddleFar)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(isOpenRadar);
            }
        }
        else
        {
            GetComponent<MeshRenderer>().enabled = isOpenRadar;
        }
    }
}
