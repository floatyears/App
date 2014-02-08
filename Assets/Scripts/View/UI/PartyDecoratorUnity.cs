using UnityEngine;
using System.Collections.Generic;

public class PartyDecoratorUnity : UIComponentUnity, IUIParty {

	private GameObject leftArrowBtn;
	private GameObject rightArrowBtn;
	private int currentPartyIndex;
	private int partyTotalCount;
	private UILabel currentPartyIndexLabel;
	private UILabel partyTotalCountLabel;
	private UILabel partyIndexPrefixLabel;
	private UILabel partyIndexSuffixLabel;
	UILabel hpLabel;
	UILabel costLabel;

	private int pageIndexOrigin = 1;

	private Dictionary< int, UITexture > unitTexureDic = new Dictionary< int, UITexture>();
	private Dictionary< int, string > partyIndexDic = new Dictionary< int, string >();
	private Dictionary< int, UnitBaseInfo > unitBaseInfo = new Dictionary< int, UnitBaseInfo >();

	public override void Init ( UIInsConfig config, IUIOrigin origin ) {
		base.Init (config, origin);

		InitInfoPanel();
		InitPartyPage();

	}
	
	public override void ShowUI () {
		base.ShowUI ();

		ShowTween();
	}
	
	public override void HideUI () {
		base.HideUI ();
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

	//Init Party Page
	private void InitPartyPage() {
		InitIndexTextDic();
		InitPagingBtn();
		InitRightIndexText();
		InitLeftIndexText();
		InitUnitTexture();
	}
	
	private void InitIndexTextDic() {
		partyIndexDic.Add( 1, "st");
		partyIndexDic.Add( 2, "nd");
		partyIndexDic.Add( 3, "rd");
		partyIndexDic.Add( 4, "th");
		partyIndexDic.Add( 5, "th");
	}
	
	private void InitPagingBtn() {
		leftArrowBtn = FindChild("PartyPages/BtnLeft");
		rightArrowBtn = FindChild("PartyPages/BtnRight");
		UIEventListener.Get( leftArrowBtn ).onClick = BackPage;
		UIEventListener.Get( rightArrowBtn ).onClick = ForwardPage;
	}
	
	private void InitRightIndexText() {
		currentPartyIndex = 1;
		partyTotalCount = UIConfig.partyTotalCount;
		currentPartyIndexLabel = FindChild< UILabel >("PartyPages/Label_Cur_Party");
		partyTotalCountLabel = FindChild< UILabel >("PartyPages/Label_Party_Count");
		currentPartyIndexLabel.text = currentPartyIndex.ToString();
		partyTotalCountLabel.text = partyTotalCount.ToString();
	}
	
	private void InitLeftIndexText() {
		partyIndexPrefixLabel = FindChild< UILabel >("PartyPages/Label_Party_Index_Prefix");
		partyIndexSuffixLabel = FindChild< UILabel >("PartyPages/Label_Party_Index_Suffix");
		partyIndexPrefixLabel.text = currentPartyIndex.ToString();
		partyIndexSuffixLabel.text = partyIndexDic[ currentPartyIndex ].ToString();
	}
	
	private void InitUnitTexture() {
		UITexture temp;
		for( int i = 1; i < 5; i++) {
			temp = FindChild< UITexture >("PartyPages/Unit" + i.ToString() + "/role" );
			UIEventListenerCustom.Get(temp.transform.parent.gameObject).LongPress = ViewUnitDetailInfo;
			temp.enabled = false;
			unitTexureDic.Add(i, temp);
		}
		RequestPartyInfo( currentPartyIndex );
	}

	private void InitInfoPanel() {
		hpLabel = FindChild< UILabel >("PartyInfoPanel/content_status/Label_HP_Value");
		costLabel = FindChild< UILabel >( "PartyInfoPanel/content_status/Label_Cost_Value");
	}
	
	//Deal with Party Page Events	
	private void BackPage( GameObject btn ) {
		//Debug.Log("Back Page");
		currentPartyIndex = Mathf.Abs( (currentPartyIndex - 1) % partyTotalCount );
		if( currentPartyIndex == 0 )
			currentPartyIndex = partyTotalCount ;
		currentPartyIndexLabel.text = currentPartyIndex.ToString();
		partyIndexPrefixLabel.text = currentPartyIndex.ToString();
		partyIndexSuffixLabel.text = partyIndexDic[ currentPartyIndex ].ToString();
		RequestPartyInfo( currentPartyIndex );
	}
	
	private void ForwardPage( GameObject btn ) {
		//Debug.Log("Forward Page");
		currentPartyIndex++;
		if (currentPartyIndex > partyTotalCount) {
			currentPartyIndex = pageIndexOrigin;
		} 
		currentPartyIndexLabel.text = currentPartyIndex.ToString();
		partyIndexPrefixLabel.text = currentPartyIndex.ToString();
		partyIndexSuffixLabel.text = partyIndexDic[ currentPartyIndex ].ToString();
		RequestPartyInfo( currentPartyIndex );
	}
	
	//Request Party Page Info from Logic Component
	private void RequestPartyInfo( int pageIndex ) {
		//origin is UnitsComponent -- Logic Interface
		IUIParty partyInterface = origin as IUIParty;
		if( partyInterface == null )
			ShowPartyInfo( null );
		partyInterface.PartyPaging( pageIndex );
	}

	public void PartyPaging(object data)
	{
		if (data == null)
			ShowPartyInfo(null);
		else{
			Dictionary< string, object > viewInfo = data as Dictionary<string, object >;
			if ( viewInfo == null )
				return;
			ShowPartyInfo( viewInfo);
		}
	}

	private void ShowPartyInfo( Dictionary< string, object > info ) {
		int totalHPCount = 0;
		if (info == null){
			foreach (var item in unitTexureDic.Values)
				item.enabled = false;
		}else{
			if( info.ContainsKey( "avatar")) {
				unitBaseInfo = info[ "avatar" ] as Dictionary< int, UnitBaseInfo>;
			}else 
				return;
			
			if( info.ContainsKey( "hp")) {
				totalHPCount = (int)info["hp"];
			}else 
				return;
			
			//show textures
			foreach (var item in unitTexureDic){
				if (unitBaseInfo.ContainsKey(item.Key)){
					unitTexureDic [item.Key].enabled = true;
					string path = unitBaseInfo [item.Key].GetHeadPath;
					unitTexureDic [item.Key].mainTexture = Resources.Load(path) as Texture2D;
				}else
					unitTexureDic [item.Key].enabled = false;
			}
			
			//show hp label
			hpLabel.text = totalHPCount.ToString();
		}
	}

	//LongPress
	private void ViewUnitDetailInfo(GameObject target){
		int posID = -1;
		foreach (var item in unitTexureDic){
			if (target == item.Value.gameObject.transform.parent.gameObject)
				posID = item.Key;
		}
		MsgCenter.Instance.Invoke(CommandEnum.EnterUnitInfo, unitBaseInfo [posID]);
	}

	private void ShowTween() {
		TweenPosition[ ] list = 
			gameObject.GetComponentsInChildren< TweenPosition >();
		if( list == null )
			return;
		foreach( var tweenPos in list) {		
			if( tweenPos == null )
				continue;
			tweenPos.Reset();
			tweenPos.PlayForward();
		}
	}

	public void Callback(object data)
	{
	
	}


}
