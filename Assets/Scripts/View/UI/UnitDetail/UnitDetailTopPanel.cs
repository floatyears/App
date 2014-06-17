using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class UnitDetailTopPanel : UIComponentUnity,IUICallback {

	UISprite type;
	UILabel cost;
	UILabel number;
	UILabel name;
	UISprite lightStar;
	UISprite grayStar;

	private int grayWidth = 28;
	private int lightWidth = 30;
//	UISprite star;
//	UISprite type;

//	private List<UISprite> stars;

	public bool fobidClick = false;

	UIButton favBtn;
	
	public override void Init ( UIInsConfig config, IUICallback origin ) {
		base.Init (config, origin);
		//GetUnitMaterial();
		//InitEffect();
		InitUI();
	}
	
	public override void ShowUI () {
		base.ShowUI ();
		UIManager.Instance.HideBaseScene();

		MsgCenter.Instance.AddListener(CommandEnum.ShowUnitDetail, CallBackUnitData);
		MsgCenter.Instance.AddListener(CommandEnum.ShowFavState,  ShowFavState);
		MsgCenter.Instance.AddListener(CommandEnum.LevelUp, CallBackUnitData);
		//TODO:
		//StartCoroutine ("nextState");
//		NoviceGuideStepEntityManager.Instance ().StartStep ();
	}
	
	//	IEnumerator nextState()
	//	{
	//		yield return new WaitForSeconds (1);
	//		NoviceGuideStepEntityManager.Instance ().NextState ();
	//	}
	
	public override void HideUI () {
		MsgCenter.Instance.RemoveListener(CommandEnum.ShowUnitDetail, CallBackUnitData);
		MsgCenter.Instance.RemoveListener(CommandEnum.LevelUp, CallBackUnitData);
		MsgCenter.Instance.RemoveListener(CommandEnum.ShowFavState,  ShowFavState);
		base.HideUI ();
//		if (IsInvoking ("CreatEffect")) {
//			CancelInvoke("CreatEffect");
//		}
//		//ClearEffectCache();
//		UIManager.Instance.ShowBaseScene();
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}
	
	
	//----------Init functions of UI Elements----------
	void InitUI() {
		favBtn = transform.FindChild("Button_Lock").GetComponent<UIButton>();
		UIEventListener.Get(favBtn.gameObject).onClick = CollectCurUnit;

		cost = transform.FindChild("Cost").GetComponent<UILabel>();
		number = transform.FindChild("No").GetComponent<UILabel>();
		name = transform.FindChild("Name").GetComponent<UILabel>();
		type = transform.FindChild ("Type").GetComponent<UISprite> ();
		grayStar = transform.FindChild ("Star2").GetComponent<UISprite> ();
		lightStar = transform.FindChild ("Star2/Star1").GetComponent<UISprite> ();
	}

	TUserUnit oldBlendUnit = null;
	TUserUnit newBlendUnit = null;
	
	private TUserUnit curUserUnit;
	private void CallBackUnitData(object d){
		TUserUnit userUnit = d as TUserUnit;
		
		if (userUnit != null) {
			ShowInfo (userUnit);
		} else {
			RspLevelUp rlu = d as RspLevelUp;
			if(rlu ==null) {
				return;
			}
			PlayLevelUp(rlu);
		}
	}

	RspLevelUp levelUpData;
	void PlayLevelUp(RspLevelUp rlu) {
		levelUpData = rlu;
//		oldBlendUnit = DataCenter.Instance.oldUserUnitInfo;
		newBlendUnit = DataCenter.Instance.UserUnitList.GetMyUnit(levelUpData.blendUniqueId);
		ShowInfo (newBlendUnit);
		gameObject.SetActive (false);
//		Debug.LogError (newBlendUnit.UnitInfo.ID);
//		Debug.LogError ("unitBodyTex : " + unitBodyTex + " newBlendUnit : " + newBlendUnit + " newBlendUnit.UnitInfo : " + newBlendUnit.UnitInfo.GetAsset (UnitAssetType.Profile));
//		DGTools.ShowTexture (unitBodyTex, newBlendUnit.UnitInfo.GetAsset (UnitAssetType.Profile));
//		unitInfoTabs.SetActive (false);
//		SetEffectCamera ();
//		StartCoroutine (CreatEffect ());
	}

	private void ShowInfo(TUserUnit data){
		curUserUnit = data;
		//ShowFavView(curUserUnit.IsFavorite);
		
		TUnitInfo unitInfo = data.UnitInfo;
		number.text = data.UnitID.ToString();
		
		//hp
		//		.text = data.Hp.ToString();
		
		//atk
		//		atkLabel.text = data.Attack.ToString();
		
		//name
		name.text = unitInfo.Name;
		
		//type
		type.spriteName = "type_" + unitInfo.UnitType;
		
		//cost
		cost.text = unitInfo.Cost.ToString();
		
		//race  
		//		raceLabel.text = unitInfo.UnitRace;
		
		//rare
		Debug.Log ("rare : " + unitInfo.Rare + "max rare: " + unitInfo.MaxRare);	
		int len = 0;
		if (unitInfo.MaxRare > unitInfo.Rare) {
			grayStar.enabled = true;
			grayStar.width = (unitInfo.MaxRare - unitInfo.Rare) * grayWidth;
			len = 2*unitInfo.Rare - unitInfo.MaxRare;
		} else {
			grayStar.enabled = false;
			len = unitInfo.Rare;
		}
		lightStar.width = unitInfo.Rare*lightWidth;
		Debug.Log ("position:  " +len * 15  );
		grayStar.transform.localPosition = new Vector3(len * 15,-82,0);
		//rareLabel.text = unitInfo.Rare.ToString();
		
		//		levelLabel.text = data.Level.ToString();
		
		//		//next level need
		//		if ((data.Level > unitInfo.MaxLevel ) 
		//		    || (data.Level == unitInfo.MaxLevel && data.NextExp <= 0) ) {
		//			levelLabel.text = unitInfo.MaxLevel.ToString();
		//			needExpLabel.text = "Max";
		//		} else {
		//			needExpLabel.text = data.NextExp.ToString();
		//		}
	}

	private void CollectCurUnit(GameObject go){
		bool isFav = (curUserUnit.IsFavorite == 1) ? true : false;
		EFavoriteAction favAction = isFav ? EFavoriteAction.DEL_FAVORITE : EFavoriteAction.ADD_FAVORITE;
		UnitFavorite.SendRequest(OnRspChangeFavState, curUserUnit.ID, favAction);
	}

	private void OnRspChangeFavState(object data){
		//Debug.Log("OnRspChangeFavState(), start...");
		if(data == null) {Debug.LogError("OnRspChangeFavState(), data is NULL"); return;}
		bbproto.RspUnitFavorite rsp = data as bbproto.RspUnitFavorite;
		if (rsp.header.code != (int)ErrorCode.SUCCESS){
			LogHelper.LogError("OnRspChangeFavState code:{0}, error:{1}", rsp.header.code, rsp.header.error);
			ErrorMsgCenter.Instance.OpenNetWorkErrorMsgWindow(rsp.header.code);
			
			return;
		}
		curUserUnit.IsFavorite = (curUserUnit.IsFavorite==1) ? 0 : 1;
		//		Debug.LogError ("curUserUnit : " + curUserUnit.TUserUnitID);
		UpdateFavView(curUserUnit.IsFavorite);
	}

	private void UpdateFavView(int isFav){
		//ResourceManager.Instance.LoadLocalAsset("","");
		UISprite background = favBtn.transform.FindChild("Background").GetComponent<UISprite>();
		//Debug.Log("Name is : " + curUserUnit.UnitInfo.Name + "  UpdateFavView(), isFav : " + (isFav == 1));
		if(isFav == 1){
			background.spriteName = "Fav_Lock_Close";
			background.spriteName = "Fav_Lock_Close";
			background.spriteName = "Fav_Lock_Close";
			//Debug.Log("UpdateFavView(), isFav == 1, background.spriteName is Fav_Lock_Close");
		}
		else{
			background.spriteName = "Fav_Lock_Open";
			background.spriteName = "Fav_Lock_Open";
			background.spriteName = "Fav_Lock_Open";
			//Debug.Log("UpdateFavView(), isFav != 1, background.spriteName is Fav_Lock_Open");
		}
	}

	public void ShowPanel(){
		gameObject.SetActive (true);
	}

	private void ShowFavState(object msg){
		UpdateFavView(curUserUnit.IsFavorite);
	}

	private void DestoryFavBtn(){

	}

}
