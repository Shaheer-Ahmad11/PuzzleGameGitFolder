using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AllGamesManager : MonoBehaviour
{
    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Allman");
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
    // public static bool IsPointerOverGameObject()
    // {
    //     //check mouse
    //     if (EventSystem.current.IsPointerOverGameObject())
    //         return true;
    //     //check touch
    //     if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
    //     {
    //         if (EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
    //             return true;
    //     }

    //     return false;
    // }
    public static bool IsPointerOverGameObject()
    {
        //check mouse
        if (EventSystem.current.IsPointerOverGameObject())
            return true;
        //check touch
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId) && EventSystem.current.currentSelectedGameObject != null)
                if (EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
                    return true;
        }

        return false;
    }
}
