using UnityEngine;
using System.Collections;

public class StageItemView : MonoBehaviour {

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
			SetIcon();
			SetPosition();
			AddEventListener();
		}
	}
	
	private void SetPosition(){
		float x = 0f;
		float y = 0f;
		if(data.Pos != null){
			x = data.Pos.x;
			y = data.Pos.y;
		}
		else{
			Debug.LogError("Stage.Pos is NULL!");
			//this.gameObject.SetActive(false);
		}
		gameObject.transform.localPosition = new Vector3(x, y, 0);
	}

	private void SetIcon(){
		UISprite icon = transform.FindChild("Icon/Background").GetComponent<UISprite>();
		bool isClear = DataCenter.Instance.QuestClearInfo.IsStoryStageClear(data.ID);
		Debug.Log("StageItemView.SetIcon(), isClear is : " + isClear);
		Debug.Log("StageItemView.SetIcon(), stageId is : " + data.ID);
		if(isClear){
			icon.spriteName = "icon_stage_" + data.ID;
		}
		else{
			icon.spriteName = "icon_stage_lock";
		}
	}

	private void AddEventListener(){
		if(data == null)
			UIEventListener.Get(this.gameObject).onClick = null;
		else
			UIEventListener.Get(this.gameObject).onClick = ClickItem;
	}

	private void ClickItem(GameObject item){
		Debug.Log(string.Format("StageItemView.ClickItem(), Picking Stage...stageId is {0}, Stage name is : {1}", data.ID, data.StageName));
		//QuestItemView thisQuestItemView = this.GetComponent<QuestItemView>();
		//UIManager.Instance.ChangeScene(SceneEnum.FriendSelect);//before
		//MsgCenter.Instance.Invoke(CommandEnum.OnPickQuest, thisQuestItemView);//after
	
	}
}
