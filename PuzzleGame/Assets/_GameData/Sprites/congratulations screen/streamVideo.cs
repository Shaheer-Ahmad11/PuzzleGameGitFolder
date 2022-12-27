
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.IO;



public class streamVideo : MonoBehaviour
{

    public VideoPlayer videoPlayer;

    public void Start()
    {
        Debug.Log("Video Enabled");
        gameObject.transform.GetChild(3).GetComponent<VideoPlayer>().Play();
    }


}