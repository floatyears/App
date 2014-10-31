using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class EventItemView : MonoBehaviour{
	UILabel time;
	UIAtlas atlas;
	UISprite timeBg;

	public List<string> stageOrderList1 = new List<string>(){
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

	private StageInfo data;
	public StageInfo Data{
		get{
			return data;
		}
		set{
			data = value;
			if(time == null){
				time = gameObject.transform.FindChild ("Time").GetComponent<UILabel>();
				timeBg = gameObject.transform.FindChild("TimeBg").GetComponent<UISprite>();
			}
				
			if(data == null){
				Debug.LogError("StageItemView, Data is NULL!");
				return;
			}



//			if(DataCenter.gameState == GameState.Normal) {
				SetIconView();
//			}
//			else{
//				SetEvolveIcon();
//			}

			SetPosition();
		}
	}

	public Callback evolveCallback;
	
	private void SetPosition(){
		float x = 0;
		float y = 0;
		if(data.pos != null){
			x = data.pos.x;
			y = data.pos.y;
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
//			StageState clearState = DataCenter.Instance.QuestData.QuestClearInfo.GetStoryStageState (data.ID);
//			ShowIconByState (clearState);
//		} else if(data.Type == bbproto.QuestType.E_QUEST_EVENT){
//
//		}
		UILabel eventName = gameObject.transform.FindChild ("EventName").GetComponent<UILabel>();
		eventName.text = TextCenter.GetText("EventName"+data.cityId.ToString());

		uint currentTime = GameTimer.GetInstance().GetCurrentSeonds();
		Debug.Log ("id: " + data.id + " current time: " + currentTime + " start time: " + data.StartTime + " end time: " + data.EndTime);
		if (data.StartTime < currentTime) {
			if(currentTime < data.EndTime){
//				time.enabled = false;
				ShowIconByState (StageState.EVENT_OPEN);
				time.text = string.Format(TextCenter.GetText("Stage_Event_Remain") , GameTimer.GetFormatRemainTime(data.EndTime - currentTime));
				timeBg.spriteName = "remain_bg";
			}
			else{
				Destroy(this.gameObject);
			}

		} else {
//			time.enabled = true;
			time.text = string.Format(TextCenter.GetText("Stage_Event_Close") , GameTimer.GetFormatRemainTime(data.StartTime - currentTime));
			timeBg.spriteName = "start_bg";
			ShowIconByState (StageState.EVENT_CLOSE);
		}

	}

	void SetEvolveIcon() {
//		UISprite icon = transform.FindChild("Icon/Background").GetComponent<UISprite>();
//		StageState clearState = StageState.CLEAR; //DataCenter.Instance.QuestData.QuestClearInfo.GetStoryStageState(data.ID);
//		ShowIconByState(clearState);
		UIEventListenerCustom.Get(this.gameObject).onClick = StepIntoNextScene;
	}

	private void StepIntoNextScene(GameObject item){
//		ModuleManager.Instance.ShowModule(ModuleEnum.QuestSelectModule); //do before
//		if (DataCenter.gameState == GameState.Evolve && evolveCallback != null) {
//			ModuleManager.Instance.ShowModule(ModuleEnum.QuestSelectModule);
//			evolveCallback ();
//		} else {
//			MsgCenter.Instance.Invoke(CommandEnum.GetQuestInfo, data); //do after		
			ModuleManager.Instance.ShowModule(ModuleEnum.QuestSelectModule,"data", data);
//		}
	}

	private void ShowTip(GameObject item){
		Debug.Log("StageItemView.ShowTip(), this stage is not accessible...");
	
		for (int i = 0; i < gameObject.transform.childCount; i++)
			if(transform.GetChild(i).name == "Tip") return;
		
		GameObject tipObj = new GameObject("Tip");
		UILabel tipLabel = tipObj.AddComponent<UILabel>();
		tipLabel.text = TextCenter.GetText("Stage_Locked");
		tipLabel.depth = 6;
//		tipLabel.trueTypeFont = ViewManager.Instance.DynamicFont;
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
		ResourceManager.Instance.LoadLocalAsset ("Atlas/Event_Atlas", o => {
			atlas = (o as GameObject).GetComponent<UIAtlas>();

			UISprite icon = transform.FindChild("Icon/Background").GetComponent<UISprite>();
			UISprite attr = transform.FindChild("Icon/Attr").GetComponent<UISprite>();
			UISprite bg = transform.FindChild("Background").GetComponent<UISprite>();
			icon.atlas = atlas;
			attr.atlas = atlas;
			bg.atlas = atlas;

			if(data.cityId == 101){
				bg.spriteName = "icon_event_1";
			}else{
				bg.spriteName = "icon_event_2";
			}
			TweenRotation ro = attr.GetComponent<TweenRotation>();
			TweenRotation ro1 = icon.GetComponent<TweenRotation>();

			if(state == StageState.EVENT_OPEN){
				ShowIconAccessState(attr);
				
				//			string sourcePath = "Prefabs/UI/UnitItem/ArriveStagePrefab";
				//			GameObject prefab = Resources.Load(sourcePath) as GameObject;
				//			NGUITools.AddChild(gameObject, prefab);
				icon.spriteName = "icon_event_a";
				icon.width = 73;
				icon.height = 73;
//				attr.spriteName = data.Type;

				ro.enabled = true;
				ro.ResetToBeginning();
				ro1.enabled = true;
				ro1.ResetToBeginning();

				UIEventListenerCustom.Get(this.gameObject).onClick = StepIntoNextScene;
			}else if(state == StageState.EVENT_CLOSE){
				icon.spriteName = "icon_event_b";
				icon.width = 78;
				icon.height = 78;
				ro.enabled = false;
				ro1.enabled = false;
				UIEventListenerCustom.Get(this.gameObject).onClick = ShowTip;
			}
		});

	}

	private void ShowIconAccessState(UISprite icon){
//		int stagePos;
//		int.TryParse(,out stagePos);

		icon.spriteName = stageOrderList1[ (int)data.id%10 - 1 ];
	}


}
