using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CamLookTargetNew : MonoBehaviour
{
 public Transform[] cameras;
    public Transform[] target;
    // Start is called before the first frame update
    void Awake()
    {
        StartPathAni("0");
    }
    private void OnEnable()
    {
        AppDelegate.CameraPathAni += StartPathAni;
    }
    private void OnDisable()
    {
        AppDelegate.CameraPathAni -= StartPathAni;
    }
    private void StartPathAni(string s)
    {
        if(s=="1")
        {
            StartPath(0);
        }
        else
        {
            foreach (var item in cameras)
            {
                item.gameObject.SetActive(false);
             
            }
        }
    }
    public void StartPath(int camNum)
    {
     
        foreach (var item in cameras)
        {
            item.gameObject.SetActive(false);
           
        }
        if(camNum>= cameras.Length)
        {
            Debug.Log("所有的相机路径走完");
            return;
        }
        cameras[camNum].gameObject.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].LookAt(target[i]);
        }
    }
}
