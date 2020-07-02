using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System.Xml;

public class DataManager : Singleton<DataManager>
{
    #region xml名字
    public const string JIE_NENG = "JieNeng";
    public const string SHU_SHI = "ShuShi";
    public const string YUN_DONG = "YunDong";
    #endregion


    public void LoadSceneXML(string xmlName)
    {
        TextAsset ta = Resources.Load("SceneConfigME5/" + xmlName) as TextAsset;

        if (ta != null)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(FormatXML(ta.text));
            XmlNodeList rootXml = xmlDoc.SelectSingleNode("root").ChildNodes;

            foreach (XmlElement setting in rootXml)
            {
                #region 加载物体
                if (setting.Name == "loadObject")
                {
                    //  Debug.Log(setting.Name);
                    Dictionary<string, string> prefabList = new Dictionary<string, string>();
                    Dictionary<string, string> uiList = new Dictionary<string, string>();
                    foreach (XmlElement obj in setting)
                    {
                        if (obj.Name == "UI")
                        {
                            if (obj.IsEmpty) { return; }
                            foreach (XmlElement uiobj in obj)
                            {
                                if (!string.IsNullOrEmpty(uiobj.InnerText))
                                {
                                    uiList.Add(uiobj.InnerText, uiobj.GetAttribute("showTime") + "_" + uiobj.GetAttribute("hideTime"));
                                }
                            }
                        }
                        else if (obj.Name == "prefab")
                        {
                            if (obj.IsEmpty) { return; }
                            foreach (XmlElement prefabobj in obj)
                            {
                                if (!string.IsNullOrEmpty(prefabobj.InnerText))
                                {
                                    prefabList.Add(prefabobj.InnerText, prefabobj.GetAttribute("showTime") + "_" + prefabobj.GetAttribute("hideTime"));
                                }
                            }
                        }
                    }

                    SceneInitialization.Instance.LoadPrefab(prefabList, uiList);
                }
                #endregion

                #region 场景中物体的显示和隐藏
                if (setting.Name == "sceneObject")
                {
                    //Dictionary<string, string> objList = new Dictionary<string, string>();
                    List<string> objNameList = new List<string>();
                    List<string> objTimeList = new List<string>();
                    foreach (XmlElement sceneObj in setting)
                     {
                         if (!string.IsNullOrEmpty(sceneObj.InnerText))
                         {
                            objNameList.Add(sceneObj.InnerText);
                            objTimeList.Add(sceneObj.GetAttribute("showTime") + "_" + sceneObj.GetAttribute("hideTime"));
                            //objList.Add(sceneObj.InnerText, sceneObj.GetAttribute("showTime") + "_" + sceneObj.GetAttribute("hideTime"));
                         }
                     }

                    SceneInitialization.Instance.SceneObjShowHide(objNameList, objTimeList);
                }
                #endregion

                #region 相机位置
                if (setting.Name == "cameraPosition")
                {
                    // Debug.Log(setting.Name);
                    Vector3 camPos = Vector3.one;
                    string camEngle = null;
                    string targetPos = null;
                    foreach (XmlElement obj in setting)
                    {
                        if (obj.Name == "cameraVector3")
                        {
                            if (!string.IsNullOrEmpty(obj.InnerText))
                            {
                                if (obj.InnerText.Contains(","))
                                {
                                    // Debug.Log( obj.InnerText);
                                    if (obj.InnerText.Split(',').Length == 3)
                                    {
                                        camPos = new Vector3(float.Parse(obj.InnerText.Split(',')[0]), float.Parse(obj.InnerText.Split(',')[1]), float.Parse(obj.InnerText.Split(',')[2]));
                                    }
                                }
                            }

                        }
                        if (obj.Name == "cameraEngle")
                        {
                            if (!string.IsNullOrEmpty(obj.InnerText))
                            {
                                if (obj.InnerText.Contains(","))
                                {
                                    // Debug.Log(obj.Name+" / "+obj.InnerText);
                                    if (obj.InnerText.Split(',').Length == 3)
                                        camEngle = obj.InnerText;
                                }
                            }
                        }
                        if (obj.Name == "targetVector3")
                        {
                            if (!string.IsNullOrEmpty(obj.InnerText))
                            {
                                if (obj.InnerText.Contains(","))
                                {
                                    // Debug.Log(obj.Name+" / "+obj.InnerText);
                                    if (obj.InnerText.Split(',').Length == 3)
                                        targetPos = obj.InnerText;
                                }
                            }
                        }
                    }

                    SceneInitialization.Instance.SetCameraState(camPos, camEngle, targetPos);
                }
                #endregion

                #region 物体的路径动画
                if (setting.Name == "SetPath")
                {
                    foreach (XmlElement obj in setting)
                    {

                        //Debug.Log(setting.Name);
                        string modelName = "";
                        float waitTime = 0;
                        Vector3[] pathInfo = null;
                        float pathTime = 1;
                        string lookType = "ahead";

                        foreach (XmlElement path in obj)
                        {
                            //Debug.Log(path.InnerText);
                            if (path.Name == "modelName")
                            {
                                if (!string.IsNullOrEmpty(path.InnerText))
                                {
                                    modelName = path.InnerText;
                                }

                            }
                            else if (path.Name == "waitTime")
                            {
                                if (!string.IsNullOrEmpty(path.InnerText))
                                {
                                    waitTime = float.Parse(path.InnerText);
                                }

                            }
                            else if (path.Name == "pathInfo")
                            {
                                if (!string.IsNullOrEmpty(path.InnerText))
                                {
                                    if (path.InnerText.Contains(",") && path.InnerText.Contains("_"))
                                    {
                                        for (int i = 0; i < path.InnerText.Split('_').Length; i++)
                                        {
                                            if (path.InnerText.Split('_')[i].Split(',').Length != 3)
                                            {
                                                Debug.Log("路径非法字符");
                                                return;
                                            }
                                        }
                                        pathInfo = new Vector3[path.InnerText.Split('_').Length];
                                        for (int i = 0; i < path.InnerText.Split('_').Length; i++)
                                        {
                                            pathInfo[i] = new Vector3(float.Parse(path.InnerText.Split('_')[i].Split(',')[0]),
                                                float.Parse(path.InnerText.Split('_')[i].Split(',')[1]),
                                                float.Parse(path.InnerText.Split('_')[i].Split(',')[2]));
                                        }

                                    }
                                }

                            }
                            else if (path.Name == "pathTime")
                            {
                                if (!string.IsNullOrEmpty(path.InnerText))
                                {
                                    pathTime = float.Parse(path.InnerText);
                                }

                            }
                            else if (path.Name == "lookType")
                            {
                                if (!string.IsNullOrEmpty(path.InnerText))
                                {
                                    lookType = path.InnerText;
                                }

                            }
                        }
                        if (!string.IsNullOrEmpty(modelName) && pathInfo != null)
                        {
                             SceneInitialization.Instance.ObjectPathState(modelName, pathInfo, pathTime, lookType, waitTime); 
                        }

                    }
                }
                #endregion

                #region UV animation
                if (setting.Name == "UVanimation")
                {
                    foreach (XmlElement obj in setting)
                    {
                        //Debug.Log(setting.Name);
                        string modelName = "";
                        float speedX = 0;
                        float delay = 0;
                        float speedY = 0;

                        foreach (XmlElement ani in obj)
                        {
                           // Debug.Log(ani.InnerText);
                            if (ani.Name == "modelName")
                            {
                                if (!string.IsNullOrEmpty(ani.InnerText))
                                {
                                    modelName = ani.InnerText;
                                }
                            }
                            else if (ani.Name == "speedX")
                            {
                                if (!string.IsNullOrEmpty(ani.InnerText))
                                {
                                    speedX = float.Parse(ani.InnerText);
                                }
                            }
                            else if (ani.Name == "speedY")
                            {
                                if (!string.IsNullOrEmpty(ani.InnerText))
                                {
                                    speedY = float.Parse(ani.InnerText);
                                }
                            }
                            else if (ani.Name == "delay")
                            {
                                if (!string.IsNullOrEmpty(ani.InnerText))
                                {
                                    delay = float.Parse(ani.InnerText);
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(modelName))
                        {SceneInitialization.Instance.SetUVanimation(modelName, delay, speedX, speedY); }
                    }
                }
                #endregion

                #region model Animation
                if (setting.Name == "modelAnimation")
                {
                    foreach (XmlElement obj in setting)
                    {

                       // Debug.Log(setting.Name);
                        string modelName = "";
                        int animationStateNumber = 0;
                        float waitTime = 0;

                        foreach (XmlElement ani in obj)
                        {
                           // Debug.Log(ani.InnerText);
                            if (ani.Name == "modelName")
                            {
                                if (!string.IsNullOrEmpty(ani.InnerText))
                                {
                                    modelName = ani.InnerText;
                                }
                            }
                            else if (ani.Name == "animationStateNumber")
                            {
                                if (!string.IsNullOrEmpty(ani.InnerText))
                                {
                                    animationStateNumber = int.Parse(ani.InnerText);
                                }
                            }
                            else if (ani.Name == "waitTime")
                            {
                                if (!string.IsNullOrEmpty(ani.InnerText))
                                {
                                    waitTime = float.Parse(ani.InnerText);
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(modelName))
                        {
                            SceneInitialization.Instance.SetModelAnimation(modelName, animationStateNumber, waitTime); 
                        }

                    }
                }
                #endregion

                #region UI animation
                if (setting.Name == "UIanimation")
                {
                    foreach (XmlElement obj in setting)
                    {

                        //Debug.Log(setting.Name);
                        string name = "";
                        float waitTime = 0;

                        foreach (XmlElement ani in obj)
                        {
                            //Debug.Log(ani.InnerText);
                            if (ani.Name == "name")
                            {
                                if (!string.IsNullOrEmpty(ani.InnerText))
                                {
                                    name = ani.InnerText;
                                }
                            }
                            else if (ani.Name == "waitTime")
                            {
                                if (!string.IsNullOrEmpty(ani.InnerText))
                                {
                                    waitTime = float.Parse(ani.InnerText);
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(name))
                        {
                             SceneInitialization.Instance.SetUIAni(name, waitTime); 
                        }
                    }
                }
                #endregion

                #region 灯光的开关
                if (setting.Name == "lightControl")
                {
                    foreach (XmlElement obj in setting)
                    {

                        //Debug.Log(setting.Name);
                        string name = "";
                        string waitTime = "";
                        string state = "";
                        foreach (XmlElement ani in obj)
                        {
                            //Debug.Log(ani.InnerText);
                            if (ani.Name == "name")
                            {
                                if (!string.IsNullOrEmpty(ani.InnerText))
                                {
                                    name = ani.InnerText;
                                }
                            }
                            else if (ani.Name == "waitTime")
                            {
                                if (!string.IsNullOrEmpty(ani.InnerText))
                                {
                                    waitTime = ani.InnerText;
                                }
                            }
                            else if (ani.Name == "state")
                            {
                                if (!string.IsNullOrEmpty(ani.InnerText))
                                {
                                    state = ani.InnerText;
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(name))
                        {
                            SceneInitialization.Instance.SetLightState(name,float.Parse( waitTime), float.Parse(state));
                        }
                    }
                }
                #endregion

                #region 车速

                if (setting.Name == "carSpeed")
                {
                    foreach (XmlElement speed in setting)
                    {
                        SceneInitialization.Instance.SetCarSpeed(speed.InnerText, speed.GetAttribute("waitTime"));
                    }
                }
                #endregion

                #region 更改材质

                if (setting.Name == "changeMaterial")
                {
                    foreach (XmlElement modelName in setting)
                    {
                        SceneInitialization.Instance.SetMaterial(modelName.InnerText,float.Parse( modelName.GetAttribute("waitTime")), modelName.GetAttribute("materialName"));
                    }
                }
                #endregion

                #region 颜色渐变

                if (setting.Name == "lerpColor")
                {
                    foreach (XmlElement modelName in setting)
                    {
                        if (!string.IsNullOrEmpty(modelName.GetAttribute("colorA")) && !string.IsNullOrEmpty(modelName.GetAttribute("colorB"))&& !string.IsNullOrEmpty(modelName.InnerText))
                        {
                            //byte rfrom = byte.Parse(modelName.GetAttribute("colorA").Split(',')[0]);
                            //byte gfrom = byte.Parse(modelName.GetAttribute("colorA").Split(',')[1]);
                            //byte bfrom = byte.Parse(modelName.GetAttribute("colorA").Split(',')[2]);
                            //byte afrom = byte.Parse(modelName.GetAttribute("colorA").Split(',')[3]);

                            //byte rto = byte.Parse(modelName.GetAttribute("colorB").Split(',')[0]);
                            //byte gto = byte.Parse(modelName.GetAttribute("colorB").Split(',')[1]);
                            //byte bto = byte.Parse(modelName.GetAttribute("colorB").Split(',')[2]);
                            //byte ato = byte.Parse(modelName.GetAttribute("colorB").Split(',')[3]);

                            SceneInitialization.Instance.SetTweenColor(modelName.InnerText,
                                float.Parse(modelName.GetAttribute("waitTime")),
                                float.Parse(modelName.GetAttribute("lerpTime")),
                                modelName.GetAttribute("colorA"), modelName.GetAttribute("colorB")
                                );
                        }
                    }
                }
                #endregion

                #region 文案

                if (setting.Name == "textWord")
                {
                    if(!string.IsNullOrEmpty(setting.InnerText))
                  SceneInitialization.Instance.SetUIText(setting.InnerText);
                }
                #endregion

                #region 拖尾

                if (setting.Name == "setTrail")
                {
                    foreach (XmlElement trailObject in setting)
                    {
                        if (!string.IsNullOrEmpty(trailObject.InnerText))
                        {
                            SceneInitialization.Instance.SetTrail(trailObject.InnerText,float.Parse( trailObject.GetAttribute("waitTime")));
                        }
                    }
                }
                #endregion

                #region 相机背景色

                if (setting.Name == "cameraBackgroundColor")
                {
                    //byte rto = byte.Parse(setting.InnerText.Split(',')[0]);
                    //byte gto = byte.Parse(setting.InnerText.Split(',')[1]);
                    //byte bto = byte.Parse(setting.InnerText.Split(',')[2]);
                    //byte ato = byte.Parse(setting.InnerText.Split(',')[3]);
                    if (!string.IsNullOrEmpty(setting.InnerText))
                    {
                        SceneInitialization.Instance.CameraBackground(setting.InnerText);
                    }
                }
                #endregion

                #region 后视镜翻转

                if (setting.Name == "rearViewMirror")
                {
                    foreach (XmlElement item in setting)
                    {
                        string modelName = "";
                        float waitTime = 0;
                        string rotateVector = "";
                        float angle = 0;
                        float speed = 0;
                        foreach (XmlElement mirrorData in item)
                        {
                            if (!string.IsNullOrEmpty(mirrorData.InnerText))
                            {
                                if (mirrorData.Name == "modelName")
                                {
                                    modelName = mirrorData.InnerText;
                                }
                                else if (mirrorData.Name == "waitTime")
                                {
                                    waitTime = float.Parse(mirrorData.InnerText);
                                }
                                else if (mirrorData.Name == "rotateVector")
                                {
                                    rotateVector = mirrorData.InnerText;
                                }
                                else if (mirrorData.Name == "angel")
                                {
                                    angle = float.Parse(mirrorData.InnerText);
                                }
                                else if (mirrorData.Name == "speed")
                                {
                                    speed = float.Parse(mirrorData.InnerText);
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(modelName) && !string.IsNullOrEmpty(rotateVector) && angle != 0)
                        {
                            SceneInitialization.Instance.SetRearViewMirror(modelName, waitTime, rotateVector, angle, speed);
                        }
                    }
                }
                #endregion

                #region 直线位移

                if (setting.Name == "linerMove")
                {
                    foreach (XmlElement item in setting)
                    {
                        string modelName = "";
                        float waitTime = 0;
                        string rotateVector = "";
                        float distance = 0;
                        float speed = 0;
                        foreach (XmlElement mirrorData in item)
                        {
                            if (!string.IsNullOrEmpty(mirrorData.InnerText))
                            {
                                if (mirrorData.Name == "modelName")
                                {
                                    modelName = mirrorData.InnerText;
                                }
                                else if (mirrorData.Name == "waitTime")
                                {
                                    waitTime = float.Parse(mirrorData.InnerText);
                                }
                                else if (mirrorData.Name == "rotateVector")
                                {
                                    rotateVector = mirrorData.InnerText;
                                }
                                else if (mirrorData.Name == "distance")
                                {
                                    distance = float.Parse(mirrorData.InnerText);
                                }
                                else if (mirrorData.Name == "speed")
                                {
                                    speed = float.Parse(mirrorData.InnerText);
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(modelName) && !string.IsNullOrEmpty(rotateVector) && distance != 0)
                        {
                            SceneInitialization.Instance.SetLineMove(modelName, waitTime, rotateVector, distance, speed);
                        }
                    }
                }
                #endregion

                #region 车辆个性化功能开关

                if (setting.Name == "CarControlTrigger")
                {
                    foreach (XmlElement item in setting)
                    {
                        if (item.Name == "radar")
                        {
                            if ("1" == item.InnerText)
                            {
                                AppDelegate.Instance.OnRadarOpenCloseEvent(true);
                            }
                            else
                            {
                                AppDelegate.Instance.OnRadarOpenCloseEvent(false);
                            }
                        }
                        else if (item.Name == "frontLightfarNearAutoControl")
                        {
                            if ("1" == item.InnerText)
                            {
                                AppDelegate.Instance.OnFrontLightfarNearAutoControlDeleEvent(true);
                            }
                            else
                            {
                                AppDelegate.Instance.OnFrontLightfarNearAutoControlDeleEvent(false);

                            }
                        }
                        else if (item.Name == "frontLightrotateByRoad")
                        {
                            if ("1" == item.InnerText)
                            {
                                AppDelegate.Instance.OnFrontLightrotateByRoadEvent(true);
                            }
                            else
                            {
                                AppDelegate.Instance.OnFrontLightrotateByRoadEvent(false);
                            }
                        }
                        else if (item.Name == "wheelLRrotate")
                        {
                            if ("1" == item.InnerText)
                            {
                                
                            }
                            else
                            {
                               
                            }
                        }
                        else if (item.Name == "rearMirrorTrueReflect")
                        {
                            if ("1" == item.InnerText)
                            {
                               
                            }
                            else
                            {
                               
                            }
                        }


                    }
                }
                #endregion
            }

        }
    }


    private string FormatXML(string xml)
    {
        if (xml.Contains("，"))
        {
            xml = xml.Replace('，', ',');
        }

        return xml;
    }
    /// <summary>
    /// 数字有空格也可以格式化
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    private string RemoveContentSpace(string content)
    {
        if (content.Contains(" "))
        {
            content = content.Replace(" ", "");
        }
        return content;
    }
    public Dictionary<string, string> carSettingDic = new Dictionary<string, string>();
    public void XmlRead()
    {
        //TextAsset ta = Resources.Load("VehicleControlInformation") as TextAsset;

        //if (ta!=null)
        //{
        //    XmlDocument xmlDoc = new XmlDocument();
        //    xmlDoc.LoadXml(ta.text);
        //    XmlNodeList rootXml = xmlDoc.SelectSingleNode("root").ChildNodes;
        //    foreach (XmlElement setting in rootXml)
        //    {
        //       // Debug.Log("type: " + setting.GetAttribute("type")+" _ "+setting.Name + "  :  " + setting.InnerText);
        //        carSettingDic.Add(setting.Name , setting.InnerText);
        //    }

        //}
    }
}