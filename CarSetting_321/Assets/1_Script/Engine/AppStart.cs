using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public enum LoadType
{
    onLine,
    hierarchy
}
public class AppStart : MonoBehaviour
{
    public LoadType loadType;
    private CarController carController;
    public static bool IsGameStart = false;
  
    void Awake()
    {
        AppDelegate.CarSettingEvent += SetCar;
        AppDelegate.Restart += Restart;
        AppDelegate.ReleaseMemory += RelaseMemory;
        Init();
    }
    private void OnDisable()
    {
        AppDelegate.CarSettingEvent -= SetCar;
        AppDelegate.Restart -= Restart;
        AppDelegate.ReleaseMemory -= RelaseMemory;
    }

    private void Init()
    {
        //switch(loadType)
        //{
        //    case LoadType.hierarchy:
        //        SetCar();
        //        break;
        //    case LoadType.onLine:
        //        NetWorkManager.Instance.LoadBundleURL();
        //        StartCoroutine(NetWorkManager.Instance.InitAssetBundleManager());
        //        break;
        //}
       
    }
    private void SetCar()
    {
        //StartCoroutine(WaitCarShow());
    }
    private IEnumerator WaitCarShow()
    {
        while (GameObject.Find("Exterior_cluster4_low") ==null)
        {
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("加载所用时间："+Time.time);

        carController = new CarController();
    }
    private void FixedUpdate()
    {
        carController?.Run();
    }
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.N) && Input.GetKeyDown(KeyCode.M))
        //{
        //    GetComponent<ShowFps>().enabled = !GetComponent<ShowFps>().enabled;
        //}
       // Debug.Log(CarController.carSpeed_finall);
    }
    private void RelaseMemory(string s)
    {
        Resources.UnloadUnusedAssets();
        AssetBundle.UnloadAllAssetBundles(true);
        System.GC.Collect();
        object[] gameObjects;
        gameObjects = GameObject.FindObjectsOfType(typeof(Transform));
        foreach (Transform go in gameObjects)
        {
            if (go.GetComponent<Test>() == null && go.GetComponent<AppStart>() == null)
            {
                Destroy(go.gameObject);
            }
        }
    }
    private void Restart(string s)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
