using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ResourceManager : MonoBehaviour{

	public const string RelyOnSource = "RelyOnResource";

	public const string ResourceInit = "Resource_Init";

	public static Dictionary<ResourceAssetBundle,AssetBundleObj> assetBundles = new Dictionary<ResourceAssetBundle,AssetBundleObj> ();

	private static ResourceManager instance;

	public static List<int> exceptionList = new List<int>(){1,5,9,49,50,51,52,53,54,55,56,57,58,59,60,61,63,65,67,69,71,73,75,77,79,81,83,86,88,90,92,94,96,122,124,126,128,130,132,187,190,193,196,199,202};

	public static ResourceManager Instance
	{
		get {
			if(instance == null)
				instance = FindObjectOfType<ResourceManager>();
			return instance;
		}
	}
	
	public void Init(DataListener callback){
		int num = 1;
		assetBundles[ResourceAssetBundle.PROTOBUF] = new AssetBundleObj(ResourceAssetBundle.PROTOBUF,ResourceManager.ResourceInit,new List<ResourceCallback>{o=>{num--; if(num <= 0)callback(null);}},GetBundleTypeByKey(ResourceAssetBundle.PROTOBUF));
		StartCoroutine(DownloadResource(ResourceAssetBundle.PROTOBUF));
	}

	private Dictionary<string,object> objectDic = new Dictionary<string, object>();

	public Object LoadLocalAsset( string path, ResourceCallback callback ) {
		//Debug.Log ("load res: " + path);
		//the following resource will not be dynamiclly download.
		if (string.IsNullOrEmpty (path)) {
			return null;	
		}

//		if (path.IndexOf ("Loading") >= 0 || path.IndexOf ("UIInsConfig") >= 0 || path.IndexOf ("ScreenMask") >= 0 || path.IndexOf ("CommonNoteWindow") >= 0) {
//			Debug.Log("path: " + path);
//			if (callback != null){
//				callback (Resources.Load (path));
//				return null;
//			}else{
//				return Resources.Load (path);
//			}
//		}

		if (path.IndexOf ("Language") == 0 || path.IndexOf ("Protobuf") == 0 || (path.IndexOf ("Avatar") == 0 && path.IndexOf("100") < 0) || (path.IndexOf ("Profile") == 0 && exceptionList.IndexOf(int.Parse(path.Substring(8))) < 0) || path.IndexOf("Atlas") == 0) {
#if UNITY_EDITOR
			

//			ResourceAssetBundle key = GetBundleKeyByPath(path);
//			
//			if(!assetBundles.ContainsKey(key)){
//							assetBundles[key] = new AssetBundleObj(key,path,new List<ResourceCallback>(){callback},GetBundleTypeByKey(key));
//				StartCoroutine(DownloadResource(key));
//			}else{
//				if(assetBundles[key].isLoading){
////					Debug.Log("======path: " + path);
//					if(assetBundles[key].callbackList.ContainsKey(path)){
////						Debug.Log("add callback1: " + path + " " + callback);
//						assetBundles[key].callbackList[path].Add(callback);
//					}else {
////						Debug.Log("add callback2: " + path + " " + callback);
//						assetBundles[key].callbackList.Add(path,new List<ResourceCallback>(){callback});
//					}
//				}else{
//					if(callback != null){
////						Debug.Log("resource load: " + path + " key: " + assetBundles[key].assetBundle);
//						callback(assetBundles[key].assetBundle.Load(path.Substring(path.LastIndexOf('/')+1), GetBundleTypeByKey(key)));
//						return null;
//					}else{
//						return assetBundles[key].assetBundle.Load(path.Substring(path.LastIndexOf('/')+1),  GetBundleTypeByKey(key));
//					}
//				}
//				
//			}
//			return null;

			string ext = null;
			if(path.IndexOf ("Prefabs") == 0){
				ext = ".prefab";

//				return uiAssets.Load(path);
			}else if(path.IndexOf ("Atlas") == 0){
				ext = ".prefab";
			}else if(path.IndexOf ("Language") == 0){
				ext = ".txt";
			}else if(path.IndexOf ("Protobuf") == 0){
				if(path.IndexOf ("skills") >= 0){
					ext = ".json";
				}else{
					ext = ".bytes";
				}
			}else if(path.IndexOf ("Avatar") == 0){
				ext = ".prefab";
			}else if(path.IndexOf ("Profile") == 0){
				ext = ".png";

				int num = 0;
				int.TryParse(path.Substring(path.LastIndexOf('/')),out num);
//				(int)(num/20)
			}

//			Debug.Log ("assets load: " + "Assets/ResourceDownload/" + path + ext + "  "  + Resources.LoadAssetAtPath <Object>("Assets/ResourceDownload/" + path+ ext));
			if(callback != null){
				callback(Resources.LoadAssetAtPath<Object> ("Assets/ResourceDownload/" + path + ext));
				return null;
			}else{
				return Resources.LoadAssetAtPath<Object> ("Assets/ResourceDownload/" + path + ext);
			}
#else 
//			if(path.IndexOf ("Config") == 0){
//
//			}else if(path.IndexOf ("Prefabs") == 0){
//				ext = ".prefab";
//			}else if(path.IndexOf ("Language") == 0){
//				ext = ".txt";
//			}else if(path.IndexOf ("Protobuf") == 0){
//				if(path.IndexOf ("skills") >= 0){
//					ext = ".json";
//				}else{
//					ext = ".bytes";
//				}
//			}else if(path.IndexOf ("Avatar") == 0 || path.IndexOf ("Profile") == 0){
//				ext = ".png";
//			}

			
			ResourceAssetBundle key = GetBundleKeyByPath(path);
			
			if(!assetBundles.ContainsKey(key)){
				assetBundles[key] = new AssetBundleObj(key,path,new List<ResourceCallback>(){callback},GetBundleTypeByKey(key));
				StartCoroutine(DownloadResource(key));
			}else{
				if(assetBundles[key].isLoading){
//					Debug.Log("======path: " + path);
					if(assetBundles[key].callbackList.ContainsKey(path)){
//						Debug.Log("add callback1: " + path + " " + callback);
						assetBundles[key].callbackList[path].Add(callback);
					}else {
//						Debug.Log("add callback2: " + path + " " + callback);
						assetBundles[key].callbackList.Add(path,new List<ResourceCallback>(){callback});
					}
				}else{
					if(callback != null){
						Debug.Log("resource load: " + path + " key: " + assetBundles[key].assetBundle);
						callback(assetBundles[key].assetBundle.Load(path.Substring(path.LastIndexOf('/')+1), GetBundleTypeByKey(key)));
						return null;
					}else{
						return assetBundles[key].assetBundle.Load(path.Substring(path.LastIndexOf('/')+1),  GetBundleTypeByKey(key));
					}
				}
				
			}
			return null;

//			Debug.Log ("resource load no editor");
//			if(callback != null){
//				callback(Resources.Load (path));
//				return null;
//			}else{
//				return Resources.Load (path);
//			}
//			return null;

#endif
		} else {
//			Debug.Log ("resource load from resource: " + path);
			if(callback != null){
				callback(Resources.Load (path));
				return null;
			}else{
				return Resources.Load (path);
			}
			return null;
		}
		return null;

	}

	IEnumerator DownloadResource(ResourceAssetBundle key){

		string url = 
#if UNITY_EDITOR
			"file:///Users/Resource/" + GetBundleUrlByKey(key);
#elif UNITY_IOS
	"file://" + Application.persistentDataPath + "/"+ GetBundleUrlByKey(key);
#elif UNITY_ANDROID
	"file:///" + Application.persistentDataPath + "/assets/"+ GetBundleUrlByKey(key);
#else

	string.Empty;
#endif
//		File file = new File (Application.persistentDataPath + "!/assets/" + GetBundleUrlByKey (key));

		assetBundles [key].isLoading = true;
		Debug.Log ("download start url: " + url);

		WWW www = new WWW (url);
		if (!string.IsNullOrEmpty (www.error)) {
			Debug.Log(www.error);		
		}
		yield return www;

		if (string.IsNullOrEmpty (www.error)) {
			Debug.Log ("download complete url: " + url);
			assetBundles [key].assetBundle = www.assetBundle;
			assetBundles [key].isLoading = false;
			
			if(checkRelies(assetBundles [key])){
				assetBundles [key].ExeCallback ();
			}	
		}else{
			Debug.LogError("load err: " + www.error);
		}


	}

	private ResourceAssetBundle GetBundleKeyByPath(string path){
//		Debug.Log ("download async: " + path);

		if(path.IndexOf ("Prefabs") == 0){

			return ResourceAssetBundle.UI;

		}else if(path.IndexOf ("Atlas") == 0){
			if(path.IndexOf("Event_Atlas") >= 0){
				return ResourceAssetBundle.EventAtlas;
			}
		}else if(path.IndexOf ("Language") == 0){
			return ResourceAssetBundle.LANGUAGE;
		}else if(path.IndexOf ("Protobuf") == 0){
//			if(path.IndexOf ("skills") >= 0){
//
//			}else{
//
//			}
			return ResourceAssetBundle.PROTOBUF;
		}else if(path.IndexOf ("Avatar") == 0){
			int num = 0;
			int.TryParse(path.Substring(path.LastIndexOf('_')+1),out num);
//			if(exceptionList.IndexOf(num) >= 0){
//				return ResourceAssetBundle.AVATAR_EXCEPTION;
//			}
			switch(num){
			case 0:
				return ResourceAssetBundle.AVATAR_0;
			case 1:
				return ResourceAssetBundle.AVATAR_1;
			case 2:
				return ResourceAssetBundle.AVATAR_2;
			case 3:
				return ResourceAssetBundle.AVATAR_3;
			case 4:
				return ResourceAssetBundle.AVATAR_4;
			case 5:
				return ResourceAssetBundle.AVATAR_5;
			case 6:
				return ResourceAssetBundle.AVATAR_6;
			case 7:
				return ResourceAssetBundle.AVATAR_7;
			case 8:
				return ResourceAssetBundle.AVATAR_8;
			case 9:
				return ResourceAssetBundle.AVATAR_9;
			case 10:
				return ResourceAssetBundle.AVATAR_10;
			case 11:
				return ResourceAssetBundle.AVATAR_11;
			default:
				return ResourceAssetBundle.NONE;
			}
		}else if(path.IndexOf ("Profile") == 0){
			int num = 0;
			//Debug.Log("profile:-----------------" + path.Substring(path.LastIndexOf('/')));
			int.TryParse(path.Substring(path.LastIndexOf('/') + 1),out num);
//			if(exceptionList.IndexOf(num) >= 0){
//				return ResourceAssetBundle.PROFILE_EXCEPTION;
//			}
			switch((int)((num-1)/20)){
				case 0:
					return ResourceAssetBundle.PROFILE_0;
				case 1:
					return ResourceAssetBundle.PROFILE_1;
				case 2:
					return ResourceAssetBundle.PROFILE_2;
				case 3:
					return ResourceAssetBundle.PROFILE_3;
				case 4:
					return ResourceAssetBundle.PROFILE_4;
				case 5:
					return ResourceAssetBundle.PROFILE_5;
				case 6:
					return ResourceAssetBundle.PROFILE_6;
				case 7:
					return ResourceAssetBundle.PROFILE_7;
				case 8:
					return ResourceAssetBundle.PROFILE_8;
				case 9:
					return ResourceAssetBundle.PROFILE_9;
				case 10:
					return ResourceAssetBundle.PROFILE_10;
			case 11:
				return ResourceAssetBundle.PROFILE_11;
			default:
					return ResourceAssetBundle.NONE;
			}
		}
		if (path == RelyOnSource) {
			return ResourceAssetBundle.UI;
		}
		return ResourceAssetBundle.NONE;
	}

	private string langStr = 
	#if LANGUAGE_CN
	 "Language_cn.unity3d";
	#elif LANGUAGE_EN
	 "Language_en.unity3d";
	#else
	"Language_en.unity3d";
	#endif

	private string protoStr = 
	#if LANGUAGE_CN
	 "Protobuf_cn.unity3d";
	#elif LANGUAGE_EN
	 "Protobuf_en.unity3d";
	#else
	"Protobuf_en.unity3d";
	#endif

	private string GetBundleUrlByKey(ResourceAssetBundle key){
		switch (key) {
			case ResourceAssetBundle.UI:
				return "AllUI.unity3d";
			case ResourceAssetBundle.UI_ATLAS:
				return "UI_Atlas.unity3d";
			case ResourceAssetBundle.LANGUAGE:
				return langStr;
			case ResourceAssetBundle.PROTOBUF:
				return protoStr;
//			case ResourceAssetBundle.PROFILE_EXCEPTION:
//				return "Profile_";
			case ResourceAssetBundle.PROFILE_0:
				return "Profile_0.unity3d";
			case ResourceAssetBundle.PROFILE_1:
				return "Profile_1.unity3d";
			case ResourceAssetBundle.PROFILE_2:
				return "Profile_2.unity3d";
			case ResourceAssetBundle.PROFILE_3:
				return "Profile_3.unity3d";
			case ResourceAssetBundle.PROFILE_4:
				return "Profile_4.unity3d";
			case ResourceAssetBundle.PROFILE_5:
				return "Profile_5.unity3d";
			case ResourceAssetBundle.PROFILE_6:
				return "Profile_6.unity3d";
			case ResourceAssetBundle.PROFILE_7:
				return "Profile_7.unity3d";
			case ResourceAssetBundle.PROFILE_8:
				return "Profile_8.unity3d";
			case ResourceAssetBundle.PROFILE_9:
				return "Profile_9.unity3d";
			case ResourceAssetBundle.PROFILE_10:
				return "Profile_10.unity3d";
			case ResourceAssetBundle.PROFILE_11:
				return "Profile_11.unity3d";
			case ResourceAssetBundle.AVATAR_0:
				return "Atlas_Avatar_0.unity3d";
			case ResourceAssetBundle.AVATAR_1:
				return "Atlas_Avatar_1.unity3d";
			case ResourceAssetBundle.AVATAR_2:
				return "Atlas_Avatar_2.unity3d";
			case ResourceAssetBundle.AVATAR_3:
				return "Atlas_Avatar_3.unity3d";
			case ResourceAssetBundle.AVATAR_4:
				return "Atlas_Avatar_4.unity3d";
			case ResourceAssetBundle.AVATAR_5:
				return "Atlas_Avatar_5.unity3d";
			case ResourceAssetBundle.AVATAR_6:
				return "Atlas_Avatar_6.unity3d";
			case ResourceAssetBundle.AVATAR_7:
				return "Atlas_Avatar_7.unity3d";
			case ResourceAssetBundle.AVATAR_8:
				return "Atlas_Avatar_8.unity3d";
			case ResourceAssetBundle.AVATAR_9:
				return "Atlas_Avatar_9.unity3d";
			case ResourceAssetBundle.AVATAR_10:
				return "Atlas_Avatar_10.unity3d";
			case ResourceAssetBundle.AVATAR_11:
				return "Atlas_Avatar_11.unity3d";
//			case ResourceAssetBundle.AVATAR_EXCEPTION:
//				return "";
			case ResourceAssetBundle.EventAtlas:
				return "Event_Atlas.unity3d";
			default:
				break;
		}
		return null;
	}

	private System.Type GetBundleTypeByKey(ResourceAssetBundle key){
		switch (key) {
			case ResourceAssetBundle.UI:
				return typeof(GameObject);
			case ResourceAssetBundle.UI_ATLAS:
				return typeof(GameObject);
			case ResourceAssetBundle.LANGUAGE:
				return typeof(TextAsset);
			case ResourceAssetBundle.PROTOBUF:
				return typeof(TextAsset);
			case ResourceAssetBundle.PROFILE_0:
			case ResourceAssetBundle.PROFILE_1:
			case ResourceAssetBundle.PROFILE_2:
			case ResourceAssetBundle.PROFILE_3:
			case ResourceAssetBundle.PROFILE_4:
			case ResourceAssetBundle.PROFILE_5:
			case ResourceAssetBundle.PROFILE_6:
			case ResourceAssetBundle.PROFILE_7:
			case ResourceAssetBundle.PROFILE_8:
			case ResourceAssetBundle.PROFILE_9:
			case ResourceAssetBundle.PROFILE_10:
				return typeof(Texture2D);
			case ResourceAssetBundle.AVATAR_0:
			case ResourceAssetBundle.AVATAR_1:
			case ResourceAssetBundle.AVATAR_2:
			case ResourceAssetBundle.AVATAR_3:
			case ResourceAssetBundle.AVATAR_4:
			case ResourceAssetBundle.AVATAR_5:
			case ResourceAssetBundle.AVATAR_6:
			case ResourceAssetBundle.AVATAR_7:
			case ResourceAssetBundle.AVATAR_8:
			case ResourceAssetBundle.AVATAR_9:
			case ResourceAssetBundle.AVATAR_10:
			case ResourceAssetBundle.EventAtlas:
				return typeof(GameObject);
			default:
				break;
		}
		return typeof(Object);
	}
	
	Object LoadLocalFromCache(string assetName,string path) {
		object obj = null;
		if (objectDic.TryGetValue (assetName, out obj)) {
			return (Object)obj;
		} 
		else {
			if (path.IndexOf ("Config") == 0 || path.IndexOf ("Prefabs") == 0) {
				#if UNITY_EDITOR
				Debug.Log ("resource load no cache");
				Debug.Log ("assets load: " + "Assets/Resources/" + path + Resources.LoadAssetAtPath <Object>("Assets/ResourceDownload/" + path));
				obj = Resources.LoadAssetAtPath("Assets/ResourceDownload/" + path + assetName,typeof(GameObject));
				#else 
				Debug.Log ("resource load no editor no cache");
				obj = Resources.Load(path + assetName);
				#endif
			} else {
				obj = Resources.Load(path + assetName);
			}

			objectDic.Add(assetName,obj);
			return (Object)obj;
		}
	}

	private bool checkRelies(AssetBundleObj aObj){
		bool allComplete = true;
		foreach (var item in aObj.relies) {
			if(!ResourceManager.assetBundles.ContainsKey(item)){
				allComplete = false;
				assetBundles[item] = new AssetBundleObj(item,RelyOnSource,new List<ResourceCallback>(){o=>{
					if(checkRelies(aObj)){
//						Debug.Log("rely resource complete");
						aObj.ExeCallback();
					}
					}});
//				Debug.Log(aObj.name + " rely on: " + item);
				StartCoroutine(DownloadResource(item));
			}else if(ResourceManager.assetBundles[item].isLoading){
				allComplete = false;
			}

		}
//		Debug.Log ("resource reply: " + allComplete);
		return allComplete;
	}
}

