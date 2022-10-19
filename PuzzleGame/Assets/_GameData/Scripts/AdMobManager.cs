//AdMOB scprit 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.iOS;
using GoogleMobileAds.Api;
using System;

public class AdMobManager : MonoBehaviour
{
    // ca-app-pub-3940256099942544~3347511713 test App Id
    [Header("Banner Id")]
    public string Admob_BannerID_IOS = "";
    public string Admob_BannerID_Android = "";

    [Header("Interstitial Id")]
    public string Admob_InterstialID_IOS = "";
    public string Admob_InterstialID_Android = "";

    [Header("Rewarded Id")]
    public string Admob_rewardedID_IOS;
    public string Admob_rewardedID_Android;


    private BannerView bannerAd;
    private InterstitialAd interstitial;
    private RewardedAd rewarded;
    public bool isRewardedReady = false;
    public bool isInterstitialReady = false;
    // private RewardBasedVideoAd rewardBasedVideo;

    public static AdMobManager instance;


    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

    }
    void Start()
    {
        MobileAds.Initialize(InitializationStatus => { });
        this.RequestInterstitial();
        this.RequestRewardBasedVideo();
        this.RequestBanner();
    }

    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder().Build();
    }
    void RequestBanner()
    {
#if UNITY_IOS
	string adunitid = Admob_BannerID_IOS;

#elif UNITY_ANDROID
        string adunitid = Admob_BannerID_Android;
#endif
        this.bannerAd = new BannerView(adunitid, AdSize.Banner, AdPosition.Bottom);
        this.bannerAd.LoadAd(CreateAdRequest());
    }


    public void RequestInterstitial()
    {

#if UNITY_IOS
    string adunitid = Admob_InterstialID_IOS;
#elif UNITY_ANDROID
        string adunitid = Admob_InterstialID_Android;
#endif

        //   if(this.interstitialAd!=null){
        //     this.interstitialAd.Destroy();
        //   }
        //Initialize adUnit
        this.interstitial = new InterstitialAd(adunitid);

        // Called when an ad request has successfully loaded.
        this.interstitial.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        this.interstitial.OnAdOpening += HandleOnAdOpening;
        // Called when the ad is closed.
        this.interstitial.OnAdClosed += HandleOnAdClosed;

        // load ad with request 
        this.interstitial.LoadAd(this.CreateAdRequest());

    }
    public void ShowInterstitial()
    {
        if (this.interstitial != null)
        {
            if (this.interstitial.IsLoaded())
            {
                this.interstitial.Show();
            }
        }
        else if (this.interstitial == null)
        {
            this.RequestInterstitial();
            if (this.interstitial.IsLoaded())
                this.interstitial.Show();
            else
                return;
        }
        else

            return;
    }

    public void RequestRewardBasedVideo()
    {

#if UNITY_IOS
    string adUnitId = Admob_rewardedID_IOS;
#elif UNITY_ANDROID
        string adUnitId = Admob_rewardedID_Android;
#endif
        // Initialize ad unit
        this.rewarded = new RewardedAd(adUnitId);

        // Called when an ad request has successfully loaded.
        this.rewarded.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        this.rewarded.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        this.rewarded.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        this.rewarded.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        this.rewarded.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        this.rewarded.OnAdClosed += HandleRewardedAdClosed;


        //load ad with request
        this.rewarded.LoadAd(this.CreateAdRequest());
    }
    public void ShowRewardBasedVideo()
    {
        if (this.rewarded != null)
        {
            if (this.rewarded.IsLoaded())
            {
                this.rewarded.Show();
            }

        }
        else if (this.rewarded == null)
        {

            this.RequestRewardBasedVideo();
            if (this.rewarded.IsLoaded())
            {
                this.rewarded.Show();
            }
            else
                return;
        }
        else return;
    }


    #region  ADS Interstitial events
    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
        isInterstitialReady = true;
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("InterstitialFailedToReceiveAd event received with message: "
                            + args.LoadAdError.GetMessage());
        this.RequestInterstitial();
        isInterstitialReady = false;

    }
    public void HandleOnAdOpening(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpening event received");
        isInterstitialReady = false;
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
        interstitial.Destroy();
        this.RequestInterstitial();
    }
    #endregion

    #region ADS Rewarded events
    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdOpening event received");
        isRewardedReady = false;
    }
    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        this.RequestRewardBasedVideo();
        MonoBehaviour.print("HandleRewardedAdClosed event received");
    }
    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdLoaded event received : successfull loaded");
        isRewardedReady = true;
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdFailedToLoad event received with message: failed to load ");
        isRewardedReady = false;
    }
    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdFailedToShow event received with message: " + args.Message);
    }
    public void HandleUserEarnedReward(object sender, Reward args)
    {
        this.RequestRewardBasedVideo();
        MonoBehaviour.print("HandleRewardedAdRewarded event received for reward earned.");

        // if (SceneShopController.isWatchVideoAds == 1)
        // {
        //     GameObject.FindObjectOfType<SceneShopController>().VideoAdsCallBack();
        // }
        // if (ShopCoins.isClickFreeCoins == 1)
        // {
        //     GameObject.FindObjectOfType<ShopCoins>().onClickFreeCoinCallBack();
        // }
    }
    #endregion


}
