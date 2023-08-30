using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeManager : MonoBehaviour
{
    public static HomeManager instance;
    public Color on, off;
    public GameObject LevelSelectButtonPrefab, puzzleLevelSelectPanel, differenceLevelSelectPanel, CardsLevelselectPanel, soundButton, vibrationButton;
    public Transform puzzleLevelPanel, differenceLevelPanel, CardsLevelPanel;
    public int totalPuzzleLevels, _currentPuzzleLevel, totalDifferenceLevels, _currentDifferenceLevel, _currentcardslevel, totalCardsLevels;

    public Text[] dateField;
    public string[] inspirationalQuotes;


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
        _currentcardslevel = PlayerPrefs.GetInt("cardslevel");
        _currentPuzzleLevel = PlayerPrefs.GetInt("puzzlelevel");
        _currentDifferenceLevel = PlayerPrefs.GetInt("level");
        if (_currentPuzzleLevel >= totalPuzzleLevels)
        {
            _currentPuzzleLevel = 1;
        }
        if (_currentDifferenceLevel >= totalDifferenceLevels || _currentDifferenceLevel < 1)
        {
            _currentDifferenceLevel = 1;
        }
        if (_currentcardslevel >= totalCardsLevels)
        {
            _currentcardslevel = 1;
        }
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
            SoundManager.instance.isSound = true;
            soundButton.transform.GetChild(0).gameObject.SetActive(false);
            soundButton.transform.GetChild(1).gameObject.SetActive(true);
            soundButton.GetComponent<Image>().color = on;

        }
        else
        {
            isSound = false;
            SoundManager.instance.isSound = false;
            soundButton.transform.GetChild(0).gameObject.SetActive(true);
            soundButton.transform.GetChild(1).gameObject.SetActive(false);
            soundButton.GetComponent<Image>().color = off;
        }
        if (PlayerPrefs.GetInt("Vibration") == 1)
        {
            isVibration = true;
            vibrationButton.transform.GetChild(0).gameObject.SetActive(false);
            vibrationButton.transform.GetChild(1).gameObject.SetActive(true);
            vibrationButton.GetComponent<Image>().color = on;
        }
        else
        {
            isVibration = false;
            vibrationButton.transform.GetChild(0).gameObject.SetActive(true);
            vibrationButton.transform.GetChild(1).gameObject.SetActive(false);
            vibrationButton.GetComponent<Image>().color = off;
        }
        foreach (var s in SoundManager.instance.sounds)
        {
            SoundManager.instance.Stop(s.name);
        }
        SoundManager.instance.Play("LoginBG");
        string _date = getDate();
       
        foreach(Text t in dateField)
        {
            t.text = _date;

        }
        // loadPuzzleLevels();
        // loadDifferenceLevel();
    }
    private string getDate()
    {
        int _day = System.DateTime.Now.Day;
        string month = System.DateTime.Now.ToString("MMMM");
        
        return month+" "+_day;
    }
    public void onPuzzleButonClick()
    {
        puzzleLevelSelectPanel.SetActive(true);

    }
    public void loadPuzzleLevelScene()
    {
        SceneManager.LoadScene("puzzleGameScene");

    }
    public void loadCardsLevel()
    {
        SceneManager.LoadScene("MatchingCards");
    }
    public void onSpotDiffButonClick()
    {
        differenceLevelSelectPanel.SetActive(true);
    }
    public void loadDifferencelevelScene()
    {
        SceneManager.LoadScene("lvlGame");
    }
    public void onSettingsClick()
    {
        if (AdNetwork.instance.isInterstitialReady)
        {
            AdNetwork.instance.showInterstitialAd();
        }
        // if (AdMobManager.instance.isInterstitialReady)
        // { AdMobManager.instance.ShowInterstitial(); }
        // else
        // { return; }
    }
    public void onLoginClick()
    {
        // if (AdMobManager.instance.isRewardedReady)
        // {
        //     AdMobManager.instance.ShowRewardBasedVideo();
        // }
        // else
        // {
        //     return;
        // }
    }

    public void loadPuzzleLevels()
    {

        _currentPuzzleLevel = PlayerPrefs.GetInt("puzzlelevel");
        // Debug.Log(_currentDifferenceLevel);
        if (_currentPuzzleLevel >= totalPuzzleLevels || _currentPuzzleLevel < 1)
        {
            _currentPuzzleLevel = 1;
            PlayerPrefs.SetInt("puzzlelevel", 1);
        }
        for (int i = 1; i <= totalPuzzleLevels; i++)
        {
            GameObject puzzlelevelbutton = Instantiate(LevelSelectButtonPrefab, puzzleLevelPanel);
            puzzlelevelbutton.name = i.ToString();
            Sprite puzzleimage = Resources.Load<Sprite>("Puzzle/" + i);
            puzzlelevelbutton.transform.GetChild(0).GetComponent<Image>().sprite = puzzleimage;
            puzzlelevelbutton.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(loadPuzzleLevelScene);
            if (i == _currentPuzzleLevel)
            {
                puzzlelevelbutton.transform.GetChild(3).gameObject.SetActive(false);
            }
            if (i < _currentPuzzleLevel)
            {
                puzzlelevelbutton.transform.GetChild(1).gameObject.SetActive(false);
                puzzlelevelbutton.transform.GetChild(2).gameObject.SetActive(false);
                puzzlelevelbutton.transform.GetChild(3).gameObject.SetActive(false);
            }
            else if (i != _currentPuzzleLevel)
            {
                puzzlelevelbutton.transform.GetChild(2).gameObject.SetActive(false);
                puzzlelevelbutton.transform.GetChild(3).gameObject.SetActive(true);
            }
        }
    }
    public void loadDifferenceLevel()
    {
        _currentDifferenceLevel = PlayerPrefs.GetInt("level");
        // Debug.Log(_currentDifferenceLevel);
        if (_currentDifferenceLevel > totalDifferenceLevels || _currentDifferenceLevel < 1)
        {
            _currentDifferenceLevel = 1;
            PlayerPrefs.SetInt("level", _currentDifferenceLevel);
        }
        for (int i = 1; i <= totalDifferenceLevels; i++)
        {
            GameObject differencelevelbutton = Instantiate(LevelSelectButtonPrefab, differenceLevelPanel);
            differencelevelbutton.name = i.ToString();
            Sprite differenceImage = Resources.Load<Sprite>("Difference/MainIcons/" + i);
            differencelevelbutton.transform.GetChild(0).GetComponent<Image>().sprite = differenceImage;
            differencelevelbutton.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(loadDifferencelevelScene);
            if (i == _currentDifferenceLevel)
            {
                differencelevelbutton.transform.GetChild(3).gameObject.SetActive(false);
                differencelevelbutton.transform.GetChild(1).gameObject.SetActive(true);
                differencelevelbutton.transform.GetChild(2).gameObject.SetActive(true);
            }
            else if (i < _currentDifferenceLevel)
            {
                differencelevelbutton.transform.GetChild(1).gameObject.SetActive(false);
                differencelevelbutton.transform.GetChild(2).gameObject.SetActive(false);
                differencelevelbutton.transform.GetChild(3).gameObject.SetActive(false);
            }
            else if (i > _currentDifferenceLevel)
            {
                differencelevelbutton.transform.GetChild(1).gameObject.SetActive(true);
                differencelevelbutton.transform.GetChild(2).gameObject.SetActive(false);
                differencelevelbutton.transform.GetChild(3).gameObject.SetActive(true);
            }
        }

    }
    public void loadMcCardsLevel()
    {
        _currentcardslevel = PlayerPrefs.GetInt("cardslevel");
        if (_currentcardslevel >= totalCardsLevels || _currentcardslevel < 1)
        {
            _currentcardslevel = 1;
            PlayerPrefs.SetInt("cardslevel", 1);
        }
        // Debug.Log(_currentcardslevel);
        for (int i = 1; i <= totalCardsLevels; i++)
        {
            GameObject Cardslevelbutton = Instantiate(LevelSelectButtonPrefab, CardsLevelPanel);
            Cardslevelbutton.name = i.ToString();
            Sprite CardsImage = Resources.Load<Sprite>("MatchingCards/" + i);
            Cardslevelbutton.transform.GetChild(0).GetComponent<Image>().sprite = CardsImage;
            Cardslevelbutton.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(loadCardsLevel);
            if (i == _currentcardslevel)
            {
                Cardslevelbutton.transform.GetChild(3).gameObject.SetActive(false);
            }
            else if (i < _currentcardslevel)
            {
                Cardslevelbutton.transform.GetChild(1).gameObject.SetActive(false);
                Cardslevelbutton.transform.GetChild(2).gameObject.SetActive(false);
                Cardslevelbutton.transform.GetChild(3).gameObject.SetActive(false);
            }
            else if (i != _currentcardslevel)
            {
                Cardslevelbutton.transform.GetChild(2).gameObject.SetActive(false);
                Cardslevelbutton.transform.GetChild(3).gameObject.SetActive(true);
            }
        }

    }
    public void onMatchingCardsBtnClick()
    {
        CardsLevelselectPanel.SetActive(true);
    }
    public void onSoundButtonClick()
    {
        if (isSound)
        {
            PlayerPrefs.SetInt("Sound", 0);
            isSound = false;
            SoundManager.instance.isSound = false;
            foreach (var s in SoundManager.instance.sounds)
            {
                SoundManager.instance.Stop(s.name);
            }
            soundButton.transform.GetChild(0).gameObject.SetActive(true);
            soundButton.transform.GetChild(1).gameObject.SetActive(false);
            soundButton.GetComponent<Image>().color = off;
        }
        else
        {
            PlayerPrefs.SetInt("Sound", 1);
            isSound = true;
            SoundManager.instance.isSound = true;
            SoundManager.instance.Play("LoginBG");
            soundButton.transform.GetChild(0).gameObject.SetActive(false);
            soundButton.transform.GetChild(1).gameObject.SetActive(true);
            soundButton.GetComponent<Image>().color = on;
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
            vibrationButton.GetComponent<Image>().color = off;
        }
        else
        {
            PlayerPrefs.SetInt("Vibration", 1);
            isVibration = true;
            vibrationButton.transform.GetChild(0).gameObject.SetActive(false);
            vibrationButton.transform.GetChild(1).gameObject.SetActive(true);
            vibrationButton.GetComponent<Image>().color = on;
        }
        CheckSound.instance.checkit();
    }
    public void onClickCategory(Text inspirationalQuotesText)
    {
        inspirationalQuotesText.text = inspirationalQuotes[Random.Range(0, inspirationalQuotes.Length)];
    }

    public void onShopButtonClick()
    {
        foreach (var s in SoundManager.instance.sounds)
        {
            SoundManager.instance.Stop(s.name);
        }
        SoundManager.instance.Play("ShopBG");
    }
    public void onShopCloseButtonClick()
    {
        foreach (var s in SoundManager.instance.sounds)
        {
            SoundManager.instance.Stop(s.name);
        }
        SoundManager.instance.Play("LoginBG");
    }
}