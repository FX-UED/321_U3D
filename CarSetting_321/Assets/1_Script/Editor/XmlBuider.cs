using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Xml;
public class XmlBuider : EditorWindow
{
    //xml的名字
    string xmlName,xmlName_cn = "";
    #region 添加预制体
    static GameObject[] prefabObjList;
    static GameObject[] prefabUIList;
    static int objcreateCount=0;
    static int UIcreateCount=0;

    float[] showTime;
    float[] hideTime;

    float[] showTime_UI;
    float[] hideTime_UI;
    #endregion

    #region 场景中的物体显示隐藏
    static string[] sceneObjList;
    static int sceneObjCount = 0;
    float[] sceneObj_showTime;
    float[] sceneObj_hideTime;

    #endregion


    #region 相机位置
    static Vector3 m_cameraVector3;
    static Vector3 m_cameraEngle;
    static Vector3 m_targetVector3=new Vector3(0,0,0);

    #endregion
    #region SetPath
    static int setPathCount;
    static GameObject[] SetPath_modelName;
    static float[]      SetPath_waitTime;
    static string[]     SetPath_pathInfo;
    static float[]      SetPath_pathTime;
    static string[]     SetPath_lookType;

    #endregion
    #region UVanimation
    static int UVanimationCount;
    static GameObject[] UVanimation_modelName;
    static float[] UVanimation_speedX;
    static float[] UVanimation_speedY;
    static float[] UVanimation_delay;

    #endregion
    #region modelAnimation
    static int modelAnimationCount;
    static GameObject[] modelAnimation_modelName;
    static int[]        modelAnimation_animationStateNumber;
    static float[]      modelAnimation_waitTime;

    #endregion
    #region 灯光的开关
    static int lightControlCount;
    static GameObject[] lightControl_name;
    static int[] lightControl_state;
    static float[] lightControl_waitTime;
    #endregion
    #region 车辆行驶
    static int carSpeedCount;
    static int[] carSpeed_state;
    static float[] carSpeed_waitTime;
    #endregion
    #region 更改材质
    static int changeMaterialCount;
    static string[] changeMaterial_modelName;
    static string[] changeMaterial_materialName;
    static float[] changeMaterial_waitTime;
    #endregion
    #region 物体变色
    static int lerpColorCount;
    static string[] lerpColor_modelName;
    static float[] lerpColor_waitTime;
    static float[] lerpColor_lerpTime;
    static Color[] lerpColor_colorA;
    static Color[] lerpColor_colorB;
    #endregion
    #region 车灯控制
    static string positionLight;
    static string brakeLight;
    static string logoLight;
    static string turnLeftLight;
    static string turnRightLight;


    static string farLight;
    static string frontDecorateLight;
    static string lowBeamLight;
    static string highBeamLight;

    static string positionLight_waitTime;
    static string brakeLight_waitTime;
    static string logoLight_waitTime;
    static string turnLeftLight_waitTime;
    static string turnRightLight_waitTime;

    static string farLight_waitTime;
    static string frontDecorateLight_waitTime;
    static string lowBeamLight_waitTime;
    static string highBeamLight_waitTime;

    #endregion
    #region 拖尾的效果开关
    static int setTrailCount;
    static string[] trailObject;
    static float[] trail_waitTime;
    #endregion
    #region 屏幕背景色
    static Color cameraBackgroundColor;
    #endregion
    #region 车外后视镜翻转
    static int rearViewMirrorCount;
    static string[] rearViewMirror_modelName;
    static string[] rearViewMirror_rotateVector;
    static float[] rearViewMirror_speed;
    static float[] rearViewMirror_angel;
    static float[] rearViewMirror_waitTime;
    #endregion
    #region 直线位移
    static int linerMoveCount;
    static string[] linerMove_modelName;
    static string[] linerMove_rotateVector;
    static float[] linerMove_speed;
    static float[] linerMove_distance;
    static float[] linerMove_waitTime;
    #endregion
    #region 车辆功能开关，1是开，0是关
    static int radar;
    static int frontLightfarNearAutoControl;
    static int frontLightrotateByRoad;
    static int wheelLRrotate;
    static int rearMirrorTrueReflect;
    #endregion
    string screenLog;//屏幕打印出的文字
    Vector2 scrollPos;
    void OnGUI()
    {
        GUILayout.BeginVertical();
        scrollPos =EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(600), GUILayout.Height(700));
        #region 绘制标题

        GUILayout.Space(10);
        GUI.skin.label.fontSize = 15;
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label("ME5车控配置编辑器");

        #endregion
        #region 绘制文本-xml名字
        GUILayout.Space(10);
        xmlName = EditorGUILayout.TextField("请输入xml的名字（英文）", xmlName);
        xmlName_cn = EditorGUILayout.TextField("请输入xml的名字（中文）", xmlName_cn);
        #endregion
        # region 添加物体
        GUILayout.Space(10);
        GUILayout.Label("添加3D物体");
        objcreateCount = EditorGUILayout.IntField("请输入 非UI预制体 的数量", objcreateCount);
        if (GUILayout.Button("ok"))
        {
            SetListObj();
        }
        if (prefabObjList != null)
        {
            if (prefabObjList.Length == objcreateCount)
            {
                for (int i = 0; i < objcreateCount; i++)
                {
                    GUILayout.Space(5);
                    prefabObjList[i] = (GameObject)EditorGUILayout.ObjectField("prefab obj",
                        prefabObjList[i], typeof(GameObject), true);
                    showTime[i] = EditorGUILayout.FloatField("第几秒开始出现：", showTime[i]);
                    hideTime[i] = EditorGUILayout.FloatField("第几秒开始消失：", hideTime[i]);
                }
            }
        }
        GUILayout.Space(10);
        GUILayout.Label("添加UI");
        UIcreateCount = EditorGUILayout.IntField("请输入 UI预制体 的数量", UIcreateCount);
        if (GUILayout.Button("ok"))
        {
            SetListUI();
        }
        if (prefabUIList != null)
        {
            if (prefabUIList.Length == UIcreateCount)
            {
                for (int i = 0; i < UIcreateCount; i++)
                {
                    GUILayout.Space(5);
                    prefabUIList[i] = (GameObject)EditorGUILayout.ObjectField("prefab UI", 
                        prefabUIList[i], typeof(GameObject), true);
                    showTime_UI[i] = EditorGUILayout.FloatField("第几秒开始出现：", showTime_UI[i]);
                    hideTime_UI[i] = EditorGUILayout.FloatField("第几秒开始消失：", hideTime_UI[i]);
                }
            }
        }
        #endregion

