using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeManager : MonoBehaviour
{
    public GameObject LevelSelectButtonPrefab, puzzleLevelSelectPanel;
    public Transform puzzleLevelPanel;
    public int totalPuzzleLevels, _currentLevel;

    private void Start()
    {
        loadPuzzleLevels();
    }
    public void onPuzzleButonClick()
    {
        puzzleLevelSelectPanel.SetActive(true);
        // SceneManager.LoadScene("puzzleGameScene");

    }
    public void onSpotDiffButonClick()
    {
        SceneManager.LoadScene("lvlGame");
    }
    public void onSettingsClick()
    {
        if (AdMobManager.instance.isInterstitialReady)
        { AdMobManager.instance.ShowInterstitial(); }
        else
        { return; }
    }
    public void onLoginClick()
    {
        if (AdMobManager.instance.isRewardedReady)
        {
            AdMobManager.instance.ShowRewardBasedVideo();
        }
        else
        {
            return;
        }
    }

    void loadPuzzleLevels()
    {
        for (int i = 0; i <= totalPuzzleLevels; i++)
        {
            if (i != 0)
            {
                GameObject puzzlelevelbutton = Instantiate(LevelSelectButtonPrefab, puzzleLevelPanel);
                puzzlelevelbutton.name = i.ToString();
                Sprite puzzleimage = Resources.Load<Sprite>("Puzzle/" + i);
                puzzlelevelbutton.transform.GetChild(0).GetComponent<Image>().sprite = puzzleimage;
                if (i < _currentLevel)
                {
                    puzzlelevelbutton.transform.GetChild(1).gameObject.SetActive(false);
                    puzzlelevelbutton.transform.GetChild(2).gameObject.SetActive(false);
                    puzzlelevelbutton.transform.GetChild(3).gameObject.SetActive(false);
                }

                else if (i != _currentLevel)
                {
                    puzzlelevelbutton.transform.GetChild(2).gameObject.SetActive(false);
                    puzzlelevelbutton.transform.GetChild(3).gameObject.SetActive(true);
                }
            }


        }
    }



}





