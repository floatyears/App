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

	public bool fobidClick = false;
	GameObject unitLock;

	public override void Init ( UIInsConfig config, IUICallback origin ) {
		base.Init (config, origin);
		InitUI();
	}
	
	public override void ShowUI () {
		base.ShowUI ();
		UIManager.Instance.HideBaseScene();

		MsgCenter.Instance.AddListener(CommandEnum.ShowUnitDetail, CallBackUnitData);
		MsgCenter.Instance.AddListener(CommandEnum.ShowFavState,  ShowFavState);
		MsgCenter.Instance.AddListener(CommandEnum.LevelUp, CallBackUnitData);
	}
	
	public override void HideUI () {
		MsgCenter.Instance.RemoveListener(CommandEnum.ShowUnitDetail, CallBackUnitData);
		MsgCenter.Instance.RemoveListener(CommandEnum.LevelUp, CallBackUnitData);
		MsgCenter.Instance.RemoveListener(CommandEnum.ShowFavState,  ShowFavState);
		base.HideUI ();
		DestoryUnitLock();
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

	void InitUI() {
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
		newBlendUnit = DataCenter.Instance.UserUnitList.GetMyUnit(levelUpData.blendUniqueId);
		ShowInfo (newBlendUnit);
		gameObject.SetActive (false);
	}

	private void ShowInfo(TUserUnit data){
		curUserUnit = data;
		TUnitInfo unitInfo = data.UnitInfo;
		number.text = data.UnitID.ToString();
	
		name.text = unitInfo.Name;
	
		type.spriteName = "type_" + unitInfo.UnitType;

		cost.text = unitInfo.Cost.ToString();

		//Debug.Log ("rare : " + unitInfo.Rare + "max rare: " + unitInfo.MaxRare);	
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
		//Debug.Log ("position:  " +len * 15  );
		grayStar.transform.localPosition = new Vector3(len * 15,-82,0);
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
		curUserUnit.IsFavorite = (curUserUnit.IsFavorite == 1) ? 0 : 1;
		UpdateFavView(curUserUnit.IsFavorite);
	}

	private void UpdateFavView(int isFav){
		Debug.LogError("unitLock.name : " + unitLock.name);
		UISprite background = unitLock.transform.FindChild("Background").GetComponent<UISprite>();
		Debug.Log("Name is : " + curUserUnit.UnitInfo.Name + "  UpdateFavView(), isFav : " + (isFav == 1));
		if(isFav == 1){
			background.spriteName = "lock_closed";
			Debug.Log("UpdateFavView(), isFav == 1, background.spriteName is Fav_Lock_Close");
		}
		else{
			background.spriteName = "lock_open";
			Debug.Log("UpdateFavView(), isFav != 1, background.spriteName is Fav_Lock_Open");
		}
	}

	public void ShowPanel(){
		gameObject.SetActive (true);
	}

	private void ShowFavState(object msg){
		AddUnitLock();
		UpdateFavView(curUserUnit.IsFavorite);
	}


	/// <summary>
	/// Destories the unit lock.
	/// </summary>
	private void DestoryUnitLock(){
		if(unitLock == null){
			Debug.LogError("unitLock == null, do noting...");
			return;
		}
		Debug.LogError("unitLock != null, destory it...");
		GameObject.Destroy(unitLock);
	}

	private void AddUnitLock(){
		if(unitLock != null){
			Debug.LogError("unitLock != null, add one...");
			return;
		}
		string path = "Prefabs/UI/UnitDetail/FavLock";
		ResourceManager.Instance.LoadLocalAsset(path, CreateUnitLock);
	}

	private void CreateUnitLock(Object obj){
		GameObject prefab = obj as GameObject;
		unitLock = GameObject.Instantiate(prefab) as GameObject;
		unitLock.transform.parent = transform;
		unitLock.transform.localScale = Vector3.one;
		unitLock.transform.localPosition = new Vector3(300, -300, 0);
		UIEventListener.Get(unitLock).onClick = ClickLock;
	}
	
	private void ClickLock(GameObject go){
		bool isFav = (curUserUnit.IsFavorite == 1) ? true : false;
		EFavoriteAction favAction = isFav ? EFavoriteAction.DEL_FAVORITE : EFavoriteAction.ADD_FAVORITE;
		UnitFavorite.SendRequest(OnRspChangeFavState, curUserUnit.ID, favAction);
	}
}
