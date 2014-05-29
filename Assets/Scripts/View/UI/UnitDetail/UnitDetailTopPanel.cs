using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
		base.HideUI ();
		if (IsInvoking ("CreatEffect")) {
			CancelInvoke("CreatEffect");
		}
		//ClearEffectCache();
		UIManager.Instance.ShowBaseScene();
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}
	
	
	//----------Init functions of UI Elements----------
	void InitUI() {

		cost = transform.FindChild("Cost").GetComponent<UILabel>();
		number = transform.FindChild("No").GetComponent<UILabel>();
		name = transform.FindChild("Name").GetComponent<UILabel>();
		type = transform.FindChild ("Type").GetComponent<UISprite> ();
		grayStar = transform.FindChild ("Star2").GetComponent<UISprite> ();
		lightStar = transform.FindChild ("Star2/Star1").GetComponent<UISprite> ();
	}

	private void CallBackUnitData(object d){
		TUserUnit data = d as TUserUnit; 
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
		//Debug.Log ("rare : " + );	
		int len = 0;
		if (unitInfo.MaxRare > unitInfo.Rare) {
			grayStar.width = (unitInfo.MaxRare - unitInfo.Rare) * grayWidth;
			len = 2*unitInfo.Rare - unitInfo.MaxRare;
		} else {
			len = unitInfo.Rare;
		}
		lightStar.width = unitInfo.Rare*lightWidth;
		Debug.Log ("position:  " +len / 2 * 30 );
		grayStar.transform.localPosition = new Vector3(len / 2 * 30,-82,0);
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
}
