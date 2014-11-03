using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using bbproto;

public class BattleEnemyItem : MonoBehaviour {
    [HideInInspector]
    public EnemyInfo enemyInfo;
    [HideInInspector]
    public UITexture texture;
	[HideInInspector]
	public UnitInfo enemyUnitInfo;

    private UISprite dropTexture;
    private UILabel bloodLabel;
    private UISprite bloodSprite;
	private UISprite bloodBgSprite;
    private UILabel nextLabel;
	private UILabel stateSprite;

    private UIPanel effect;
    private Vector3 attackPosition;
    private Vector3 localPosition;

    private UILabel hurtValueLabel;
    private Queue<GameObject> hurtValueQueue = new Queue<GameObject>();
    private Vector3 hurtLabelPosition = Vector3.zero;
    private Vector3 initHurtLabelPosition = Vector3.zero;

	private UILabel stateLabel;

	private UISprite stateExceptionSprite;
	private Dictionary<StateEnum, GameObject> stateCache = new Dictionary<StateEnum, GameObject> ();
	private Vector3 initStateExceptionSprite;

	public void Init(EnemyInfo te, Callback callBack) {
		stateLabel = transform.FindChild("SateLabel").GetComponent<UILabel>();
		
		texture = transform.FindChild("Texture").GetComponent<UITexture>();
		UIEventListenerCustom.Get (texture.gameObject).onClick = TargetEnemy;
		dropTexture = transform.FindChild("Drop").GetComponent<UISprite>();
		dropTexture.enabled = false;
		localPosition = texture.transform.localPosition;
		//        attackPosition = new Vector3(localPosition.x, BattleBackground.ActorPosition.y, localPosition.z);
		bloodSprite = transform.FindChild("BloodSprite").GetComponent<UISprite>();
		bloodBgSprite = transform.FindChild("BloodSpriteBG").GetComponent<UISprite>();
		nextLabel = transform.FindChild("NextLabel").GetComponent<UILabel>();
		effect = transform.FindChild("Effect").GetComponent<UIPanel>();
		hurtValueLabel = transform.FindChild("HurtLabel").GetComponent<UILabel>();

		hurtValueLabel.gameObject.SetActive(false);

		
		stateSprite = transform.FindChild("StateSprite").GetComponent<UILabel>();

		stateExceptionSprite = transform.FindChild("StateExceptionSprite").GetComponent<UISprite>();
		initStateExceptionSprite = stateExceptionSprite.transform.localPosition;
		
		RefreshData (te,callBack);
	}

	public void RefreshData(EnemyInfo te, Callback callBack){
		enemyInfo = te;
		SetData(te);
		stateSprite.text = string.Empty;
		enemyUnitInfo = DataCenter.Instance.UnitData.GetUnitInfo (te.UnitID); //UnitInfo[te.UnitID];
		ResourceManager.Instance.GetAvatar(UnitAssetType.Profile,te.UnitID,o=> {
			Texture2D tex = o as Texture2D;
			if (tex == null) {
				texture.mainTexture = null;
				stateSprite.transform.localPosition = texture.transform.localPosition + new Vector3(0f, 100f, 0f);
			} else {
				DGTools.ShowTexture(texture,tex);

				SetBloodSpriteWidth ();
				stateSprite.transform.localPosition = texture.transform.localPosition + new Vector3 (0f, tex.height * 0.5f, 0f);
			}
//			Debug.Log ("texture enable: " + texture.enabled);
			initHurtLabelPosition = stateSprite.transform.localPosition - new Vector3(0f,texture.height * 0.2f,0f);
			hurtLabelPosition = new Vector3(initHurtLabelPosition.x, initHurtLabelPosition.y - hurtValueLabel.height * 2, initHurtLabelPosition.z);
			callBack();
		});
	}


    void OnEnable() {
        MsgCenter.Instance.AddListener(CommandEnum.SkillPosion, SkillPosion);
        MsgCenter.Instance.AddListener(CommandEnum.BePosion, BePosion);
    }

    void OnDisable() {
        MsgCenter.Instance.RemoveListener(CommandEnum.SkillPosion, SkillPosion);
        MsgCenter.Instance.RemoveListener(CommandEnum.BePosion, BePosion);
    }
	
