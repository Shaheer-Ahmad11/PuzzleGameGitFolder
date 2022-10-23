using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSound : MonoBehaviour
{
    public static CheckSound instance;
    private void Start()
    {
        if (instance == null)
        { instance = this; }
    }

    public void checkit()
    {
        Debug.Log("Sound " + HomeManager.isSound);
        Debug.Log("Vibration" + HomeManager.isVibration);
    }
}
