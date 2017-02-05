using UnityEngine;
using UnityEditor;

public class Menu: Editor {

	const string Ads = "TraceAds";

	[MenuItem("Trace/Configuration/Configure Admob")]
	static void Admob()
	{
#if UNITY_ANDROID
		PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, Ads);
#elif UNITY_IOS
		PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, Ads);
#endif
	}

	[MenuItem("Trace/Configuration/Reset Configuration")]
	static void ResetAll()
	{
		#if UNITY_ANDROID
		PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, "");
		#elif UNITY_IOS
		PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, "");
		#endif
	}

	[MenuItem("Trace/Contact Support")]
	static void Support()
	{
		Application.OpenURL("mailto:mintonne@gmail.com");
	}

	[MenuItem("Trace/Rate Trace")]
	static void Rate()
	{
		UnityEditorInternal.AssetStore.Open("content/60174"); 
	}

	[MenuItem("Trace/More Unity Assets")]
	static void More()
	{
		Application.OpenURL("https://www.assetstore.unity3d.com/en/#!/search/page=1/sortby=popularity/query=publisher:18385");
	}
}