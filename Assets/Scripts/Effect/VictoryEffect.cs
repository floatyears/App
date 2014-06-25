using UnityEngine;
using System.Collections;

public class VictoryEffect : UIBaseUnity {
	private UISprite levelProgress;
	private UILabel coinLabel;
	private UILabel empiricalLabel;

	private UISprite leftWing;
	private UISprite rightWing;
	private UISprite niuJiao;
	private UISprite frontCircle;
	private UISprite backCircle;

	private UIButton sureButton;
	private GameObject parent;
	private GameObject dropItem;

	private int coinNumber = 0;
	private int empireNumber = 0;

	private bool canPlayAnimation		= false;
	private Vector3 niuJiaoMoveTarget	= Vector3.zero;
	private Vector3 niuJiaoCurrent		= Vector3.zero;
	private Vector3 leftWingAngle1 		= new Vector3 (0f, 0f, -30f);
	private Vector3 leftWingAngle2 		= new Vector3 (0f, 0f, 3f);
	private Vector3 leftWingAngle3 		= new Vector3 (0f, 0f, -15f);

	private Vector3 rightWingAngle1 	= new Vector3 (0f, 0f, 30f);
	private Vector3 rightWingAngle2 	= new Vector3 (0f, 0f, -3f);
	private Vector3 rightWingAngle3 	= new Vector3 (0f, 0f, 15f);
	private Callback sureButtonCallback;

	public BattleQuest battleQuest;

	public override void Init (string name) {
		base.Init (name);
		FindComponent ();
	}

	public override void ShowUI () {
		base.ShowUI ();
		gameObject.SetActive (true);
		Debug.LogError ("CommandEnum.StopInput");
		MsgCenter.Instance.Invoke (CommandEnum.StopInput, null);
	}

	public override void HideUI () {
		base.HideUI ();
		gameObject.SetActive (false);
	}

	public override void DestoryUI () {
		base.DestoryUI ();
		Destroy (gameObject);
	}
	
	float currentExp = 0;
	float gotExp = 0;
	float add = 0;
	int currentTotalExp = 0;
	int rank = 0;
	
	public void ShowData(TRspClearQuest clearQuest){
		if (clearQuest == null) {
			return;	
		}

		int nextEmp = DataCenter.Instance.UserInfo.NextExp;
		int maxEmp = clearQuest.exp;
		gotExp= clearQuest.gotExp;
		rank = DataCenter.Instance.oldAccountInfo.Rank;
		int totleExp = DataCenter.Instance.oldAccountInfo.CurPrevExp; 
		currentExp = clearQuest.exp - totleExp ;
		currentTotalExp = DataCenter.Instance.GetUnitValue (TPowerTableInfo.UserExpType, rank);
		add = (float)gotExp * 0.05f;
		int curCoin = DataCenter.Instance.AccountInfo.Money;
		int maxCoin = clearQuest.money;
		int gotCoin = clearQuest.gotMoney;
		float addCoin = gotCoin * 0.05f;

//		Debug.LogError ("======= ShowData =======================");
//		Debug.LogError ("clearQuest.exp  : " + clearQuest.exp );
//		Debug.LogError ("gotexp : " + gotExp);
//		Debug.LogError ("currentExp : " + currentExp);
//		Debug.LogError ("rank : " + rank);
//		Debug.LogError ("currentTotalExp : " + currentTotalExp);
//		Debug.LogError ("add : " + add);
//		Debug.LogError ("curcoin : " + curCoin);
//		Debug.LogError ("maxCoin : " + maxCoin);
//		Debug.LogError ("gotCoin : " + gotCoin);
//		Debug.LogError ("addCoin : " + addCoin);
//		Debug.LogError ("=======================================");

		StartCoroutine (UpdateLevelNumber ());
		StartCoroutine (UpdateCoinNumber (addCoin, curCoin, gotCoin));

		for (int i = 0; i < clearQuest.gotUnit.Count; i++) {
			GameObject go = NGUITools.AddChild(parent, dropItem);
			go.SetActive(true);
			uint unitID = clearQuest.gotUnit[i].UnitInfo.ID;
			go.name = i.ToString();
			UISprite sprite = go.transform.Find("Avatar").GetComponent<UISprite>();
			DataCenter.Instance.GetAvatarAtlas(unitID, sprite);

			DataCenter.Instance.CatalogInfo.AddHaveUnit(clearQuest.gotUnit[i].UnitInfo.ID);
		}

		parent.GetComponent<UIGrid> ().repositionNow = true;
	}

	IEnumerator UpdateLevelNumber () {
		yield return new WaitForSeconds (1f);
		while (gotExp > 0) {	
			if(gotExp - add<= 0) {
				add = gotExp;
			}
			gotExp -= add;
			currentExp += add;
			int showValue = (int)currentExp;
			empiricalLabel.text = showValue.ToString();
			float progress = currentExp / currentTotalExp;
			levelProgress.fillAmount = progress;
			if(currentExp >= currentTotalExp) {
				currentExp -= currentTotalExp;
				rank++;
				battleQuest.battle.ShieldInput(false);
				
				battleQuest.questFullScreenTips.ShowTexture(QuestFullScreenTips.RankUp, RankUp);
				currentTotalExp = DataCenter.Instance.GetUnitValue (TPowerTableInfo.UserExpType, rank);
			}
			yield return new WaitForSeconds(0.1f);
		}
	}

				                                  
	void RankUp() {
		battleQuest.battle.ShieldInput(true);
//		MsgCenter.Instance.Invoke (CommandEnum.MeetEnemy, false);
	}
	
