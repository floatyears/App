using UnityEngine;
using System.Collections;

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

	public Object LoadAssetFromResources(string name,ResourceEuum rEnum)
	{
		string reallyPath = DisposePathByType(rEnum);

		return Resources.Load(reallyPath);
	}

	string DisposePathByType (ResourceEuum rEnum)
	{
		switch (rEnum) 
		{
			case ResourceEuum.Prefab:
				return prefabPath;
				break;
			case ResourceEuum.Image:
				return texturePath;
				break;
			default:
					break;
		}	

		return "";
	}
}
