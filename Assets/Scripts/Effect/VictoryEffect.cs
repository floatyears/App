using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VictoryEffect : UIComponentUnity {
	private UISlider levelProgress;
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

	public override void Init (UIInsConfig config, IUICallback origin) {
		base.Init (config, origin);
		FindComponent ();
	}

	public override void ShowUI () {
		base.ShowUI ();
		gameObject.SetActive (true);
		MsgCenter.Instance.AddListener (CommandEnum.VictoryData, VictoryData);

		UIManager.Instance.HideBaseScene ();

		if (UIManager.Instance.baseScene.PrevScene == SceneEnum.UnitDetail || UIManager.Instance.baseScene.PrevScene == SceneEnum.ShowCardEffect) {
			StartShowGetCard();
		}
	}

	public override void HideUI () {
		base.HideUI ();
		gameObject.SetActive (false);
		MsgCenter.Instance.RemoveListener (CommandEnum.VictoryData, VictoryData);
		UIManager.Instance.ShowBaseScene ();
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

	void VictoryData (object data) {
		TRspClearQuest trcq = data as TRspClearQuest;
		ShowData (trcq);

		PlayAnimation ();
	}

	public void ShowData(TRspClearQuest clearQuest){
		if (clearQuest == null) {
			return;	
		}
		rspClearQuest = clearQuest;

		AudioManager.Instance.PlayBackgroundAudio (AudioEnum.music_home);

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
			TUserUnit tuu = clearQuest.gotUnit[i];
		
			GameObject go = NGUITools.AddChild(parent, dropItem);
			go.SetActive(true);
			uint unitID = tuu.UnitInfo.ID;
			go.name = i.ToString();
			UISprite sprite = go.transform.Find("Avatar").GetComponent<UISprite>();
			DataCenter.Instance.GetAvatarAtlas(unitID, sprite);
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
		iTween.ScaleTo (goAnim, iTween.Hash ("y", 0f, "time", 0.3f, "oncomplete", "RecoverScale", "oncompletetarget", gameObject));
		AudioManager.Instance.PlayAudio (AudioEnum.sound_grid_turn);
	}

	void RecoverScale () {
		goAnim.transform.Find ("Avatar").GetComponent<UISprite> ().enabled = true;
		goAnim.transform.Find ("Sprite_Mask").GetComponent<UISprite> ().enabled = false;
		iTween.ScaleTo (goAnim, iTween.Hash ("y", 1f, "time", 0.3f, "oncomplete", "AnimEnd", "oncompletetarget", gameObject));
	}

	void AnimEnd () {
		if (showUserUnit.UnitInfo.Rare >= 4) {
			AudioManager.Instance.PlayAudio(AudioEnum.sound_card_4);
		}

		if (DataCenter.Instance.CatalogInfo.IsHaveUnit (showUserUnit.Object.unitId)) {
			StartShowGetCard ();
		} else {
			DataCenter.Instance.CatalogInfo.AddHaveUnit(showUserUnit.Object.unitId);
			UIManager.Instance.ChangeScene(SceneEnum.ShowCardEffect);
			MsgCenter.Instance.Invoke(CommandEnum.ShowNewCard, showUserUnit);
		}
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
			levelProgress.value = progress;
			if(currentExp >= currentTotalExp) {
				currentExp -= currentTotalExp;
				rank++;
				AudioManager.Instance.PlayAudio(AudioEnum.sound_rank_up);
				currentTotalExp = DataCenter.Instance.GetUnitValue (TPowerTableInfo.UserExpType, rank);
			}
			yield return new WaitForSeconds(0.1f);
		}
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

			coinLabel.text = ((int)curCoin).ToString ();
			yield return new WaitForSeconds(0.1f);
		}

		coinLabel.text = ((int)curCoin).ToString ();
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
	}

	void Sure(GameObject go) {
		DestoryUI ();
		if (DataCenter.gameState == GameState.Evolve) {
			UIManager.Instance.baseScene.PrevScene = SceneEnum.Home;
			UIManager.Instance.ChangeScene (SceneEnum.UnitDetail);
			MsgCenter.Instance.Invoke (CommandEnum.ShowUnitDetail, rspClearQuest.evolveUser);

			HideUI();
//			DataCenter.gameState = GameState.Normal;
			AudioManager.Instance.PlayAudio (AudioEnum.sound_card_evo);
		} else {
			TFriendInfo friendHelper = ConfigBattleUseData.Instance.BattleFriend;
			if (friendHelper != null && !DataCenter.Instance.supportFriendManager.CheckIsMyFriend(friendHelper)) {
				UIManager.Instance.ChangeScene(SceneEnum.Result);
				MsgCenter.Instance.Invoke(CommandEnum.ShowFriendPointUpdateResult, ConfigBattleUseData.Instance.BattleFriend);
			} else {
				UIManager.Instance.ChangeScene(SceneEnum.Home);
			}
		}
	}

	public void PlayAnimation () {
		if (!gameObject.activeSelf) {
			ShowUI ();
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