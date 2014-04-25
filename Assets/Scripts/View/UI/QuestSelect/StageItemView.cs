using UnityEngine;
using System.Collections;

public class StageItemView : MonoBehaviour {
	private UITexture backgound;
	private UILabel nameLabel;

	public static StageItemView Inject(GameObject view){
		StageItemView stageItemView = view.GetComponent<StageItemView>();
		if(stageItemView == null) stageItemView = view.AddComponent<StageItemView>();
		return stageItemView;
	}

	private static GameObject prefab;
	public static GameObject Prefab{
		get{
			if(prefab == null){
				string sourcePath = "Prefabs/UI/Quest/StageItem";
				prefab = Resources.Load(sourcePath) as GameObject;
			}
			return prefab;
		}
	}

	public void Awake(){
		backgound = transform.FindChild("Background").GetComponent<UITexture>();
	}

	private uint cityID;
	public uint CityID{
		get{
			return cityID;
		}
		set{
			cityID = value;
		}
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
			ShowBackground();
			ShowNameLabel();


		}
	}

	private void ShowBackground(){
		string sourcePath = string.Format("{0}_{1}", cityID, Data.ID);
		Texture2D tex = Resources.Load(sourcePath) as Texture2D;
		backgound.mainTexture = tex;
	}

	private void ShowNameLabel(){

	}

	private void GenerateQuestInfo(){
		for (int i = 0; i < data.QuestInfo.Count; i++){
			GameObject cell = NGUITools.AddChild(this.gameObject, QuestItemView.Prefab);
			cell.name = i.ToString();
			float pos_x = 0;//Get
			float pos_y = 0;//Get
			cell.transform.localPosition = new Vector3(pos_x, pos_y, 0);

			QuestItemView questItemView = QuestItemView.Inject(cell);
			questItemView.Data = data.QuestInfo[ i ];
		}
	}
	
}