        #region 场景中物体的显示和隐藏
        GUILayout.Space(10);
        GUILayout.Label("场景中物体显示和隐藏");
        sceneObjCount = EditorGUILayout.IntField("请输入控制的数量", sceneObjCount);
        if (GUILayout.Button("ok"))
        {
            SetListObjInScene();
        }
        if (sceneObjList != null)
        {
            if (sceneObjList.Length == sceneObjCount)
            {
                for (int i = 0; i < sceneObjCount; i++)
                {
                    GUILayout.Space(5);
                    sceneObjList[i] = EditorGUILayout.TextField("物体节点路径",sceneObjList[i]);
                    sceneObj_showTime[i] = EditorGUILayout.FloatField("第几秒开始出现：", sceneObj_showTime[i]);
                    sceneObj_hideTime[i] = EditorGUILayout.FloatField("第几秒开始消失：", sceneObj_hideTime[i]);
                }
            }
        }
       
        #endregion

        #region 相机位置
        GUILayout.Space(10);
        GUILayout.Label("相机");
        m_cameraVector3 = EditorGUILayout.Vector3Field("请输入相机的位置坐标", m_cameraVector3);
        m_cameraEngle = EditorGUILayout.Vector3Field("请输入相机的旋转坐标", m_cameraEngle);
        m_targetVector3 = EditorGUILayout.Vector3Field("请输入相机注视目标点坐标", m_targetVector3);
        #endregion
        #region     //路径
        GUILayout.Space(10);
        GUILayout.Label("路径");
        setPathCount = EditorGUILayout.IntField("请输入 路径物体 的数量", setPathCount);
        if (GUILayout.Button("ok"))
        {
            SetListSetPath();
        }
        if (SetPath_modelName != null)
        {
            if (SetPath_modelName.Length == setPathCount)
            {
                for (int i = 0; i < setPathCount; i++)
                {
                    GUILayout.Space(5);
                    SetPath_modelName[i] = (GameObject)EditorGUILayout.ObjectField("prefab obj",
                        SetPath_modelName[i], typeof(GameObject), true);
                    SetPath_waitTime[i] = EditorGUILayout.FloatField("等待的时间：", SetPath_waitTime[i]);
                    SetPath_pathTime[i] = EditorGUILayout.FloatField("所用的时间：", SetPath_pathTime[i]);
                    SetPath_pathInfo[i] = EditorGUILayout.TextField("路径经过坐标0,0,0_1,1,1_2,2,2", SetPath_pathInfo[i]);
                    SetPath_lookType[i] = EditorGUILayout.TextField("向前看ahead,固定的目标x,y,z", SetPath_lookType[i]);
                }
            }
        }
        #endregion
        #region   //UV动画
        GUILayout.Space(10);
        GUILayout.Label("UV动画");
        UVanimationCount = EditorGUILayout.IntField("请输入UV动画物体 的数量", UVanimationCount);
        if (GUILayout.Button("ok"))
        {
            SetListUVanimation();
        }
        if (UVanimation_modelName != null)
        {
            if (UVanimation_modelName.Length == UVanimationCount)
            {
                for (int i = 0; i < UVanimationCount; i++)
                {
                    GUILayout.Space(5);
                    UVanimation_modelName[i] = (GameObject)EditorGUILayout.ObjectField("prefab obj",
                        UVanimation_modelName[i], typeof(GameObject), true);
                    UVanimation_speedX[i] = EditorGUILayout.FloatField("x轴速度：", UVanimation_speedX[i]);
                    UVanimation_speedY[i] = EditorGUILayout.FloatField("y轴速度：", UVanimation_speedY[i]);
                    UVanimation_delay[i] = EditorGUILayout.FloatField("等待时间：", UVanimation_delay[i]);
                }
            }
        }
        #endregion
        #region  //模型动画
        GUILayout.Space(10);
        GUILayout.Label("模型动画");
        modelAnimationCount = EditorGUILayout.IntField("请输入模型动画的数量", modelAnimationCount);
        if (GUILayout.Button("ok"))
        {
            SetListmodelAnimation();
        }
        if (modelAnimation_modelName != null)
        {
            if (modelAnimation_modelName.Length == modelAnimationCount)
            {
                for (int i = 0; i < modelAnimationCount; i++)
                {
                    GUILayout.Space(5);
                    modelAnimation_modelName[i] = (GameObject)EditorGUILayout.ObjectField("prefab obj",
                        modelAnimation_modelName[i], typeof(GameObject), true);
                    modelAnimation_animationStateNumber[i] = EditorGUILayout.IntField("动画状态数值：", modelAnimation_animationStateNumber[i]);
                    modelAnimation_waitTime[i] = EditorGUILayout.FloatField("等待时间：", modelAnimation_waitTime[i]);
                }
            }
        }
        #endregion
        #region  //灯光的开关
        GUILayout.Space(10);
        GUILayout.Label("灯光的开关");
        lightControlCount = EditorGUILayout.IntField("灯光开关的数量", lightControlCount);
        if (GUILayout.Button("ok")) {SetListlightControl();}
        if (lightControl_name != null)
        {
            if (lightControl_name.Length == lightControlCount)
            {
                for (int i = 0; i < lightControlCount; i++)
                {
                    GUILayout.Space(5);
                    lightControl_name[i] = (GameObject)EditorGUILayout.ObjectField("灯光物体（不可以重名）",
                        lightControl_name[i], typeof(GameObject), true);
                    lightControl_state[i] = EditorGUILayout.IntField("状态数值（开1、关0）：", lightControl_state[i]);
                    lightControl_waitTime[i] = EditorGUILayout.FloatField("等待时间：", lightControl_waitTime[i]);
                }
            }
        }
        #endregion
        #region  //车辆行驶
        GUILayout.Space(10);
        GUILayout.Label("车速");
        carSpeedCount = EditorGUILayout.IntField("车速变化的数量", carSpeedCount);
        if (GUILayout.Button("ok")) { SetListcarSpeed(); }
        if (carSpeed_state != null)
        {
            if (carSpeed_state.Length == carSpeedCount)
            {
                for (int i = 0; i < carSpeedCount; i++)
                {
                    GUILayout.Space(5);
                    carSpeed_state[i] = EditorGUILayout.IntField("车速数值：", carSpeed_state[i]);
                    carSpeed_waitTime[i] = EditorGUILayout.FloatField("等待时间：", carSpeed_waitTime[i]);
                }
            }
        }
        #endregion
        #region  //更改材质
        GUILayout.Space(10);
        GUILayout.Label("更改材质");
        changeMaterialCount = EditorGUILayout.IntField("更改材质的数量", changeMaterialCount);
        if (GUILayout.Button("ok")) { SetListchangeMaterial(); }
        if (changeMaterial_modelName != null)
        {
            if (changeMaterial_modelName.Length == changeMaterialCount)
            {
                for (int i = 0; i < changeMaterialCount; i++)
                {
                    GUILayout.Space(5);
                    changeMaterial_modelName[i] = EditorGUILayout.TextField("物体相对于顶部父级的路径",
                changeMaterial_modelName[i]);
                    changeMaterial_materialName[i] = EditorGUILayout.TextField("材质名字",
               changeMaterial_materialName[i]);
                    changeMaterial_waitTime[i] = EditorGUILayout.FloatField("等待时间：", changeMaterial_waitTime[i]);
                }
            }
        }
        #endregion
        #region  //物体变色
        GUILayout.Space(10);
        GUILayout.Label("物体变色");
        lerpColorCount = EditorGUILayout.IntField("物体变色的数量", lerpColorCount);
        if (GUILayout.Button("ok")) { SetListlerpColor(); }
        if (lerpColor_modelName != null)
        {
            if (lerpColor_modelName.Length == lerpColorCount)
            {
                for (int i = 0; i < lerpColorCount; i++)
                {
                    GUILayout.Space(5);
                    lerpColor_modelName[i] = EditorGUILayout.TextField("物体相对于顶部父级的路径",
                lerpColor_modelName[i]);

                    lerpColor_colorA[i] = (Color)EditorGUILayout.ColorField("起始颜色",lerpColor_colorA[i]);
                    lerpColor_colorB[i] = (Color)EditorGUILayout.ColorField("起始颜色", lerpColor_colorB[i]);

                    lerpColor_waitTime[i] = EditorGUILayout.FloatField("等待时间：", lerpColor_waitTime[i]);
                    lerpColor_lerpTime[i] = EditorGUILayout.FloatField("变色时间：", lerpColor_lerpTime[i]);
                }
            }
        }
        #endregion
        #region  //车灯控制
        //GUILayout.Space(10);
        //GUILayout.Label("车灯控制");
        //frontDecorateLight = EditorGUILayout.TextField("前大灯（开1关0）", frontDecorateLight);
        //frontDecorateLight_waitTime = EditorGUILayout.TextField("前大灯 开启时间（s）", frontDecorateLight_waitTime);
        //GUILayout.Space(5);
        //brakeLight = EditorGUILayout.TextField("刹车灯（开1关0）", brakeLight);
        //brakeLight_waitTime = EditorGUILayout.TextField("刹车灯 开启时间（s）", brakeLight_waitTime);
        //GUILayout.Space(5);
        //logoLight = EditorGUILayout.TextField("logo灯（开1关0）", logoLight);
        //logoLight_waitTime = EditorGUILayout.TextField("logo灯 开启时间（s）", logoLight_waitTime);
        //GUILayout.Space(5);
        //farLight = EditorGUILayout.TextField("远光（开1关0）", farLight);
        //farLight_waitTime = EditorGUILayout.TextField("远光 开启时间（s）", farLight_waitTime);
        //GUILayout.Space(5);
        //positionLight = EditorGUILayout.TextField("示廓灯（开1关0）", positionLight);
        //positionLight_waitTime = EditorGUILayout.TextField("示廓灯 开启时间（s）", positionLight_waitTime);
        //GUILayout.Space(5);
        #endregion
        #region  //拖尾的效果开关
        GUILayout.Space(10);
        GUILayout.Label("拖尾的效果开关");
        setTrailCount = EditorGUILayout.IntField("拖尾的数量", setTrailCount);
        if (GUILayout.Button("ok")) { SetListTrail(); }
        if (trailObject != null)
        {
            if (trailObject.Length == setTrailCount)
            {
                for (int i = 0; i < setTrailCount; i++)
                {
                    GUILayout.Space(5);
                    trailObject[i] = EditorGUILayout.TextField("物体相对于顶部父级的路径",trailObject[i]);
                    trail_waitTime[i] = EditorGUILayout.FloatField("等待时间：", trail_waitTime[i]);
                }
            }
        }
        #endregion
        #region  //屏幕背景色
        GUILayout.Space(10);
        GUILayout.Label("屏幕背景色");
        cameraBackgroundColor = EditorGUILayout.ColorField("屏幕背景颜色", cameraBackgroundColor);
        #endregion
        #region  //车外后视镜翻转
        GUILayout.Space(10);
        GUILayout.Label("物体的旋转");
        rearViewMirrorCount = EditorGUILayout.IntField("物体的数量", rearViewMirrorCount);
        if (GUILayout.Button("ok")) { SetListrearViewMirror(); }
        if (rearViewMirror_modelName != null)
        {
            if (rearViewMirror_modelName.Length == rearViewMirrorCount)
            {
                for (int i = 0; i < rearViewMirrorCount; i++)
                {
                    GUILayout.Space(5);
                    rearViewMirror_modelName[i] = EditorGUILayout.TextField("物体相对于顶部父级的路径",
                rearViewMirror_modelName[i]);
                    rearViewMirror_rotateVector[i] = EditorGUILayout.TextField("旋转轴x/y/z",
            rearViewMirror_rotateVector[i]);

                    rearViewMirror_speed[i] = EditorGUILayout.FloatField("速度：", rearViewMirror_speed[i]);
                    rearViewMirror_angel[i] = EditorGUILayout.FloatField("角度：", rearViewMirror_angel[i]);
                    rearViewMirror_waitTime[i] = EditorGUILayout.FloatField("等待时间：", rearViewMirror_waitTime[i]);
                }
            }
        }
        #endregion
        #region  //直线位移
        GUILayout.Space(10);
        GUILayout.Label("直线位移");
        linerMoveCount = EditorGUILayout.IntField("直线位移", linerMoveCount);
        if (GUILayout.Button("ok")) { SetListlinerMove(); }
        if (linerMove_modelName != null)
        {
            if (linerMove_modelName.Length == linerMoveCount)
            {
                for (int i = 0; i < linerMoveCount; i++)
                {
                    GUILayout.Space(5);
                    linerMove_modelName[i] = EditorGUILayout.TextField("物体相对于顶部父级的路径",
                linerMove_modelName[i]);
                    linerMove_rotateVector[i] = EditorGUILayout.TextField("方向：x/y/z",
            linerMove_rotateVector[i]);

                    linerMove_speed[i] = EditorGUILayout.FloatField("速度：", linerMove_speed[i]);
                    linerMove_distance[i] = EditorGUILayout.FloatField("距离：", linerMove_distance[i]);
                    linerMove_waitTime[i] = EditorGUILayout.FloatField("等待时间：", linerMove_waitTime[i]);
                }
            }
        }
        #endregion
        #region  //车辆功能开关，1是开，0是关
        GUILayout.Space(10);
        GUILayout.Label("车辆功能开关，开1，关0");
        radar = EditorGUILayout.IntField("雷达", radar); GUILayout.Space(5);
        //frontLightfarNearAutoControl = EditorGUILayout.IntField("自动远近光", frontLightfarNearAutoControl); GUILayout.Space(5);
        frontLightrotateByRoad = EditorGUILayout.IntField("大灯随动转向", frontLightrotateByRoad); GUILayout.Space(5);
        //wheelLRrotate = EditorGUILayout.IntField("车轮左右转向", wheelLRrotate); GUILayout.Space(5);
        //rearMirrorTrueReflect = EditorGUILayout.IntField("后视镜真实反射", rearMirrorTrueReflect);

