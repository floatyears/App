using UnityEngine;
using System.Collections;
using bbproto;

public class MyUnitItem : BaseUnitItem {
	protected DataListener callback;

	protected UISprite lightSpr;
	protected UILabel partyLabel;
	public UILabel PartyLabel{
		get { 
			return partyLabel; 
		}
	}
	protected UISprite lockSpr;

	protected bool isParty;
	public bool IsParty{
		get{
			return isParty;
		}
		set{
			isParty = value;
			UpdatePartyState();
		}
	}

	private bool isFavorite;
	public bool IsFavorite {
		get{
			return isFavorite;
		}
		set{
			isFavorite = value;
			UpdateFavoriteState ();
		}
	}

	private bool isFocus;
	public bool IsFocus{
		get{
			return isFocus;
		}
		set{
			isFocus = value;
			UpdateFocus();
		}
	}

	protected override void InitUI(){
		base.InitUI();
		lightSpr = transform.FindChild("Sprite_Light").GetComponent<UISprite>();
		lockSpr = transform.FindChild("Sprite_Lock").GetComponent<UISprite>();
		partyLabel = transform.FindChild("Label_Party").GetComponent<UILabel>();
		partyLabel.enabled = false;
		partyLabel.text = TextCenter.GetText("Text_Party");
		partyLabel.color = new Color (0.9333f, 0.192f, 0.192f);
//		lockSpr.enabled = false;

		IsParty = false;
	}

	protected virtual void UpdatePartyState(){

	}
	protected virtual void UpdateFocus(){

	}

	protected virtual void UpdateFavoriteState(){
		lockSpr.spriteName = isFavorite ? "Lock_close" : "Lock_open";
	}

	protected virtual void BehindChangeUserUnit(UserUnit tuu) {

	}

	public override void SetData<T> (T data, params object[] args)
	{
		base.SetData (data, args);

		IsFavorite = (userUnit != null && userUnit.isFavorite == 1) ? true : false;
		if (args.Length > 0) {
			callback = args[0] as DataListener;	
		}
		if (userUnit != null) {
			int i = DataCenter.Instance.UnitData.PartyInfo.UnitIsInParty (userUnit);
			if(i > 0){
				IsParty = true;
				partyLabel.text = TextCenter.GetText("Text_Party") + " " + i;
				partyLabel.enabled = true;
			}else{
				IsParty = false;
				partyLabel.enabled = false;
			}
			tag = "Untagged";
			if (userUnit.unitId == 1 || userUnit.unitId == 5 || userUnit.unitId == 9) {
				tag = "unit_leader";	
			}		
		}

	}


	
}
