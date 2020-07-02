using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager:Singleton<InputManager>
{

    public bool InputDown(bool ui = true)
    {
#if (UNITY_EDITOR|| UNITY_WEBPLAYER || UNITY_STANDALONE_WIN)
        return Input.GetMouseButtonDown(0);
#elif (UNITY_ANDROID || UNITY_IPHONE)
       if(Input.touchCount > 0)
       {
           return Input.GetTouch(0).phase == TouchPhase.Began;
       }else
       {
           return false;
       }
#endif
        return false;
    }
    public bool InputUp()
    {
#if (UNITY_EDITOR || UNITY_WEBPLAYER || UNITY_STANDALONE_WIN)
        return Input.GetMouseButtonUp(0);
#elif (UNITY_ANDROID || UNITY_IPHONE)
       if(Input.touchCount > 0)
       {
           return Input.GetTouch(0).phase == TouchPhase.Ended;
       }else
       {
           return false;
       }
#endif
        return false;
    }
    public bool InputClick()
    {
#if (UNITY_EDITOR||UNITY_WEBPLAYER || UNITY_STANDALONE_WIN)
        return Input.GetMouseButton(0);
#elif (UNITY_ANDROID || UNITY_IPHONE)
       if(Input.touchCount > 0)
       {
           return Input.GetTouch(0).phase == TouchPhase.Ended;
       }
       else
       {
           return false;
       }
#endif
        return false;
    }
    public Vector3 InputPostion()
    {
#if (UNITY_EDITOR||UNITY_WEBPLAYER || UNITY_STANDALONE_WIN)
        return Input.mousePosition;
#elif (UNITY_ANDROID || UNITY_IPHONE)
       if(Input.touchCount > 0)
       {
           return  new Vector3(Input.GetTouch(0).position.x,Input.GetTouch(0).position.y);
       }
       else
       {
           return Vector3.zero;
       }
#endif
        return Vector3.zero;
    }
    public bool twoTouchPointDown()
    {
#if (UNITY_ANDROID || UNITY_IPHONE)
        if (Input.touchCount == 2)
        {
            if ((Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Stationary) && Input.GetTouch(1).phase == TouchPhase.Began)
            {
                return true;
            }
        }
#endif
        return false;
    }
    public Vector3[] getTwoTouchPostion()
    {
#if (UNITY_ANDROID || UNITY_IPHONE)
        if (Input.touchCount == 2)
        {
            Vector3[] a = new Vector3[2];
            a[0] = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
            a[1] = new Vector3(Input.GetTouch(1).position.x, Input.GetTouch(1).position.y);
            return a;
        }
        else
        {
            return null;
        }
#else
        return null;
#endif
    }
    public bool twoTouchPointMove()
    {
#if (UNITY_ANDROID || UNITY_IPHONE)
        if (Input.touchCount == 2)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                return true;
            }
        }
#endif
        return false;
    }
    public bool twoTouchPointUp()
    {
#if (UNITY_ANDROID || UNITY_IPHONE)
        if (Input.touchCount == 2)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(1).phase == TouchPhase.Ended)
            {
                return true;
            }
        }
#endif
        return false;
    }
    public GameObject GetRaycastGameObject(Camera startCamera, Vector3 point)
    {
        if (startCamera == null) { return null; }

        Ray ray = startCamera.ScreenPointToRay(point);
        RaycastHit hit;
        //当射线碰撞到对象时
        if (Physics.Raycast(ray, out hit))
        {
            return hit.transform.gameObject;
        }
        return null;
    }
    public Vector3 GetRaycastPoint(Camera startCamera, Vector3 point)
    {
        Ray ray = startCamera.ScreenPointToRay(point);
        RaycastHit hit;
        //当射线碰撞到对象时
        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }
        return new Vector3();
    }
}
