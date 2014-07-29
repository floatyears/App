//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using bbproto;
//
//public class UnitDetailTopPanel : UIComponentUnity,IUICallback {
//	UISprite type;
//	UILabel cost;
//	UILabel number;
//	UILabel name;
//	UISprite lightStar;
//	UISprite grayStar;
//
//	private int grayWidth = 28;
//	private int lightWidth = 30;
//
//	public bool fobidClick = false;
//	GameObject unitLock;
//
//	public override void Init ( UIInsConfig config, IUICallback origin ) {
//		base.Init (config, origin);
//		InitUI();
//	}
//	
//	public override void ShowUI () {
//		base.ShowUI ();
//		UIManager.Instance.HideBaseScene();
//
//		MsgCenter.Instance.AddListener(CommandEnum.ShowUnitDetail, CallBackUnitData);
//		MsgCenter.Instance.AddListener(CommandEnum.ShowFavState,  ShowFavState);
//		MsgCenter.Instance.AddListener(CommandEnum.LevelUp, CallBackUnitData);
//	}
//	
//	public override void HideUI () {
//		MsgCenter.Instance.RemoveListener(CommandEnum.ShowUnitDetail, CallBackUnitData);
//		MsgCenter.Instance.RemoveListener(CommandEnum.LevelUp, CallBackUnitData);
//		MsgCenter.Instance.RemoveListener(CommandEnum.ShowFavState,  ShowFavState);
//
////		if (!gameObject.activeSelf) {
////			gameObject.SetActive(true);
////		}
//		base.HideUI ();
//
//	}
//	
//	public override void DestoryUI () {
//		base.DestoryUI ();
//	}
//
//	void InitUI() {
//		cost = transform.FindChild("Cost").GetComponent<UILabel>();
//		number = transform.FindChild("No").GetComponent<UILabel>();
//		name = transform.FindChild("Name").GetComponent<UILabel>();
//		type = transform.FindChild ("Type").GetComponent<UISprite> ();
//		grayStar = transform.FindChild ("Star2").GetComponent<UISprite> ();
//		lightStar = transform.FindChild ("Star2/Star1").GetComponent<UISprite> ();
//	}
//	
//
////	RspLevelUp levelUpData;
////	void PlayLevelUp(RspLevelUp rlu) {
////		levelUpData = rlu;
////		newBlendUnit = DataCenter.Instance.UserUnitList.GetMyUnit(levelUpData.blendUniqueId);
//////		ShowInfo (newBlendUnit);
//////		gameObject.SetActive (false);
////	}
//
//
//
//
//
//
//
//
//	public void ShowPanel(){
//		gameObject.SetActive (true);
//	}
//
//
//
//
//	/// <summary>
//	/// Destories the unit lock.
//	/// </summary>
//
//
//	
//
//}
