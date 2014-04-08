using UnityEngine;
using System.Collections;

public class UnitView : MonoBehaviour {
	public enum SortRule{
		ByLevel			= 0,
		ByAttack		= 1,
		ByHP				= 2,
		ByAttribute		= 3,
		ByRace			= 4
	}
	
	public static UnitView Inject(GameObject item){
		UnitView view = item.AddComponent<UnitView>();
		if (view == null) view = item.AddComponent<UnitView>();
		return view;
	}

	protected TUserUnit userUnit;
	public TUserUnit UserUnit{
		get{
			return userUnit;
		}
		set{
			if(userUnit != null && userUnit.Equals(value)) return;
			userUnit = value;
			RefreshState();
		}
	}

	private int index;
	public int Index{
		get{
			return index;
		}
		set{
			index = value;
		}
	}

	private bool isEnable;
	public bool IsEnable{
		get{
			return isEnable;
		}
		set{
//			if(isEnable == value) return;
			isEnable = value;
			UpdatEnableState();
		}
	}


	private SortRule currentSortRule;
	public SortRule CurrentSortRule{
		get{
			return currentSortRule;
		}
		set{
			currentSortRule = value;
			UpdateCrossFadeText();
		}
	}


	private bool canCrossed = true;
	private bool isCrossed;
	private UITexture avatarTex;
	private UISprite maskSpr;
    
	private UILabel crossFadeLabel;


	public void Init(TUserUnit userUnit){
		this.userUnit = userUnit;
		InitUI();
		InitState();
	}
	
	protected virtual void InitUI(){
		avatarTex = transform.FindChild("Texture_Avatar").GetComponent<UITexture>();
		crossFadeLabel = transform.FindChild("Label_Cross_Fade").GetComponent<UILabel>();
		maskSpr = transform.FindChild("Sprite_Mask").GetComponent<UISprite>();
	}

	protected virtual void InitState(){
		if(userUnit == null){
			Debug.LogError("UnitView.Init(), userUnit is Null, return!");
			IsEnable = false;
			avatarTex.mainTexture = null;
			CancelInvoke("UpdateCrossFadeState");
			return;
		}

		IsEnable = true;
		avatarTex.mainTexture = userUnit.UnitInfo.GetAsset(UnitAssetType.Avatar);
		CurrentSortRule = SortRule.ByLevel;
		if(canCrossed){
			if (IsInvoking("UpdateCrossFadeState"))
				CancelInvoke("UpdateCrossFadeState");
			InvokeRepeating("UpdateCrossFadeState", 0f, 1f);
		}
	}

	private void RefreshState(){
		if(userUnit == null){
			Debug.LogError("UnitView.Init(), userUnit is Null, return!");
			IsEnable = false;
			avatarTex.mainTexture = null;
			CancelInvoke("UpdateCrossFadeState");
			return;
		}
		
		IsEnable = true;
		avatarTex.mainTexture = userUnit.UnitInfo.GetAsset(UnitAssetType.Avatar);
		CurrentSortRule = SortRule.ByLevel;
		if(canCrossed){
			if (IsInvoking("UpdateCrossFadeState"))
				CancelInvoke("UpdateCrossFadeState");
			InvokeRepeating("UpdateCrossFadeState", 0f, 1f);
		}
	}

	private void UpdatEnableState(){
		maskSpr.enabled = !isEnable;
		UIEventListenerCustom.Get(this.gameObject).LongPress = PressItem;
		if(isEnable)
			UIEventListenerCustom.Get(this.gameObject).onClick = ClickItem;
		else
			UIEventListenerCustom.Get(this.gameObject).onClick = null;
	}
	


	private void UpdateCrossFadeState(){
		if(isCrossed){
			crossFadeLabel.text = crossFadeBeforeText;
			crossFadeLabel.color = Color.yellow;
			isCrossed = false;
		}
		else{
			crossFadeLabel.text = crossFadeAfterText;
			crossFadeLabel.color = Color.red;
			isCrossed = true;
		}
	}

	protected virtual void ClickItem(GameObject item){}

	private void PressItem(GameObject item){
		UIManager.Instance.ChangeScene(SceneEnum.UnitDetail);
		MsgCenter.Instance.Invoke(CommandEnum.ShowUnitDetail, userUnit);
	}
	
	private string crossFadeBeforeText;
	private string crossFadeAfterText;
	
	private void UpdateCrossFadeText(){
		switch (currentSortRule){
			case SortRule.ByLevel : 
				CrossFadeByLevel();
				break;
			case SortRule.ByAttack : 
				CrossFadeByAttack();
				break;
			case SortRule.ByAttribute : 
				CrossFadeByAttribute();
				break;
			case SortRule.ByHP : 
				CrossFadeByHp();
				break;
			case SortRule.ByRace :
				CrossFadeByRace();
				break;
			default:
				break;
		}
	}
	
	private void CrossFadeByLevel(){
		crossFadeBeforeText = "Lv" + userUnit.Level.ToString();
		if(userUnit.AddNumber == 0){ 
			canCrossed = false;
			crossFadeLabel.text = crossFadeBeforeText;
			crossFadeLabel.color = Color.yellow;
		}
		else 
			crossFadeAfterText = "+" + userUnit.AddNumber.ToString();
	}

	private void CrossFadeByHp(){}
	private void CrossFadeByRace(){}
	private void CrossFadeByAttack(){}
	private void CrossFadeByAttribute(){}



}