public enum ResourceAssetBundle{
	NONE,
	UI,
	UI_ATLAS,
	LANGUAGE,
	PROTOBUF,
	PROFILE_0,
	PROFILE_1,
	PROFILE_2,
	PROFILE_3,
	PROFILE_4,
	PROFILE_5,
	PROFILE_6,
	PROFILE_7,
	PROFILE_8,
	PROFILE_9,
	PROFILE_10,
	PROFILE_11,
//	PROFILE_EXCEPTION,

	AVATAR_0,
	AVATAR_1,
	AVATAR_2,
	AVATAR_3,
	AVATAR_4,
	AVATAR_5,
	AVATAR_6,
	AVATAR_7,
	AVATAR_8,
	AVATAR_9,
	AVATAR_10,
	AVATAR_11,
//	AVATAR_EXCEPTION,

	EventAtlas
}

public class AssetBundleObj{

	//the assetbundle pointer
	public AssetBundle assetBundle;

	//whether the resource is loading
	public bool isLoading;

	//all the callback attached to the resource(first param is path,seceond is callback).
	public Dictionary<string, List<ResourceCallback>> callbackList ;

	//the resource relies on the other assetbundle.
	public List<ResourceAssetBundle> relies;

	//the resource name
	public ResourceAssetBundle name;

