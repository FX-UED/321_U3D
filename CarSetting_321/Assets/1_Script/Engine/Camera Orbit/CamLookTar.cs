using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamLookTar : MonoBehaviour
{
    public Transform[] cameras;
    public Transform[] target;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in cameras)
        {
            item.gameObject.SetActive(false);
        }
        StartPath(0);
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
