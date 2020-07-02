using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
/// <summary>
/// 根据xml文件加载场景物体
/// </summary>
public class SceneInitialization : DDOLSingleton<SceneInitialization>
{
    /// <summary>
    /// 所有加载到场景里的物体, 物体名字，物体
    /// </summary>
    Dictionary<string, GameObject> objLoadedDic = new Dictionary<string, GameObject>();

    /// <summary>
  /// 主相机
  /// </summary>
    public Camera mianCam { get { return Camera.main; } }

    /// <summary>
    /// UI画布
    /// </summary>
    Transform uiCanvas;
    private void UnloadSceneObjects()
    {
        if(objLoadedDic!=null)
        {
            foreach (var item in objLoadedDic)
            {
                item.Value.SetActive(false);
            }
        }
    }
    #region 加载物体到场景中，隐藏、显示，
    public void LoadPrefab(Dictionary<string, string> objPrefabsDic=null, Dictionary<string, string> uiPrefabDic=null)
    {
        UnloadSceneObjects();
        //非UI物体
        if (objPrefabsDic != null)
        {
            foreach (var item in objPrefabsDic)
            {   //如果加载过这个物体
                if (!objLoadedDic.ContainsKey(item.Key))
                {
                    //如果没加载过
                    if (Resources.Load("Prefab/" + item.Key) != null)
                    {
                        GameObject go = Instantiate(Resources.Load("Prefab/" + item.Key)) as GameObject;
                        go.name = item.Key;
                        //加入字典
                        objLoadedDic.Add(item.Key, go);
                    }
                    else
                    {
                        Debug.LogError("error  -  输入的名字有误:" + item.Key);
                    }
                }
                //设置协程
                float waitime = 0;
                float hidetime = -1;
                if (!string.IsNullOrEmpty(item.Value.Split('_')[0])) { waitime = float.Parse(item.Value.Split('_')[0]); }
                if (!string.IsNullOrEmpty(item.Value.Split('_')[1])) { hidetime = float.Parse(item.Value.Split('_')[1]); }

                StartCoroutine(ObjShowHide(objLoadedDic[item.Key], waitime, hidetime));
            }
        }
        // ui
        if (uiPrefabDic != null)
        {
            if (null == uiCanvas) { uiCanvas = GameObject.Find("Canvas").transform; }

            foreach (var item in uiPrefabDic)
            {
                if (!objLoadedDic.ContainsKey(item.Key))
                {
                    //如果没加载过
                    if (Resources.Load("UI/" + item.Key) != null)
                    {
                        GameObject go = Instantiate(Resources.Load("UI/" + item.Key), uiCanvas) as GameObject;
                        go.name = item.Key;
                        //加入字典
                        objLoadedDic.Add(item.Key, go);
                    }
                    else
                    {
                        Debug.LogError("error  -  输入的名字有误:" + item.Key);
                    }
                }
                //设置协程
                float waitime = 0;
                float hidetime = -1;
                if (!string.IsNullOrEmpty(item.Value.Split('_')[0])) { waitime = float.Parse(item.Value.Split('_')[0]); }
                if (!string.IsNullOrEmpty(item.Value.Split('_')[1])) { hidetime = float.Parse(item.Value.Split('_')[1]); }

                StartCoroutine(ObjShowHide(objLoadedDic[item.Key], waitime, hidetime));
            }
        }
    }

    public void SceneObjShowHide(List<string> objName, List<string> objTime)
    {
        if (objName != null)
        {
            for (int i = 0; i < objName.Count; i++)
            {
                GameObject tempObj = null;
                if(objName[i].Contains("/"))
                {
                    if (GameObject.Find(objName[i].Split('/')[0]) != null)
                    {
                        Transform parent = GameObject.Find(objName[i].Split('/')[0]).transform;
                        string newName = objName[i].Substring(objName[i].Split('/')[0].Length+1,objName[i].Length- objName[i].Split('/')[0].Length-1);
  
                        tempObj = parent.Find(newName).gameObject;
                        Debug.Log(tempObj.name);
                    }
                }
                else if(GameObject.Find(objName[i]) != null)
                {
                    tempObj = GameObject.Find(objName[i]);

                }
                //设置协程
                float waitime = 0;
                float hidetime = -1;
                if (!string.IsNullOrEmpty(objTime[i].Split('_')[0])) { waitime = float.Parse(objTime[i].Split('_')[0]); }
                if (!string.IsNullOrEmpty(objTime[i].Split('_')[1])) { hidetime = float.Parse(objTime[i].Split('_')[1]); }
                if (tempObj != null)
                    StartCoroutine(ObjShowHide(tempObj, waitime, hidetime));
                else
                {

                    Debug.Log(objName[i]+",物体没有找到！！！");
                }
            }
        }
    }

