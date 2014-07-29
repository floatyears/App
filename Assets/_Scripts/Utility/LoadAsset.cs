using UnityEngine;
using System.Collections.Generic;

public class LoadAsset 
{
	private static LoadAsset instance ;

	public static LoadAsset Instance
	{
		get
		{
			if(instance == null)
				instance = new LoadAsset();

			return instance;
		}
	}

	public void StartLoad()
	{
		GameInput.OnUpdate += HandleOnUpdate;
	}

	public void CancelLoad()
	{
		GameInput.OnUpdate -= HandleOnUpdate;
	}

	void HandleOnUpdate ()
	{
		// to do load
	}

	private string prefabPath = "Prefabs/"; 

	private string texturePath = "Texture/";

	private Dictionary<string,Object> objectDic = new Dictionary<string, Object>();

	Object GetCache(string name)
	{
		if(objectDic.ContainsKey(name))
			return objectDic[name];
		else
			return null;
	}

	public void LoadAssetFromResources(string name,ResourceEuum rEnum, ResourceCallback callback)
	{
//		Object obj = GetCache(name);

//		if(obj == null)
//		{
		string reallyPath = DisposePathByType(rEnum) + name;

		ResourceManager.Instance.LoadLocalAsset (reallyPath,o=>{
			callback(o);
		});//Resources.Load(reallyPath);
			
//			objectDic.Add(name,obj);
//		}

//		return obj;
	}

//	public Texture2D LoadMapItem()
//	{
//		MapConfig mc = BattleQuest.mapConfig;
//
//		return ResourceManager.Instance.LoadLocalAsset (mc.GetMapPath ()) as Texture2D;
//	}

	public object LoadAssetFromResources(int id)
	{
		ItemData loadData = Config.Instance.CardData[id];
		string reallyPath = DisposePathByType(loadData.resourceEnum) + loadData.itemName;
		return Resources.Load(reallyPath);
	}

	string DisposePathByType (ResourceEuum rEnum)
	{
		switch (rEnum) 
		{
		case ResourceEuum.Prefab:
			return prefabPath;

		case ResourceEuum.Image:
			return texturePath;

		default:
			return "";
		}	
	}


}