    public void ReduceDefense(object data) {
		AttackInfoProto reduceDefense = data as AttackInfoProto;
		if (reduceDefense == null) {
            return;
        }

		if (reduceDefense.attackRound == 0) {
			ShowStateException (StateEnum.ReduceDefense, true); 	// remove 
		} else {
			ShowStateException(StateEnum.ReduceDefense);
		}
    }

//    Queue<AttackInfo> attackQueue = new Queue<AttackInfo>();
   	public void AttackEnemy(object data) {
        AttackInfoProto ai = data as AttackInfoProto;
        if (ai == null || ai.enemyID != enemyInfo.EnemySymbol || ai.attackValue == 0) {
            return;
        }
//        attackQueue.Enqueue(ai);
		GameTimer.GetInstance().AddCountDown(0.3f, ()=>{
//			AttackInfo ai1 = attackQueue.Dequeue();
			if (!string.IsNullOrEmpty (stateSprite.text)) {
				stateSprite.text = string.Empty;
			}
			if (DGTools.RestraintType (ai.attackType, enemyInfo.GetUnitType ())) {
				stateLabel.text = weak; // DGTools.ShowSprite (stateSprite, "Weak"); // weak == attack count atlas sprite name.
			} else if (DGTools.RestraintType (ai.attackType, enemyInfo.GetUnitType (), true)) {
				stateLabel.text = guard;// DGTools.ShowSprite (stateSprite, "Guard"); // weak == attack count atlas sprite name.
			}
			iTween.ScaleFrom (stateSprite.gameObject, iTween.Hash ("scale", new Vector3 (2f, 2f, 2f), "time", 0.4f, "easetype", iTween.EaseType.easeInQuart, "oncomplete", "HideStateSprite", "oncompletetarget", gameObject));
			DGTools.PlayAttackSound (ai.attackType);

			GameObject hurtLabel = NGUITools.AddChild(gameObject, hurtValueLabel.gameObject);
			hurtLabel.SetActive(true);
			hurtLabel.transform.localPosition = initHurtLabelPosition;
			hurtValueQueue.Enqueue(hurtLabel);
			UILabel info = hurtLabel.GetComponent<UILabel>();
			info.text = "-" + ai.injuryValue.ToString(); //掉血数字
			iTween.MoveTo(hurtLabel, iTween.Hash("position", hurtLabelPosition, "time", 0.8f, "easetype", iTween.EaseType.easeInBack, "oncomplete", "RemoveHurtLabel", "oncompletetarget", gameObject, "islocal", true));
//			battleEnemy.EnemyItemPlayEffect (this, ai);
		});
    }


	const string weak = "Weak";
	const string guard = "Guard";

	void HideStateSprite () {
		stateSprite.text = string.Empty;
	}

    void RemoveHurtLabel() {
        Destroy(hurtValueQueue.Dequeue());
    }

    public void InjuredShake() {
        iTween.ShakeScale(texture.gameObject, iTween.Hash("amount", new Vector3(0.5f, 0.5f, 0.5f), "time", 0.2f));
    }

	AttackInfoProto posionAttack;

    void BePosion(object data) {
		posionAttack = data as AttackInfoProto;
		if (posionAttack == null) {
            return;	
        }

        Debug.Log("play posion animation");
		Debug.Log("posion round : " + posionAttack.attackRound);

		ShowStateException (StateEnum.Poison);
    }

    void SkillPosion(object data) {
        AttackInfoProto ai = data as AttackInfoProto;
        if (ai == null) {
            return;	
        }
		if (ai.attackRound == 0) {
			ShowStateException (StateEnum.Poison,true);
		} else {
			ShowStateException (StateEnum.Poison);
		}
    }

    public void DestoryUI() {
		if (gameObject != null) {
			Destroy(gameObject);
		}
    }

	void SetBloodSpriteWidth() {
		float width = texture.width * 0.6f;
		bloodSprite.width = (int)width;
		bloodBgSprite.width = (int)width + 4;
		float bloodX = texture.transform.localPosition.x - width * 0.5f;
		Vector3 pos = bloodSprite.transform.localPosition;
		bloodSprite.transform.localPosition = new Vector3 (bloodX, pos.y, pos.z);
		bloodBgSprite.transform.localPosition = new Vector3 (bloodX - 2, pos.y, pos.z);
	}
	
    void DropItem() {
//        if (!texture.enabled) {
		texture.enabled = false;
		bloodSprite.enabled = false;
		nextLabel.enabled = false;
		bloodBgSprite.enabled = false;

		UnitInfo tui = enemyInfo.dropUnit.UnitInfo;
		BattleConfigData.Instance.storeBattleData.GetLastQuestData ().getUnit.Add (enemyInfo.dropUnit.dropId);
		dropTexture.enabled = true;
		dropTexture.spriteName = DGTools.GetUnitDropSpriteName(tui.rare);
	    iTween.ShakeRotation(dropTexture.gameObject, iTween.Hash("z", 20, "time", 0.5f));  //"oncomplete","DorpEnd","oncompletetarget",gameObject
	    GameTimer.GetInstance().AddCountDown(0.5f, DropEnd);
		AudioManager.Instance.PlayAudio(AudioEnum.sound_get_chess);
//        }
    }