	IEnumerator UpdateCoinNumber (float addCoin, float curCoin, float gotCoin) {
		yield return new WaitForSeconds (1f);
		while (gotCoin > 0) {
			if(gotCoin - addCoin <= 0) {
				addCoin = gotCoin;
			}	
			gotCoin-= addCoin;
			curCoin += addCoin;

			coinLabel.text = ((int)curCoin).ToString ();
			yield return new WaitForSeconds(0.1f);
		}

		coinLabel.text = ((int)curCoin).ToString ();
	}

	void FindComponent () {
		MsgCenter.Instance.Invoke (CommandEnum.StopInput, null);
		levelProgress = FindChild<UISprite> ("LvProgress");
		coinLabel = FindChild<UILabel>("CoinValue");
		empiricalLabel = FindChild<UILabel>("EmpiricalValue");
		leftWing = FindChild<UISprite>("LeftWing");
		rightWing = FindChild<UISprite>("RightWing");
		niuJiao = FindChild<UISprite>("NiuJiao");
		frontCircle = FindChild<UISprite>("FrontCircle");
		backCircle = FindChild<UISprite>("BackCircle");
		sureButton = FindChild<UIButton>("Button");
		UIEventListener.Get (sureButton.gameObject).onClick = Sure;
		niuJiaoCurrent = niuJiao.transform.localPosition;
		niuJiaoMoveTarget = new Vector3 (niuJiaoCurrent.x, niuJiaoCurrent.y - 20f, niuJiaoCurrent.z);
		parent = transform.Find ("VertialDrapPanel/SubPanel/Table").gameObject;
		dropItem = parent.transform.Find ("MyUnitPrefab").gameObject;
		dropItem.transform.parent = parent.transform.parent;
		dropItem.SetActive (false);
	}

	void Sure(GameObject go) {
		if (sureButtonCallback != null) {
			sureButtonCallback();
		}
		DestoryUI ();
	}

	public void PlayAnimation (Callback callback) {
		sureButtonCallback = callback;

		if (currentState == UIState.UIHide) {
			ShowUI();	
		}
		PrevAnimation ();
	}

	IEnumerator UpdateCoinNumber (int coinNum, int addNum) {
		coinNum ++;
		addNum --;
		coinLabel.text = coinNum.ToString();
		yield return 0;
		if (addNum != 0) { 
			StartCoroutine(UpdateCoinNumber(coinNum, addNum));
		}
	}

	IEnumerator UpdateLevelNumber ( int empire, int maxEmpire) {
		empire += 2;
		if (empire > maxEmpire) {
			empire = maxEmpire;
		}
		float progree = (float)empire / (float)maxEmpire;
		empiricalLabel.text = empire.ToString();
		levelProgress.fillAmount = progree;
		yield return 0;
		if (empire < maxEmpire) {
			StartCoroutine(UpdateLevelNumber(empire,maxEmpire));
		}
	}

	void PrevAnimation () {
		iTween.ScaleFrom (gameObject, iTween.Hash ("scale", Vector3.zero, "time", 0.8f, "easetype", iTween.EaseType.easeOutElastic, "oncomplete", "StartRotateWing", "oncompletetarget", gameObject));
	}

	void ScaleEnd() { }
	
	void StartRotateWing () {
		canPlayAnimation = true;
		iTween.RotateTo(leftWing.gameObject,iTween.Hash("rotation",leftWingAngle1,"time", 1f,"easetype",iTween.EaseType.easeInOutQuart,"delay",0.3f));
		iTween.RotateTo(rightWing.gameObject,iTween.Hash("rotation",rightWingAngle1,"time", 1f,"easetype",iTween.EaseType.easeInOutQuart,"oncomplete","PlayNext","oncompletetarget",gameObject,"delay",0.3f));
	}

	void PlayNext () {
		RotateWingBack ();
		StartMoveNiuJiao ();
	}

	void RotateWingBack () {
		iTween.RotateTo(leftWing.gameObject,iTween.Hash("rotation",leftWingAngle2,"time",3.5f,"easetype",iTween.EaseType.easeInOutCubic));
		iTween.RotateTo(rightWing.gameObject,iTween.Hash("rotation",rightWingAngle2,"time",3.5f,"easetype",iTween.EaseType.easeInOutCubic,"oncomplete","RotateWingFront","oncompletetarget",gameObject));
	}

	void RotateWingFront () {
		iTween.RotateTo(leftWing.gameObject,iTween.Hash("rotation",leftWingAngle3,"time",3.5f,"easetype",iTween.EaseType.easeInOutCubic));
		iTween.RotateTo(rightWing.gameObject,iTween.Hash("rotation",rightWingAngle3,"time",3.5f,"easetype",iTween.EaseType.easeInOutCubic,"oncomplete","RotateWingBack","oncompletetarget",gameObject));
	}

	void StartMoveNiuJiao () {
		iTween.MoveTo(niuJiao.gameObject,iTween.Hash ("position",niuJiaoMoveTarget,"time",3.5f,"islocal",true,"oncomplete","EndMoveNiuJiao","oncompletetarget",gameObject,"easetype",iTween.EaseType.easeInOutCubic));
	}

	void EndMoveNiuJiao () {
		iTween.MoveTo(niuJiao.gameObject,iTween.Hash ("position",niuJiaoCurrent,"time",3.5f,"islocal",true,"oncomplete","StartMoveNiuJiao","oncompletetarget",gameObject,"easetype",iTween.EaseType.easeInOutCubic));
	}

	void Update() {
		if (canPlayAnimation) {
			frontCircle.transform.Rotate (Vector3.forward, 30f * Time.deltaTime, Space.Self);
			backCircle.transform.Rotate (Vector3.forward, -30f * Time.deltaTime, Space.Self);	
		}
	}
}