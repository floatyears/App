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

	private Dictionary<string,object> objectDic = new Dictionary<string, object>();

	public object LoadLocalAsset( string path, string assetName = null) {
		if (string.IsNullOrEmpty (assetName)) {
			return LoadLocalNoCache(path);
		}
		else {
			return LoadLocalFromCache(assetName,path);
		}
	}

	object LoadLocalNoCache(string path) {
		return Resources.Load (path);
	}

	object LoadLocalFromCache(string assetName,string path) {
		object obj = null;
		if (objectDic.TryGetValue (assetName, out obj)) {
			return obj;
		} 
		else {
			obj = Resources.Load(path + assetName);
			objectDic.Add(assetName,obj);
			return obj;
		}
	}
}
