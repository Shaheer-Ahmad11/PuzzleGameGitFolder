using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleGameManager : MonoBehaviour
{



    public void onBackButtonClick()
    {
        SceneManager.LoadScene("Home");
    }
}
