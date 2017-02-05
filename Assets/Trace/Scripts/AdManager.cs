using System;
using UnityEngine;
using System.Collections;
#if TraceAds
using GoogleMobileAds.Api;
#endif

/*
 * Download the package here - https://github.com/googleads/googleads-mobile-unity/releases
 * Get the Ad Units from here - https://www.google.com/admob/
*/

public class AdManager : MonoBehaviour 
{
	#if TraceAds
	#if UNITY_ANDROID
	public string InterstitialAdUnitId = "INSERT_ANDROID_INTERSTITIAL_AD_UNIT_ID_HERE";
	#elif UNITY_IOS
	public string InterstitialAdUnitId = "INSERT_IOS_INTERSTITIAL_AD_UNIT_ID_HERE";
	#else
	public const string InterstitialAdUnitId = "unexpected_platform";
	#endif

	[Tooltip("After how many 'gameovers' should we show an interstitial ad?")]
	public int ShowAdEvery = 3;

	private int c;

	internal InterstitialAd interstitial;

	internal static AdManager instance;

	private void Awake()
	{
		if(instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
			Destroy(gameObject);
	}

	private void Start()
	{
		c=ShowAdEvery;
		RequestInterstitial();
	}

	internal void RequestInterstitial()
	{
		// Initialize an InterstitialAd.
		interstitial = new InterstitialAd(InterstitialAdUnitId);

		// Called when an ad request failed to load.
		interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
		// Called when the user returned from the app after an ad click.
		interstitial.OnAdClosed += HandleOnAdClosed;
		// Called when the ad click caused the user to leave the application.
		interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;

		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder()
		//.AddTestDevice(AdRequest.TestDeviceSimulator)
		//.AddTestDevice("899065F093D129B951A7182FE15F525E")
		.Build();

		// Load the interstitial with the request.
		interstitial.LoadAd(request);
	}

	internal void ShowInterstitial()
	{
		if(ShowAdEvery<=1)
		{
			if (interstitial.IsLoaded()) 
			{
				interstitial.Show();
				RequestInterstitial();

				//Reset the ad counter
				ShowAdEvery=c;
			}
			else
				RequestInterstitial();
		}

		else if(ShowAdEvery>1)
		{
			ShowAdEvery -=1;

			if (!interstitial.IsLoaded()) 
				RequestInterstitial();
		}
	}

	void DestroyInterstitial()
	{
		// Destroy the listeners
		interstitial.OnAdFailedToLoad -= HandleOnAdFailedToLoad;
		interstitial.OnAdClosed -= HandleOnAdClosed;
		interstitial.OnAdLeavingApplication -= HandleOnAdLeavingApplication;

		interstitial.Destroy();
	}

	public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
	#if DEBUG
		print("HandleInterstitialFailedToLoad event received with message: " + args.Message);
	#endif
		DestroyInterstitial();
	}

	public void HandleOnAdClosed(object sender, EventArgs args)
	{
	#if DEBUG
		print("HandleInterstitialClosed event received");
	#endif
		DestroyInterstitial();
	}

	public void HandleOnAdLeavingApplication(object sender, EventArgs args)
	{
	#if DEBUG
		print("HandleInterstitialLeftApplication event received");
	#endif
		DestroyInterstitial();
	}
	#endif
}