	public System.Type type;

	public AssetBundleObj(ResourceAssetBundle rName,string path = null,List<ResourceCallback> callback = null,System.Type rType = null){
		callbackList = new Dictionary<string,List<ResourceCallback>>();
		if (path != null) {
			callbackList.Add (path,callback);
//			Debug.Log("======path: " + path);
		}
			
		name = rName;
		if (rType == null) {
			type = typeof(GameObject);
		} else {
			type = rType;
		}
		relies = GetResourceRelyResource(name);
	}

	public void ExeCallback(){
		foreach (var item in callbackList) {
//			Debug.Log("asset bundle: " + item.Key.Substring(item.Key.LastIndexOf('/')+1));
			if(item.Key == ResourceManager.RelyOnSource || item.Key == ResourceManager.ResourceInit){
				foreach(var c in item.Value){
					c(null);
				}
			}else{
				if(item.Value == null || item.Value.Count == 0){
					Debug.Log("no callback: " + item.Key);
				}else{
					foreach(var c1 in item.Value){
						Debug.Log("callback item: " + item.Key);
						c1(assetBundle.Load(item.Key.Substring(item.Key.LastIndexOf('/')+1),type));
					}
				}

//				if(item.Key.IndexOf("ProtoBuf/Unit") >= 0){
//
//				}
			}
			item.Value.Clear();
		}
		callbackList.Clear ();
	}


	public static List<ResourceAssetBundle> GetResourceRelyResource(ResourceAssetBundle resource){
		List<ResourceAssetBundle> relies = new List<ResourceAssetBundle> ();
		if (resource == ResourceAssetBundle.UI) {
			relies.Add(ResourceAssetBundle.UI_ATLAS);	
		}

		return relies;
	}

}
