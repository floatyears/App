using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EnemyItem : UIBaseUnity {
    [HideInInspector]
    public TEnemyInfo enemyInfo;
    [HideInInspector]
    public UITexture texture;
	[HideInInspector]
	public TUnitInfo enemyUnitInfo;

    private UISprite dropTexture;
    private UILabel bloodLabel;
    private UISprite bloodSprite;
    private UILabel nextLabel;
	private UISprite stateSprite;

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
	
	[HideInInspector]
	public BattleEnemy battleEnemy;

    void OnEnable() {
        MsgCenter.Instance.AddListener(CommandEnum.EnemyAttack, EnemyAttack);
        MsgCenter.Instance.AddListener(CommandEnum.EnemyRefresh, EnemyRefresh);
        MsgCenter.Instance.AddListener(CommandEnum.EnemyDead, EnemyDead);
        MsgCenter.Instance.AddListener(CommandEnum.AttackEnemy, Attack);
        MsgCenter.Instance.AddListener(CommandEnum.SkillPosion, SkillPosion);
        MsgCenter.Instance.AddListener(CommandEnum.BePosion, BePosion);
        MsgCenter.Instance.AddListener(CommandEnum.ReduceDefense, ReduceDefense);
        MsgCenter.Instance.AddListener(CommandEnum.DropItem, DropItem);
    }

    void OnDisable() {
		MsgCenter.Instance.RemoveListener(CommandEnum.EnemyAttack, EnemyAttack);
        MsgCenter.Instance.RemoveListener(CommandEnum.EnemyRefresh, EnemyRefresh);
        MsgCenter.Instance.RemoveListener(CommandEnum.EnemyDead, EnemyDead);
        MsgCenter.Instance.RemoveListener(CommandEnum.AttackEnemy, Attack);
        MsgCenter.Instance.RemoveListener(CommandEnum.SkillPosion, SkillPosion);
        MsgCenter.Instance.RemoveListener(CommandEnum.BePosion, BePosion);
        MsgCenter.Instance.RemoveListener(CommandEnum.ReduceDefense, ReduceDefense);
        MsgCenter.Instance.RemoveListener(CommandEnum.DropItem, DropItem);
    }
	
    void ReduceDefense(object data) {
		AttackInfo reduceDefense = data as AttackInfo;
		if (reduceDefense == null) {
            return;
        }

		if (reduceDefense.AttackRound == 0) {
			ShowStateException (StateEnum.ReduceDefense, true); 	// remove 
		} else {
			ShowStateException(StateEnum.ReduceDefense);
		}
    }

    Queue<AttackInfo> attackQueue = new Queue<AttackInfo>();
    void Attack(object data) {
        AttackInfo ai = data as AttackInfo;
//		Debug.LogError("attack enemy");
        if (ai == null || ai.EnemyID != enemyInfo.EnemySymbol || ai.AttackValue == 0) {
            return;
        }
        attackQueue.Enqueue(ai);
        GameTimer.GetInstance().AddCountDown(0.3f, Effect);
    }

    void Effect() {
		AttackInfo ai = attackQueue.Dequeue();
		DisposeRestraint (ai);
		DGTools.PlayAttackSound (ai.AttackType);
        ShowHurtInfo(ai.InjuryValue);
		battleEnemy.EnemyItemPlayEffect (this, ai);
    }

	void DisposeRestraint(AttackInfo ai) {
		if (!string.IsNullOrEmpty (stateSprite.spriteName)) {
			stateSprite.spriteName = string.Empty;
		}

		if (DGTools.RestraintType (ai.AttackType, enemyInfo.GetUnitType ())) {
			DGTools.ShowSprite (stateSprite, "Weak"); // weak == attack count atlas sprite name.
			ShakeStateSprite ();
		} else if (DGTools.RestraintType (ai.AttackType, enemyInfo.GetUnitType (), true)) {
			DGTools.ShowSprite (stateSprite, "Guard"); // weak == attack count atlas sprite name.
			ShakeStateSprite ();
		}
	}

	void ShakeStateSprite () {
		iTween.ScaleFrom (stateSprite.gameObject, iTween.Hash ("scale", new Vector3 (2f, 2f, 2f), "time", 0.4f, "easetype", iTween.EaseType.easeInQuart, "oncomplete", "HideStateSprite", "oncompletetarget", gameObject));
	}

	void HideStateSprite () {
		stateSprite.spriteName = string.Empty;
	}

    void ShowHurtInfo(int injuredValue) {
        GameObject hurtLabel = NGUITools.AddChild(gameObject, hurtValueLabel.gameObject);
        hurtLabel.SetActive(true);
        hurtLabel.transform.localPosition = initHurtLabelPosition;
        hurtValueQueue.Enqueue(hurtLabel);
        UILabel info = hurtLabel.GetComponent<UILabel>();
        info.text = injuredValue.ToString();
        iTween.MoveTo(hurtLabel, iTween.Hash("position", hurtLabelPosition, "time", 0.8f, "easetype", iTween.EaseType.easeInBack, "oncomplete", "RemoveHurtLabel", "oncompletetarget", gameObject, "islocal", true));
    }

    void RemoveHurtLabel() {
        Destroy(hurtValueQueue.Dequeue());
    }


    void ShowInjuredEffect(AttackInfo ai) {
//		GameObject obj = DataCenter.Instance.GetEffect(ai) as GameObject;
//		DGTools.PlayAttackSound(ai.AttackType);
//		InjuredShake();
//        if (obj != null) {
//            prevObject = NGUITools.AddChild(effect.gameObject, obj);
//			if(ai.AttackType == 1) {
//				prevObject.transform.localScale = new Vector3(400f, 300f, 300f);
//			}else{
//				prevObject.transform.localScale = new Vector3(1f, 1f, 1f);
//			}
//        }
    }

    public void InjuredShake() {
        iTween.ShakeScale(texture.gameObject, iTween.Hash("amount", new Vector3(0.5f, 0.5f, 0.5f), "time", 0.2f));
    }

	AttackInfo posionAttack;

    void BePosion(object data) {
		posionAttack = data as AttackInfo;
		if (posionAttack == null) {
            return;	
        }
        Debug.Log("play posion animation");
		Debug.Log("posion round : " + posionAttack.AttackRound);
//		stateLabel.text = "positon : " + posionAttack.AttackRound;

		ShowStateException (StateEnum.Poison);
    }

    void SkillPosion(object data) {
        AttackInfo ai = data as AttackInfo;
        if (ai == null) {
            return;	
        }
		if (ai.AttackRound == 0) {
//			stateLabel.text = "";
			ShowStateException (StateEnum.Poison,true);
		} else {
//			stateLabel.text = "positon : " + posionAttack.AttackRound;
			ShowStateException (StateEnum.Poison);
		}
//        Debug.Log("posion round : " + ai.AttackRound);
    }
	
    public void Init(TEnemyInfo te, Callback callBack) {
		stateLabel = FindChild<UILabel>("SateLabel");

        texture = FindChild<UITexture>("Texture");
		UIEventListener.Get (texture.gameObject).onClick = TargetEnemy;
        dropTexture = FindChild<UISprite>("Drop");
        dropTexture.enabled = false;
        localPosition = texture.transform.localPosition;
        attackPosition = new Vector3(localPosition.x, BattleBackground.ActorPosition.y, localPosition.z);
        bloodSprite = FindChild<UISprite>("BloodSprite");
        nextLabel = FindChild<UILabel>("NextLabel");
        effect = FindChild<UIPanel>("Effect");
		hurtValueLabel = FindChild<UILabel>("HurtLabel");
        enemyInfo = te;
        hurtValueLabel.gameObject.SetActive(false);
        SetData(te);

		stateSprite = FindChild<UISprite>("StateSprite");
		stateSprite.spriteName = string.Empty;

		stateExceptionSprite = FindChild<UISprite>("StateExceptionSprite");
		initStateExceptionSprite = stateExceptionSprite.transform.localPosition;

		enemyUnitInfo = DataCenter.Instance.GetUnitInfo (te.UnitID); //UnitInfo[te.UnitID];
		enemyUnitInfo.GetAsset(UnitAssetType.Profile,o=>{
			Texture2D tex = o as Texture2D;

			if (tex == null) {
				texture.mainTexture = null;
				stateSprite.transform.localPosition = texture.transform.localPosition + new Vector3(0f, 100f, 0f);
				ResetHurtLabelPosition();
			} else {
				DGTools.ShowTexture(texture,tex);
				SetBloodSpriteWidth ();
				stateSprite.transform.localPosition = texture.transform.localPosition + new Vector3 (0f, tex.height * 0.5f, 0f);
				ResetHurtLabelPosition();
			}

			callBack();
		});

    }

    public override void DestoryUI() {
		if (currentState == UIState.UIDestory) {
			return;
		}
        base.DestoryUI();
		if (gameObject != null) {
			Destroy(gameObject);
		}
    }

	public void CompressTextureSize(float proportion) {
		if (proportion < 1f) {
//			Debug.LogError("CompressTexture: width:"+texture.width+" => "+(int)(texture.width * proportion));
			texture.width = (int)(texture.width * proportion);
			texture.height = (int)(texture.height * proportion);
			SetBloodSpriteWidth ();
			ResetHurtLabelPosition ();
		}
	}

	void ResetHurtLabelPosition() {
		initHurtLabelPosition = stateSprite.transform.localPosition - new Vector3(0f,texture.height * 0.2f,0f);
		hurtLabelPosition = new Vector3(initHurtLabelPosition.x, initHurtLabelPosition.y - hurtValueLabel.height * 2, initHurtLabelPosition.z);
	}

	void SetBloodSpriteWidth() {
		float width = texture.width * 0.6f;
		bloodSprite.width = (int)width;
		float x = texture.transform.localPosition.x;
		float bloodX = x - width * 0.5f;
		Vector3 pos = bloodSprite.transform.localPosition;
		bloodSprite.transform.localPosition = new Vector3 (bloodX, pos.y, pos.z);
	}
	
    public void DropItem(object data) {
        int pos = (int)data;
        if (pos == (int)enemyInfo.EnemySymbol && !texture.enabled) {
			if(enemyInfo.drop == null) {
				return;
			}
			TUnitInfo tui = enemyInfo.drop.UnitInfo;
			dropTexture.enabled = true;
			dropTexture.spriteName = DGTools.GetUnitDropSpriteName(tui.Rare);
            iTween.ShakeRotation(dropTexture.gameObject, iTween.Hash("z", 20, "time", 0.5f));  //"oncomplete","DorpEnd","oncompletetarget",gameObject
            GameTimer.GetInstance().AddCountDown(0.5f, DorpEnd);
			AudioManager.Instance.PlayAudio(AudioEnum.sound_get_chess);
        }
    }

	void TargetEnemy(GameObject go) {
//		Debug.LogError ("TargetEnemy : " + enemyInfo);
		MsgCenter.Instance.Invoke (CommandEnum.TargetEnemy, enemyInfo);
	}
	
    void DorpEnd() {
        DestoryUI();
    }

    void EnemyDead(object data) {
        TEnemyInfo te = data as TEnemyInfo;
        if (te == null || te.EnemySymbol != enemyInfo.EnemySymbol) {
            return;		
        }
        AudioManager.Instance.PlayAudio(AudioEnum.sound_enemy_die);
        texture.enabled = false;
//		Debug.LogError ("enemydead next label clear ");
        nextLabel.text = "";
    }

    Queue<TEnemyInfo> tempQue = new Queue<TEnemyInfo>();
    void EnemyRefresh(object data) {
        TEnemyInfo te = data as TEnemyInfo;
        if (te == null) {
            return;		
        }

        if (te.EnemySymbol != enemyInfo.EnemySymbol) {
            return;		
        }
        tempQue.Enqueue(te);
        GameTimer.GetInstance().AddCountDown(0.2f, RefreshData);
    }

    void RefreshData() {
        enemyInfo = tempQue.Dequeue();
        float value = (float)enemyInfo.initBlood / enemyInfo.GetInitBlood();
        SetBlood(value);
		SetNextLabel(enemyInfo.initAttackRound);
    }

    void EnemyAttack(object data) {
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

    void SetData(TEnemyInfo seu) {
		bloodSprite.fillAmount =(float)seu.initBlood / seu.GetInitBlood();
		SetNextLabel(seu.initAttackRound);
    }

    void SetBlood(float value) {
        StartCoroutine(CountBloodValue(value));
    }

    IEnumerator CountBloodValue(float fillAmount) {
		float value = (bloodSprite.fillAmount - fillAmount) / 5f;
        while (bloodSprite.fillAmount > fillAmount) {
			bloodSprite.fillAmount -= value;
            yield return 0;
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
//		foreach (var item in stateCache.Values) {
//			Vector3 localposition = ins.localPosition;
//			float distance = Vector3.Distance(localposition, item.transform.localPosition);
//			if(distance <= 2){	// deviation distance.
//				ins.localPosition = new Vector3(localposition.x - 30f, localposition.y, localposition.z); 
//			}
//			else{
//				break;
//			}
//		}
		DGTools.SortStateItem (stateCache, ins, -30f); // -30f. enemy state sprite sort is right to left.
		stateCache.Add (se, ins.gameObject);
	}
}
