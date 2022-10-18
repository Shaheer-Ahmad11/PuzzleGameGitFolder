using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PanZoom : MonoBehaviour
{
    Vector3 touchStart;
    public float ZoomMax, ZoomMin;
    bool lockpanzoom, zooming, zoomed;

    void Update()
    {
        // if (DragDrop.instance.e == null && !lockpanzoom)
        {
            if (Camera.main.orthographicSize < 5)
            {
                zoomed = false;
            }
            else
            {
                zoomed = true;
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (!zooming)
                {
                    touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
            }
            //Zooming with touch
            if (Input.touchCount == 2)
            {
                zooming = true;
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);
                Vector3 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector3 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
                float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float currentMagnitude = (touchZero.position - touchOne.position).magnitude;
                float difference = currentMagnitude - prevMagnitude;
                zoom(difference * 0.01f);

            }
            else if (Input.GetMouseButton(0))
            {
                if (!zooming && !zoomed)
                {
                    Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Camera.main.transform.position += direction;
                }
            }
            if (Input.touchCount <= 1)
            {
                Invoke("checkZooming", 0.3f);
            }
            zoom(Input.GetAxis("Mouse ScrollWheel"));
        }
    }
    void checkZooming()
    {
        zooming = false;
    }
    private void zoom(float increment)
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, ZoomMin, ZoomMax);
    }
    public void lockPanZoom()
    {
        GameObject lockbtn = EventSystem.current.currentSelectedGameObject;
        if (!lockpanzoom)
        {
            lockpanzoom = true;
            lockbtn.GetComponent<Image>().color = Color.red;
        }
        else
        {
            lockpanzoom = false;
            lockbtn.GetComponent<Image>().color = Color.white;
        }
    }
}