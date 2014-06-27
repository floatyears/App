using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventItemView : MonoBehaviour{
	UILabel time;

	public List<string> stageOrderList1 = new List<string>(){
		{"icon_stage_special"},
		{"icon_stage_fire"},
		{"icon_stage_water"},
		{"icon_stage_wind"},
		{"icon_stage_light"},
		{"icon_stage_dark"},
		{"icon_stage_none"}
	};

	public List<string> stageOrderList2 = new List<string>(){
		{"icon_stage_fire"},
		{"icon_stage_water"},
		{"icon_stage_wind"},
		{"icon_stage_light"},
		{"icon_stage_dark"},
		{"icon_stage_none"},
		{"icon_stage_special"}
	};

	public Font myFont;
	private StageState stageClearState;
	public StageState StageClearState{
		get{
			return stageClearState;
		}
		set{
			stageClearState = value;
		}
	}

	public static EventItemView Inject(GameObject view){
		EventItemView eventItemView = view.GetComponent<EventItemView>();
		if(eventItemView == null) eventItemView = view.AddComponent<EventItemView>();

		return eventItemView;
	}

	private static GameObject prefab;
	public static GameObject Prefab{
		get{
			if(prefab == null){
				string sourcePath = "Prefabs/UI/Quest/EventItemPrefab";
				prefab = ResourceManager.Instance.LoadLocalAsset(sourcePath, null) as GameObject;
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
			if(time == null)
				time = gameObject.transform.FindChild ("Time").GetComponent<UILabel>();
			if(data == null){
				Debug.LogError("StageItemView, Data is NULL!");
				return;
			}

			if(DataCenter.gameState == GameState.Normal) {
				SetIconView();
			}
			else{
				SetEvolveIcon();
			}

			SetPosition();
		}
	}

	public Callback evolveCallback;
	
	private void SetPosition(){
		float x = 0;
		float y = 0;
		if(data.Pos != null){
			x = data.Pos.x - 320f;
			y = data.Pos.y - 450f;
		}
		else{
			Debug.LogError("Stage.Pos is NULL!" + "  gameObject  is : " + gameObject);
			this.gameObject.SetActive(false);
		}
		gameObject.transform.localPosition = new Vector3(x, y, 0);
	}

	private void SetIconView(){
//		UISprite icon = transform.FindChild("Icon/Background").GetComponent<UISprite>();
//		if (data.Type == bbproto.QuestType.E_QUEST_STORY) {
//			StageState clearState = DataCenter.Instance.QuestClearInfo.GetStoryStageState (data.ID);
//			ShowIconByState (clearState);
//		} else if(data.Type == bbproto.QuestType.E_QUEST_EVENT){
//
//		}
		uint currentTime = GameTimer.GetInstance().GetCurrentSeonds();
		if (data.StartTime > currentTime) {
			if(currentTime < data.endTime){
//				time.enabled = false;
				ShowIconByState (StageState.EVENT_OPEN);
				time.text = TextCenter.GetText("Stage_Event_Remain") + GameTimer.GetTimeBySeconds(data.endTime - currentTime);
			}
			else{
				Destroy(this.gameObject);
			}

		} else {
//			time.enabled = true;
			time.text = TextCenter.GetText("Stage_Event_Close") + GameTimer.GetTimeBySeconds(data.StartTime - currentTime);
			ShowIconByState (StageState.EVENT_CLOSE);
		}

	}

	void SetEvolveIcon() {
//		UISprite icon = transform.FindChild("Icon/Background").GetComponent<UISprite>();
//		StageState clearState = StageState.CLEAR; //DataCenter.Instance.QuestClearInfo.GetStoryStageState(data.ID);
//		ShowIconByState(clearState);
		UIEventListener.Get(this.gameObject).onClick = StepIntoNextScene;
	}

	private void StepIntoNextScene(GameObject item){
		UIManager.Instance.ChangeScene(SceneEnum.QuestSelect); //do before
		if (DataCenter.gameState == GameState.Evolve && evolveCallback != null) {
			evolveCallback ();
		} else {
			MsgCenter.Instance.Invoke(CommandEnum.GetQuestInfo, data); //do after		
		}
	}

	private void ShowTip(GameObject item){
		Debug.Log("StageItemView.ShowTip(), this stage is not accessible...");
	
		for (int i = 0; i < gameObject.transform.childCount; i++)
			if(transform.GetChild(i).name == "Tip") return;
		
		GameObject tipObj = new GameObject("Tip");
		UILabel tipLabel = tipObj.AddComponent<UILabel>();
		tipLabel.text = TextCenter.GetText("Stage_Locked");
		tipLabel.depth = 6;
		tipLabel.trueTypeFont = ViewManager.Instance.DynamicFont;
		tipLabel.fontSize = 36;
		tipLabel.color = Color.yellow;
		tipLabel.width = 200;
		tipObj.transform.parent = transform;
		tipObj.transform.localScale = Vector3.one;
		tipObj.transform.localPosition = Vector3.zero;

		TweenAlpha tweenAlpha = tipObj.AddComponent<TweenAlpha>();
		tweenAlpha.from = 1f;
		tweenAlpha.to = 0f;
		tweenAlpha.duration = 2f;
		tweenAlpha.PlayForward();
		tweenAlpha.eventReceiver = gameObject;
		tweenAlpha.callWhenFinished = "DestoryTipObj";
	}

	private void DestoryTipObj(){
		Debug.Log("DestoryTipObj()...");
		GameObject.Destroy(transform.FindChild("Tip").gameObject);
	}


	public void ShowIconByState(StageState state){
		UISprite icon = transform.FindChild("Icon/Background").GetComponent<UISprite>();

		if(state == StageState.EVENT_OPEN){
			ShowIconAccessState(icon);
			
//			string sourcePath = "Prefabs/UI/UnitItem/ArriveStagePrefab";
//			GameObject prefab = Resources.Load(sourcePath) as GameObject;
//			NGUITools.AddChild(gameObject, prefab);
			UIEventListener.Get(this.gameObject).onClick = StepIntoNextScene;
		}else if(state == StageState.EVENT_CLOSE){
			icon.spriteName = "icon_stage_lock";
			UIEventListener.Get(this.gameObject).onClick = ShowTip;
		}
	}

	private void ShowIconAccessState(UISprite icon){
		int stagePos = int.Parse(gameObject.name);

		if(data.CityId == 1){
			//first city by the order of list1
			icon.spriteName = stageOrderList1[ stagePos ];
		}
		else{
			//others by order of list2
			icon.spriteName = stageOrderList2[ stagePos ];
		}
	}


}
