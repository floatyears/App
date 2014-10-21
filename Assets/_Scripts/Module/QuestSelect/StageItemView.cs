using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class StageItemView : MonoBehaviour{
	public string[] stageOrderList1 = new string[7] {
		"icon_stage_special",
		"icon_stage_fire",
		"icon_stage_water",
		"icon_stage_wind",
		"icon_stage_light",
		"icon_stage_dark",
		"icon_stage_none"
	};

	public string[] stageOrderList2 = new string[7] {
		"icon_stage_fire",
		"icon_stage_water",
		"icon_stage_wind",
		"icon_stage_light",
		"icon_stage_dark",
		"icon_stage_none",
		"icon_stage_special"
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
			if(data == null){
				Debug.LogError("StageItemView, Data is NULL!");
				return;
			}

//			if(DataCenter.gameState == GameState.Normal) {
				SetIconView();

				ShowStar();
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
			x = data.pos.x;  // - 320f
			y = data.pos.y;  // - 450f
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
			StageState clearState = DataCenter.Instance.QuestData.QuestClearInfo.GetStoryStageState (data.id);
			ShowIconByState (clearState);
			
//		} else if(data.Type == bbproto.QuestType.E_QUEST_EVENT){
//
//		}

	}

	void SetEvolveIcon() {
//		UISprite icon = transform.FindChild("Icon/Background").GetComponent<UISprite>();
//		StageState clearState = StageState.CLEAR; //DataCenter.Instance.QuestData.QuestClearInfo.GetStoryStageState(data.ID);
//		ShowIconByState(clearState);
		UIEventListenerCustom.Get(this.gameObject).onClick = StepIntoNextScene;
	}

	private void StepIntoNextScene(GameObject item){
		AudioManager.Instance.PlayAudio (AudioEnum.sound_click);

//		ModuleManager.Instance.ShowModule(ModuleEnum.QuestSelectModule); //do before
//		ModuleManager.Instance.ShowModule(ModuleEnum.QuestSelectModule);

//		if (DataCenter.gameState == GameState.Evolve && evolveCallback != null) {
//			evolveCallback ();
//			ModuleManager.Instance.ShowModule(ModuleEnum.QuestSelectModule);
//		} else {
//			MsgCenter.Instance.Invoke(CommandEnum.GetQuestInfo, data); //do after		
			ModuleManager.Instance.ShowModule(ModuleEnum.QuestSelectModule,"data",data);
//		}
	}

	private void ShowTip(GameObject item){
		Debug.Log("StageItemView.ShowTip(), this stage is not accessible...");

		AudioManager.Instance.PlayAudio (AudioEnum.sound_click_invalid);
	
		for (int i = 0; i < gameObject.transform.childCount; i++)
			if(transform.GetChild(i).name == "Tip") return;
		
		GameObject tipObj = new GameObject("Tip");
		UILabel tipLabel = tipObj.AddComponent<UILabel>();
		tipLabel.text = TextCenter.GetText("Stage_Locked");
		tipLabel.depth = 6;
//		tipLabel.trueTypeFont = ViewManager.Instance.DynamicFont;
		tipLabel.fontSize = 36;
		tipLabel.color = Color.red;
//		tipLabel.fon
		tipLabel.effectStyle = UILabel.Effect.Outline;
		tipLabel.effectColor = Color.white;
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

	private void ShowStar() {

		UISprite star = transform.FindChild("Star").GetComponent<UISprite>();
		int stageStar = DataCenter.Instance.NormalCopyInfo.GetStageStar(data.id);
		star.width = star.height * stageStar;
	}

	GameObject insPrefab = null;
	public void ShowIconByState(StageState state){
		UISprite icon = transform.FindChild("Icon/Background").GetComponent<UISprite>();
		tag = "Untagged";
		if(state == StageState.LOCKED){
			icon.spriteName = "icon_stage_lock";
			UIEventListenerCustom.Get(this.gameObject).onClick = ShowTip;
		}
		else if(state == StageState.NEW){
			tag = "stage_new";
			ShowIconAccessState(icon);
			if(insPrefab == null) {
				string sourcePath = "Prefabs/UI/UnitItem/ArriveStagePrefab";
				GameObject prefab = Resources.Load(sourcePath) as GameObject;
				insPrefab = NGUITools.AddChild(gameObject, prefab);
			}

			UIEventListenerCustom.Get(this.gameObject).onClick = StepIntoNextScene;
		}
		else if(state == StageState.CLEAR){
			ShowIconAccessState(icon);
			UIEventListenerCustom.Get(this.gameObject).onClick = StepIntoNextScene;
		}
	}

	private void ShowIconAccessState(UISprite icon){
		int stagePos = int.Parse(gameObject.name);

		if(data.cityId == 1){
			//first city by the order of list1
			icon.spriteName = stageOrderList1[ stagePos ];
		}
		else{
			//others by order of list2
			icon.spriteName = stageOrderList2[ stagePos ];
		}
	}


}
