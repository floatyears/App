using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourceManager : MonoBehaviour{

	public const string RelyOnSource = "RelyOnResource";

	public static Dictionary<ResourceAssetBundle,AssetBundleObj> assetBundles = new Dictionary<ResourceAssetBundle,AssetBundleObj> ();

	private static ResourceManager instance;

	public static ResourceManager Instance
	{
		get {
			if(instance == null)
				instance = FindObjectOfType<ResourceManager>();
			return instance;
		}
	}

	private AssetBundle uiAssets;

	private Dictionary<string,object> objectDic = new Dictionary<string, object>();

	public Object LoadLocalAsset( string path, ResourceCallback callback ) {
		//the following resource will not be dynamiclly download.
		if (string.IsNullOrEmpty (path)) {
			return null;	
		}

		if (path.IndexOf ("Loading") >= 0 || path.IndexOf ("UIInsConfig") >= 0 || path.IndexOf ("ScreenMask") >= 0 || path.IndexOf ("CommonNoteWindow") >= 0) {
			if (callback != null){
				callback (Resources.Load (path));
				return null;
			}else{
				return Resources.Load (path);
			}

		}

		if (path.IndexOf ("Config") == 0 || path.IndexOf ("Prefabs") == 0 || path.IndexOf ("Language") == 0 || path.IndexOf ("Protobuf") == 0 || path.IndexOf ("Avatar") == 0 || path.IndexOf ("Profile") == 0 || path.IndexOf ("Atlas") == 0) {
#if UNITY_EDITOR
			string ext = null;
			if(path.IndexOf ("Config") == 0){
				ext = ".json";
			}else if(path.IndexOf ("Prefabs") == 0){
				ext = ".prefab";

				ResourceAssetBundle key = GetBundleKeyByPath(path);

				if(!assetBundles.ContainsKey(key)){
					assetBundles[key] = new AssetBundleObj(key,path,callback);
					StartCoroutine(DownloadResource(key));
				}else{
					if(assetBundles[key].isLoading){
						assetBundles[key].callbackList.Add(path,callback);
					}else{
						if(callback != null){
							callback(assetBundles[key].assetBundle.Load(path.Substring(path.LastIndexOf('/')+1), typeof(GameObject)));
							return null;
						}else{
							return assetBundles[key].assetBundle.Load(path.Substring(path.LastIndexOf('/')+1));
						}
					}

				}
				return null;
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
			}else if(path.IndexOf ("Avatar") == 0 || path.IndexOf ("Profile") == 0){
				ext = ".png";
			}

			Debug.Log ("assets load: " + "Assets/ResourceDownload/" + path + ext + "  "  + Resources.LoadAssetAtPath <Object>("Assets/ResourceDownload/" + path+ ext));
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
			Debug.Log ("resource load no editor");
			if(callback != null){
				callback(Resources.Load (path));
				return null;
			}else{
				return Resources.Load (path);
			}
			return null;

#endif
		} else {
			Debug.Log ("resource load from resource: " + path);
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
//	#if UNITY_IPHONE
		"file://" + Application.dataPath + "/ResourceDownload/" + GetBundleUrlByKey(key);
//	#elif UNITY_ANDROID
//		"file://" + "jar:file://" + Application.dataPath + "/ResourceDownload/" + GetBundleUrlByKey(key);
//	#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
//		"file://" + Application.dataPath + "/ResourceDownload/" + GetBundleUrlByKey(key);
//	#else
//		string.Empty;
//	#endif
		assetBundles [key].isLoading = true;

		Debug.Log ("download url: " + url);
		WWW www = new WWW (url);
		if (!string.IsNullOrEmpty (www.error)) {
			Debug.Log(www.error);		
		}
		yield return www;
		assetBundles [key].isLoading = false;
		assetBundles [key].assetBundle = www.assetBundle;

		if(checkRelies(assetBundles [key])){
			assetBundles [key].ExeCallback ();
		}

	}

	private ResourceAssetBundle GetBundleKeyByPath(string path){
//		Debug.Log ("download async: " + path);
		if (path.IndexOf ("Prefabs") >= 0) {
			return ResourceAssetBundle.UI;
		}
		if (path == RelyOnSource) {
			return ResourceAssetBundle.UI;
		}
		return ResourceAssetBundle.NONE;
	}

	private string GetBundleUrlByKey(ResourceAssetBundle key){
		switch (key) {
			case ResourceAssetBundle.UI:
				return "Output/AllUI.unity3d";
			case ResourceAssetBundle.UI_ATLAS:
				return "Output/UI_Atlas.unity3d";
			case ResourceAssetBundle.AVATAR:
				break;
			default:
				break;
		}
		return null;
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
				assetBundles[item] = new AssetBundleObj(item,RelyOnSource,o=>{
					if(checkRelies(aObj)){
						Debug.Log("rely resource complete");
						aObj.ExeCallback();
					}
				});
				Debug.Log(aObj.name + " rely on: " + item);
				StartCoroutine(DownloadResource(item));
			}else if(ResourceManager.assetBundles[item].isLoading){
				allComplete = false;
			}

		}
		return allComplete;
	}
}

public enum ResourceAssetBundle{
	NONE,
	UI,
	UI_ATLAS,
	AVATAR
}

public class AssetBundleObj{

	//the assetbundle pointer
	public AssetBundle assetBundle;

	//whether the resource is loading
	public bool isLoading;

	//all the callback attached to the resource(first param is path,seceond is callback).
	public Dictionary<string, ResourceCallback> callbackList ;

	//the resource relies on the other assetbundle.
	public List<ResourceAssetBundle> relies;

	//the resource name
	public ResourceAssetBundle name;

	public AssetBundleObj(ResourceAssetBundle rName,string path = null,ResourceCallback callback = null){
		callbackList = new Dictionary<string,ResourceCallback >();
		if(path != null)
			callbackList.Add (path,callback);
		name = rName;
		relies = GetResourceRelyResource(name);
	}

	public void ExeCallback(){
		foreach (var item in callbackList) {
//			Debug.Log("asset bundle: " + item.Key.Substring(item.Key.LastIndexOf('/')+1));
			if(item.Key == ResourceManager.RelyOnSource){
				item.Value(null);
			}else{
				item.Value(assetBundle.Load(item.Key.Substring(item.Key.LastIndexOf('/')+1)));
			}
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
