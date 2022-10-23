using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeManager : MonoBehaviour
{
    public static HomeManager instance;
    public GameObject LevelSelectButtonPrefab, puzzleLevelSelectPanel, soundButton, vibrationButton;
    public Transform puzzleLevelPanel;
    public int totalPuzzleLevels, _currentLevel;

    public static bool isSound, isVibration;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        if (!PlayerPrefs.HasKey("Sound"))
        {
            PlayerPrefs.SetInt("Sound", 1);
        }
        if (!PlayerPrefs.HasKey("Vibration"))
        {
            PlayerPrefs.SetInt("Vibration", 1);
        }
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            isSound = true;
            soundButton.transform.GetChild(0).gameObject.SetActive(false);
            soundButton.transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            isSound = false;
            soundButton.transform.GetChild(0).gameObject.SetActive(true);
            soundButton.transform.GetChild(1).gameObject.SetActive(false);
        }
        if (PlayerPrefs.GetInt("Vibration") == 1)
        {
            isVibration = true;
            vibrationButton.transform.GetChild(0).gameObject.SetActive(false);
            vibrationButton.transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            isVibration = false;
            vibrationButton.transform.GetChild(0).gameObject.SetActive(true);
            vibrationButton.transform.GetChild(1).gameObject.SetActive(false);
        }
        loadPuzzleLevels();
    }
    public void onPuzzleButonClick()
    {
        puzzleLevelSelectPanel.SetActive(true);

    }
    public void loadPuzzleLevel(){
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
                puzzlelevelbutton.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(loadPuzzleLevel);
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

    public void onSoundButtonClick()
    {
        if (isSound)
        {
            PlayerPrefs.SetInt("Sound", 0);
            isSound = false;
            soundButton.transform.GetChild(0).gameObject.SetActive(true);
            soundButton.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            PlayerPrefs.SetInt("Sound", 1);
            isSound = true;
            soundButton.transform.GetChild(0).gameObject.SetActive(false);
            soundButton.transform.GetChild(1).gameObject.SetActive(true);
        }
        CheckSound.instance.checkit();
    }
    public void onVibrationButtonClick()
    {
        if (isVibration)
        {

            PlayerPrefs.SetInt("Vibration", 0);
            isVibration = false;
            vibrationButton.transform.GetChild(0).gameObject.SetActive(true);
            vibrationButton.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            PlayerPrefs.SetInt("Vibration", 1);
            isVibration = true;
            vibrationButton.transform.GetChild(0).gameObject.SetActive(false);
            vibrationButton.transform.GetChild(1).gameObject.SetActive(true);
        }
        CheckSound.instance.checkit();
    }

}