    /// <summary>
    /// 隐藏、显示 的  协程
    /// </summary>
    /// <param name="go"></param>
    /// <param name="showTime">显示隐藏的时间格式为：00_00</param>
    /// <param name="hideTime"></param>
    /// <returns></returns>
    private IEnumerator ObjShowHide(GameObject go , float showTime=0, float hideTime=0)
    {
        if(showTime>0 && hideTime>0)
        {
            if (showTime < hideTime)
            {
                go.SetActive(false);
                yield return new WaitForSeconds(showTime);
                go.SetActive(true);
                yield return new WaitForSeconds(hideTime);
                go.SetActive(false);
            }
            else
            {
                go.SetActive(true);
                yield return new WaitForSeconds(hideTime);
                go.SetActive(false);
                yield return new WaitForSeconds(showTime);
                go.SetActive(true);
            }
        }
        else if(hideTime <= 0 && showTime>0)
        {
            go.SetActive(false);
            yield return new WaitForSeconds(showTime);
            go.SetActive(true);
        }
        else if(showTime<=0 && hideTime > 0)
        {
            go.SetActive(true);
            yield return new WaitForSeconds(hideTime);
            go.SetActive(false);
        }
        else if (showTime < 0 && hideTime <0)
        {
            go.SetActive(false);
        }
        else
        {
            go.SetActive(true);
        }
    }
    #endregion

    #region 相机属性


    public void SetCameraState(Vector3 cameraPos,  string rot = null,string target= null)
    {
        // 位置
        mianCam.transform.position = cameraPos;

        // 旋转
        if (rot != null)
        {
            Vector3 camRot = new Vector3(float.Parse(rot.Split(',')[0]), float.Parse(rot.Split(',')[1]), float.Parse(rot.Split(',')[2]));
            mianCam.transform.localEulerAngles = camRot;
        }

        //目标点
        if (target != null)
        {
            Vector3 targetPos = new Vector3(float.Parse(target.Split(',')[0]), float.Parse(target.Split(',')[1]), float.Parse(target.Split(',')[2]));
            mianCam.GetComponent<UltimateOrbitCamera>().SetTarget(targetPos);
            //mianCam.transform.LookAt(targetPos);
        }
    }

    #endregion

    #region 物体的路径动画

    public void ObjectPathState(string mTransName, Vector3[] waypoints, float duration, string lookType, float delay)
    {
        if (objLoadedDic.TryGetValue(mTransName, out var _gameobject))
        {
            var look = 0.01f;
            if (lookType == "ahead")
            {
                _gameobject.transform.DOPath(waypoints, duration, PathType.Linear, PathMode.Full3D).SetOptions(false).SetLookAt(look).SetDelay(delay);
            }
            else
            {
                float x = float.Parse(lookType.Split(',')[0]);
                float y = float.Parse(lookType.Split(',')[1]);
                float z = float.Parse(lookType.Split(',')[2]);
                Vector3 target = new Vector3(x, y, z);
                _gameobject.transform.DOPath(waypoints, duration, PathType.CatmullRom, PathMode.Full3D).SetOptions(false).SetLookAt(target).SetDelay(delay);
            }
        }
    }
    #endregion

    #region UV 动画