	void TargetEnemy(GameObject go) {
//		MsgCenter.Instance.Invoke (CommandEnum.TargetEnemy, enemyInfo);
		BattleAttackManager.Instance.TargetEnemy (enemyInfo);
	}
	
    void DropEnd() {
		texture.enabled = true;
		bloodSprite.enabled = true;
		nextLabel.enabled = true;
		bloodBgSprite.enabled = true;
		dropTexture.enabled = false;

		gameObject.SetActive (false);
		ModuleManager.SendMessage (ModuleEnum.BattleTopModule, "refresh");
    }

    public void EnemyDead() {
        AudioManager.Instance.PlayAudio(AudioEnum.sound_enemy_die);
		while (hurtValueQueue.Count > 0) {
			GameObject.Destroy(hurtValueQueue.Dequeue());	
		}
		if (enemyInfo.dropUnit != null) {
			DropItem();
		}else{
			gameObject.SetActive (false);
		}

    }

    Queue<EnemyInfo> tempQue = new Queue<EnemyInfo>();
    public void EnemyRefresh(object data) {
        EnemyInfo te = data as EnemyInfo;
        if (te == null) {
            return;		
        }

        if (te.EnemySymbol != enemyInfo.EnemySymbol) {
            return;		
        }
        tempQue.Enqueue(te);
		GameTimer.GetInstance().AddCountDown(0.2f, ()=>{
			enemyInfo = tempQue.Dequeue();
			float value = (float)enemyInfo.currentHp / enemyInfo.GetInitBlood();
			//		Debug.LogError ("RefreshData : " + value);
			SetBlood(value);
			SetNextLabel(enemyInfo.currentNext);
		});
    }

    public void EnemyAttack(object data) {
        uint id = (uint)data;
        if (id == enemyInfo.EnemySymbol) {
            iTween.ScaleTo(gameObject, new Vector3(1.5f, 1.5f, 1f), 0.2f);
            iTween.MoveTo(texture.gameObject, iTween.Hash("position", attackPosition, "time", 0.2f, "oncomplete", "MoveBack", "oncompletetarget", gameObject, "islocal", true, "easetype", iTween.EaseType.easeInCubic));
        }
    }

    void MoveBack() {
        iTween.ScaleTo(gameObject, new Vector3(1f, 1f, 1f), 0.2f);
        iTween.MoveTo(texture.gameObject, iTween.Hash("position", localPosition, "time", 0.2f, "islocal", true, "easetype", iTween.EaseType.easeOutCubic));
    }

    void SetData(EnemyInfo seu) {
		bloodSprite.fillAmount =(float)seu.currentHp / seu.GetInitBlood();
		SetNextLabel(seu.currentNext);
    }

    void SetBlood(float value) {
        StartCoroutine(CountBloodValue(value));
    }

    IEnumerator CountBloodValue(float fillAmount) {
		yield return new WaitForSeconds (0.15f);
		float value = (bloodSprite.fillAmount - fillAmount) / 5f;

        while (bloodSprite.fillAmount > fillAmount) {
			bloodSprite.fillAmount -= value;
//			Debug.LogError("CountBloodValue : " + bloodSprite.fillAmount);
            yield return 1;
        }

        bloodSprite.fillAmount = fillAmount;
    }

    void SetBloodLabel(int seu) {
    }

    void SetNextLabel(int seu) {
        nextLabel.text = "Next " + seu;
    }

	void ShowStateException(StateEnum se, bool clear = false) {
		if (se == StateEnum.None) {
			return;	
		}

		if (stateCache.ContainsKey (se)) {
			if (clear) {
				GameObject go = stateCache [se];
				Destroy (go);
				stateCache.Remove (se);
			}
			return;
		} 

		if (clear) {
			return;	
		}

		Transform ins = NGUITools.AddChild (stateExceptionSprite.transform.parent.gameObject, stateExceptionSprite.gameObject).transform;
		UISprite sprite = ins.GetComponent<UISprite> ();
		sprite.enabled = true;
		ins.localPosition = initStateExceptionSprite;
		DGTools.SortStateItem (stateCache, ins, -30f); // -30f. enemy state sprite sort is right to left.
		stateCache.Add (se, ins.gameObject);
	}
}
