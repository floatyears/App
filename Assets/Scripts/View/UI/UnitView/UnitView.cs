using UnityEngine;
using System.Collections;

public class UnitView : MonoBehaviour {
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
			if(userUnit != null &&  userUnit.Equals(value)) {
				ExecuteCrossFade();
			}
			else{
				userUnit = value;
				RefreshState();
			}
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


	protected bool canCrossed = true;
	protected bool isCrossed;
	protected UITexture avatarTex;
	protected UISprite maskSpr;
	protected UILabel crossFadeLabel;
	
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
			//Debug.LogError("UnitView.InitState(), userUnit is NULL, return...");
			SetEmptyState();
			return;
		}
		SetCommonState();
	}

	protected virtual void RefreshState(){
		if(userUnit == null){
			SetEmptyState();
			return;
		}
		IsEnable = true;
		avatarTex.mainTexture = userUnit.UnitInfo.GetAsset(UnitAssetType.Avatar);
		ExecuteCrossFade();
	}
	private void ExecuteCrossFade(){
//		CheckCross();
//	
//		crossFadeLabel.text = crossFadeBeforeText = "Lv." + userUnit.Level;
//		crossFadeLabel.color = Color.yellow;
		CrossFadeLevelFirst();
		if(canCrossed){
			if (IsInvoking("UpdateCrossFadeState"))
				CancelInvoke("UpdateCrossFadeState");
			InvokeRepeating("UpdateCrossFadeState", 0f, 1f);
		}
	}

	void CheckCross() {
		if(userUnit.AddNumber == 0){ 
			canCrossed = false;
		}
		else{
			canCrossed = true;;
		}
	}

	protected virtual void UpdatEnableState(){
		maskSpr.enabled = !isEnable;
		UIEventListenerCustom.Get(this.gameObject).LongPress = PressItem;
		if(isEnable)
			UIEventListenerCustom.Get(this.gameObject).onClick = ClickItem;
		else
			UIEventListenerCustom.Get(this.gameObject).onClick = null;
	}

	private void UpdateCrossFadeState(){
//		Debug.LogError("UpdateCrossFadeState : " + isCrossed + " gameobject : " + gameObject);
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

	protected virtual void PressItem(GameObject item){
		UIManager.Instance.ChangeScene(SceneEnum.UnitDetail);
		MsgCenter.Instance.Invoke(CommandEnum.ShowUnitDetail, userUnit);
	}
	
	private string crossFadeBeforeText;
	private string crossFadeAfterText;
	
	private void UpdateCrossFadeText(){
		switch (currentSortRule){
			case SortRule.ByID : 
				CrossFadeLevelFirst();
				break;
			case SortRule.ByAttack : 
				CrossFadeAttackFirst();
				break;
			case SortRule.ByAttribute : 
				CrossFadeLevelFirst();
				break;
			case SortRule.ByHP : 
				CrossFadeHpFirst();
				break;
			case SortRule.ByRace :
				CrossFadeLevelFirst();
				break;
			case SortRule.ByGetTime : 
				CrossFadeLevelFirst();
				break;
			case SortRule.ByAddPoint :
				CrossFadeLevelFirst();
				break;
			case SortRule.ByIsCollected : 
				CrossFadeLevelFirst();
				break;
			default:
				break;
		}
	}
	
	private void CrossFadeLevelFirst(){
		crossFadeBeforeText = "Lv" + userUnit.Level;
		if(userUnit.AddNumber == 0){ 
			canCrossed = false;
			crossFadeLabel.text = crossFadeBeforeText;
			crossFadeLabel.color = Color.yellow;
			if(IsInvoking("UpdateCrossFadeState"))
				CancelInvoke("UpdateCrossFadeState");
		}
		else {
			canCrossed = true;
			//Debug.LogError(" userUnit.AddNumber : " +  userUnit.AddNumber + " userunit : " + userUnit.UnitID);
			crossFadeAfterText = "+" + userUnit.AddNumber;
		}
			
	}

	private void CrossFadeHpFirst(){
		crossFadeBeforeText = userUnit.Hp.ToString();
		crossFadeAfterText = "+" + userUnit.AddNumber;
	}

	private void CrossFadeAttackFirst(){
		crossFadeBeforeText = userUnit.Attack.ToString();
		crossFadeAfterText = "+" + userUnit.AddNumber;
	}

	private void SetEmptyState(){
		IsEnable = false;
		avatarTex.mainTexture = null;
		CancelInvoke("UpdateCrossFadeState");
		crossFadeLabel.text = string.Empty;
	}

	private void SetCommonState(){
		IsEnable = true;
		avatarTex.mainTexture = userUnit.UnitInfo.GetAsset(UnitAssetType.Avatar);
		CurrentSortRule = SortRule.ByGetTime;
		ExecuteCrossFade();
	}

}