    public void SetUVanimation(string uvaName, float delay, float speedX, float speedY)
    {
        if (objLoadedDic.TryGetValue(uvaName, out var _gameobject))
        {
            if(_gameobject.GetComponent<UVAnimation>()!=null)
            StartCoroutine(SetUVanimationState(_gameobject.GetComponent<UVAnimation>(), delay, speedX, speedY));
        }
    }
    private IEnumerator SetUVanimationState(UVAnimation uva, float delay, float speedX, float speedY)
    {
        yield return new WaitForSeconds(delay);
        uva.SpeedX = speedX;
        uva.SpeedY = speedY;
    }

    #endregion

    #region model animation

    public void SetModelAnimation(string _name, int state, float waitTime)
    {
        if(GameObject.Find(_name)!=null)
        {
            if(GameObject.Find(_name).GetComponent<Animator>()!=null)
            {
                StartCoroutine(ModelAnimation(GameObject.Find(_name).GetComponent<Animator>(), state, waitTime));
            }
            else
            {
                Debug.LogError(" animator is null");
            }
        }
        else
        {
            if (objLoadedDic.TryGetValue(_name, out var _gameobject))
            {
                if (_gameobject.GetComponent<Animator>() != null)
                    StartCoroutine(ModelAnimation(_gameobject.GetComponent<Animator>(), state, waitTime));
            }
        }

    }
    private IEnumerator ModelAnimation(Animator modleAni, int state, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        modleAni.SetInteger("changeState", state);
    }
    #endregion

    #region ui animation
    public void SetUIAni(string name, float waitTime)
    {
        if (objLoadedDic.TryGetValue(name, out var _gameobject))
        {
            StartCoroutine(UIAnimation(_gameobject, waitTime));
        }
    }
    private IEnumerator UIAnimation(GameObject objUI, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        objUI.SetActive(true);
    }
    #endregion

    #region 灯光状态

    public void SetLightState(string name, float waitTime, float trigger)
    {
        if (objLoadedDic.TryGetValue(name, out var _gameobject))
        {
            if (_gameobject.GetComponent<Light>() != null)
                StartCoroutine(LightControl(_gameobject.GetComponent<Light>(), waitTime, trigger));
        }
    }

    private IEnumerator LightControl(Light light, float waitTime, float trigger)
    {
        yield return new WaitForSeconds(waitTime);
        light.enabled = trigger ==1 ? true : false;
    }
    #endregion

    #region 车速设置

    public void SetCarSpeed(string speed, string waittime)
    {
        StartCoroutine(CarSpeedControl(speed, waittime));
    }

    private IEnumerator CarSpeedControl(string speed, string waitTime)
    {
        yield return new WaitForSeconds(float.Parse(waitTime));
        CarController.carSpeed_finall = float.Parse(speed);
    }
    #endregion

    #region 材质变化
    /// <summary>
    /// 直接换材质球
    /// </summary>
    /// <param name="modelName"></param>
    /// <param name="waittime"></param>
    /// <param name="materialName"></param>
    public void SetMaterial(string modelName, float waittime,string materialName)
    {
        if (modelName.Contains("/"))
        {
            string modelName2 = modelName.Substring(modelName.IndexOf('/') + 1);
            if (objLoadedDic.TryGetValue(modelName.Split('/')[0], out var _gameobject))
            {
                Transform t = _gameobject.transform.Find(modelName2);
                if (t != null)
                {
                    if (t.GetComponent<MeshRenderer>() != null)
                        StartCoroutine(MaterialChange(t.GetComponent<MeshRenderer>(), waittime, materialName));
                }
            }
        }
        else
        {
            if (GameObject.Find(modelName) != null)
            {
                if (GameObject.Find(modelName).GetComponent<MeshRenderer>() != null)
                {
                    StartCoroutine(MaterialChange(GameObject.Find(modelName).GetComponent<MeshRenderer>(), waittime, materialName));
                }
                else
                {
                    Debug.LogError(" MeshRenderer is null");
                }
            }
           else if (objLoadedDic.TryGetValue(modelName, out var _gameobject))
            {
                if (_gameobject.GetComponent<MeshRenderer>() != null)
                    StartCoroutine(MaterialChange(_gameobject.GetComponent<MeshRenderer>(), waittime, materialName));
            }
        }
      
    }

