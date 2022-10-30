using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FacebookLoginExp : MonoBehaviour
{
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
    private void Start()
    {
        StartCoroutine(checkLogin());
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
    public Button fb;
    public void onClickLogin()
    {
        var perms = new List<string>() { "public_profile", "email" };
        FB.LogInWithReadPermissions(perms, AuthCallback);

    }
    void DealWithFbMenus(bool isLoggedIn)
    {
        if (isLoggedIn)
        {
            FB.API("/me?fields=first_name", HttpMethod.GET, DisplayUsername);
            FB.API("/me/picture?type=med", HttpMethod.GET, DisplayProfilePic);
        }
    }
    public Text FB_userName;
    bool isName, isPic;
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
    public Image FB_userDp;
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
            Debug.Log("User cancelled login");

        }
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
