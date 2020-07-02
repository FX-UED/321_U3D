using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyManager : Singleton<CopyManager>
{
    public void CopyAll(Transform from,Transform to)
    {
        Transform[] sonFrom = from.GetComponentsInChildren<Transform>(true);
        Transform[] sonTo = to.GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < sonFrom.Length; i++)
        {
            for (int ii = 0; ii < sonTo.Length; ii++)
            {
                if(string.Equals(sonFrom[i].name,sonTo[ii].name))
                {
                    CopyComponents(sonFrom[i], sonTo[ii]);
                    break;
                }
            }
        }
    }
    /// <summary>
    /// 忽略了多维子材质，待优化
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    private void CopyComponents(Transform from,Transform to)
    {
        to.gameObject.SetActive(from.gameObject.activeSelf);

        if (to.GetComponent<MeshRenderer>()!=null)
        {
            to.GetComponent<MeshRenderer>().material.shader = from.GetComponent<MeshRenderer>().sharedMaterial.shader;
            to.GetComponent<MeshRenderer>().material.CopyPropertiesFromMaterial(from.GetComponent<MeshRenderer>().sharedMaterial);
        }
        
    }
}
