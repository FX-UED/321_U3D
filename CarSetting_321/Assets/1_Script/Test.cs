using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;
using LitJson;
public class Test : MonoBehaviour
{
    public InputField inputF;
    public Button loadSceneBtn;
    public Transform sceneBtns;
    public Text timeCounter;
    private bool isCountTime=false;
    private float pastTime;

    string doorFrontLeft_on = @"{ ""FLDoorStatus"" : ""1""  }";//打开左前门
    string doorFrontLeft_off = @"{ ""FLDoorStatus"" : ""0""  }";//关闭左前门
    string doorFrontRight_on = @"{ ""FRDoorStatus"" : ""1""  }";//打开右前门
    string doorFrontRight_off = @"{ ""FRDoorStatus"" : ""0""  }";//关闭右前门

    string doorBackLeft_on = @"{ ""RLDoorStatus"" : ""1""  }";//打开左前门
    string doorBackLeft_off = @"{ ""RLDoorStatus"" : ""0""  }";//关闭左前门
    string doorBackRight_on = @"{ ""RRDoorStatus"" : ""1""  }";//打开右前门
    string doorBackRight_off = @"{ ""RRDoorStatus"" : ""0""  }";//关闭右前门

    string mirrorLeft_on = @"{ ""LMirror"" : ""1""  }";//打开左前门
    string mirrorLeft_off = @"{ ""LMirror"" : ""0""  }";//关闭左前门
    string mirrorRight_on = @"{ ""RMirror"" : ""1""  }";//打开右前门
    string mirrorRight_off = @"{ ""RMirror"" : ""0""  }";//关闭右前门

    string windowFrontLeft_on = @"{     ""windowFrontLeft"" : ""1""  }";//打开左前门
    string windowFrontLeft_off = @"{    ""windowFrontLeft"" : ""0""  }";//关闭左前门
    string windowFrontRight_on = @"{    ""windowFrontRight"" : ""1""  }";//打开右前门
    string windowFrontRight_off = @"{   ""windowFrontRight"" : ""0""  }";//关闭右前门
    string windowBackLeft_on = @"{      ""windowBackLeft"" : ""1""  }";//打开左前门
    string windowBackLeft_off = @"{     ""windowBackLeft"" : ""0""  }";//关闭左前门
    string windowBackRight_on = @"{     ""windowBackRight"" : ""1""  }";//打开右前门
    string windowBackRight_off = @"{    ""windowBackRight"" : ""0""  }";//关闭右前门

    string doorTrunk_on = @"{ ""doorTrunk"" : ""1""  }";//尾箱
    string doorTrunk_off = @"{ ""doorTrunk"" : ""0""  }";//尾箱


    string wheel_0 = @"{ ""wheel"" : ""0""  }";//轮毂0
    string wheel_1 = @"{ ""wheel"" : ""1""  }";//轮毂1

    string carPaint_white = @"{ ""carPaint"" : ""white""  }";//车漆-白
    string carPaint_black = @"{ ""carPaint"" : ""black""  }";//车漆-白
    string carPaint_blue = @"{ ""carPaint"" : ""blue""  }";//车漆-白
    string carPaint_red = @"{ ""carPaint"" : ""red""  }";//车漆-白
    string carPaint_skyblue = @"{ ""carPaint"" : ""skyblue""  }";//车漆-白
    string carPaint_zong = @"{ ""carPaint"" : ""zong""  }";//车漆-白

    void Start()
    {
        // DataManager.Instance.XmlRead();
        if(loadSceneBtn!=null)
        loadSceneBtn.onClick.AddListener(LoadScene);
        if (PlayerPrefs.HasKey("xmlNameInput"))
        {
            if(inputF!=null)
            inputF.text = PlayerPrefs.GetString("xmlNameInput");
        }
        if (sceneBtns != null)
        {
            for (int i = 0; i < sceneBtns.childCount; i++)
            {
                int temp = i;//临时变量赋值给delegate，i传递不过去
                sceneBtns.GetChild(i).GetComponent<Button>().onClick.AddListener(delegate
                {
                    LoadSceneBtn(sceneBtns.GetChild(temp).gameObject);

                });
            }
        }
        AppStart.IsGameStart = true;
        AppDelegate.Instance.OnFrontLightrotateByRoadEvent(true);
        AppDelegate.Instance.OnFrontLightfarNearAutoControlDeleEvent(true);
    }

