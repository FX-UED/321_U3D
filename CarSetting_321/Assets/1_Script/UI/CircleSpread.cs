using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleSpread : MonoBehaviour
{
    public Image circleMask;
    public Image[] circles;
    public float speed;
    private float maxThanMask=2;
    private Vector2 startCircleSize;
    // Start is called before the first frame update
    void Start()
    {
        startCircleSize = circles[0].rectTransform.sizeDelta;
    }
    private void SpreadMethod()
    {
        for (int i = 0; i < circles.Length; i++)
        {
            circles[i].rectTransform.sizeDelta = new Vector2(circles[i].rectTransform.sizeDelta.x+speed, circles[i].rectTransform.sizeDelta.y+speed);
            if (circles[i].rectTransform.sizeDelta.x> circleMask.rectTransform.sizeDelta.x+ maxThanMask)
            {
                circles[i].rectTransform.sizeDelta = startCircleSize;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        SpreadMethod();
    }
}