    private IEnumerator MaterialChange(MeshRenderer mrender, float waittime, string materialName)
    {
        yield return new WaitForSeconds(waittime);
        mrender.material = Resources.Load("Materials/" + materialName) as Material;
    }

    /// <summary>
    /// 颜色的渐变.
    /// </summary>
    /// <param name="modelName"></param>
    /// <param name="waittime"></param>
    /// <param name="lerpTime"></param>
    /// <param name="from">hex</param>
    /// <param name="to">hex</param>
    public void SetTweenColor(string modelName, float waittime, float lerpTime,string from, string to)
    {
      
        MeshRenderer mRenter=new MeshRenderer();
        if (modelName.Contains("/"))
        {
            string modelName2 = modelName.Substring(modelName.IndexOf('/') + 1);
            if (objLoadedDic.TryGetValue(modelName.Split('/')[0], out var _gameobject))
            {
                Transform t = _gameobject.transform.Find(modelName2);
                if (t != null)
                {
                    if (t.GetComponent<MeshRenderer>() != null) { mRenter = t.GetComponent<MeshRenderer>(); }
                       
                }
            }
        }
        else
        {
            if (objLoadedDic.TryGetValue(modelName, out var _gameobject))
            {
                if (_gameobject.GetComponent<MeshRenderer>() != null) { mRenter = _gameobject.GetComponent<MeshRenderer>(); }

            }
        }
        Debug.Log(mRenter.material.name);
        mRenter.material.color =HexToColor( from);
        TweenControl.Instance.ColorTo(mRenter.material, HexToColor(to),lerpTime,waittime,1, LoopType.Restart, Ease.Linear);
    }


    #endregion

    #region UI 文本
    public void SetUIText(string text)
    {
        if (null == uiCanvas) { uiCanvas = GameObject.Find("Canvas").transform; }
        uiCanvas.Find("TextExplain").GetComponent<Text>().text = text;
    }

    #endregion

    #region 拖尾

    public void SetTrail(string objName,float waitime)
    {
        if(GameObject.Find(objName)!=null)
        {
            StartCoroutine(TrailSetting(GameObject.Find(objName).GetComponent<TrailRenderer>(),waitime));
        }
    }
    private IEnumerator TrailSetting(TrailRenderer tr,float waitime)
    {
        tr.time = 0;
        yield return new WaitForSeconds(waitime);
        tr.time = 999999;
    }
    #endregion

    #region 后视镜翻转

