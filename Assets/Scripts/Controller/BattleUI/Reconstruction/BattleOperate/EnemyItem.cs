using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EnemyItem : UIBaseUnity {
    [HideInInspector]
    public TEnemyInfo enemyInfo;
    [HideInInspector]
    public UITexture texture;
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

	[HideInInspector]
	public BattleEnemy battleEnemy;
//	public GameObject prevEffect = null;

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
    }

    Queue<AttackInfo> attackQueue = new Queue<AttackInfo>();
    void Attack(object data) {
        AttackInfo ai = data as AttackInfo;
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
        iTween.MoveTo(hurtLabel, iTween.Hash("position", hurtLabelPosition, "time", 1f, "easetype", iTween.EaseType.easeOutQuart, "oncomplete", "RemoveHurtLabel", "oncompletetarget", gameObject, "islocal", true));
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
    }

    void SkillPosion(object data) {
        AttackInfo ai = data as AttackInfo;
        if (ai == null) {
            return;	
        }
        Debug.Log("posion round : " + ai.AttackRound);
    }
	
    public void Init(TEnemyInfo te) {
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
        initHurtLabelPosition = hurtValueLabel.transform.localPosition;
        hurtLabelPosition = new Vector3(initHurtLabelPosition.x, initHurtLabelPosition.y + hurtValueLabel.height * 3, initHurtLabelPosition.z);
        enemyInfo = te;
        hurtValueLabel.gameObject.SetActive(false);
        SetData(te);
		TUnitInfo tui = DataCenter.Instance.GetUnitInfo (te.UnitID); //UnitInfo[te.UnitID];
		Texture2D tex = tui.GetAsset(UnitAssetType.Profile);
		if (tex == null) {
			texture.mainTexture = null;
			return;		
		}
		DGTools.ShowTexture (texture, tex);
		stateSprite = FindChild<UISprite>("StateSprite");
		stateSprite.spriteName = string.Empty;
		stateSprite.transform.localPosition = texture.transform.localPosition + new Vector3 (0f, tex.height, 0f);
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
		Debug.LogError ("id : " + seu.EnemyID + " seu.initBlood " + seu.initBlood + " SetData : " + seu.GetInitBlood ());
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
        nextLabel.text = "Next : " + seu;
    }
}
