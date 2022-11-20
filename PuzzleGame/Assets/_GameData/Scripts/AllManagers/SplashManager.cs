using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SplashManager : MonoBehaviour
{

    void Start()
    {
        Invoke("loadHome", 2f);
    }

    void loadHome()
    {
        SceneManager.LoadScene("Home");
    }
}