    #region 阅读灯
    public void Btn_LeftReadingLightController(string s)
    {
        string t = " {\"leftReadLight\":\"" + s + "\"}";

        NativeManager.Instance.VehicleControl(t);//驾驶模式-个性化" {\"driveModeCustomSpeedSetting\":\"f\"}"
    }
    public void Btn_RightReadingLightController(string s)
    {
        string t = " {\"rightReadLight\":\"" + s + "\"}";
        Debug.Log("rightReadLight :" + s);
        NativeManager.Instance.VehicleControl(t);//驾驶模式-个性化" {\"driveModeCustomSpeedSetting\":\"f\"}"
    }
    #endregion

    #region 氛围灯 测试

    public void Btn_atmosphereStatic()
    {
        NativeManager.Instance.VehicleControl(@"{ ""atmosphereLightMode"" : ""1""  }");
    }
    public void Btn_atmosphereDymic()
    {
        NativeManager.Instance.VehicleControl(@"{ ""atmosphereLightMode"" : ""2""  }");
    }
    public void Btn_atmosphereSpeed(float speed)
    {
        string t = " {\"atmosphereLightSpeed\":\"" + speed + "\"}";
        NativeManager.Instance.VehicleControl(t);
    }
    public void Btn_atmosphereColor(string hex)
    {
        string t = " {\"atmosphereLightColor\":\"" + hex + "\"}";
        NativeManager.Instance.VehicleControl(t);
    }
    public void Btn_atmosphereBrightness(float hex)
    {
        string t = " {\"atmosphereLightBrightness\":\"" + hex + "\"}";
        NativeManager.Instance.VehicleControl(t);
    }
    public void Btn_atmosphereColorType(int value)
    {
        string t = " {\"atmosphereLightColorType\":\"" + value + "\"}";
        NativeManager.Instance.VehicleControl(t);
    }
    #endregion

