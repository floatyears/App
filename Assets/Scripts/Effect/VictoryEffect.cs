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

//	private Vector3 localScale;
//	private Vector3 targetScale;

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

//	//------------------------------------------------------------------------------------------------
//	 test data
//	private int maxEmpirical = 100;
//	private int currentEmpirical = 50;
//	private int addEmpirical = 20;
//	//------------------------------------------------------------------------------------------------

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
//		Debug.LogError ("DestoryUI");
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

		int nextEmp = GlobalData.userInfo.NextExp;
		int maxEmp = clearQuest.exp;

		gotExp= clearQuest.gotExp;
		rank = GlobalData.userInfo.Rank;
		currentExp = GlobalData.userInfo.CurRankExp;
		currentTotalExp = GlobalData.Instance.GetUnitValue (TPowerTableInfo.UserExpType, rank);
		add = (float)gotExp / 10f;
		Debug.LogError ("UpdateLevelNumber");
		StartCoroutine (UpdateLevelNumber ());
//		int curCoin = GlobalData.accountInfo.Money;
//		int maxCoin = clearQuest.money;
//		int gotCoin = clearQuest.gotMoney;
	}

	IEnumerator UpdateLevelNumber () {
//		Debug.LogError ("UpdateLevelNumber gotExp : " + gotExp);
		while (gotExp > 0) {
			float addNum = gotExp - add;
			Debug.LogError ("UpdateLevelNumber addNum : " + addNum);
			if (addNum <= 0) {
				add = gotExp;
			} else {
				gotExp = addNum;
				currentExp += add;
			}
			gotExp -= add;
			currentExp += add;
			int showValue = (int)currentExp;
			empiricalLabel.text = showValue.ToString ();
			Debug.LogError ("UpdateLevelNumber showValue : " + addNum);
//			Debug.LogError(empiricalLabel.text);
			float progress = currentExp / currentTotalExp;
			levelProgress.fillAmount = progress;
			if(currentExp >= currentTotalExp) {
				currentExp -= currentTotalExp;
				rank++;
				currentTotalExp = GlobalData.Instance.GetUnitValue (TPowerTableInfo.UserExpType, rank);
			}
			yield return 0;
		}
	}

	void FindComponent () {
		MsgCenter.Instance.Invoke (CommandEnum.StopInput, null);
//		localScale = transform.localScale;
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
	}

	void Sure(GameObject go) {
		DestoryUI ();
		ControllerManager.Instance.ExitBattle ();
		UIManager.Instance.ExitBattle ();
	}
//
//	void Start() {
//		Init("aa");
//		PlayAnimation (TempFun,new VictoryInfo(100,0,0,100));
//
//	}
	
	public void ShowInfo(VictoryInfo vi) {
//		coinNumber = vi.Coin;
//		empireNumber = vi.maxEmpire;
//		StartCoroutine( UpdateCoinNumber (vi.startCoin, coinNumber));
//		StartCoroutine (UpdateLevelNumber (vi.currentEmpire, empireNumber));
	}
	VictoryInfo tempVictory;
	public void PlayAnimation (Callback callback,VictoryInfo vi) {
		sureButtonCallback = callback;
		tempVictory = vi;
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
//		ShowInfo (tempVictory);
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

public struct VictoryInfo {
	public int maxEmpire;
	public int currentEmpire ;
	public int startCoin;
	public int Coin;
	public VictoryInfo(int mEmpire, int cEmpire,int sCoin,int coin) {
		maxEmpire = mEmpire;
		currentEmpire = cEmpire;
		startCoin = sCoin;
		Coin = coin;
	}
}
