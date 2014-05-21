using UnityEngine;
using System.Collections;

public class StageItemView : MonoBehaviour {
	public Font myFont;
	private bool isFarthestArrive;
	public bool IsFarthestArrived{
		get{
			return isFarthestArrive;
		}
		set{
			isFarthestArrive = value;
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
			//Debug.Log("StageItemView :: Stage's Quest Count = " + data.QuestInfo.Count);
			Debug.Log("StageItemView :: " + " name is " + gameObject.name +", isClear == " 
			          + DataCenter.Instance.QuestClearInfo.IsStoryStageClear(data.ID));
			SetIcon();
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
//			Debug.LogError("Stage.Pos is NULL!" + "  gameObject  is : " + gameObject);
			//this.gameObject.SetActive(false);
		}
		gameObject.transform.localPosition = new Vector3(x, y, 0);
	}

	private void SetIcon(){
		UISprite icon = transform.FindChild("Icon/Background").GetComponent<UISprite>();
		bool isClear = DataCenter.Instance.QuestClearInfo.IsStoryStageClear(data.ID);

		if(isClear){
			icon.spriteName = "icon_stage_" + (int.Parse(gameObject.name) + 1 );
			UIEventListener.Get(this.gameObject).onClick = StepIntoNextScene;
		}
		else{
			UISprite circleSpr = transform.FindChild("Sprite_Circle").GetComponent<UISprite>();
			UISprite handSpr = transform.FindChild("Sprite_Hand").GetComponent<UISprite>();
			
			circleSpr.gameObject.SetActive(isFarthestArrive);
			handSpr.gameObject.SetActive(isFarthestArrive);

			if(isFarthestArrive){
				Debug.Log("stageID = " + data.ID + ", isFarthestArrive = " + isFarthestArrive);
				icon.spriteName = icon.spriteName = "icon_stage_" + (int.Parse(gameObject.name) + 1 );

				iTween.ScaleTo(circleSpr.gameObject, iTween.Hash("x", 0.75f, "y", 0.75f, "time", 1f, 
				                                                 "looptype", iTween.LoopType.pingPong,
				                                                 "easetype", iTween.EaseType.linear));
				iTween.MoveFrom(handSpr.gameObject, iTween.Hash("x", 0f, "y", 50f, "time", 1f, "islocal", true, 
				                                                "looptype", iTween.LoopType.pingPong,
				                                                "easetype", iTween.EaseType.linear));

				UIEventListener.Get(this.gameObject).onClick = StepIntoNextScene;
		}
		else{
			icon.spriteName = "icon_stage_lock";
				UIEventListener.Get(this.gameObject).onClick = ShowTip;
		}

	}
	}

	private void StepIntoNextScene(GameObject item){
		UIManager.Instance.ChangeScene(SceneEnum.QuestSelect); //before
		if (DataCenter.gameState == GameState.Evolve && evolveCallback != null) {
			evolveCallback ();
		} else {
			MsgCenter.Instance.Invoke(CommandEnum.GetQuestInfo, data); //after		
		}
	}

	private void ShowTip(GameObject item){
		Debug.Log("StageItemView.ShowTip(), this stage is not accessible...");
	
		for (int i = 0; i < gameObject.transform.childCount; i++)
			if(transform.GetChild(i).name == "Tip") return;
		
		GameObject tipObj = new GameObject("Tip");
		UILabel tipLabel = tipObj.AddComponent<UILabel>();
		tipLabel.text = "Not Open";
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
}