    public void OnDriveModeSpeedSetting(float f)
    {
        string t = " {\"driveModeCustomSpeedSetting\":\" " + f + "\"}";
        Debug.Log("OnDriveModeSpeedSetting : " + t);
        NativeManager.Instance.VehicleControl(t);//驾驶模式-个性化" {\"driveModeCustomSpeedSetting\":\"f\"}"
    }
    public void OnDriveModeEnergyRecovery(float f)
    {
        string t = " {\"driveModeCustomEnergyRecovery\":\" " + f + "\"}";
        Debug.Log("driveModeCustomEnergyRecovery : " + t);
        NativeManager.Instance.VehicleControl(t);//驾驶模式-个性化" {\"driveModeCustomSpeedSetting\":\"f\"}"
    }
    public void Btn_jieneng()
    {
        Debug.Log("aa");

        NativeManager.Instance.VehicleControl(@"{ ""driveMode"" : ""JieNeng""  }");//驾驶模式-节能
    }
    public void Btn_shushi()
    {
        NativeManager.Instance.VehicleControl(@"{ ""driveMode"" : ""ShuShi""  }");//驾驶模式-舒适
    }
    public void Btn_yundong()
    {
        NativeManager.Instance.VehicleControl(@"{ ""driveMode"" : ""YunDong""  }");//驾驶模式-运动
    }
    public void Btn_gexinghua()
    {
        NativeManager.Instance.VehicleControl(@"{ ""driveMode"" : ""GeXingHua""  }");//驾驶模式-个性化
    }
    private void Update()
    {
        if(timeCounter!=null&& isCountTime)
        {
            timeCounter.text = (Time.time- pastTime).ToString();
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            CarController.carSpeed_finall = 60;
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            CarController.carSpeed_finall = 20;
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            CarController.carSpeed_finall = 10;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            NativeManager.Instance.VehicleControl(doorFrontLeft_on);
            NativeManager.Instance.VehicleControl(doorFrontRight_on);
            NativeManager.Instance.VehicleControl(doorBackLeft_on);
            NativeManager.Instance.VehicleControl(doorBackRight_on);
            NativeManager.Instance.VehicleControl(doorTrunk_on);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            NativeManager.Instance.VehicleControl(windowFrontLeft_on);
            NativeManager.Instance.VehicleControl(windowFrontRight_on);
            NativeManager.Instance.VehicleControl(windowBackLeft_on);
            NativeManager.Instance.VehicleControl(windowBackRight_on);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            NativeManager.Instance.VehicleControl(mirrorLeft_on);
            NativeManager.Instance.VehicleControl(mirrorRight_on);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            NativeManager.Instance.VehicleControl(doorFrontLeft_off);
            NativeManager.Instance.VehicleControl(doorFrontRight_off);
            NativeManager.Instance.VehicleControl(doorBackLeft_off);
            NativeManager.Instance.VehicleControl(doorBackRight_off);
            NativeManager.Instance.VehicleControl(doorTrunk_off);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            NativeManager.Instance.VehicleControl(windowFrontLeft_off);
            NativeManager.Instance.VehicleControl(windowFrontRight_off);
            NativeManager.Instance.VehicleControl(windowBackLeft_off);
            NativeManager.Instance.VehicleControl(windowBackRight_off);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            NativeManager.Instance.VehicleControl(mirrorLeft_off);
            NativeManager.Instance.VehicleControl(mirrorRight_off);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            NativeManager.Instance.VehicleControl(@"{ ""driveMode"" : ""JieNeng""  }");//驾驶模式-节能
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            NativeManager.Instance.VehicleControl(@"{ ""driveMode"" : ""ShuShi""  }");//驾驶模式-舒适
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            NativeManager.Instance.VehicleControl(@"{ ""driveMode"" : ""YunDong""  }");//驾驶模式-运动
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            NativeManager.Instance.VehicleControl(@"{ ""driveMode"" : ""GeXingHua""  }");//驾驶模式-个性化
        }
        // -------------------------------------- 车漆颜色 --------------------------------------
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            NativeManager.Instance.VehicleControl(carPaint_white);
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            NativeManager.Instance.VehicleControl(carPaint_black);
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            NativeManager.Instance.VehicleControl(carPaint_blue);
        }
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            NativeManager.Instance.VehicleControl(carPaint_skyblue);
        }
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            NativeManager.Instance.VehicleControl(carPaint_red);
        }
        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            NativeManager.Instance.VehicleControl(carPaint_zong);
        }
    }

    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(inputF.text))
        {
            DataManager.Instance.LoadSceneXML(inputF.text);//测试
            PlayerPrefs.SetString("xmlNameInput", inputF.text);
            isCountTime = true;
            pastTime = Time.time;
        }
    }
    public void LoadSceneBtn(GameObject btn)
    {
        Debug.Log("加载："+ btn.name);
        if (!string.IsNullOrEmpty(btn.name))
        {
            DataManager.Instance.LoadSceneXML(btn.name);//测试
    
        }
    }
    float a = 0;

    /*
    string doorFrontLeft_on =   @"{ ""doorFrontLeft"" : ""1""  }";//打开左前门
    string doorFrontLeft_off =  @"{ ""doorFrontLeft"" : ""0""  }";//关闭左前门
    string doorFrontRight_on =  @"{ ""doorFrontRight"" : ""1""  }";//打开右前门
    string doorFrontRight_off = @"{ ""doorFrontRight"" : ""0""  }";//关闭右前门

    string doorBackLeft_on = @"{ ""doorBackLeft"" : ""1""  }";//打开左前门
    string doorBackLeft_off = @"{ ""doorBackLeft"" : ""0""  }";//关闭左前门
    string doorBackRight_on = @"{ ""doorBackRight"" : ""1""  }";//打开右前门
    string doorBackRight_off = @"{ ""doorBackRight"" : ""0""  }";//关闭右前门

    string windowFrontLeft_on = @"{     ""windowFrontLeft"" : ""1""  }";//打开左前门
    string windowFrontLeft_off = @"{    ""windowFrontLeft"" : ""0""  }";//关闭左前门
    string windowFrontRight_on = @"{    ""windowFrontRight"" : ""1""  }";//打开右前门
    string windowFrontRight_off = @"{   ""windowFrontRight"" : ""0""  }";//关闭右前门
    string windowBackLeft_on = @"{      ""windowBackLeft"" : ""1""  }";//打开左前门
    string windowBackLeft_off = @"{     ""windowBackLeft"" : ""0""  }";//关闭左前门
    string windowBackRight_on = @"{     ""windowBackRight"" : ""1""  }";//打开右前门
    string windowBackRight_off = @"{    ""windowBackRight"" : ""0""  }";//关闭右前门

    string skylight_on = @"{ ""skylight"" : ""1""  }";//天窗
    string skylight_off = @"{ ""skylight"" : ""0""  }";//天窗

    string doorTrunk_on = @"{ ""doorTrunk"" : ""1""  }";//尾箱
    string doorTrunk_off = @"{ ""doorTrunk"" : ""0""  }";//尾箱

    string runSpeed_slow =      @"{ ""runSpeed"" : ""30""  }";//输入行驶速度
    string runSpeed_stop =      @"{ ""runSpeed"" : ""0""  }";

    string wheel_0 = @"{ ""wheel"" : ""0""  }";//轮毂0
    string wheel_1 = @"{ ""wheel"" : ""1""  }";//轮毂1

    string weather_0 = @"{ ""weather"" : ""sunny""  }";//晴天
    string weather_1 = @"{ ""weather"" : ""rain""  }";//下雨
    string weather_2 = @"{ ""weather"" : ""snow""  }";//下雪

    string time_mor = @"{ ""time"" : ""morning""  }";//白天
    string time_noon = @"{ ""time"" : ""afternoon""  }";//白天
    string time_night = @"{ ""time"" : ""night""  }";//夜晚

    string cam_front = @"{ ""cameraVector"" : ""front""  }";//前视图
    string cam_back = @"{ ""cameraVector"" : ""back""  }";//后视图
    string cam_left = @"{ ""cameraVector"" : ""left""  }";//左视图
    string cam_right = @"{ ""cameraVector"" : ""right""  }";//右视图
    string cam_top = @"{ ""cameraVector"" : ""top""  }";//顶视图
    string cam_free = @"{ ""cameraVector"" : ""free""  }";//自由

    string carPaint_white= @"{ ""carPaint"" : ""white""  }";//车漆-白
    string carPaint_black= @"{ ""carPaint"" : ""black""  }";//车漆-白
    string carPaint_blue= @"{ ""carPaint"" : ""blue""  }";//车漆-白
    string carPaint_red= @"{ ""carPaint"" : ""red""  }";//车漆-白
    string carPaint_skyblue= @"{ ""carPaint"" : ""skyblue""  }";//车漆-白
    string carPaint_zong= @"{ ""carPaint"" : ""zong""  }";//车漆-白

    string carLight_1 = @"{ ""carLight"" : ""1""  }";//车漆-白
    string carLight_0 = @"{ ""carLight"" : ""0""  }";//车漆-白
    string warning_0 = @"{ ""warning"" : ""0""  }";//消除警告
    string warning_engine = @"{ ""warning"" : ""engine""  }";//显示警告

    string front_charging_on= @"{ ""charge"" : ""front_on""  }";//前充电
    string front_charging_off= @"{ ""charge"" : ""front_off""  }";//前充电off
    string back_charging_on= @"{ ""charge"" : ""back_on""  }";//后充电
    string back_charging_off= @"{ ""charge"" : ""back_off""  }";//后充电off
    private void Start()
    {
        DontDestroyOnLoad(this);
        //if (Application.platform == RuntimePlatform.WebGLPlayer)
        //    this.enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        // --------------------------------------充电 --------------------------------------
        if (Input.GetKeyDown(KeyCode.Z))
        {
            NativeManager.Instance.JsToUnity(front_charging_on);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            NativeManager.Instance.JsToUnity(front_charging_off);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            NativeManager.Instance.JsToUnity(back_charging_on);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            NativeManager.Instance.JsToUnity(back_charging_off);
        }

        // --------------------------------------动画 --------------------------------------
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            NativeManager.Instance.JsToUnity(doorFrontLeft_on);
            NativeManager.Instance.JsToUnity(doorFrontRight_on);
            NativeManager.Instance.JsToUnity(doorBackLeft_on);
            NativeManager.Instance.JsToUnity(doorBackRight_on);
            NativeManager.Instance.JsToUnity(skylight_on);
            NativeManager.Instance.JsToUnity(doorTrunk_on);

            NativeManager.Instance.JsToUnity(windowFrontLeft_on);
            NativeManager.Instance.JsToUnity(windowFrontRight_on);
            NativeManager.Instance.JsToUnity(windowBackLeft_on);
            NativeManager.Instance.JsToUnity(windowBackRight_on);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            NativeManager.Instance.JsToUnity(doorFrontLeft_off);
            NativeManager.Instance.JsToUnity(doorFrontRight_off);
            NativeManager.Instance.JsToUnity(doorBackLeft_off);
            NativeManager.Instance.JsToUnity(doorBackRight_off);
            NativeManager.Instance.JsToUnity(skylight_off);
            NativeManager.Instance.JsToUnity(doorTrunk_off);


            NativeManager.Instance.JsToUnity(windowFrontLeft_off);
            NativeManager.Instance.JsToUnity(windowFrontRight_off);
            NativeManager.Instance.JsToUnity(windowBackLeft_off);
            NativeManager.Instance.JsToUnity(windowBackRight_off);
        }
        // -------------------------------------- 换轮毂 --------------------------------------
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            NativeManager.Instance.JsToUnity(wheel_0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            NativeManager.Instance.JsToUnity(wheel_1);
        }
        // -------------------------------------- 相机路径动画 --------------------------------------
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            NativeManager.Instance.JsToUnity(@"{ ""cameraPathAnimation"" : ""1""  }");
        }
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            NativeManager.Instance.JsToUnity(@"{ ""cameraPathAnimation"" : ""0""  }");
        }
        // -------------------------------------- 行驶速度 --------------------------------------
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            NativeManager.Instance.JsToUnity(@"{ ""runSpeed"" : ""30""  }");// @"{ ""runSpeed"" : ""30""  }"
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            NativeManager.Instance.JsToUnity(@"{ ""runSpeed"" : ""60""  }");
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            NativeManager.Instance.JsToUnity(@"{ ""runSpeed"" : ""90""  }");
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            NativeManager.Instance.JsToUnity(@"{ ""runSpeed"" : ""110""  }");
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            NativeManager.Instance.JsToUnity(@"{ ""runSpeed"" : ""120""  }");
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            NativeManager.Instance.JsToUnity(runSpeed_stop);
        }
        // -------------------------------------- 天气 --------------------------------------

        if (Input.GetKeyDown(KeyCode.A))
        {
            NativeManager.Instance.JsToUnity(weather_0);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            NativeManager.Instance.JsToUnity(weather_1);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            NativeManager.Instance.JsToUnity(weather_2);
        }
        // -------------------------------------- 时间 --------------------------------------
        if (Input.GetKeyDown(KeyCode.F))
        {
            NativeManager.Instance.JsToUnity(time_mor);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            NativeManager.Instance.JsToUnity(time_noon);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            NativeManager.Instance.JsToUnity(time_night);
        }

        // -------------------------------------- 摄影机视角 --------------------------------------
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            NativeManager.Instance.JsToUnity(cam_front);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            NativeManager.Instance.JsToUnity(cam_left);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            NativeManager.Instance.JsToUnity(cam_right);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            NativeManager.Instance.JsToUnity(cam_back);
        }
        if (Input.GetKeyDown(KeyCode.End))
        {
            NativeManager.Instance.JsToUnity(cam_top);
        }
        if (Input.GetKeyDown(KeyCode.Home))
        {
            NativeManager.Instance.JsToUnity(cam_free);
        }

        // -------------------------------------- 车漆颜色 --------------------------------------
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            NativeManager.Instance.JsToUnity(carPaint_white);
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            NativeManager.Instance.JsToUnity(carPaint_black);
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            NativeManager.Instance.JsToUnity(carPaint_blue);
        }
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            NativeManager.Instance.JsToUnity(carPaint_skyblue);
        }
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            NativeManager.Instance.JsToUnity(carPaint_red);
        }
        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            NativeManager.Instance.JsToUnity(carPaint_zong);
        }

        //------------------------------------------------------------------车灯开关----------------------------
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            NativeManager.Instance.JsToUnity(carLight_1);
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            NativeManager.Instance.JsToUnity(carLight_0);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("GC.Collect----------------");
            Resources.UnloadUnusedAssets();
            AssetBundle.UnloadAllAssetBundles(true);
            System.GC.Collect();
            object[] gameObjects;
            gameObjects = GameObject.FindObjectsOfType(typeof(Transform));
            foreach (Transform go in gameObjects)
            {
                if(go.GetComponent<Test>()==null&& go.GetComponent<AppStart>() == null)
                {
                    Destroy(go.gameObject);
                }
              
            }
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            SceneManager.LoadScene(0);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            NativeManager.Instance.JsToUnity(warning_engine);
            
        }
    }
    */
}