        #endregion
        #region 屏幕打印出的文字
        GUILayout.Space(10);
        GUILayout.Label("屏幕打印出的文字");
        screenLog = EditorGUILayout.TextField("物体相对于顶部父级的路径", screenLog);
        #endregion


        EditorGUILayout.Space();
        //添加名为"Save Bug"按钮，用于调用SaveBug()函数
        if (  GUILayout.Button( "Save xml",GUILayout.Height(30) )  ){ SaveXml();}
        //if (GUILayout.Button("test")) { LoadXmlToEditor(""); }
        GUILayout.EndScrollView();
        GUILayout.EndVertical();
    }
    #region 设置数组
    public void SetListlinerMove()//直线位移
    {
        linerMove_modelName = new string[linerMoveCount];
        linerMove_rotateVector = new string[linerMoveCount];
        linerMove_speed = new float[linerMoveCount];
        linerMove_distance = new float[linerMoveCount];
        linerMove_waitTime = new float[linerMoveCount];
    }
    public void SetListTrail()//拖尾的效果开关
    {
        trailObject = new string[setTrailCount];
        trail_waitTime = new float[setTrailCount];
    }
    public void SetListrearViewMirror()//车外后视镜翻转
    {
        rearViewMirror_modelName = new string[rearViewMirrorCount];
        rearViewMirror_rotateVector = new string[rearViewMirrorCount];
        rearViewMirror_speed = new float[rearViewMirrorCount];
        rearViewMirror_angel = new float[rearViewMirrorCount];
        rearViewMirror_waitTime = new float[rearViewMirrorCount];

    }
    public void SetListlerpColor()//物体变色
    {
        lerpColor_modelName = new string[lerpColorCount];
        lerpColor_waitTime = new float[lerpColorCount];
        lerpColor_lerpTime = new float[lerpColorCount];
        lerpColor_colorA = new Color[lerpColorCount];
        lerpColor_colorB = new Color[lerpColorCount];
    }
    public void SetListchangeMaterial()
    {
        changeMaterial_modelName = new string[changeMaterialCount];
        changeMaterial_materialName = new string[changeMaterialCount];
        changeMaterial_waitTime = new float[changeMaterialCount];
    }
    public void SetListcarSpeed()
    {
        carSpeed_state = new int[carSpeedCount];
        carSpeed_waitTime = new float[carSpeedCount];
    }
    public void SetListlightControl()
    {
        lightControl_name = new GameObject[lightControlCount];
        lightControl_state = new int[lightControlCount];
        lightControl_waitTime = new float[lightControlCount];
    }
    public void SetListmodelAnimation()
    {
        modelAnimation_modelName = new GameObject[modelAnimationCount];
        modelAnimation_animationStateNumber = new int[modelAnimationCount];
        modelAnimation_waitTime = new float[modelAnimationCount];
    }
    public void SetListUVanimation()
    {
        UVanimation_modelName = new GameObject[UVanimationCount];
        UVanimation_speedX = new float[UVanimationCount];
        UVanimation_speedY = new float[UVanimationCount];
        UVanimation_delay = new float[UVanimationCount];

    }
    public void SetListSetPath()
    {
        SetPath_modelName = new GameObject[setPathCount];
        SetPath_waitTime = new float[setPathCount];
        SetPath_pathInfo = new string[setPathCount];
        SetPath_pathTime = new float[setPathCount];
        SetPath_lookType = new string[setPathCount];

    }
    public void SetListObj()
    {
        prefabObjList = new GameObject[objcreateCount];
        showTime = new float[objcreateCount];
        hideTime = new float[objcreateCount];
    }
    public void SetListObjInScene()
    {
        sceneObjList = new string[sceneObjCount];
        sceneObj_showTime = new float[sceneObjCount];
        sceneObj_hideTime = new float[sceneObjCount];
    }
    public void SetListUI()
    {
        prefabUIList = new GameObject[UIcreateCount];
        showTime_UI = new float[UIcreateCount];
        hideTime_UI = new float[UIcreateCount];
    }

    #endregion
    //用于保存当前信息
    void SaveXml()
    {
        if (!canSaveXml()) { Debug.LogError("保存失败！请检查 "); return; }
        string path = Application.dataPath+ "/Resources/SceneConfigME5/" + xmlName + ".xml";
        if (Directory.Exists(Application.dataPath + "/Resources/SceneConfigME5/" + xmlName)){ Debug.LogError("路径中已存在同名文件，请删除后再试！");return; }
        CreateConfigFile(path);
        AssetDatabase.Refresh();
    }
    public void CreateConfigFile(string _path)
    {
        #region 设置xml文本格式
        XmlDocument xmlDoc = new XmlDocument();
        XmlDeclaration xmldecl = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", "");
        xmlDoc.AppendChild(xmldecl);
        #endregion
        XmlElement root = xmlDoc.CreateElement(XmlConvert.EncodeName("root"));
        root.SetAttribute("ps", xmlName_cn);
        #region <loadObject ps="添加预制体">
        XmlElement loadObject = xmlDoc.CreateElement(XmlConvert.EncodeName("loadObject"));
                       loadObject.SetAttribute("ps", "添加预制体");
        if(UIcreateCount>0)
        {
            XmlElement UI = xmlDoc.CreateElement("UI");
             
            for (int i = 0; i < prefabUIList.Length; i++)
            {
                if (prefabUIList[i] == null) { Debug.LogError("物体不能为空");return; }
                XmlElement name = xmlDoc.CreateElement("name");
                name.InnerText = prefabUIList[i].name;
                name.SetAttribute("showTime", showTime_UI[i].ToString());
                name.SetAttribute("hideTime", hideTime_UI[i].ToString());
                UI.AppendChild(name);
            }
            loadObject.AppendChild(UI);
        }
        if (objcreateCount > 0)
        {
            XmlElement prefab = xmlDoc.CreateElement("prefab");

            for (int i = 0; i < prefabObjList.Length; i++)
            {
                if (prefabObjList[i] == null) { Debug.LogError("物体不能为空"); return; }
                XmlElement name = xmlDoc.CreateElement("name");
                name.InnerText = prefabObjList[i].name;
                name.SetAttribute("showTime", showTime[i].ToString());
                name.SetAttribute("hideTime", hideTime[i].ToString());
                prefab.AppendChild(name);
            }
            loadObject.AppendChild(prefab);
        }

        root.AppendChild(loadObject);
        #endregion
        #region <loadObject ps="场景中物体的显示和隐藏">
        XmlElement sceneObject = xmlDoc.CreateElement(XmlConvert.EncodeName("sceneObject"));
        sceneObject.SetAttribute("ps", "场景中物体的显示和隐藏");
        if (sceneObjCount > 0)
        {
            for (int i = 0; i < sceneObjList.Length; i++)
            {
                if (sceneObjList[i] == null) { Debug.LogError("物体不能为空"); return; }
                XmlElement name = xmlDoc.CreateElement("name");
                name.InnerText = sceneObjList[i];
                name.SetAttribute("showTime", sceneObj_showTime[i].ToString());
                name.SetAttribute("hideTime", sceneObj_hideTime[i].ToString());
                sceneObject.AppendChild(name);
            }
      
        }

        root.AppendChild(sceneObject);
        #endregion

        #region <cameraPosition ps="相机位置">

        XmlElement cameraPosition = xmlDoc.CreateElement(XmlConvert.EncodeName("cameraPosition"));
        cameraPosition.SetAttribute("ps", "相机位置");

            XmlElement cameraVector3 = xmlDoc.CreateElement("cameraVector3");
            cameraVector3.InnerText=m_cameraVector3.ToString().Replace("(","").Replace(")", "").Replace(" ", "");

            XmlElement cameraEngle = xmlDoc.CreateElement("cameraEngle");
            cameraEngle.InnerText = m_cameraEngle.ToString().Replace("(", "").Replace(")", "").Replace(" ", "");

            XmlElement cameraTarget = xmlDoc.CreateElement("targetVector3");
        cameraTarget.InnerText = m_targetVector3.ToString().Replace("(", "").Replace(")", "").Replace(" ", "");

        cameraPosition.AppendChild(cameraVector3);
        cameraPosition.AppendChild(cameraEngle);
        cameraPosition.AppendChild(cameraTarget);

        root.AppendChild(cameraPosition);
        #endregion
        #region SetPath
        XmlElement SetPath = xmlDoc.CreateElement(XmlConvert.EncodeName("SetPath"));
        SetPath.SetAttribute("ps", "物体的路径运动");
        if (setPathCount > 0)
        {
            for (int i = 0; i < setPathCount; i++)
            {
                XmlElement path = xmlDoc.CreateElement("path");
                path.SetAttribute("ps", "No."+(i+1));

                if (SetPath_modelName[i] == null) { Debug.LogError("物体不能为空"); return; }
                XmlElement modelName = xmlDoc.CreateElement("modelName");
                    modelName.InnerText = SetPath_modelName[i].name;
                    path.AppendChild(modelName);

                    XmlElement waitTime = xmlDoc.CreateElement("waitTime");
                    waitTime.InnerText = SetPath_waitTime[i].ToString();
                    path.AppendChild(waitTime);

                    XmlElement pathInfo = xmlDoc.CreateElement("pathInfo");
                    pathInfo.InnerText = SetPath_pathInfo[i];
                    path.AppendChild(pathInfo);

                    XmlElement pathTime = xmlDoc.CreateElement("pathTime");
                    pathTime.InnerText = SetPath_pathTime[i].ToString();
                    path.AppendChild(pathTime);

                    XmlElement lookType = xmlDoc.CreateElement("lookType");
                    lookType.InnerText = SetPath_lookType[i];
                    path.AppendChild(lookType);

                SetPath.AppendChild(path);
            }
        }

        root.AppendChild(SetPath);
        #endregion
        #region UV动画
        XmlElement UVanimation = xmlDoc.CreateElement(XmlConvert.EncodeName("UVanimation"));
        UVanimation.SetAttribute("ps", "UV动画");
        if (UVanimationCount > 0)
        {
            for (int i = 0; i < UVanimationCount; i++)
            {
                XmlElement animation = xmlDoc.CreateElement("animation");
                animation.SetAttribute("ps", "No." + (i + 1));
                if (UVanimation_modelName[i] == null) { Debug.LogError("物体不能为空"); return; }
                XmlElement modelName = xmlDoc.CreateElement("modelName");
                    modelName.InnerText = UVanimation_modelName[i].name;
                    animation.AppendChild(modelName);

                    XmlElement speedX = xmlDoc.CreateElement("speedX");
                    speedX.InnerText = UVanimation_speedX[i].ToString();
                    animation.AppendChild(speedX);

                    XmlElement speedY = xmlDoc.CreateElement("speedY");
                    speedY.InnerText = UVanimation_speedY[i].ToString();
                    animation.AppendChild(speedY);

                    XmlElement delay = xmlDoc.CreateElement("delay");
                    delay.InnerText = UVanimation_delay[i].ToString();
                    animation.AppendChild(delay);

                UVanimation.AppendChild(animation);
            }
        }
        root.AppendChild(UVanimation);
        #endregion
        #region 模型动画
        XmlElement modelAnimation = xmlDoc.CreateElement(XmlConvert.EncodeName("modelAnimation"));
        modelAnimation.SetAttribute("ps", "模型动画");
        if (modelAnimationCount > 0)
        {
            for (int i = 0; i < modelAnimationCount; i++)
            {
                XmlElement animation = xmlDoc.CreateElement("animation");
                animation.SetAttribute("ps", "No." + (i + 1));
                if (modelAnimation_modelName[i] == null) { Debug.LogError("物体不能为空"); return; }
                XmlElement modelName = xmlDoc.CreateElement("modelName");
                modelName.InnerText = modelAnimation_modelName[i].name;
                animation.AppendChild(modelName);

                XmlElement animationStateNumber = xmlDoc.CreateElement("animationStateNumber");
                animationStateNumber.InnerText = modelAnimation_animationStateNumber[i].ToString();
                animation.AppendChild(animationStateNumber);

                XmlElement waitTime = xmlDoc.CreateElement("waitTime");
                waitTime.InnerText = modelAnimation_waitTime[i].ToString();
                animation.AppendChild(waitTime);

                modelAnimation.AppendChild(animation);
            }
        }
        root.AppendChild(modelAnimation);
        #endregion
        #region 灯光的开关
        XmlElement lightControl = xmlDoc.CreateElement(XmlConvert.EncodeName("lightControl"));
        lightControl.SetAttribute("ps", "灯光的开关");
        if (lightControlCount > 0)
        {
            for (int i = 0; i < lightControlCount; i++)
            {
                XmlElement lightObj = xmlDoc.CreateElement("lightObj");
                lightObj.SetAttribute("ps", "No." + (i + 1));
                if (lightControl_name[i] == null) { Debug.LogError("物体不能为空"); return; }
                XmlElement name = xmlDoc.CreateElement("name");
                name.InnerText = lightControl_name[i].name;
                lightObj.AppendChild(name);

                XmlElement state = xmlDoc.CreateElement("state");
                state.InnerText = lightControl_state[i].ToString();
                lightObj.AppendChild(state);

                XmlElement waitTime = xmlDoc.CreateElement("waitTime");
                waitTime.InnerText = lightControl_waitTime[i].ToString();
                lightObj.AppendChild(waitTime);

                lightControl.AppendChild(lightObj);
            }
        }
        root.AppendChild(lightControl);
        #endregion
        #region 车辆行驶
        XmlElement carSpeed = xmlDoc.CreateElement(XmlConvert.EncodeName("carSpeed"));
        carSpeed.SetAttribute("ps", "车辆行驶");
        if (carSpeedCount > 0)
        {
            for (int i = 0; i < carSpeedCount; i++)
            {
                XmlElement speed = xmlDoc.CreateElement("speed");
                speed.InnerText = carSpeed_state[i].ToString();
                speed.SetAttribute("waitTime", carSpeed_waitTime[i].ToString());
                carSpeed.AppendChild(speed);
            }
        }
        root.AppendChild(carSpeed);
        #endregion
        #region 更改材质
        XmlElement changeMaterial = xmlDoc.CreateElement(XmlConvert.EncodeName("changeMaterial"));
        changeMaterial.SetAttribute("ps", "更改材质");
        if (changeMaterialCount > 0)
        {
            for (int i = 0; i < changeMaterialCount; i++)
            {
                XmlElement modelName = xmlDoc.CreateElement("modelName");//
                modelName.InnerText = changeMaterial_modelName[i];
                modelName.SetAttribute("waitTime", changeMaterial_waitTime[i].ToString());
                modelName.SetAttribute("materialName", changeMaterial_materialName[i].ToString());
                changeMaterial.AppendChild(modelName);
            }
        }
        root.AppendChild(changeMaterial);
        #endregion
        #region 物体变色
        XmlElement lerpColor = xmlDoc.CreateElement(XmlConvert.EncodeName("lerpColor"));
        lerpColor.SetAttribute("ps", "物体变色");
        if (lerpColorCount > 0)
        {
            for (int i = 0; i < lerpColorCount; i++)
            {
                XmlElement modelName = xmlDoc.CreateElement("modelName");//
                modelName.InnerText = lerpColor_modelName[i];
                modelName.SetAttribute("waitTime", lerpColor_waitTime[i].ToString());
                modelName.SetAttribute("lerpTime", lerpColor_lerpTime[i].ToString());
                modelName.SetAttribute("colorA",ColorToHex( lerpColor_colorA[i]));
                modelName.SetAttribute("colorB", ColorToHex(lerpColor_colorB[i]));
                lerpColor.AppendChild(modelName);
            }
        }
        root.AppendChild(lerpColor);
        #endregion
        #region 车灯
        //XmlElement carLight = xmlDoc.CreateElement(XmlConvert.EncodeName("carLight"));
        //carLight.SetAttribute("ps", "车灯");

        //XmlElement lightStatepositionLight = xmlDoc.CreateElement(XmlConvert.EncodeName("lightState"));//位置灯
        //lightStatepositionLight.InnerText = positionLight;
        //lightStatepositionLight.SetAttribute("lightType", "positionLight");
        //lightStatepositionLight.SetAttribute("waitTime", positionLight_waitTime);
        //carLight.AppendChild(lightStatepositionLight);

        //XmlElement lightStatefrontDecorateLight = xmlDoc.CreateElement(XmlConvert.EncodeName("lightState"));//前大灯
        //lightStatefrontDecorateLight.InnerText = frontDecorateLight;
        //lightStatefrontDecorateLight.SetAttribute("lightType", "frontDecorateLight");
        //lightStatefrontDecorateLight.SetAttribute("waitTime", frontDecorateLight_waitTime);
        //carLight.AppendChild(lightStatefrontDecorateLight);

        //XmlElement lightStatefarLight = xmlDoc.CreateElement(XmlConvert.EncodeName("lightState"));//远光灯
        //lightStatefarLight.InnerText = farLight;
        //lightStatefarLight.SetAttribute("lightType", "farLight");
        //lightStatefarLight.SetAttribute("waitTime", farLight_waitTime);
        //carLight.AppendChild(lightStatefarLight);

        //XmlElement lightStatelogoLight = xmlDoc.CreateElement(XmlConvert.EncodeName("lightState"));//logo灯
        //lightStatelogoLight.InnerText = logoLight;
        //lightStatelogoLight.SetAttribute("lightType", "logoLight");
        //lightStatelogoLight.SetAttribute("waitTime", logoLight_waitTime);
        //carLight.AppendChild(lightStatelogoLight);

        //XmlElement lightStatefogLight = xmlDoc.CreateElement(XmlConvert.EncodeName("lightState"));//雾灯
        //lightStatefogLight.InnerText = brakeLight;
        //lightStatefogLight.SetAttribute("lightType", "fogLight");
        //lightStatefogLight.SetAttribute("waitTime", brakeLight_waitTime);
        //carLight.AppendChild(lightStatefogLight);


        //root.AppendChild(carLight);
        #endregion

        #region 拖尾
        XmlElement setTrail = xmlDoc.CreateElement(XmlConvert.EncodeName("setTrail"));
        setTrail.SetAttribute("ps", "拖尾");
        if (setTrailCount > 0)
        {
            for (int i = 0; i < setTrailCount; i++)
            {
                XmlElement trailObjectXmlElement = xmlDoc.CreateElement("trailObject");
                trailObjectXmlElement.InnerText = trailObject[i];
                trailObjectXmlElement.SetAttribute("waitTime", trail_waitTime[i].ToString());

                setTrail.AppendChild(trailObjectXmlElement);
            }
        }
        root.AppendChild(setTrail);
        #endregion
        #region 车外后视镜翻转
        XmlElement rearViewMirror = xmlDoc.CreateElement(XmlConvert.EncodeName("rearViewMirror"));
        rearViewMirror.SetAttribute("ps", "车外后视镜翻转");
        if (rearViewMirrorCount > 0)
        {
            for (int i = 0; i < rearViewMirrorCount; i++)
            {
                XmlElement item = xmlDoc.CreateElement("item");
                item.SetAttribute("ps", "No." + (i + 1));

                XmlElement modelName = xmlDoc.CreateElement("modelName");
                modelName.InnerText = rearViewMirror_modelName[i];
                item.AppendChild(modelName);

                XmlElement waitTime = xmlDoc.CreateElement("waitTime");
                waitTime.InnerText = rearViewMirror_waitTime[i].ToString();
                item.AppendChild(waitTime);

                XmlElement rotateVector = xmlDoc.CreateElement("rotateVector");
                rotateVector.InnerText = rearViewMirror_rotateVector[i].ToString();
                item.AppendChild(rotateVector);

                XmlElement speed = xmlDoc.CreateElement("speed");
                speed.InnerText = rearViewMirror_speed[i].ToString();
                item.AppendChild(speed);

                XmlElement angel = xmlDoc.CreateElement("angel");
                angel.InnerText = rearViewMirror_angel[i].ToString();
                item.AppendChild(angel);
                rearViewMirror.AppendChild(item);
            }
        }
        root.AppendChild(rearViewMirror);
        #endregion
        #region 直线位移
        XmlElement linerMove = xmlDoc.CreateElement(XmlConvert.EncodeName("linerMove"));
        linerMove.SetAttribute("ps", "直线位移");
        if (linerMoveCount > 0)
        {
            for (int i = 0; i < linerMoveCount; i++)
            {
                XmlElement item = xmlDoc.CreateElement("item");
                item.SetAttribute("ps", "No." + (i + 1));

                    XmlElement modelName = xmlDoc.CreateElement("modelName");
                    modelName.InnerText = linerMove_modelName[i];
                    item.AppendChild(modelName);

                    XmlElement waitTime = xmlDoc.CreateElement("waitTime");
                    waitTime.InnerText = linerMove_waitTime[i].ToString();
                    item.AppendChild(waitTime);

                    XmlElement rotateVector = xmlDoc.CreateElement("rotateVector");
                    rotateVector.InnerText = linerMove_rotateVector[i].ToString();
                    item.AppendChild(rotateVector);

                    XmlElement speed = xmlDoc.CreateElement("speed");
                    speed.InnerText = linerMove_speed[i].ToString();
                    item.AppendChild(speed);

                    XmlElement distance = xmlDoc.CreateElement("distance");
                    distance.InnerText = linerMove_distance[i].ToString();
                    item.AppendChild(distance);

                linerMove.AppendChild(item);
            }
        }
        root.AppendChild(linerMove);
        #endregion
        #region 车辆功能开关，1是开，0是关
        XmlElement CarControlTrigger = xmlDoc.CreateElement(XmlConvert.EncodeName("CarControlTrigger"));
        CarControlTrigger.SetAttribute("ps", "车辆功能开关，1是开，0是关");

        XmlElement XmlElement_radar = xmlDoc.CreateElement(XmlConvert.EncodeName("radar"));//位置灯
        XmlElement_radar.SetAttribute("ps", "车后方雷达");
        XmlElement_radar.InnerText = radar.ToString();
        CarControlTrigger.AppendChild(XmlElement_radar);

        XmlElement XmlElement_frontLightfarNearAutoControl = xmlDoc.CreateElement(XmlConvert.EncodeName("frontLightfarNearAutoControl"));//位置灯
        XmlElement_frontLightfarNearAutoControl.SetAttribute("ps", "自动远近光灯");
        XmlElement_frontLightfarNearAutoControl.InnerText = frontLightfarNearAutoControl.ToString();
        CarControlTrigger.AppendChild(XmlElement_frontLightfarNearAutoControl);

        XmlElement XmlElement_frontLightrotateByRoad = xmlDoc.CreateElement(XmlConvert.EncodeName("frontLightrotateByRoad"));//位置灯
        XmlElement_frontLightrotateByRoad.SetAttribute("ps", "车灯随动转向");
        XmlElement_frontLightrotateByRoad.InnerText = frontLightrotateByRoad.ToString();
        CarControlTrigger.AppendChild(XmlElement_frontLightrotateByRoad);

        XmlElement XmlElement_wheelLRrotate = xmlDoc.CreateElement(XmlConvert.EncodeName("wheelLRrotate"));//位置灯
        XmlElement_wheelLRrotate.SetAttribute("ps", "车轮左右转向");
        XmlElement_wheelLRrotate.InnerText = wheelLRrotate.ToString();
        CarControlTrigger.AppendChild(XmlElement_wheelLRrotate);

        XmlElement XmlElement_rearMirrorTrueReflect = xmlDoc.CreateElement(XmlConvert.EncodeName("rearMirrorTrueReflect"));//位置灯
        XmlElement_rearMirrorTrueReflect.SetAttribute("ps", "后视镜真实反射");
        XmlElement_rearMirrorTrueReflect.InnerText = rearMirrorTrueReflect.ToString();
        CarControlTrigger.AppendChild(XmlElement_rearMirrorTrueReflect);

        root.AppendChild(CarControlTrigger);
        #endregion

        #region 输入显示到屏幕的文字
        XmlElement textWord = xmlDoc.CreateElement(XmlConvert.EncodeName("textWord"));
        textWord.SetAttribute("ps", "输入显示到屏幕的文字");
        textWord.InnerText = screenLog;
        root.AppendChild(textWord);

        #endregion
        #region 屏幕背景色hex
        XmlElement XmlElement_cameraBackgroundColor = xmlDoc.CreateElement(XmlConvert.EncodeName("cameraBackgroundColor"));
        XmlElement_cameraBackgroundColor.SetAttribute("ps", "屏幕背景色hex");
        XmlElement_cameraBackgroundColor.InnerText = ColorToHex( cameraBackgroundColor);
        root.AppendChild(XmlElement_cameraBackgroundColor);

        #endregion


        xmlDoc.AppendChild(root);
        xmlDoc.Save(_path);
        Debug.Log("文件生成成功，对应路径："+ _path);
    }

    [MenuItem("XmlBuider/build editor")]
    static void Open()
    {
        XmlBuider myWindow = (XmlBuider)EditorWindow.GetWindow(typeof(XmlBuider), false, "Xml build editor", true);//创建窗口
        myWindow.Show();//展示
    }
    bool canSaveXml()
    {
        if (string.IsNullOrEmpty(xmlName)) { return false; }
        return true;
    }
    public string ColorToHex(Color color)
    {
        int r = Mathf.RoundToInt(color.r * 255.0f);
        int g = Mathf.RoundToInt(color.g * 255.0f);
        int b = Mathf.RoundToInt(color.b * 255.0f);
        int a = Mathf.RoundToInt(color.a * 255.0f);
        string hex = string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", r, g, b, a);
        return hex;
    }

    public void LoadXmlToEditor(string path)
    {
        objcreateCount = 3;
        SetListObj();
    }
}
//绘制描述文本区域
//GUILayout.Space(10);
//GUILayout.BeginHorizontal();
//GUILayout.Label("Description", GUILayout.MaxWidth(80));
//description = EditorGUILayout.TextArea(description, GUILayout.MaxHeight(75));
//GUILayout.EndHorizontal();
//绘制当前正在编辑的场景
//GUILayout.Space(10);
//GUI.skin.label.fontSize = 12;
//GUI.skin.label.alignment = TextAnchor.UpperLeft;
//GUILayout.Label("Currently Scene:" + EditorSceneManager.GetActiveScene().name);