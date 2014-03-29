using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EnemyItem : UIBaseUnity {
    [HideInInspector]
    public TEnemyInfo enemyInfo;
    [HideInInspector]
    public UITexture
        texture;
    private UITexture dropTexture;

    private UILabel bloodLabel;
    private UISprite bloodSprite;
    private UILabel nextLabel;
    private UIPanel effect;
    private Vector3 attackPosition;
    private Vector3 localPosition;

    private UILabel hurtValueLabel;
    private Queue<GameObject> hurtValueQueue = new Queue<GameObject>();
    private Vector3 hurtLabelPosition = Vector3.zero;
    private Vector3 initHurtLabelPosition = Vector3.zero;

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

    GameObject prevObject = null;

    void ReduceDefense(object data) {
        TClass<int,int,float> tc = data as TClass<int,int,float>;
        if (tc == null) {
            return;	
        }
    }

    Queue<AttackInfo> attackQueue = new Queue<AttackInfo>();
    void Attack(object data) {
        AttackInfo ai = data as AttackInfo;
        if (ai == null || ai.EnemyID != enemyInfo.EnemySymbol) {
            return;
        }
        if (prevObject != null) {
            Destroy(prevObject);
        }
        attackQueue.Enqueue(ai);
        GameTimer.GetInstance().AddCountDown(0.3f, Effect);
    }

    void Effect() {
		AttackInfo ai = attackQueue.Dequeue();
		DGTools.PlayAttackSound (ai.AttackType);
        ShowHurtInfo(ai.InjuryValue);
        InjuredShake();
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

	//================== old code. not use.
    void ShowInjuredEffect(int attackType) {
        GameObject obj = DataCenter.Instance.GetEffect(attackType) as GameObject;
        DGTools.PlayAttackSound(attackType);
        if (obj != null) {
            prevObject = NGUITools.AddChild(effect.gameObject, obj);
            prevObject.transform.localScale = new Vector3(100f, 100f, 100f);
            InjuredShake();
        }
    }
	//==================

    void InjuredShake() {
        iTween.ShakeScale(texture.gameObject, iTween.Hash("amount", new Vector3(0.5f, 0.5f, 0.5f), "time", 0.2f));
    }

    void BePosion(object data) {
        AttackInfo ai = data as AttackInfo;
        if (ai == null) {
            return;	
        }
        Debug.Log("play posion animation");
        Debug.Log("posion round : " + ai.AttackRound);
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
        dropTexture = FindChild<UITexture>("Drop");
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
		texture.mainTexture = tex;
		if (tex == null) {
			return;		
		}
		texture.width = tex.width;
		texture.height = tex.height;
    }

    public override void DestoryUI() {
		if (currentState == UIState.UIDestory) {
			return;
		}
        base.DestoryUI();
//		Debug.LogError ("DestoryUI : " + enemyInfo.EnemySymbol);
		if (gameObject != null) {
			Destroy(gameObject);
		}
    }
	
    public void DropItem(object data) {
        int pos = (int)data;
        if (pos == enemyInfo.EnemySymbol && !texture.enabled) {
            dropTexture.enabled = true;
            iTween.ShakeRotation(dropTexture.gameObject, iTween.Hash("z", 20, "time", 0.5f));  //"oncomplete","DorpEnd","oncompletetarget",gameObject
            GameTimer.GetInstance().AddCountDown(0.5f, DorpEnd);

			AudioManager.Instance.PlayAudio(AudioEnum.sound_get_chess);
        }

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
        //DestoryUI ();
        texture.enabled = false;
        nextLabel.text = "";
//		DropItem ();
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
//		enemyInfo = te;

        tempQue.Enqueue(te);

        GameTimer.GetInstance().AddCountDown(0.5f, RefreshData);
    }

    void RefreshData() {
        enemyInfo = tempQue.Dequeue();
        float value = (float)enemyInfo.GetBlood() / enemyInfo.GetInitBlood();
        SetBlood(value);
        SetNextLabel(enemyInfo.GetRound());
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
        SetBloodLabel(seu.GetBlood());
        SetNextLabel(seu.GetRound());
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
