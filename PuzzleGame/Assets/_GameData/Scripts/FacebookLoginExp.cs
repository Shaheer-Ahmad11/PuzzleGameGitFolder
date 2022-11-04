using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;

public class FacebookLoginExp : MonoBehaviour
{
    public Button fb;
    public Text FB_userName;
    public Image FB_userDp;
    bool isName, isPic;
    [Header("PlayFab app id")]
    public string titleId;
    public static int puzzlelevel, differencelevel, matchingcards;
    public static FacebookLoginExp Instance;
    public bool isLoggedin;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }



        if (!FB.IsInitialized)
        {

            // Initialize the Facebook SDK
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
    }
    private void Start()
    {
        // PlayerPrefs.DeleteAll();
        if (!PlayerPrefs.HasKey("puzzlelevel"))
        {
            PlayerPrefs.SetInt("puzzlelevel", 1);
        }
        if (!PlayerPrefs.HasKey("level"))
        {
            PlayerPrefs.SetInt("level", 1);
        }
        if (!FB.IsLoggedIn)
        {
            if (!alreadyLoaded)
            {
                HomeManager.instance.loadPuzzleLevels();
                HomeManager.instance.loadDifferenceLevel();
                alreadyLoaded = true;
            }
        }
        StartCoroutine(checkLogin());

    }
    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            Debug.Log("Initialized Facebook SDK");

            // Continue with Facebook SDK
            // ...
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else
        {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }

    public void onClickLogin()
    {
        var perms = new List<string>() { "public_profile", "email" };
        FB.LogInWithReadPermissions(perms, AuthCallback);
    }
    private void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            // AccessToken class will have session details
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
            // Print current access token's User ID
            Debug.Log("UserId " + aToken.UserId);
            // Print current access token's granted permissions
            foreach (string perm in aToken.Permissions)
            {
                Debug.Log(perm);
            }
            Invoke("loadCurrent", 1f);
        }
        else
        {
            Debug.Log(result.Error + " :User cancelled login");
        }
        PlayFabClientAPI.LoginWithFacebook(new PlayFab.ClientModels.LoginWithFacebookRequest
        {
            TitleId = titleId,
            AccessToken = AccessToken.CurrentAccessToken.TokenString,
            CreateAccount = true
        }, playfabloginsuccess, playfabloginfailed);
    }
    void loadCurrent()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    void playfabloginsuccess(PlayFab.ClientModels.LoginResult result)
    {
        Debug.Log(result + " PlayFab Login Success");
        getDataFromPlayFab();
    }
    void playfabloginfailed(PlayFabError error)
    {
        Debug.Log(error.ErrorMessage + " failed");
        if (!alreadyLoaded)
        {
            HomeManager.instance.loadPuzzleLevels();
            HomeManager.instance.loadDifferenceLevel();
            alreadyLoaded = true;
        }
    }
    IEnumerator checkLogin()
    {
        DealWithFbMenus(FB.IsLoggedIn);
        if (FB.IsLoggedIn)
        {
            PlayFabClientAPI.LoginWithFacebook(new PlayFab.ClientModels.LoginWithFacebookRequest
            {
                TitleId = titleId,
                AccessToken = AccessToken.CurrentAccessToken.TokenString,
                CreateAccount = true
            }, playfabloginsuccess, playfabloginfailed);
        }
        yield return new WaitForSeconds(1);
        if (!isName && !isPic)
        {
            StartCoroutine(checkLogin());
        }
    }
    void DealWithFbMenus(bool isLoggedIn)
    {
        //get UserName and profile picture from facebook
        if (isLoggedIn)
        {
            FB.API("/me?fields=first_name", HttpMethod.GET, DisplayUsername);
            FB.API("/me/picture?type=med", HttpMethod.GET, DisplayProfilePic);
        }
    }

    void DisplayUsername(IResult result)
    {
        if (result.Error == null)
        {
            FB_userName.gameObject.SetActive(true);
            string name = "" + result.ResultDictionary["first_name"];
            FB_userName.text = name;
            Debug.Log("" + name);
            isName = true;
        }
        else
        {
            FB_userName.gameObject.SetActive(false);
            Debug.Log(result.Error);
        }
    }

    void DisplayProfilePic(IGraphResult result)
    {
        if (result.Error == null)
        {
            FB_userDp.gameObject.transform.parent.gameObject.SetActive(true);
            Debug.Log("Profile Pic");
            // FB_userDp.sprite = Sprite.Create(result.Texture, new Rect(0, 0, 128, 128), new Vector2());
            var size = Mathf.Min(result.Texture.width, result.Texture.height);
            FB_userDp.sprite = Sprite.Create(result.Texture, new Rect(0, 0, size, size), Vector2.one * 0.5f);
            isPic = true;

        }
        else
        {
            FB_userDp.gameObject.transform.parent.gameObject.SetActive(false);
            Debug.Log(result.Error);
        }
    }




    public void getDataFromPlayFab()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), onDataRecived, playfabloginfailed);

    }
    bool alreadyLoaded = false;
    void onDataRecived(GetUserDataResult result)
    {
        Debug.Log(result + ": Data Recived Success");
        if (result.Data != null && result.Data.ContainsKey("currentPuzzleLevel") && result.Data.ContainsKey("currentDifferenceLecel"))
        {
            Debug.Log("Puzzle Level = " + result.Data["currentPuzzleLevel"].Value);
            Debug.Log("difference Level = " + result.Data["currentDifferenceLecel"].Value);
            HomeManager.instance._currentPuzzleLevel = int.Parse(result.Data["currentPuzzleLevel"].Value);
            HomeManager.instance._currentDifferenceLevel = int.Parse(result.Data["currentDifferenceLecel"].Value);
            if (PlayerPrefs.GetInt("puzzlelevel") > HomeManager.instance._currentPuzzleLevel)
            {
                HomeManager.instance._currentPuzzleLevel = PlayerPrefs.GetInt("puzzlelevel");
                saveDatatoPlayFab();
            }
            if (PlayerPrefs.GetInt("level") > HomeManager.instance._currentDifferenceLevel)
            {
                HomeManager.instance._currentDifferenceLevel = PlayerPrefs.GetInt("level");
                saveDatatoPlayFab();
            }
            {
                PlayerPrefs.SetInt("puzzlelevel", HomeManager.instance._currentPuzzleLevel);
                PlayerPrefs.SetInt("level", HomeManager.instance._currentDifferenceLevel);
            }
            if (!alreadyLoaded)
            {
                HomeManager.instance.loadPuzzleLevels();
                HomeManager.instance.loadDifferenceLevel();
            }
            // SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            alreadyLoaded = true;
        }
        else
        {

            saveDatatoPlayFab();
        }
    }
    public void saveDatatoPlayFab()
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                //write your data here
                {"currentPuzzleLevel", (PlayerPrefs.GetInt("puzzlelevel").ToString()) },
                {"currentDifferenceLecel",(PlayerPrefs.GetInt("level").ToString())}

    }
        };
        PlayFabClientAPI.UpdateUserData(request, OnDataSend, playfabloginfailed);
    }
    void OnDataSend(UpdateUserDataResult result)
    {
        Debug.Log(result + " Data Sent Success");
    }

    private void Update()
    {
        if (FB.IsLoggedIn)
        {
            fb.gameObject.SetActive(false);
            isLoggedin = true;
        }
        else
        {
            fb.gameObject.SetActive(true);
            isLoggedin = false;
        }
    }
}
