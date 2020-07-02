using UnityEngine;
using System.Collections;
public class UltimateOrbitCamera : MonoBehaviour
{
    public Transform target;
    private float distance = 10f;
    public float maxDistance = 25f;
    public float minDistance = 1f;
    public bool mouseControl = true;
    public string mouseAxisX = "Mouse X";
    public string mouseAxisY = "Mouse Y";
    public string mouseAxisZoom = "Mouse ScrollWheel";
    public bool keyboardControl = false;
    public string kbPanAxisX = "Horizontal";
    public string kbPanAxisY = "Vertical";
    public bool kbUseZoomAxis = false;
    public KeyCode zoomInKey = KeyCode.R;
    public KeyCode zoomOutKey = KeyCode.F;
    public string kbZoomAxisName = "";
    public float initialAngleX = 0f;
    public float initialAngleY = 0f;
    public bool invertAxisX = false;
    public bool invertAxisY = false;
    public bool invertAxisZoom = false;
    public float xSpeed = 0.4f;
    public float ySpeed = 0.4f;
    public float zoomSpeed = 5f;
    public float dampeningX = 0.9f;
    public float dampeningY = 0.9f;
    public float smoothingZoom = 0.1f;
    public bool limitY = true;
    public float yMinLimit = 0f;
    public float yMaxLimit = 85f;
    public float yLimitOffset = 0f;
    public bool limitX = true;
    public float xMinLimit = -20f;
    public float xMaxLimit = 20f;
    public float xLimitOffset = 0f;
    public bool clickToRotate = true;
    public bool leftClickToRotate = true;
    public bool rightClickToRotate = false;
    public bool autoRotateOn = false;
    public bool autoRotateReverse = false;
    public float autoRotateSpeed = 0.1f;
    public bool SpinEnabled = false;
    public bool spinUseAxis = false;
    public KeyCode spinKey = KeyCode.LeftControl;
    public string spinAxis = "";
    public float maxSpinSpeed = 3f;
    private bool spinning = false;
    private float spinSpeed = 0f;
    public bool cameraCollision = false;
    public float collisionRadius = 0.25f;
    private float xVelocity = 0f;
    private float yVelocity = 0f;
    private float zoomVelocity = 0f;
    private float targetDistance = 10f;
    private float x = 0f;
    private float y = 0f;
    private Vector3 position;
    [HideInInspector]
    public int invertXValue = 1;
    [HideInInspector]
    public int invertYValue = 1;
    [HideInInspector]
    public int invertZoomValue = 1;
    [HideInInspector]
    public int autoRotateReverseValue = 1;
    private Ray ray;
    private RaycastHit hit;
    private Transform _transform;
    //--------------yan qi----------------
    [Header("点击缩放的距离")]
    public float pointZoomDistance=0;
    private bool isPointZoom = false;
//#if UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8
//    private float pinchDist = 0f;
//#endif
    private void Awake()
    {
       
    }
    public void SetTarget(Vector3 tar)
    {
        if(target!=null)
        {
            target.transform.position = tar;
            enabled = true;
            return;
        }

        GameObject obj = new GameObject("MainCamera_Target");
        obj.transform.position = tar;
        target = obj.transform;
        enabled = true;
    }
    void OnEnable()
    {
        //transform.localEulerAngles = new Vector3(0, 90, 0);
        //transform.position = new Vector3(-6, 0.9f, 0);
        targetDistance = distance;
        if (invertAxisX)
        {
            invertXValue = -1;
        }
        else
        {
            invertXValue = 1;
        }
        if (invertAxisY)
        {
            invertYValue = -1;
        }
        else
        {
            invertYValue = 1;
        }
        if (invertAxisZoom)
        {
            invertZoomValue = -1;
        }
        else
        {
            invertZoomValue = 1;
        }
        if (autoRotateOn)
        {
            autoRotateReverseValue = -1;
        }
        else
        {
            autoRotateReverseValue = 1;
        }
        _transform = transform;
        if (GetComponent<Rigidbody>() != null)
            GetComponent<Rigidbody>().freezeRotation = true;
        x = initialAngleX;
        y = initialAngleY;
        //_transform.Rotate(new Vector3(0f, initialAngleX, 0f), Space.World);
        //_transform.Rotate(new Vector3(initialAngleY, 0f, 0f), Space.Self);
        //position = _transform.rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;
    }
    private void Start()
    {
        //if (target == null)
        //{
        //    GameObject obj = new GameObject("MainCamera_Target");
        //    obj.transform.position = Vector3.zero;
        //    target = obj.transform;

        //}
        //distance = Vector3.Distance(target.position, transform.position);
    }
    #region 点击后缩放相机距离
    private float curTargetDis = 0;
    private float curMinDis;
    void PointZoomIn(bool isDown)
    {
  
        if (isDown)
        {
            if (Mathf.Abs(xVelocity) < 0.3f && Mathf.Abs(yVelocity) < 0.3f)
            {
                return;
            }
            if (curTargetDis == 0)
            {
                curTargetDis = targetDistance;
                curMinDis = curTargetDis - pointZoomDistance;
            }
            isPointZoom = true;
            //zoom in
            zoomVelocity -= (zoomSpeed / 10) * invertZoomValue;
            if (targetDistance + zoomVelocity < curMinDis)
            {
                zoomVelocity = curMinDis - targetDistance;
            }
        }
        else
        {
            if (!isPointZoom) { return; }
            //zoom out
            zoomVelocity += (zoomSpeed / 10) * invertZoomValue;
            if (targetDistance + zoomVelocity > curTargetDis)
            {
                zoomVelocity = curTargetDis - targetDistance;
                curTargetDis = 0;
                isPointZoom = false;
            }
        }
    }
    #endregion
    void Update()
    {
        if (target != null)
        {

            #region Input
            if (autoRotateOn)
            {
                xVelocity += autoRotateSpeed * autoRotateReverseValue * Time.deltaTime;
            }
//#if UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8
//            if (Application.platform != RuntimePlatform.WindowsEditor)
//            {
//                if (Input.touches.Length == 1)
//                {
//                    xVelocity += Input.GetTouch(0).deltaPosition.x * xSpeed * invertXValue * 0.2f;
//                    yVelocity -= Input.GetTouch(0).deltaPosition.y * ySpeed * invertYValue * 0.2f;
//                    PointZoomIn(true);
//                }
//                else if (Input.touches.Length == 0)
//                {
//                    PointZoomIn(false);
//                }
//            }
//#endif
//#if UNITY_EDITOR_WIN || UNITY_WEBGL
            if (mouseControl)
            {
                if (!clickToRotate || ((leftClickToRotate && Input.GetMouseButton(0)) || (rightClickToRotate && Input.GetMouseButton(1))))
                {
                    xVelocity += Input.GetAxis(mouseAxisX) * xSpeed * invertXValue;
                    yVelocity -= Input.GetAxis(mouseAxisY) * ySpeed * invertYValue;
                    PointZoomIn(true);
                    spinning = false;
                }
                else
                {
                    PointZoomIn(false);
                }
                zoomVelocity -= Input.GetAxis(mouseAxisZoom) * zoomSpeed * invertZoomValue;
            }

//#endif
            if (SpinEnabled && ((mouseControl && clickToRotate) || keyboardControl))
            {
                if ((spinUseAxis && Input.GetAxis(spinAxis) != 0) || !spinUseAxis && Input.GetKey(spinKey))
                {
                    spinning = true;
                    spinSpeed = Mathf.Min(xVelocity, maxSpinSpeed);
                }
                if (spinning)
                {
                    xVelocity = spinSpeed;
                }
            }
            #endregion

            #region Apply_Rotation_And_Position
            if (limitX)
            {
                if (x + xVelocity < xMinLimit + xLimitOffset)
                {
                    xVelocity = (xMinLimit + xLimitOffset) - x;
                }
                else if (x + xVelocity > xMaxLimit + xLimitOffset)
                {
                    xVelocity = (xMaxLimit + xLimitOffset) - x;
                }
                x += xVelocity;
                _transform.Rotate(new Vector3(0f, xVelocity, 0f), Space.World);
            }
            else
            {
                _transform.Rotate(new Vector3(0f, xVelocity, 0f), Space.World);
            }
            if (limitY)
            {
                if (y + yVelocity < yMinLimit + yLimitOffset)
                {
                    yVelocity = (yMinLimit + yLimitOffset) - y;
                }
                else if (y + yVelocity > yMaxLimit + yLimitOffset)
                {
                    yVelocity = (yMaxLimit + yLimitOffset) - y;
                }
                y += yVelocity;
                _transform.Rotate(new Vector3(yVelocity, 0f, 0f), Space.Self);
            }
            else
            {
                _transform.Rotate(new Vector3(yVelocity, 0f, 0f), Space.Self);
            }
//#if UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8
//            if (Input.touchCount == 2)
//            {
//                if (pinchDist == 0f)
//                {
//                    pinchDist = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
//                }
//                else
//                {
//                    targetDistance += ((pinchDist - (float)Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position)) * 0.01f) * zoomSpeed * invertZoomValue;
//                    pinchDist = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
//                }
//                if (targetDistance < minDistance)
//                {
//                    targetDistance = minDistance;
//                }
//                else if (targetDistance > maxDistance)
//                {
//                    targetDistance = maxDistance;
//                }
//            }
//            else
//            {
//                pinchDist = 0f;
//            }
//#endif
            if (targetDistance + zoomVelocity < minDistance)
            {
                zoomVelocity = minDistance - targetDistance;
            }
            else if (targetDistance + zoomVelocity > maxDistance)
            {
                zoomVelocity = maxDistance - targetDistance;
            }
            targetDistance += zoomVelocity;
            distance = Mathf.Lerp(distance, targetDistance, smoothingZoom);
            if (cameraCollision)
            {
                ray = new Ray(target.position, (_transform.position - target.position).normalized);
                if (Physics.SphereCast(ray.origin, collisionRadius, ray.direction, out hit, distance))
                {
                    distance = hit.distance;
                }
            }
            #endregion
            position = _transform.rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;
            _transform.position = position;
            if (!SpinEnabled || !spinning)
                xVelocity *= dampeningX;
            yVelocity *= dampeningY;
            zoomVelocity = 0;
        }
        else
        {
            Debug.LogWarning("Orbit Cam - No Target Given");
            enabled = false;
        }
    }
}