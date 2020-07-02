using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 以X轴负方向 为运动方向的跑酷运动方案
/// </summary>
public class RunController : MonoBehaviour
{
    #region 变量

    
    public Transform singleTerrain;
    public int terrainCount=15;
    /// <summary>
    /// 地形链表
    /// </summary>
    public List<Transform> m_terrainList;

    /// <summary>
    /// 地形长度
    /// </summary>
    public float m_terrainSize = 99f;

    /// <summary>
    /// 运动速度
    /// </summary>
    public float m_speed = 0.6f;

    /// <summary>
    /// 运动朝向
    /// </summary>
    public Vector3 offset=new Vector3(0,0,-1000);

    /// <summary>
    /// 重置地形的临界点
    /// </summary>
    private float minEdge;

    #endregion

    void Start()
    {
        InitBuilding();
        InitTerrian();
    }


    /// <summary>
    /// 动态随机建筑方法
    /// </summary>
    private void InitBuilding()
    {
        if (!singleTerrain) { Debug.LogError("缺少地形变量"); return; }

        m_terrainList.Add(singleTerrain);

        //复制两个地形
        for (int i = 0; i < terrainCount; i++)
        {
            GameObject objTerrain0 = Instantiate(singleTerrain.gameObject, singleTerrain.position, singleTerrain.rotation);
            objTerrain0.transform.SetParent(singleTerrain.parent, false);
            m_terrainList.Add(objTerrain0.transform);
        }

        //初始化地形的位置
        for (int i = 0; i < m_terrainList.Count; i++)
        {
            m_terrainList[i].transform.localPosition = new Vector3(0, 0, ( i-(int)(m_terrainList.Count/2)) * m_terrainSize);
        }
    }
    private void InitTerrian()
    {
        //设置地形移动的边界
        minEdge = m_terrainList[0].localPosition.z - m_terrainSize;
    }
    /// <summary>
    /// 地形的移动
    /// </summary>
    private void TerrianMove()
    {
        if(m_terrainList.Count<=0 || CarController.curSpeed <= 0)
        {
            return;
        }
        foreach (var item in m_terrainList)
        {
            item.localPosition = Vector3.MoveTowards(item.localPosition, offset, Time.deltaTime * CarController.curSpeed * m_speed);
            
        }
        //重置地形位置
        int index = m_terrainList.Count - 1;
        if (m_terrainList[0].localPosition.z <= minEdge)
        {
            m_terrainList[0].localPosition = m_terrainList[index].localPosition + new Vector3(0,0,m_terrainSize * 1);
            Transform g = m_terrainList[0];
            m_terrainList.RemoveAt(0);
            m_terrainList.Insert(index, g);

            //建筑的随机变化
            //g.GetComponent<SingleTerrainControl>().RandomBuildings();
        }
    }
    void FixedUpdate()
    {
        //运动
        TerrianMove();
    }

}