    public void SetRearViewMirror(string modelName, float waitime,string rotateVector,float angle,float speed)
    {
        if (modelName.Contains("/"))
        {
            string modelName2 = modelName.Substring(modelName.IndexOf('/') + 1);
            if (objLoadedDic.TryGetValue(modelName.Split('/')[0], out var _gameobject))
            {
                Transform t = _gameobject.transform.Find(modelName2);
                if (t != null)
                {
                    StartCoroutine(RearViewMirrorXYZ(t, waitime, rotateVector,  angle, speed));
                }
            }
        }
        else
        {
            if (objLoadedDic.TryGetValue(modelName, out var _gameobject))
            {
                StartCoroutine(RearViewMirrorXYZ(_gameobject.transform, waitime, rotateVector,  angle, speed));
            }
        }
    }
    private IEnumerator RearViewMirror(Transform tr, float waitime, string rotateVector,  float angle,float rotSpeed)
    {
        yield return new WaitForSeconds(waitime);
        float curAngle=0;
        switch (rotateVector)
        {
            case "up":
                curAngle=tr.localEulerAngles.x;
                break;
            case "down":
                curAngle = tr.localEulerAngles.x;
                break;
            case "left":
                curAngle = tr.localEulerAngles.y;
                break;
            case "right":
                curAngle = tr.localEulerAngles.y;
                break;
        }
       
        while (true)
        {
            if(rotateVector== "up")//
            {
                tr.localEulerAngles = new Vector3(tr.localEulerAngles.x + Time.deltaTime * rotSpeed, tr.localEulerAngles.y, tr.localEulerAngles.z);
                if (FormateAngle(tr.localEulerAngles.x) - curAngle >= angle)
                {
                    tr.localEulerAngles = new Vector3(curAngle+ angle, tr.localEulerAngles.y, tr.localEulerAngles.z);
                    break;
                }
            }
            else if (rotateVector == "down")
            {
                tr.localEulerAngles = new Vector3(tr.localEulerAngles.x - Time.deltaTime * rotSpeed, tr.localEulerAngles.y, tr.localEulerAngles.z);
                if (curAngle- FormateAngle(tr.localEulerAngles.x) >= angle)
                {
                    tr.localEulerAngles = new Vector3(curAngle - angle, tr.localEulerAngles.y, tr.localEulerAngles.z);
                    break;
                }
            }
            else if (rotateVector == "left")//
            {
                tr.localEulerAngles = new Vector3(tr.localEulerAngles.x, tr.localEulerAngles.y + Time.deltaTime * rotSpeed, tr.localEulerAngles.z);
                if (FormateAngle(tr.localEulerAngles.y) - curAngle >= angle)
                {
                    tr.localEulerAngles = new Vector3(tr.localEulerAngles.x, curAngle + angle, tr.localEulerAngles.z);
                    break;
                }
            }
            else if (rotateVector == "right")
            {
                tr.localEulerAngles = new Vector3(tr.localEulerAngles.x, tr.localEulerAngles.y - Time.deltaTime * rotSpeed, tr.localEulerAngles.z);
                if ( curAngle- FormateAngle(tr.localEulerAngles.y) >= angle)
                {
                    tr.localEulerAngles = new Vector3(tr.localEulerAngles.x, curAngle - angle, tr.localEulerAngles.z);
                    break;
                }
            }
      
            yield return null;
        }

    }
    private IEnumerator RearViewMirrorXYZ(Transform tr, float waitime, string rotateVector, float angle, float rotSpeed)
    {
        yield return new WaitForSeconds(waitime);
        float curAngle = 0;
        switch (rotateVector)
        {
            case "x":
                curAngle = tr.localEulerAngles.x;
                break;
            case "y":
                curAngle = tr.localEulerAngles.y;
                break;
            case "z":
                curAngle = tr.localEulerAngles.z;
                break;
        }

        while (true)
        {
            if (rotateVector == "x")//
            {
                if(angle>0)
                {
                    tr.localEulerAngles = new Vector3(tr.localEulerAngles.x + Time.deltaTime * rotSpeed, tr.localEulerAngles.y, tr.localEulerAngles.z);
                    if (curAngle - FormateAngle(tr.localEulerAngles.x) >= angle)
                    {
                        tr.localEulerAngles = new Vector3(curAngle + angle, tr.localEulerAngles.y, tr.localEulerAngles.z);
                        break;
                    }
                }
                else
                {
                    tr.localEulerAngles = new Vector3(tr.localEulerAngles.x - Time.deltaTime * rotSpeed, tr.localEulerAngles.y, tr.localEulerAngles.z);
                    if (curAngle - FormateAngle(tr.localEulerAngles.x) >= -angle)
                    {
                        tr.localEulerAngles = new Vector3(curAngle + angle, tr.localEulerAngles.y, tr.localEulerAngles.z);
                        break;
                    }
                }

            }

            else if (rotateVector == "y")//
            {
                if (angle > 0)
                {
                    tr.localEulerAngles = new Vector3(tr.localEulerAngles.x, tr.localEulerAngles.y + Time.deltaTime * rotSpeed, tr.localEulerAngles.z);
                    if (FormateAngle(tr.localEulerAngles.y) - curAngle >= angle)
                    {
                        tr.localEulerAngles = new Vector3(tr.localEulerAngles.x, curAngle + angle, tr.localEulerAngles.z);
                        break;
                    }
                }
                else
                {
                    tr.localEulerAngles = new Vector3(tr.localEulerAngles.x, tr.localEulerAngles.y - Time.deltaTime * rotSpeed, tr.localEulerAngles.z);
                    if (FormateAngle(tr.localEulerAngles.y) - curAngle >= -angle)
                    {
                        tr.localEulerAngles = new Vector3(tr.localEulerAngles.x, curAngle + angle, tr.localEulerAngles.z);
                        break;
                    }
                }
            }
            else if (rotateVector == "z")
            {
                if (angle > 0)
                {
                    tr.localEulerAngles = new Vector3(tr.localEulerAngles.x, tr.localEulerAngles.y, tr.localEulerAngles.z + Time.deltaTime * rotSpeed);
                    if (FormateAngle(tr.localEulerAngles.z) - curAngle >= angle)
                    {
                        tr.localEulerAngles = new Vector3(tr.localEulerAngles.x, tr.localEulerAngles.y, curAngle + angle);
                        break;
                    }
                }
                else
                {
                    tr.localEulerAngles = new Vector3(tr.localEulerAngles.x, tr.localEulerAngles.y, tr.localEulerAngles.z - Time.deltaTime * rotSpeed);
                    if (FormateAngle(tr.localEulerAngles.z) - curAngle >= -angle)
                    {
                        tr.localEulerAngles = new Vector3(tr.localEulerAngles.x, tr.localEulerAngles.y, curAngle + angle);
                        break;
                    }
                }
            }

            yield return null;
        }

    }
    private float FormateAngle(float angle)
    {
        if(angle>180)
        {
            angle -= 360;
        }
        return angle;
    }
    #endregion

