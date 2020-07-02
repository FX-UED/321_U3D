using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarLightControl : MonoBehaviour
{
    [Header("车灯旋转轴物体")]
    public Transform frontLight;
    [Header("车灯在地面上的光影模型")]
    public Transform frontLight_Road;
    [Header("必须要添加一个目标关注物体名字为CarLightTarget")]
    public Transform lookTarget;

    [Header("自动远近光灯的最远值")]
    public Vector3 farLight_rotation;
    public Vector3 farLight_road_position;
    [Header("自动远近光灯的中间值")]
    public Vector3 middleLight_rotation;
    public Vector3 middleLight_road_position;

    [Header("自动远近光灯的最近值")]
    public Vector3 nearLight_rotation;
    public Vector3 nearLight_road_position;

    private bool isFrontLightrotateByRoadEvent = false;
    private bool isFrontLightfarNearAutoControl = false;
    private void OnEnable()
    {
        if(lookTarget==null)
        {
            if (GameObject.Find("CarLightTarget")!=null)
            lookTarget = GameObject.Find("CarLightTarget").transform;
        }
        AppDelegate.FrontLightrotateByRoadEvent += FrontLightrotateByRoad;
        AppDelegate.FrontLightfarNearAutoControlEvent += FrontLightfarNearAutoControl;
    }
    private void OnDisable()
    {
        AppDelegate.FrontLightfarNearAutoControlEvent -= FrontLightfarNearAutoControl;
        AppDelegate.FrontLightrotateByRoadEvent -= FrontLightrotateByRoad;
    }
    // Update is called once per frame
    void Update()
    {
        if (isFrontLightrotateByRoadEvent)
        {
            if (lookTarget != null)
            {
                transform.LookAt(lookTarget);
            }
        }
    }

    private void FrontLightDistanceControl(CarLightDistanceEnum carLightDistanceEnum)
    {
        switch(carLightDistanceEnum)
        {
            case CarLightDistanceEnum.far:
                TweenControl.Instance.RotateTo(frontLight, farLight_rotation, 1, 0, 1, DG.Tweening.LoopType.Restart, DG.Tweening.Ease.Linear);
                TweenControl.Instance.MoveTo(frontLight_Road, farLight_road_position, 1, 0, 1, DG.Tweening.LoopType.Restart, DG.Tweening.Ease.Linear);
                break;
            case CarLightDistanceEnum.middle:
                TweenControl.Instance.RotateTo(frontLight, middleLight_rotation, 1, 0, 1, DG.Tweening.LoopType.Restart, DG.Tweening.Ease.Linear);
                TweenControl.Instance.MoveTo(frontLight_Road, middleLight_road_position, 1, 0, 1, DG.Tweening.LoopType.Restart, DG.Tweening.Ease.Linear);
                break;
            case CarLightDistanceEnum.near:
                TweenControl.Instance.RotateTo(frontLight, nearLight_rotation, 1, 0, 1, DG.Tweening.LoopType.Restart, DG.Tweening.Ease.Linear);
                TweenControl.Instance.MoveTo(frontLight_Road, nearLight_road_position, 1, 0, 1, DG.Tweening.LoopType.Restart, DG.Tweening.Ease.Linear);
                break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("ai"))
        {
        
            FrontLightDistanceControl( CarLightDistanceEnum.near);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ai"))
        {
            FrontLightDistanceControl(CarLightDistanceEnum.far);
        }
    }

    private void FrontLightrotateByRoad(bool isOn)
    {
        isFrontLightrotateByRoadEvent = isOn;
    }
    private void FrontLightfarNearAutoControl(bool isOn)
    {
        isFrontLightfarNearAutoControl = isOn;
    }
}
