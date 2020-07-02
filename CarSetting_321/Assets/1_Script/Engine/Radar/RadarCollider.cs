using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarCollider : MonoBehaviour
{
    private RadarColliderGroup group;
    public RadarDistance mDistance;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }
    public void Init(RadarColliderGroup RCgroup)
    {
        group = RCgroup;

        if (GetComponent<MeshCollider>() == null)
        {
            gameObject.AddComponent<MeshCollider>();
            GetComponent<MeshCollider>().convex = true;
            GetComponent<MeshCollider>().isTrigger = true;
        }
        if (GetComponent<Rigidbody>() == null)
        {
            gameObject.AddComponent<Rigidbody>();
            GetComponent<Rigidbody>().useGravity = false;
        }
        if (name.Contains("near"))
        {
            mDistance = RadarDistance.near;
        }
        else if (name.Contains("middle"))
        {

            mDistance = RadarDistance.middle;
        }
        else if (name.Contains("far"))
        {

            mDistance = RadarDistance.far;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ai"))
        {
            group.ChangeRadarPlane(mDistance,true);
            ChildRadarControl(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ai"))
        {
            group.ChangeRadarPlane(mDistance, false);
            ChildRadarControl(true);
        }
    }

    private void ChildRadarControl(bool isShow)
    {
        if(transform.childCount>0)
        {
            transform.GetChild(0).gameObject.SetActive(isShow);
        }
    }
}
