using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class UnitDetailCenterPanel : UIComponentUnity,IUICallback  {
	UITexture unitBodyTex;
	UnitDetailTopPanel topPanel;
	GameObject materilItem;
	GameObject parent;

	public override void Init ( UIInsConfig config, IUICallback origin ) {
		base.Init (config, origin);
		InitUI();
		InitEffect ();
	}
	
	public override void ShowUI () {
		MsgCenter.Instance.AddListener(CommandEnum.ShowUnitDetail, CallBackUnitData);
		MsgCenter.Instance.AddListener (CommandEnum.LevelUp, LevelUpFunc);
		base.ShowUI ();
//		UIManager.Instance.HideBaseScene();

		//TODO:
		//StartCoroutine ("nextState");
		//NoviceGuideStepEntityManager.Instance ().StartStep ();
	}
	
	public override void HideUI () {
		MsgCenter.Instance.RemoveListener(CommandEnum.ShowUnitDetail, CallBackUnitData);
		MsgCenter.Instance.RemoveListener (CommandEnum.LevelUp, LevelUpFunc);
		base.HideUI ();
		ClearEffectCache();
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

	//----------Init functions of UI Elements----------
	void InitUI() {
		unitBodyTex = FindChild< UITexture >("detailSprite");
		topPanel = GameObject.Find ("UnitDetailTopPanel(Clone)").GetComponent<UnitDetailTopPanel> ();
		materilItem = FindChild<Transform>("MaterialItem").gameObject;
		parent = FindChild<UIGrid> ("UIGrid").gameObject;
	}

	void ClickTex(GameObject go) {

	}

	void ShowInfo(TUserUnit userUnit) {
		ShowBodyTexture( userUnit ); 
		ShowUnitScale();
	}

//	private TUserUnit curUserUnit;
	public void CallBackUnitData(object data)	{
		TUserUnit userUnit = data as TUserUnit;
		if (userUnit != null) {
			ShowInfo (userUnit);
		}
	}

	void LevelUpFunc(object data) {
		RspLevelUp rlu = data as RspLevelUp;
		if(rlu ==null) {
			return;
		}
//		Debug.LogError("RspLevelUp ; " + rlu);
		PlayLevelUp(rlu);
	} 
	
	TUserUnit oldBlendUnit = null;
	TUserUnit newBlendUnit = null;
	Vector3 targetPosition;
	RspLevelUp levelUpData;

	Queue<GameObject> material = new Queue<GameObject> ();
	int count = 0;
	void PlayLevelUp(RspLevelUp rlu) {
		levelUpData = rlu;
		oldBlendUnit = DataCenter.Instance.oldUserUnitInfo;
		newBlendUnit = DataCenter.Instance.UserUnitList.GetMyUnit(levelUpData.blendUniqueId);

		for (int i = 0; i < rlu.partUniqueId.Count; i++) {
			TUnitInfo tui = DataCenter.Instance.UserUnitList.Get(rlu.partUniqueId[i]).UnitInfo;
			GameObject go = NGUITools.AddChild(parent, materilItem);
			go.SetActive(true);
			UISprite sprite = go.transform.Find("Avatar").GetComponent<UISprite>();
			DataCenter.Instance.GetAvatarAtlas(tui.ID, sprite);
			go.transform.Find("Background").GetComponent<UISprite>().spriteName = DGTools.GetItemBackgroundName(tui.Type);
			go.transform.Find("Sprite_Avatar_Border").GetComponent<UISprite>().spriteName = DGTools.GetItemBorderName(tui.Type);
			material.Enqueue(go);

			DataCenter.Instance.UserUnitList.DelMyUnit (rlu.partUniqueId[i]);
		}
		parent.GetComponent<UIGrid> ().Reposition ();
		count = material.Count * 2;
		newBlendUnit.UnitInfo.GetAsset (UnitAssetType.Profile, o =>{
			AudioManager.Instance.PlayAudio(AudioEnum.sound_check_role);
			DGTools.ShowTexture (unitBodyTex, o as Texture2D);
			Vector3 localposition = unitBodyTex.transform.localPosition;
			targetPosition = new Vector3(localposition.x, localposition.y + unitBodyTex.height * 0.5f, localposition.z) - parent.transform.localPosition; //unitBodyTex.transform.localPosition + Vector3.up * (unitBodyTex.height * 0.5f);
			ShowUnitScale();
			SetEffectCamera();
//			wfs = new WaitForSeconds();
			StartCoroutine(SwallowUserUnit());
		});
	}

	public void SetEffectCamera() {
		Camera camera = Main.Instance.effectCamera;
		camera.transform.eulerAngles = new Vector3 (15f, 0f, 0f);
		camera.orthographicSize = 1.3f;
	}

	public void RecoverEffectCamera() {
		Camera camera = Main.Instance.effectCamera;
		camera.transform.eulerAngles = new Vector3 (0f, 0f, 0f);
		camera.orthographicSize = 1f;
	}

	List<GameObject> effectCache = new List<GameObject>();
	GameObject levelUpEffect;
	GameObject swallowEffect;
	GameObject linhunqiuEffect;

	void InitEffect(){
		string path = "Effect/effect/LevelUpEffect";
		ResourceManager.Instance.LoadLocalAsset( path , o =>{
			levelUpEffect = o as GameObject;
		});

		path = "Effect/effect/level_up01";
		ResourceManager.Instance.LoadLocalAsset( path , o =>{
			swallowEffect = o as GameObject;
		});

		path = "Effect/effect/linhunqiu1";
		ResourceManager.Instance.LoadLocalAsset( path , o =>{
			linhunqiuEffect = o as GameObject;
		});
	}

	IEnumerator SwallowUserUnit () {
		yield return new WaitForSeconds(1f);

		while (material.Count > 0) {
			GameObject go = material.Dequeue();
			iTween.ScaleTo(go, iTween.Hash("y", 0f, "time", 0.2f));
			yield return new WaitForSeconds(0.2f);
			Destroy(go);
			GameObject lhqIns = NGUITools.AddChild(parent, linhunqiuEffect);
			lhqIns.transform.localPosition = go.transform.localPosition;
			lhqIns.transform.localScale = Vector3.zero;
			iTween.ScaleTo(lhqIns, iTween.Hash("y", 1f, "time", 0.2f));
			yield return new WaitForSeconds(0.2f);

			iTween.MoveTo(lhqIns, iTween.Hash("position", targetPosition, "time", 0.3f, "islocal", true));
			Debug.LogError("targetPosition : " + targetPosition);
			yield return new WaitForSeconds(0.3f);
			Destroy(lhqIns);
			GameObject swallowEffectIns = NGUITools.AddChild(gameObject, swallowEffect);
			yield return new WaitForSeconds(0.4f);
			Destroy(swallowEffectIns);
		}

		StartCoroutine (CreatEffect ());
	}

	IEnumerator CreatEffect() {
		yield return new WaitForSeconds(0.5f);
		GameObject go = Instantiate (levelUpEffect) as GameObject;
		effectCache.Add (go);
//		yield return new WaitForSeconds(0.1f);
//		go = Instantiate (levelUpEffect) as GameObject;
//		effectCache.Add (go);
		if (effectCache.Count == count) {
			yield return new WaitForSeconds(2f);
			ClearEffectCache ();
			topPanel.ShowPanel();
			RecoverEffectCamera();
			MsgCenter.Instance.Invoke(CommandEnum.ShowLevelupInfo);
		} else {
			yield return new WaitForSeconds(1.5f);
			StartCoroutine (CreatEffect ());
		}
	}

	void ClearEffectCache(){
		for (int i = effectCache.Count - 1; i >= 0 ; i--) {
			GameObject go = effectCache[i];
			Destroy( go );
			effectCache.Remove(go);
		}
	}

	void ShowUnitScale(){
		TweenScale unitScale = gameObject.GetComponentInChildren< TweenScale >();
		TweenAlpha unitAlpha = gameObject.GetComponentInChildren< TweenAlpha >();
		
		unitAlpha.eventReceiver = this.gameObject;
		unitAlpha.callWhenFinished = "PlayCheckRoleAudio";
		
		if( unitScale == null || unitAlpha == null )
			return;
		
		unitScale.ResetToBeginning();
		unitScale.PlayForward();
		
		unitAlpha.ResetToBeginning();
		unitAlpha.PlayForward();
	}

	void ShowBodyTexture( TUserUnit data ){
		TUnitInfo unitInfo = data.UnitInfo;
		unitInfo.GetAsset( UnitAssetType.Profile, o=>{
			Texture2D target = o as Texture2D;
			unitBodyTex.mainTexture = target;
			if (target == null) {
				return;	
			}
			unitBodyTex.width = target.width;
			unitBodyTex.height = target.height;
		});

	}
}
