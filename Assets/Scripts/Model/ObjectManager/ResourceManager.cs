using UnityEngine;
using System.Collections.Generic;

public class ResourceManager {
	private static ResourceManager instance;

	public static ResourceManager Instance
	{
		get {
			if(instance == null)
				instance = new ResourceManager();
			return instance;
		}
	}

	private AssetBundle uiAssets;

	private Dictionary<string,object> objectDic = new Dictionary<string, object>();

	public Object LoadLocalAsset( string path, string assetName = null) {
		if (string.IsNullOrEmpty (assetName)) {
			return LoadLocalNoCache(path);
		}
		else {
			return LoadLocalFromCache(assetName,path);
		}
	}

	Object LoadLocalNoCache(string path) {
		//the following resource will not be dynamiclly download.
		if(path.IndexOf("Loading")> 0 || path.IndexOf("UIInsConfig")> 0 || path.IndexOf("ScreenMask")> 0 || path.IndexOf("CommonNoteWindow")> 0 ){
			return Resources.Load (path);
		}

		if (path.IndexOf ("Config") == 0 || path.IndexOf ("Prefabs") == 0 || path.IndexOf ("Language") == 0 || path.IndexOf ("Protobuf") == 0 || path.IndexOf ("Avatar") == 0 || path.IndexOf ("Profile") == 0 || path.IndexOf ("Atlas") == 0) {
#if UNITY_EDITOR
			string ext = null;
			if(path.IndexOf ("Config") == 0){
				ext = ".json";
			}else if(path.IndexOf ("Prefabs") == 0 || path.IndexOf ("Atlas") == 0){
				ext = ".prefab";

//				if(uiAssets == null){
//					uiAssets = AssetBundle.CreateFromFile("Assets/ResourceDownload/Download/AllUI");
//				}
//
//				return uiAssets.Load(path);

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

			//Debug.Log ("assets load: " + "Assets/ResourceDownload/" + path + ext + "  "  + Resources.LoadAssetAtPath <Object>("Assets/ResourceDownload/" + path+ ext));
			return Resources.LoadAssetAtPath<Object> ("Assets/ResourceDownload/" + path + ext);
#else 
//			if(path.IndexOf ("Config") == 0){
//
//
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
			return Resources.Load (path);

#endif
		} else {
			//Debug.Log ("resource load from resource: " + path);
			return Resources.Load (path);
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
}