    #region 直线位移

    public void SetLineMove(string modelName, float waitime, string moveVector, float distance,float speed)
    {
        if (modelName.Contains("/"))
        {
            string modelName2 = modelName.Substring(modelName.IndexOf('/') + 1);
            if (objLoadedDic.TryGetValue(modelName.Split('/')[0], out var _gameobject))
            {
                Transform t = _gameobject.transform.Find(modelName2);
                if (t != null)
                {
                    StartCoroutine(LineMovexyz(t, waitime, moveVector, distance, speed));
                }
            }
        }
        else
        {
            if (objLoadedDic.TryGetValue(modelName, out var _gameobject))
            {
                StartCoroutine(LineMovexyz(_gameobject.transform, waitime, moveVector, distance, speed));
            }
        }
    }
    private IEnumerator LineMovexyz(Transform tr, float waitime, string moveVector, float distance, float speed)
    {
        yield return new WaitForSeconds(waitime);

        float curDistance = 0;
        switch (moveVector)
        {
            case "y":
                curDistance = tr.localPosition.y;
                break;
            case "x":
                curDistance = tr.localPosition.x;
                break;
            case "z":
                curDistance = tr.localPosition.z;
                break;
        }

        while (true)
        {
            if (moveVector == "y")//
            {
                if(distance>0)
                {
                    tr.Translate(Vector3.up * Time.deltaTime * speed, Space.Self);
                    if (tr.localPosition.y - curDistance >= distance)
                    {
                        tr.localPosition = new Vector3(tr.localPosition.x, curDistance + distance, tr.localPosition.z);
                        break;
                    }
                }
                else
                {
                    tr.Translate(Vector3.down * Time.deltaTime * speed, Space.Self);
                    if (curDistance - tr.localPosition.y >= -distance)
                    {
                        tr.localPosition = new Vector3(tr.localPosition.x, curDistance + distance, tr.localPosition.z);
                        break;
                    }
                }
   
            }

            else if (moveVector == "x")//
            {
                if (distance < 0)
                {
                    tr.Translate(Vector3.left * Time.deltaTime * speed, Space.Self);
                    if (curDistance - tr.localPosition.x >= -distance)
                    {
                        tr.localPosition = new Vector3(curDistance + distance, tr.localPosition.y, tr.localPosition.z);
                        break;
                    }
                }
                else
                {
                    tr.Translate(Vector3.right * Time.deltaTime * speed, Space.Self);
                    if (tr.localPosition.x - curDistance >= distance)
                    {
                        tr.localPosition = new Vector3(curDistance + distance, tr.localPosition.y, tr.localEulerAngles.z);
                        break;
                    }
                }
            }

            else if (moveVector == "z")
            {
                if (distance > 0)
                {
                    tr.Translate(Vector3.forward * Time.deltaTime * speed, Space.Self);
                    if (tr.localPosition.z - curDistance >= distance)
                    {
                        tr.localPosition = new Vector3(tr.localPosition.x, tr.localPosition.y, curDistance + distance);
                        break;
                    }
                }
                else
                {
                    tr.Translate(Vector3.back * Time.deltaTime * speed, Space.Self);
                    if (curDistance - tr.localPosition.z >= -distance)
                    {
                        tr.localPosition = new Vector3(tr.localPosition.x, tr.localPosition.y, curDistance + distance);
                        break;
                    }
                }
            }
            yield return null;
        }

    }
    private IEnumerator LineMove(Transform tr, float waitime, string rotateVector, float distance, float speed)
    {
        yield return new WaitForSeconds(waitime);

        float curDistance = 0;
        switch (rotateVector)
        {
            case "up":
                curDistance = tr.localPosition.y;
                break;
            case "down":
                curDistance = tr.localPosition.y;
                break;
            case "left":
                curDistance = tr.localPosition.x;
                break;
            case "right":
                curDistance = tr.localPosition.x;
                break;
            case "front":
                curDistance = tr.localPosition.z;
                break;
            case "back":
                curDistance = tr.localPosition.z;
                break;
        }

        while (true)
        {
            if (rotateVector == "up")//
            {
                tr.Translate(Vector3.up * Time.deltaTime * speed, Space.Self);
                if (tr.localPosition.y - curDistance >= distance)
                {
                    tr.localPosition = new Vector3(tr.localPosition.x, curDistance+distance, tr.localPosition.z);
                    break;
                }
            }
            else if (rotateVector == "down")
            {
                tr.Translate(Vector3.down * Time.deltaTime * speed, Space.Self);
                if (curDistance - tr.localPosition.y >= distance)
                {
                    tr.localPosition = new Vector3(tr.localPosition.x, curDistance - distance, tr.localPosition.z);
                    break;
                }
            }
            else if (rotateVector == "left")//
            {
                tr.Translate(Vector3.left * Time.deltaTime * speed, Space.Self);
                if (curDistance- tr.localPosition.x >= distance)
                {
                    tr.localPosition = new Vector3(curDistance - distance, tr.localPosition.y, tr.localPosition.z);
                    break;
                }
            }
            else if (rotateVector == "right")
            {
                tr.Translate(Vector3.right * Time.deltaTime * speed, Space.Self);
                if (tr.localPosition.x- curDistance >= distance)
                {
                    tr.localPosition = new Vector3(curDistance + distance, tr.localPosition.y, tr.localEulerAngles.z);
                    break;
                }
            }
            else if (rotateVector == "front")
            {
                tr.Translate(Vector3.forward * Time.deltaTime * speed, Space.Self);
                if (tr.localPosition.z - curDistance >= distance)
                {
                    tr.localPosition = new Vector3(tr.localPosition.x, tr.localPosition.y, curDistance + distance);
                    break;
                }
            }
            else if (rotateVector == "back")
            {
                tr.Translate(Vector3.back * Time.deltaTime * speed, Space.Self);
                if ( curDistance- tr.localPosition.z >= distance)
                {
                    tr.localPosition = new Vector3(tr.localPosition.x, tr.localPosition.y, curDistance - distance);
                    break;
                }
            }
            yield return null;
        }

    }

    #endregion
    public void CameraBackground(string _color)
    {
        mianCam.backgroundColor = HexToColor( _color);
    }
    /// <summary>
    /// color 转换hex
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public  string ColorToHex(Color color)
    {
        int r = Mathf.RoundToInt(color.r * 255.0f);
        int g = Mathf.RoundToInt(color.g * 255.0f);
        int b = Mathf.RoundToInt(color.b * 255.0f);
        int a = Mathf.RoundToInt(color.a * 255.0f);
        string hex = string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", r, g, b, a);
        return hex;
    }
    /// <summary>
    /// hex转换到color
    /// </summary>
    /// <param name="hex"></param>
    /// <returns></returns>
    public  Color HexToColor(string hex)
    {
        byte br = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte bg = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte bb = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        byte cc = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
        float r = br / 255f;
        float g = bg / 255f;
        float b = bb / 255f;
        float a = cc / 255f;
        return new Color(r, g, b, a);
    }
}
