using UnityEngine;
using System.Collections;

public class StageItemView : MonoBehaviour {
	private UITexture mapTex;
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

	public void Awake(){}

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
			ShowMap();
			ShowName();
			GenerateQuestInfo();
		}
	}

	private void ShowMap(){
		string sourcePath = string.Format("Stage/{0}_{1}", data.CityId, data.ID);
		Debug.Log("StageItemView.ShowMap(), sourcePath : " + sourcePath);
		Texture2D tex = Resources.Load(sourcePath) as Texture2D;

		if(mapTex == null){
			Debug.LogError("mapTex == null, getting...");
			mapTex = transform.FindChild("Texture_Map").GetComponent<UITexture>();
		}
		mapTex.mainTexture = tex;
	}

	private void ShowName(){
		if(nameLabel == null){
			Debug.LogError("nameLabel == null, getting...");
			nameLabel = transform.FindChild("Label_Name").GetComponent<UILabel>();
		}
		nameLabel.text = data.StageName;
	}

	private void GenerateQuestInfo(){
		for (int i = 0; i < data.QuestInfo.Count; i++){
			GameObject cell = NGUITools.AddChild(this.gameObject, QuestItemView.Prefab);
			cell.name = string.Format("Quest_{0}", data.QuestInfo[ i ].Name);

			if(data.QuestInfo[ i ].Pos != null){
				float pos_x = data.QuestInfo[ i ].Pos.x;
				float pos_y = data.QuestInfo[ i ].Pos.y;
				cell.transform.localPosition = new Vector3(pos_x, pos_y, 0);
			}
			else{
				Debug.LogError("QuestInfo.Pos == NULL!!!");
			}

			QuestItemView questItemView = QuestItemView.Inject(cell);
			questItemView.Data = data.QuestInfo[ i ];
		}
	}
	
}
