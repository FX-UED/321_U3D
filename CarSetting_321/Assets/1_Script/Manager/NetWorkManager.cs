using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using AssetBundles;

public class NetWorkManager : DDOLSingleton<NetWorkManager>
{
    private AssetBundleManager abm;
    private string abName_texture;
    private string abName_sound;
    private string absUrl = "";
    public string[] modelNames = { "background","body","doors", "city", "trunk", "skylight" };
    private int modelCount=0;

    #region 获取Json

    /// <summary>
    /// Get 获取bundle的URL 和 ab包的版本号
    /// </summary>
    public void LoadBundleURL()
    {
        string urlConfiguration = "http://192.168.21.115/CarWebGL/APPConfiguration.txt";// tA.text;
        StartCoroutine(GetUrl(urlConfiguration, OnReceivedBundleURL));
    }
    public void OnReceivedBundleURL(string BundleURLJson)
    {
        absUrl = BundleURLJson;
        CarParent = GameObject.Find("Car").transform;
    }

    #endregion

    #region AssetBundle 下载资源

    /// <summary>
    /// 初始化 assetbundle 管理器 下载bundle
    /// </summary>
    /// <returns></returns>
    public IEnumerator InitAssetBundleManager()
    {
        while (string.IsNullOrEmpty(absUrl))
        {
            yield return new WaitForEndOfFrame();
        }
        abm = new AssetBundleManager();
        abm.SetBaseUri(absUrl);
        abm.Initialize(OnAssetBundleManagerInitialized);
    }
    private void OnAssetBundleManagerInitialized(bool success)
    {
        if (success)
        {
            for (int i = 0; i < modelNames.Length; i++)
            {
                abm.GetBundle(modelNames[i], OnAssetBundleDownloaded_Model);
            }
        }
        else
        { Debug.LogError("Error initializing ABM."); }
    }
    Transform CarParent;
    private void OnAssetBundleDownloaded_Model(AssetBundle bundle)
    {
        if (bundle != null)
        {
            //加载全部的模型
            string[] bundlenames = bundle.GetAllAssetNames();
            if (bundlenames.Length == 0) {return; }

            for (int i = 0; i < bundlenames.Length; i++)
            {
               GameObject go=(GameObject)Instantiate(bundle.LoadAsset(bundlenames[i]) as GameObject);
                go.name = go.name.Split('(')[0];
                if(go.name!="city")
                {
                    go.transform.SetParent(CarParent);
                }
            }

            //计数
            modelCount++;
            if(modelCount==modelNames.Length)
            {
                // 模型全部加装完毕，配置车辆
                AppDelegate.Instance.OnCarSetting();
            }
            abm.UnloadBundle(bundle);
        }
        abm.Dispose();
    }
    /// <summary>
    /// bundle下载完成后的回调
    /// </summary>
    /// <param name="bundle"></param>
    private void OnAssetBundleDownloaded_Texture(AssetBundle bundle)
    {

    }
    #endregion

    private IEnumerator GetUrl(string url, Action<string> getResult = null)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))//m_url_Default
        {
            yield return www.SendWebRequest();
            if (www.error != null)
            {
                Debug.Log("-=-=-==-get url error : " + www.error);
            }
            else
            {
                if (www.responseCode == 200)//请求的状态值，分别有：200请求成功、303重定向、400请求错误、401未授权、403禁止访问、404文件未找到、500服务器错误 
                {
                    getResult(www.downloadHandler.text);
                }
            }
        }
    }

    private IEnumerator GetUrl(string url, Action<byte[]> getResult = null)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))//m_url_Default
        {
            yield return www.SendWebRequest();
            if (www.error != null)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (www.responseCode == 200)//请求的状态值，分别有：200请求成功、303重定向、400请求错误、401未授权、403禁止访问、404文件未找到、500服务器错误 
                {
                    getResult(www.downloadHandler.data);
                }
            }
        }
    }
}