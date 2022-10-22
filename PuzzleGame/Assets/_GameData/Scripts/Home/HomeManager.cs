using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeManager : MonoBehaviour
{
    public GameObject LevelSelectButtonPrefab, puzzleLevelSelectPanel;
    public Transform puzzleLevelPanel;
    public Sprite[] puzzleImage;
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
        for (int i = 0; i < puzzleImage.Length; i++)
        {
            GameObject puzzlelevelbutton = Instantiate(LevelSelectButtonPrefab, puzzleLevelPanel);
            puzzlelevelbutton.name = i.ToString();
            puzzlelevelbutton.GetComponent<Image>().sprite = puzzleImage[i];
            if (i != 0)
            {
                puzzlelevelbutton.transform.GetChild(1).gameObject.SetActive(false);
                puzzlelevelbutton.transform.GetChild(2).gameObject.SetActive(true);
            }
        }
    }



}





