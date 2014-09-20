using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleResultView : ViewBase {
	private UISlider levelProgress;
	private UILabel coinLabel;
	private UILabel empiricalLabel;

	private UISprite leftWing;
	private UISprite rightWing;
	private UISprite niuJiao;
	private UISprite frontCircle;
	private UISprite backCircle;

	private TweenScale rankUpScale;
	private UILabel rankUpSprite;

	private UIButton sureButton;
	private GameObject parent;
	private GameObject dropItem;
	private Dictionary<TUserUnit, GameObject> dropItemList = new Dictionary<TUserUnit, GameObject> ();
	private TRspClearQuest rspClearQuest = null;
	private Queue<TUserUnit> getUserUnit = new Queue<TUserUnit> ();

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

	public override void Init (UIConfigItem config, Dictionary<string, object> data = null) {
		base.Init (config, data);
		FindComponent ();
	}

	public override void ShowUI () {
		base.ShowUI ();
		if (viewData != null && viewData.ContainsKey ("data")) {
			TRspClearQuest trcq = viewData["data"] as TRspClearQuest;
			ShowData (trcq);
			PlayAnimation ();		
		}

//		MsgCenter.Instance.AddListener (CommandEnum.VictoryData, VictoryData);

//		UIManager.Instance.HideBaseScene ();

//		if (UIManager.Instance.baseScene.PrevScene == ModuleEnum.UnitDetail || UIManager.Instance.baseScene.PrevScene == ModuleEnum.ShowCardEffect) {
//			StartShowGetCard();
//		}
	}

	public override void HideUI () {
		base.HideUI ();
		gameObject.SetActive (false);
//		MsgCenter.Instance.RemoveListener (CommandEnum.VictoryData, VictoryData);
//		UIManager.Instance.ShowBaseScene ();
	}

	public override void DestoryUI () {
		if(goAnim != null)
			iTween.Stop (goAnim);
		GameTimer.GetInstance ().ExitCountDonw (ShowGetCard);
		base.DestoryUI ();
	}

	void OnDestory() {

	}
	
	float currentLevelExp = 0;
	float gotExp = 0;
	float add = 0;
	int currentTotalExp = 0;
	int rank = 0;

	void ShowData(TRspClearQuest clearQuest){
		if (clearQuest == null) {
			return;	
		}
		rspClearQuest = clearQuest;

		BattleConfigData.Instance.gotFriendPoint = clearQuest.gotFriendPoint;

		AudioManager.Instance.PlayBackgroundAudio (AudioEnum.music_home);

		int nextEmp = DataCenter.Instance.UserInfo.NextExp;
		int maxEmp = clearQuest.exp;
		gotExp= clearQuest.gotExp;
		rank = DataCenter.Instance.oldAccountInfo.Rank;
		int totalPreExp = DataCenter.Instance.oldAccountInfo.CurPrevExp;
		currentLevelExp = clearQuest.exp - totalPreExp ;
		currentTotalExp = DataCenter.Instance.GetUnitValue (TPowerTableInfo.UserExpType, rank);
		add = (float)gotExp * 0.05f;
		int curCoin = DataCenter.Instance.AccountInfo.Money;
		int maxCoin = clearQuest.money;
		int gotCoin = clearQuest.gotMoney;
		float addCoin = gotCoin * 0.05f;
	
		coinLabel.text = clearQuest.gotMoney.ToString ();
		empiricalLabel.text = clearQuest.gotExp.ToString ();

		StartCoroutine (UpdateLevelNumber ());
		StartCoroutine (UpdateCoinNumber (addCoin, curCoin, gotCoin));

		for (int i = 0; i < clearQuest.gotUnit.Count; i++) {
			TUserUnit tuu = clearQuest.gotUnit[i];
		
			GameObject go = NGUITools.AddChild(parent, dropItem);
			go.SetActive(true);
			uint unitID = tuu.UnitInfo.ID;
			go.name = i.ToString();
			UISprite sprite = go.transform.Find("Avatar").GetComponent<UISprite>();
			ResourceManager.Instance.GetAvatarAtlas(unitID, sprite);
			sprite.enabled = false;
			DataCenter.Instance.CatalogInfo.AddHaveUnit(tuu.UnitInfo.ID);
			getUserUnit.Enqueue(tuu);
			dropItemList.Add(tuu, go);
		}

		StartShowGetCard ();
	}

	void StartShowGetCard() {
		if(getUserUnit.Count > 0) {
			GameTimer.GetInstance ().AddCountDown (0.5f, ShowGetCard);
		}
	}

	GameObject goAnim = null;
	TUserUnit showUserUnit = null;

	void ShowGetCard () {
		showUserUnit = getUserUnit.Dequeue ();
		goAnim = dropItemList [showUserUnit];
		iTween.ScaleTo (goAnim, iTween.Hash ("y", 0f, "time", 0.15f, "oncomplete", "RecoverScale", "oncompletetarget", gameObject));
		AudioManager.Instance.PlayAudio (AudioEnum.sound_grid_turn);
	}

	void RecoverScale () {
		goAnim.transform.Find ("Avatar").GetComponent<UISprite> ().enabled = true;
		goAnim.transform.Find ("Sprite_Mask").GetComponent<UISprite> ().enabled = false;
		iTween.ScaleTo (goAnim, iTween.Hash ("y", 1f, "time", 0.15f, "oncomplete", "AnimEnd", "oncompletetarget", gameObject));
	}

	void AnimEnd () {
		if (showUserUnit.UnitInfo.Rare >= 4) {
			AudioManager.Instance.PlayAudio(AudioEnum.sound_card_4);
		}

		if (DataCenter.Instance.CatalogInfo.IsHaveUnit (showUserUnit.Object.unitId)) {
			StartShowGetCard ();
		} else {
			DataCenter.Instance.CatalogInfo.AddHaveUnit(showUserUnit.Object.unitId);
			ModuleManager.Instance.ShowModule(ModuleEnum.ShowCardEffectModule);
			ModuleManager.Instance.ShowModule(ModuleEnum.ShowNewCardModule, showUserUnit);
		}
	}

	IEnumerator UpdateLevelNumber () {
		yield return new WaitForSeconds (1f);
		while (gotExp > 0) {
			if(gotExp - add<= 0) {
				add = gotExp;
			}
			gotExp -= add;
			currentLevelExp += add;
			int showValue = (int)currentLevelExp;
			float progress = currentLevelExp / currentTotalExp;
			levelProgress.value = progress;
			if(currentLevelExp >= currentTotalExp) {
				currentLevelExp -= currentTotalExp;
				rank++;
				AudioManager.Instance.PlayAudio(AudioEnum.sound_rank_up);
				currentTotalExp = DataCenter.Instance.GetUnitValue (TPowerTableInfo.UserExpType, rank);
				SetRankUpEnable(true);
				yield return new WaitForSeconds(0.5f);
				rankUpScale.ResetToBeginning();
				SetRankUpEnable(false);
			}
			yield return new WaitForSeconds(0.1f);
		}
	}

	void SetRankUpEnable(bool enable) {
		rankUpSprite.text = enable ? BattleFullScreenTipsView.RankUp : "";
		rankUpScale.enabled = enable;
	}
	
	void RankUp () {
//		battleQuest.battle.ShieldInput(true);
	}
	
	IEnumerator UpdateCoinNumber (float addCoin, float curCoin, float gotCoin) {
		yield return new WaitForSeconds (1f);
		while (gotCoin > 0) {
			if(gotCoin - addCoin <= 0) {
				addCoin = gotCoin;
			}	
			gotCoin-= addCoin;
			curCoin += addCoin;
			yield return new WaitForSeconds(0.1f);
		}
	}

	void FindComponent () {
		levelProgress = FindChild<UISlider> ("LvProgress");
		coinLabel = FindChild<UILabel>("CoinValue");
		empiricalLabel = FindChild<UILabel>("EmpiricalValue");
		leftWing = FindChild<UISprite>("LeftWing");
		rightWing = FindChild<UISprite>("RightWing");
		niuJiao = FindChild<UISprite>("NiuJiao");
		frontCircle = FindChild<UISprite>("FrontCircle");
		backCircle = FindChild<UISprite>("BackCircle");
		sureButton = FindChild<UIButton>("Button");
		UIEventListener.Get (sureButton.gameObject).onClick = Sure;
		sureButton.transform.Find ("Label").GetComponent<UILabel> ().text = TextCenter.GetText ("OK");
		niuJiaoCurrent = niuJiao.transform.localPosition;
		niuJiaoMoveTarget = new Vector3 (niuJiaoCurrent.x, niuJiaoCurrent.y - 20f, niuJiaoCurrent.z);
		parent = transform.Find ("VertialDrapPanel/SubPanel/Table").gameObject;
		dropItem = transform.Find ("VertialDrapPanel/SubPanel/MyUnitPrefab").gameObject;
		rankUpScale = FindChild<TweenScale>("RankPanel/RankUp");
		rankUpSprite = rankUpScale.GetComponent<UILabel> ();
	}

	void Sure(GameObject go) {
//		DestoryUI ();

		ModuleManager.Instance.EnterMainScene();
//		if (DataCenter.gameState == GameState.Evolve) {
//			UnitDetailView.isEvolve = true;
//			ModuleManager.Instance.ShowModule (ModuleEnum.UnitDetailModule,"unit",rspClearQuest.evolveUser);
////			UIManager.Instance.baseScene.PrevScene = ModuleEnum.Home;
//			HideUI ();
//			AudioManager.Instance.PlayAudio (AudioEnum.sound_card_evo);
//		} else if (!NoviceGuideStepEntityManager.isInNoviceGuide()) {
		TFriendInfo friendHelper = BattleConfigData.Instance.BattleFriend;
		bool isNull = friendHelper == null;
		bool addFriend = isNull ? false : (friendHelper.FriendState != bbproto.EFriendState.ISFRIEND || friendHelper.FriendPoint > 0);

		if (!isNull && addFriend) {
			ModuleManager.Instance.ShowModule(ModuleEnum.ResultModule);
			MsgCenter.Instance.Invoke(CommandEnum.ShowFriendPointUpdateResult, friendHelper);
		} else {
			DGTools.ChangeToQuest();
		}
//		} else {
//		}
		ModuleManager.Instance.DestroyModule (ModuleEnum.BattleResultModule);
	}

	void PlayAnimation () {
		if (!gameObject.activeSelf) {
			ShowUI ();
		}

		PrevAnimation ();
	}

	IEnumerator UpdateCoinNumber (int coinNum, int addNum) {
		coinNum ++;
		addNum --;
//		coinLabel.text = coinNum.ToString();
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
		levelProgress.value = progree;
		yield return 0;
		if (empire < maxEmpire) {
			StartCoroutine(UpdateLevelNumber(empire, maxEmpire));
		}
	}

	void PrevAnimation () {
		iTween.ScaleFrom (gameObject, iTween.Hash ("scale", Vector3.zero, "time", 0.8f, "easetype", iTween.EaseType.easeOutElastic, "oncomplete", "StartRotateWing", "oncompletetarget", gameObject));
	}

	void ScaleEnd() { }
	
	void StartRotateWing () {
		canPlayAnimation = true;
		iTween.RotateTo(leftWing.gameObject, iTween.Hash("rotation", leftWingAngle1,"time", 1f,"easetype",iTween.EaseType.easeInOutQuart,"delay",0.3f));
		iTween.RotateTo(rightWing.gameObject, iTween.Hash("rotation", rightWingAngle1,"time", 1f,"easetype",iTween.EaseType.easeInOutQuart,"oncomplete","PlayNext","oncompletetarget",gameObject,"delay",0.3f));
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