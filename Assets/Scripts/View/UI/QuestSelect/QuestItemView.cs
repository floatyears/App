using UnityEngine;
using System.Collections;

public class QuestItemView : MonoBehaviour {
	private UISprite bgSpr;
	public static GameObject prefab;
	public static GameObject Prefab{
		get{
			if(prefab == null){
				string sourcePath = "Prefabs/QuestItem";
				prefab = Resources.Load(sourcePath) as GameObject;
			}
			return prefab;
		}
	}
	
	public void Awake(){
		bgSpr = transform.FindChild("Background").GetComponent<UISprite>();
	}

	private TQuestInfo data;
	public TQuestInfo Data{
		get{
			return data;
		}
		set{
			data = value;
			if(data == null){
				Debug.LogError("QuestItemView, Data is NULL!");
				return;
			}
			
			//TODO
			bgSpr.spriteName = "1";
		}
	}
	
	
	private void CreateQuestView(){
		
	}

}
