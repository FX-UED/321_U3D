using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingControl : MonoBehaviour
{

    private MeshRenderer mRender;
    //public float speed;
    //public float startSize=0.05f;
    //public float sizeMax=0.25f;
    public Color safeColor;
    public Color dangerColor;
    public float safeBrightness = 1f;
    public float dangerBrightness = 1.5f;
    private string ring = "_Radius";
    private string ring2 = "_Radius2";
    private string ring3 = "_Radius3";
    // Start is called before the first frame update
    void Start()
    {
        mRender = GetComponent<MeshRenderer>();
        mRender.material.SetColor("_Color", safeColor);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("ai"))
        {
            Debug.Log("trigger ai");
            mRender.material.SetColor("_Color", dangerColor);
            mRender.material.SetFloat("_Brightness", dangerBrightness);

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ai"))
        {
            Debug.Log("leave ai");
            mRender.material.SetColor("_Color", safeColor);
            mRender.material.SetFloat("_Brightness", safeBrightness);
        }
    }
    private void SpreadMethod()
    {
        //mRender.material.SetFloat(ring, speed + mRender.material.GetFloat(ring));
        //if(mRender.material.GetFloat(ring)> sizeMax)
        //{
        //    mRender.material.SetFloat(ring, startSize);
        //}

        //mRender.material.SetFloat(ring2, speed + mRender.material.GetFloat(ring2));
        //if (mRender.material.GetFloat(ring2) > sizeMax)
        //{
        //    mRender.material.SetFloat(ring2, startSize);
        //}

        //mRender.material.SetFloat(ring3, speed + mRender.material.GetFloat(ring3));
        //if (mRender.material.GetFloat(ring3) > sizeMax)
        //{
        //    mRender.material.SetFloat(ring3, startSize);
        //}

    }
    // Update is called once per frame
    void Update()
    {
        //SpreadMethod();
    }
}
