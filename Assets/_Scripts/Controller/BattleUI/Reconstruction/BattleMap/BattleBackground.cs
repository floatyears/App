using UnityEngine;
using System.Collections.Generic;

public class BattleBackground : UIBaseUnity {
	private UITexture background;
	private Camera bottomCamera;
	private UITexture[] actor;
	private UISprite[] spSprite;
	private UISpriteAnimationCustom spriteAnimation;
	private GameObject battleBottom;

	private UISlider bloodBar;
	private UILabel label;
	private int initBlood = -1;
	private int initEnergyPoint = -1;
	private int currentEnergyPoint = -1;

	private static Vector3 actorPosition = Vector3.zero;
	public static Vector3 ActorPosition	{
		get {
			return actorPosition;
		}
	}

	private BattleBottom _battleBottomScript;
	public BattleBottom battleBottomScript {
		get { return _battleBottomScript; }
	}

	private BattleQuest battleQuest;

	public override void Init (string name){
		base.Init (name);
		bottomCamera = Main.Instance.bottomCamera;
		battleBottom = FindChild<Transform>("BattleBottom").gameObject;
		_battleBottomScript = battleBottom.AddComponent<BattleBottom> ();
		_battleBottomScript.Init (bottomCamera);
		actorPosition = transform.Find ("Position").localPosition;

		actor = new UITexture[5];
		spSprite = new UISprite[20];
		string path;

		for (int i = 0; i < actor.Length; i++) {
			path = "BattleBottom/Actor/" + i.ToString();
			actor[i] = 	transform.Find(path).GetComponent<UITexture>();
		}

		GameObject panel = battleBottom.transform.Find("Panel").gameObject;
		panel.layer = GameLayer.BottomInfo;
		for (int i = spSprite.Length; i > 0; i--) {
			path = "Panel/"+ i;
			spSprite[spSprite.Length - i] = battleBottom.transform.Find(path).GetComponent<UISprite>();
		}

		spriteAnimation = battleBottom.transform.Find ("Panel/HP").GetComponent<UISpriteAnimationCustom> ();
		bloodBar = battleBottom.transform.Find("Panel/Slider").GetComponent<UISlider>();
		label = battleBottom.transform.Find("Panel/HPLabel").GetComponent<UILabel>();

		InitTransform ();
	}

	public void SetBattleQuest (BattleQuest bq) {
		battleQuest = bq;
		_battleBottomScript.battleQuest = bq;
	}

	void InitTransform() {
		TUnitParty upi = DataCenter.Instance.PartyInfo.CurrentParty; 
		Dictionary<int,TUserUnit> userUnitInfo = upi.UserUnit;
		for (int i = 0; i < userUnitInfo.Count; i++) {
			TUserUnit tuu = (userUnitInfo.ContainsKey(i) ? userUnitInfo[i] : null);
			if(tuu == null) {
				continue;
			}
		}
	}

	public override void ShowUI () {
		base.ShowUI ();
		gameObject.SetActive (true);
		battleBottom.SetActive (true);
		AddListener ();
	}

	public override void CreatUI () {
		base.CreatUI ();
	}

	public override void HideUI () {
		base.HideUI ();
		gameObject.SetActive (false);
		battleBottom.SetActive (false);
		_battleBottomScript.RemoveAllSkill ();
		RemoveListener ();
	}

	public override void DestoryUI () {
		base.DestoryUI ();
		Destroy (battleBottom);
		_battleBottomScript = null;
	}

	public void InitData (int blood, int maxBlood, int energyPoint) {
		initBlood = maxBlood;
		currentBlood = blood;
		currentEnergyPoint = initEnergyPoint = energyPoint;
		SetBlood (currentBlood); 
		InitSP ();
	}

	void InitSP () {
		for (int i = 0; i < spSprite.Length; i++) {
			if(i >= initEnergyPoint) {
				spSprite[i].enabled = false;
			} else {
				spSprite[i].enabled = true;
			}
		}
	}

	private int currentBlood = 0;
	void SetBlood (int num) {
		string info = num + "/" + initBlood;
		label.text = info;
//		if (num > currentBlood || num == initBlood) {
//			spriteAnimation.Reset();
//		}
		currentBlood = num;
		bloodBar.value = DGTools.IntegerSubtriction(num, initBlood);
	}

	int preBlood = 0;
	void ListenUnitBlood (object data) {
		int blood = (int)data;
		SetBlood (blood);
	}

	void ListenEnergyPoint (object data) {
		int energyPoint = (int) data;
		for (int i = 0; i < spSprite.Length; i++) {
			UISprite sprite = spSprite[i];
			if(i < energyPoint) {
				if(!sprite.enabled) {
					sprite.enabled = true;
				}
			} else {
				if(sprite.enabled) {
					sprite.enabled = false;
				}
			}
		}
	}

	void RecoverHP(object data) {
//		AttackInfo ai = data as AttackInfo;
//
//		if (ai.AttackRange == 2) {
//			spriteAnimation.Reset();
//		}
	}

	void ShowHPAnimation(object data) {
		AudioManager.Instance.PlayAudio(AudioEnum.sound_hp_recover);
		spriteAnimation.Reset();
	}

	void AddListener () {
		MsgCenter.Instance.AddListener (CommandEnum.UnitBlood, ListenUnitBlood);
		MsgCenter.Instance.AddListener (CommandEnum.EnergyPoint, ListenEnergyPoint);
//		MsgCenter.Instance.AddListener (CommandEnum.AttackEnemy, RecoverHP);
		MsgCenter.Instance.AddListener (CommandEnum.ShowHPAnimation, ShowHPAnimation);
	}

	void RemoveListener () {
		MsgCenter.Instance.RemoveListener (CommandEnum.UnitBlood, ListenUnitBlood);
		MsgCenter.Instance.RemoveListener (CommandEnum.EnergyPoint, ListenEnergyPoint);
//		MsgCenter.Instance.RemoveListener (CommandEnum.AttackEnemy, RecoverHP);
		MsgCenter.Instance.RemoveListener (CommandEnum.ShowHPAnimation, ShowHPAnimation);
	}
}
