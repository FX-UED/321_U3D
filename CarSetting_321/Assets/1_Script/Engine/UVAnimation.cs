using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVAnimation : MonoBehaviour
{
    public float SpeedX = 0;
    public float SpeedY = 0;
    private float deltX;
    private float deltY;
    public MeshRenderer m_material;
    public LineRenderer m_lineMat; 
    // Start is called before the first frame update
    void Start()
    {
       // if (m_material == null) m_material = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (m_material)
        {
            deltX += SpeedX * Time.deltaTime;
            deltY += SpeedY * Time.deltaTime;
            m_material.material.SetTextureOffset("_MainTex", new Vector2(deltX, deltY));
        }
        else if(m_lineMat)
        {
            deltX += SpeedX * Time.deltaTime;
            deltY += SpeedY * Time.deltaTime;
            m_lineMat.material.SetTextureOffset("_MainTex", new Vector2(deltX, deltY));
        }
    }
}
