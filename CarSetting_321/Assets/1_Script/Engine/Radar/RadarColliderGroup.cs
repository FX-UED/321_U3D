using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum RadarDistance
{
    near,
    middle,
    far,
    reset
}
public class RadarColliderGroup : MonoBehaviour
{
    public float speed=1f;
    public MeshRenderer mRender;
    public Color color_far = Color.green;
    public Color color_middle = Color.yellow;
    public Color color_near = Color.red;

    public float clip_far = 0.16f;
    public float clip_middle = 0.11f;
    public float clip_near = 0.05f;
    public float clip_reset = 0.3f;
    public float m_clipPos = 0;



    private RadarDistance m_Type;
    // Start is called before the first frame update
    private void Awake()
    {
        mRender = GetComponent<MeshRenderer>();
        m_Type =  RadarDistance.reset;
        mRender.enabled = false;
    }
    void Start()
    {

        Transform[] childrenTrans = transform.GetComponentsInChildren<Transform>(true);
        foreach (var item in childrenTrans)
        {
            if (item.name.Contains("collider"))
            {
                item.gameObject.AddComponent<RadarCollider>();
                item.gameObject.GetComponent<RadarCollider>().Init(this);
            }
        }

    }

    public void ChangeRadarPlane(RadarDistance mType, bool isShow)
    {
        if (isShow == false) { Debug.Log(mType+" 离开"); }
        else { Debug.Log(mType + " 进入"); }

        mRender.enabled = isShow;
        if(mType== RadarDistance.far && isShow==false)
        {
            m_clipPos = clip_reset;
        }

        m_Type = mType;
    }
    private void ChangePlaneLangth()
    {
        if (m_Type == RadarDistance.far)
        {
            if(Mathf.Abs(clip_far- m_clipPos)>0.02f)
            {
                m_clipPos += (clip_far - m_clipPos)* 0.05f * speed;
                mRender.material.SetColor("_Color", color_far);//变色
            }
            else
            {
                m_clipPos = clip_far;
                
                mRender.material.SetFloat("_ClipPosition", m_clipPos); return;
            }
        }
        else if (m_Type == RadarDistance.middle)
        {
            if (Mathf.Abs(clip_middle - m_clipPos) > 0.02f)
            {
                m_clipPos += (clip_middle - m_clipPos) * 0.05f * speed;
            }
            else
            {
                m_clipPos = clip_middle;
                mRender.material.SetColor("_Color", color_middle);//变色
                mRender.material.SetFloat("_ClipPosition", m_clipPos); return;
            }
        }
        else if (m_Type == RadarDistance.near)
        {
            if (Mathf.Abs(clip_near - m_clipPos) > 0.02f)
            {
                m_clipPos += (clip_near - m_clipPos) * 0.05f * speed;
            }
            else
            {
                m_clipPos = clip_near;
                mRender.material.SetColor("_Color", color_near);//变色
                mRender.material.SetFloat("_ClipPosition", m_clipPos); return;
            }
        }
        else
        {
            mRender.enabled = false;
            mRender.material.SetFloat("_ClipPosition", clip_reset);
            return;
        }
        mRender.material.SetFloat("_ClipPosition", m_clipPos);
    }
    private void FixedUpdate()
    {
        ChangePlaneLangth();
    }
}
