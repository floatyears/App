using UnityEngine;
using System.Collections;

public class QuestItemView : MonoBehaviour {
	private UISprite bgSpr;
	private UILabel nameLabel;

	private static GameObject prefab;
	public static GameObject Prefab{
		get{
			if(prefab == null){
				string sourcePath = "Prefabs/UI/Quest/QuestViewItem";
				prefab = Resources.Load(sourcePath) as GameObject;
			}
			return prefab;
		}
	}

	public static QuestItemView Inject(GameObject view){
		QuestItemView stageItemView = view.GetComponent<QuestItemView>();
		if(stageItemView == null) stageItemView = view.AddComponent<QuestItemView>();
		return stageItemView;
	}
	
	public void Awake(){

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
			ShowIcon();
			ShowName();

		}
	}
	
	private void ShowIcon(){
		if(bgSpr == null){
			Debug.LogError("bgSpr == null, getting...");
			bgSpr = transform.FindChild("Sprite_Boss_Avatar").GetComponent<UISprite>();
		}
		bgSpr.atlas = DataCenter.Instance.GetAvatarAtlas(data.BossID[ 0 ]);
		bgSpr.spriteName = data.BossID[ 0 ].ToString();
	}
	
	private void ShowName(){
		if(nameLabel == null){
			Debug.LogError("nameLabel == null, getting...");
			nameLabel = transform.FindChild("Label_Name").GetComponent<UILabel>();
		}
		nameLabel.text = data.Name;
	}

}
