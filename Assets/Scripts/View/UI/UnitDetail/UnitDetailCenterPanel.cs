using UnityEngine;
using System.Collections;
using bbproto;

public class UnitDetailCenterPanel : UIComponentUnity,IUICallback  {

	UITexture unitBodyTex;
	
	public override void Init ( UIInsConfig config, IUICallback origin ) {
		base.Init (config, origin);
		//GetUnitMaterial();
		//InitEffect();
		InitUI();
	}
	
	public override void ShowUI () {
		MsgCenter.Instance.AddListener(CommandEnum.ShowUnitDetail, CallBackUnitData);
		base.ShowUI ();
//		UIManager.Instance.HideBaseScene();

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

		unitBodyTex = FindChild< UITexture >("detailSprite");
//		UIEventListener listener = UIEventListener.Get( unitBodyTex.gameObject).onClick = ClickTex;
//		ShowBodyTexture( userUnit );

//		cost = transform.FindChild("Cost").GetComponent<UILabel>();
//		number = transform.FindChild("No").GetComponent<UILabel>();
//		name = transform.FindChild("Name").GetComponent<UILabel>();
//		type = transform.FindChild ("Type").GetComponent<UISprite> ();
//		grayStar = transform.FindChild ("Star2").GetComponent<UISprite> ();
//		lightStar = transform.FindChild ("Star2/Star1").GetComponent<UISprite> ();
	}

	void ClickTex(GameObject go) {

	}

	void ShowInfo(TUserUnit userUnit) {
//		ShowFavView(curUserUnit.IsFavorite);
		ShowBodyTexture( userUnit ); 
		ShowUnitScale();
//		ShowStatusContent( userUnit );
//		ShowSkill1Content( userUnit );
//		ShowSkill2Content( userUnit );
//		ShowLeaderSkillContent( userUnit );
//		ShowActiveSkillContent( userUnit );
//		ShowProfileContent( userUnit );
		
	}

//	private TUserUnit curUserUnit;
	public void CallBackUnitData(object data)	{
		TUserUnit userUnit = data as TUserUnit;
		
//		curUserUnit = userUnit;
		
		if (userUnit != null) {
			ShowInfo (userUnit);
		
		} else {
			RspLevelUp rlu = data as RspLevelUp;
			if(rlu ==null) {
				return;
			}
			PlayLevelUp(rlu);
		}
	}
	
	TUserUnit oldBlendUnit = null;
	TUserUnit newBlendUnit = null;

	RspLevelUp levelUpData;
	void PlayLevelUp(RspLevelUp rlu) {
		levelUpData = rlu;
		oldBlendUnit = DataCenter.Instance.oldUserUnitInfo;
		newBlendUnit = DataCenter.Instance.UserUnitList.GetMyUnit(levelUpData.blendUniqueId);
//		Debug.LogError ("unitBodyTex : " + unitBodyTex + " newBlendUnit : " + newBlendUnit + " newBlendUnit.UnitInfo : " + newBlendUnit.UnitInfo);
		newBlendUnit.UnitInfo.GetAsset (UnitAssetType.Profile, o =>{
			AudioManager.Instance.PlayAudio(AudioEnum.sound_check_role);

			DGTools.ShowTexture (unitBodyTex, o as Texture2D);
			ShowUnitScale();
		});

//		unitInfoTabs.SetActive (false);
//		SetEffectCamera ();
//		StartCoroutine (CreatEffect ());
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
