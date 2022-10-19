using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeManager : MonoBehaviour
{






    public void onPuzzleButonClick()
    {
        SceneManager.LoadScene("puzzleGameScene");
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
}





