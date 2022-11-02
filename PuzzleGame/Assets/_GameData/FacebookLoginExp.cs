using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using PlayFab;
using PlayFab.ClientModels;

public class FacebookLoginExp : MonoBehaviour
{
    public Button fb;
    public Text FB_userName;
    public Image FB_userDp;
    bool isName, isPic;
    [Header("PlayFab app id")]
    public string titleId;
    public static int puzzlelevel, differencelevel, matchingcards;
    void Awake()
    {

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
    void playfabloginsuccess(PlayFab.ClientModels.LoginResult result)
    {
        Debug.Log(result + " PlayFab Login Success");
    }
    void playfabloginfailed(PlayFabError error)
    {
        Debug.Log(error.ErrorMessage + " failed");
    }
    IEnumerator checkLogin()
    {
        DealWithFbMenus(FB.IsLoggedIn);
        yield return new WaitForSeconds(1);
        if (!isName && !isPic)
        {
            StartCoroutine(checkLogin());
        }
    }
    public void getDataFromPlayFab()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), onDataRecived, playfabloginfailed);

    }
    void onDataRecived(GetUserDataResult result)
    {
        Debug.Log(result + ": Data Recived Success");
        if (result.Data != null && result.Data.ContainsKey("puzzleLevel"))
        {
            Debug.Log("Puzzle Level = " + result.Data["puzzleLevel"].Value);
        }
    }
    public void saveDatatoPlayFab()
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                //write your data here
                {"puzzleLevel", "5"}

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
        }
        else
        {
            fb.gameObject.SetActive(true);
        }
    }
}
