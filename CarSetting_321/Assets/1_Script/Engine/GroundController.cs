using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour
{
    public float SpeedX=0;
    public float SpeedY=0.05f;

    private float deltX;
    private float deltY;
    private MeshRenderer m_material;
    public LineRenderer m_lineRender;
    private void OnEnable()
    {
        if(m_material==null)
        {
            if (GetComponent<MeshRenderer>() != null)
                m_material = GetComponent<MeshRenderer>();
        }

    }
    void Update()
    {
        deltX += CarController.curSpeed * Time.deltaTime * SpeedX;
        deltY += CarController.curSpeed * Time.deltaTime * SpeedY;

        if (m_material!=null)
        {
            m_material.material.SetTextureOffset("_MainTex", new Vector2(deltX, deltY));
        }
        else if(m_lineRender!=null)
        {
            m_lineRender.material.SetTextureOffset("_MainTex", new Vector2(deltX, deltY));
        }
    }

}
