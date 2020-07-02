using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelClickEvent : MonoBehaviour
{
    public CarAniEnum AniType { get; set; }
    private Vector3 tempPointPosition;
    public void OnClick(UnityEngine.EventSystems.BaseEventData data = null)
    {
        //Debug.Log("点击了cube tran=" + transform.name);
        //AppDelegate.Instance.OnCarAnimation(AniType);
    }

    public void OnDown(UnityEngine.EventSystems.BaseEventData data = null)
    {
        tempPointPosition = Input.mousePosition;
    }

    public void OnUp(UnityEngine.EventSystems.BaseEventData data = null)
    {

        float dis = Vector3.Distance(Input.mousePosition, tempPointPosition);
        if (dis<1)
        {
            AppDelegate.Instance.OnCarAnimation(AniType);
        }
        tempPointPosition = Vector3.zero;
      
    }


}
