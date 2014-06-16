using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageItemView : MonoBehaviour{
	public List<string> stageOrderList1 = new List<string>(){
		{"icon_stage_other"},
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
		{"icon_stage_other"}
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

	public static StageItemView Inject(GameObject view){
		StageItemView stageItemView = view.GetComponent<StageItemView>();
		if(stageItemView == null) stageItemView = view.AddComponent<StageItemView>();
		return stageItemView;
	}

	private static GameObject prefab;
	public static GameObject Prefab{
		get{
			if(prefab == null){
				string sourcePath = "Prefabs/UI/Quest/StageItemPrefab";
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
		UISprite icon = transform.FindChild("Icon/Background").GetComponent<UISprite>();
		StageState clearState = DataCenter.Instance.QuestClearInfo.GetStoryStageState(data.ID);
		ShowIconByState(clearState);
	}

	void SetEvolveIcon() {
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


	private void ShowIconByState(StageState state){
		UISprite icon = transform.FindChild("Icon/Background").GetComponent<UISprite>();

		if(state == StageState.LOCKED){
			icon.spriteName = "icon_stage_lock";
			UIEventListener.Get(this.gameObject).onClick = ShowTip;
		}
		else if(state == StageState.NEW){
			ShowIconAccessState(icon);

			string sourcePath = "Prefabs/UI/UnitItem/ArriveStagePrefab";
			GameObject prefab = Resources.Load(sourcePath) as GameObject;
			NGUITools.AddChild(gameObject, prefab);
			UIEventListener.Get(this.gameObject).onClick = StepIntoNextScene;
		}
		else{
			ShowIconAccessState(icon);
			UIEventListener.Get(this.gameObject).onClick = StepIntoNextScene;
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
