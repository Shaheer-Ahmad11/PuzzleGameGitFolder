using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool debugMod = true, isoverobject;

    public int debugJumpToLevel = -99;

    public Sprite[] images;

    public Sprite spRemainingUICheck;

    public Sprite spWinStar;

    public GameObject[] pointsPanelList;

    public static int level;

    public static int remainingPoints;

    public static int stars;

    public static int levelStars;

    public static GameManager REF;

    private int touchCounter;

    private GameObject levelImageA;

    private GameObject levelImageB;

    private GameObject objWin;
    GameObject Home, restartLevel, cancleLevel;
    public GameObject heartParent;
    public int heartCount;
    private GameObject winPanel;
    private GameObject restartPanel;
    private string[] GObjs = { "Buttonhint", "Images", "UI Remaining" };
    public GameObject[] GObjsList = new GameObject[3];
    public GameObject pointsPanel;
    bool isPlayed;
    [SerializeField] private GameObject[] UIObjects;
    private void Start()
    {
        if (HomeManager.isSound)
        {
            foreach (var s in SoundManager.instance.sounds)
            {
                SoundManager.instance.Stop(s.name);
            }
            SoundManager.instance.Play("DifferenceBG");
        }
        if (REF == null)
        {
            Object.DontDestroyOnLoad(base.gameObject);
            REF = this;
            LoadLevel();
            LoadStars();
            ShowStars();
            if (debugMod && debugJumpToLevel > 0)
            {
                level = debugJumpToLevel - 1;
            }
            SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
            InitLevel();
            PlaySceneMusic(SceneManager.GetActiveScene());
        }
        else
        {
            UnityEngine.Object.Destroy(base.gameObject);
        }

    }

    private void LoadLevel()
    {
        if (PlayerPrefs.HasKey("level"))
        {
            level = PlayerPrefs.GetInt("level") - 1;
            if (level >= pointsPanelList.Length)
            {
                level = 0;
                PlayerPrefs.SetInt("level", 0);
            }
        }
        else
        {
            level = 0;
            PlayerPrefs.SetInt("level", 0);
        }
        MonoBehaviour.print("Load Level = " + level);
    }

    private void SaveLevel()
    {
        PlayerPrefs.SetInt("level", level + 1);
        PlayerPrefs.Save();
        MonoBehaviour.print("Save Level = " + level);
    }
    private void initUIObjects()
    {
        // UIObjects = new List<Button>();
        UIObjects = GameObject.FindGameObjectsWithTag("uibtn");
        foreach (GameObject button in UIObjects)
        {
            button.GetComponent<Button>().onClick.AddListener(OnUIClick);
        }
    }
    private void LoadStars()
    {
        if (PlayerPrefs.HasKey("stars"))
        {
            stars = PlayerPrefs.GetInt("stars");
        }
        else
        {
            stars = 0;
        }
        if (PlayerPrefs.HasKey("levelStars"))
        {
            levelStars = PlayerPrefs.GetInt("levelStars");
        }
        else
        {
            levelStars = 3;
        }
        MonoBehaviour.print("Load stars = " + stars + " - LevelStars = " + levelStars);
    }

    private void SaveStars()
    {
        PlayerPrefs.SetInt("stars", stars);
        PlayerPrefs.SetInt("levelStars", levelStars);
        PlayerPrefs.Save();
        MonoBehaviour.print("Save stars = " + stars + " - levelStars = " + levelStars);
    }

    private void ShowStars()
    {
        GameObject gameObject = GameObject.Find("StarsText");
        if ((bool)gameObject && stars > 0)
        {
            // gameObject.GetComponent<TextMeshPro>().text = stars.ToString();
        }
    }

    private void SceneManager_activeSceneChanged(Scene oldScene, Scene newScene)
    {
        MonoBehaviour.print("Scene Changed. New Scene is " + newScene.name + "Level=" + level);
        PlaySceneMusic(newScene);
        ShowStars();
        if (newScene.name == "lvlGame")
        {
            InitLevel();
        }
    }

    private void PlaySceneMusic(Scene newScene)
    {
        if (newScene.name == "lvlGame")
        {
            Sound.REF.PlayMusic("sndMusicLevel0" + UnityEngine.Random.Range(1, 4));
        }
        else if (newScene.name == "lvlMain")
        {
            Sound.REF.PlayMusic("sndMusicMain");
        }
    }

    private void InitLevel()
    {
        if (SceneManager.GetActiveScene().name != "lvlGame")
        {
            return;
        }
        touchCounter = 0;
        remainingPoints = 5;
        if (level * 2 + 1 >= images.Length)
        {
            level = 0;
            LoadScene.REF.GotoMain();
        }
        Object.Instantiate(pointsPanelList[level]);
        if (!debugMod)
        {
            GameObject.Find("btnNext").SetActive(value: false);
            GameObject.Find("btnPrevious").SetActive(value: false);
        }
        objWin = GameObject.Find("Win").gameObject;
        objWin.SetActive(value: false);
        GameObject gameObject = GameObject.Find("ImageA");
        GameObject gameObject2 = GameObject.Find("ImageB");
        GameObject gameObject3 = GameObject.Find("ZoomImageA");
        GameObject gameObject4 = GameObject.Find("ZoomImageB");
        if (gameObject == null || gameObject2 == null)
        {
            return;
        }
        gameObject.GetComponent<SpriteRenderer>().sprite = images[level * 2];
        gameObject2.GetComponent<SpriteRenderer>().sprite = images[level * 2 + 1];
        if (gameObject3 != null && gameObject4 != null)
        {
            gameObject3.GetComponent<SpriteRenderer>().sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
            gameObject4.GetComponent<SpriteRenderer>().sprite = gameObject2.GetComponent<SpriteRenderer>().sprite;
        }
        Vector3 position = gameObject2.transform.position;
        float y = position.y;
        Vector3 position2 = gameObject.transform.position;
        float num = y - position2.y;
        for (int i = 1; i <= 5; i++)
        {
            GameObject gameObject5 = GameObject.Find("PointA" + i);
            GameObject gameObject6 = GameObject.Find("PointB" + i);
            Transform transform = gameObject6.transform;
            Vector3 position3 = gameObject5.transform.position;
            float x = position3.x;
            Vector3 position4 = gameObject5.transform.position;
            transform.position = new Vector3(x, position4.y + num);
            if (!debugMod)
            {
                gameObject5.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);
                gameObject6.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);
            }
            Transform transform2 = gameObject6.transform;
            Vector3 localScale = gameObject5.transform.localScale;
            float x2 = localScale.x;
            Vector3 localScale2 = gameObject5.transform.localScale;
            float y2 = localScale2.y;
            Vector3 localScale3 = gameObject5.transform.localScale;
            transform2.localScale = new Vector3(x2, y2, localScale3.z);
        }
        // GameObject.Find("LevelText").GetComponent<TextMeshPro>().text = (level + 1).ToString();
        for (int i = 0; i < 3; i++)
        {
            GObjsList[i] = GameObject.Find(GObjs[i]);
        }
        initUIObjects();
        pointsPanel = GameObject.FindWithTag("PointsPanel");

        heartParent = GameObject.Find("Hearts");
        winPanel = heartParent.transform.parent.transform.GetChild(5).gameObject;

        restartPanel = heartParent.transform.parent.transform.GetChild(6).gameObject;
        restartPanel.transform.GetChild(0).GetChild(1).GetComponent<Button>().onClick.AddListener(GoToHome);
        restartPanel.transform.GetChild(0).GetChild(2).GetComponent<Button>().onClick.AddListener(RestartLevel);
        Home = GameObject.Find("Home");
        Home.GetComponent<Button>().onClick.AddListener(GoToHome);
        isPlayed = false;
        heartCount = 3;

    }

    public void PointCheck()
    {
        remainingPoints--;
        if (remainingPoints <= 0)
        {
            Invoke("GameWin", 0.5f);
        }
        GameObject.Find("RemainingUI" + (4 - remainingPoints)).GetComponent<Image>().sprite = spRemainingUICheck;
        MonoBehaviour.print("PointCheck() - RemainingPoints are " + remainingPoints);
    }

    private void GameWin()
    {
        Sound.REF.PlayMusic("sndMusicMain");
        CoinManager.instance.AddCoins(30);
        CoinManager.instance.AddDiamonds(1);
        for (int i = 0; i < GObjsList.Length; i++)
        {
            GObjsList[i].SetActive(false);
        }
        pointsPanel.SetActive(false);
        winPanel.SetActive(true);
        if (HomeManager.isSound)
        {
            SoundManager.instance.Stop("DifferenceBG");
            SoundManager.instance.Play("Victory");
        }
        // objWin.SetActive(value: true);
        MonoBehaviour.print("You Win!");
        float time = 6f;
        if (debugMod)
        {
            time = 1f;
        }
        for (int i = 0; i < levelStars; i++)
        {
            objWin.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = spWinStar;
        }
        Invoke("GoToNextLevel", time);
        level++;
        SaveLevel();
        stars += levelStars;
        levelStars = 3;
        SaveStars();
    }

    private void GoToNextLevel()
    {
        SceneManager.LoadScene("lvlGame");
    }

    public bool IsPointerOverGameObject()
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
    bool _OnUIClick;
    private void Update()
    {
        if (!Input.GetMouseButtonUp(0) || !(SceneManager.GetActiveScene().name == "lvlGame") || objWin.activeSelf)
        {
            return;
        }
        touchCounter++;
        MonoBehaviour.print("touchCounter = " + touchCounter);
        if (isoverobject || _OnUIClick)
        {
            Debug.Log(" over object");
            isoverobject = false;
            _OnUIClick = false;
        }
        else
        {
            Debug.Log("wrong touch not over object");
            heartParent.transform.GetChild(heartCount - 1).gameObject.SetActive(false);
            heartCount--;
        }
        // else if (!EventSystem.current.IsPointerOverGameObject())
        // {
        //     Debug.Log("wrong touch not over object");
        //     heartParent.transform.GetChild(heartCount - 1).gameObject.SetActive(false);
        //     heartCount--;
        // }

        if (heartCount <= 0)
        {
            // for (int i = 0; i < GObjsList.Length; i++)
            // {
            //     GObjsList[i].SetActive(false);
            // }
            // pointsPanel.SetActive(false);
            // restartPanel.SetActive(true);
            // restartLevel = GameObject.Find("RestartLevel");
            // restartLevel.GetComponent<Button>().onClick.AddListener(RestartLevel);
            // cancleLevel = GameObject.Find("CancleLevel");
            // cancleLevel.GetComponent<Button>().onClick.AddListener(GoToHome);
            // GoToHome();
            if (!isPlayed && HomeManager.isSound)
            {
                SoundManager.instance.Play("levelfailed");
                SoundManager.instance.Stop("DifferenceBG");
                isPlayed = true;
            }
            for (int i = 0; i < GObjsList.Length; i++)
            {
                GObjsList[i].SetActive(false);
            }
            pointsPanel.SetActive(false);
            if (!restartPanel.activeInHierarchy)
            {
                restartPanel.SetActive(true);
            }
            Debug.Log("Game End: Lost");
        }
        if (touchCounter > 8)
        {
            if (levelStars > 0)
            {
                levelStars--;
                SaveStars();
            }
            touchCounter = 0;
        }
    }

    public void GoToHome()
    {
        SceneManager.LoadScene("Home");
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("lvlGame");
    }

    public void OnUIClick()
    {
        Debug.Log("on UI Click = True");
        _OnUIClick = true;
        isoverobject = true;
    }
}
