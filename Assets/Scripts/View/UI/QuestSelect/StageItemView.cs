using UnityEngine;
using System.Collections;

public class StageItemView : MonoBehaviour {
	private UISprite bgSpr;
	private GameObject prefab;

	public void Awake(){
//		string sourcePath = "Prefabs/StageItem";
//		prefab = Resources.Load(sourcePath) as GameObject;

		bgSpr = transform.FindChild("Background").GetComponent<UISprite>();
	}

	private TStageInfo data;
	public TStageInfo Data{
		get{
			return data;
		}
		set{
			data = value;
			if(data == null){
				Debug.LogError("StageItemView, Data is NULL!");
				return;
			}
			//TODO
			bgSpr.spriteName = "1";
		}
	}
	
}
