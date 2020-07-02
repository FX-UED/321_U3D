using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using UnityEngine.Networking;
public class ImageManager:Singleton<ImageManager>
{
    /// <summary>
    /// 存储所有的图片
    /// </summary>
    public List<Texture2D> allTexList = new List<Texture2D>();
    public void ImportTextures(Texture2D t)
    {
        allTexList.Add(t);
    }
    /// <summary>
    /// 加载图片给UI
    /// </summary>
    /// <param name="m_Tex"></param>
    /// <param name="imgParent"></param>
    /// <param name="prefabName"></param>
    /// <param name="btnMethod"></param>
    /// <param name="number"></param>
    public void LoadTexture2UI(Texture2D m_Tex, Transform imgParent,string prefabName,UnityAction<GameObject> btnMethod,int number)
    {
        Sprite sprite = Sprite.Create(m_Tex, new Rect(0, 0, m_Tex.width, m_Tex.height), new Vector2(0, 0));
        GameObject img = null;

        //判断父节点下面是否已经添加了UIimage物体, 并且数量是否是要添加的数量
        int childCount = imgParent.childCount;
        if (childCount > number)
        {
            //已经存在了
            img = imgParent.GetChild(number).gameObject;
            img.SetActive(true);
            //隐藏多余的
            for (int i = childCount-1; i > number; i--)
            {
                imgParent.GetChild(i).gameObject.SetActive(false);
            }
        }
        else
        {
            img = MonoBehaviour.Instantiate(Resources.Load("UI/" + prefabName) as GameObject);
        }
       
        img.name = m_Tex.name;
        img.transform.SetParent(imgParent, false);

        img.GetComponent<Image>().sprite = sprite;

        img.GetComponent<Button>().onClick.RemoveAllListeners();
        img.GetComponent<Button>().onClick.AddListener(delegate() { btnMethod(img); });
    }

}
