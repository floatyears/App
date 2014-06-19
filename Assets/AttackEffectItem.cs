using UnityEngine;
using System.Collections;

public class AttackEffectItem : MonoBehaviour {
	private UISprite backGroundSprite;
	private UITexture avatarTexture;
	private UILabel skillNameLabel;
	private UILabel ATKLabel;

	private Vector3 moveEndPosition;
	private Vector3 dropEndPosition;
	private Vector3 middlePosition;
	private Callback callback;

	public void RefreshInfo(string userUnitID, int skillID, Callback cb, int atk = 0, bool recoverHP = false) {
		CheckComponentInit ();

		callback = cb;

		TUserUnit tuu = DataCenter.Instance.UserUnitList.Get (userUnitID);
		if (tuu == null) {
			Debug.LogError("userunit is null : " + userUnitID);	
			return;
		}
		backGroundSprite.spriteName = tuu.UnitType.ToString ();
		tuu.UnitInfo.GetAsset (UnitAssetType.Avatar,o=>{
			avatarTexture.mainTexture =  o as Texture2D;
			Tween ();
			if (atk == 0) {
				ATKLabel.text = "";
				return;
			}
			
			if ( skillID>=101 && skillID<=104 ) { //General RecoverHP Skill
				SkillBaseInfo sbi = DataCenter.Instance.Skill[skillID]; //(userUnitID, skillID, SkillType.NormalSkill);
				skillNameLabel.text = sbi.BaseInfo.name;
				ATKLabel.text = "HELH " + atk;
			} else {
				SkillBaseInfo sbi = DataCenter.Instance.GetSkill (userUnitID, skillID, SkillType.NormalSkill);
				if(sbi == null) {
					return;
				}
				skillNameLabel.text = sbi.BaseInfo.name;
				ATKLabel.text = "ATK " + atk;
			}
		});

	}

	public void ShowActiveSkill(Texture tex, string skillName, Callback cb) {
		callback = cb;
		CheckComponentInit ();
		skillNameLabel.text = skillName;
		ATKLabel.text = "";
		avatarTexture.mainTexture = tex;
		transform.localPosition = middlePosition;
		iTween.ScaleFrom (gameObject, iTween.Hash ("scale", new Vector3 (3f, 3f, 3f), "time", 0.5f, "easetype", iTween.EaseType.easeInOutQuart,"oncomplete","DropComplete","oncompletetarget",gameObject));
	}

	void CheckComponentInit() {
		if (backGroundSprite == null) {
			backGroundSprite = GetComponent<UISprite>();
			avatarTexture = transform.Find("Avatar").GetComponent<UITexture>();	
			skillNameLabel = transform.Find("SkillNameLabel").GetComponent<UILabel>();
			ATKLabel = transform.Find("ATKLabel").GetComponent<UILabel>();
			transform.localPosition = BattleCardArea.startPosition;
			moveEndPosition = new Vector3 (BattleCardArea.endPosition.x,  BattleCardArea.startPosition.y, BattleCardArea.endPosition.z - 10f);
			dropEndPosition = new Vector3 (moveEndPosition.x, BattleCardArea.endPosition.y, BattleCardArea.endPosition.z - 10f);
			middlePosition = new Vector3(BattleCardArea.middlePosition.x - backGroundSprite.width * 0.5f, BattleCardArea.middlePosition.y,BattleCardArea.middlePosition.z);
		}
	}
	
	private void Tween () {
		transform.localPosition = BattleCardArea.startPosition;
		iTween.MoveTo(gameObject,iTween.Hash("position",moveEndPosition,"time",0.35f,"easetype",iTween.EaseType.easeInQuart,"islocal",true,"oncomplete","MoveComplete","oncompletetarget",gameObject));
	}
	
	void MoveComplete() {
		iTween.RotateFrom (gameObject, iTween.Hash ("z", 10, "time", 0.15f, "easetype", iTween.EaseType.easeOutBounce, "oncomplete", "RotateComplete", "oncompletetarget", gameObject));
	}
	
	void RotateComplete() {
		iTween.MoveTo(gameObject,iTween.Hash("position",dropEndPosition,"time", 0.15f,"easetype",iTween.EaseType.easeOutQuart,"islocal",true,"oncomplete","DropComplete","oncompletetarget",gameObject));
	}
	
	void DropComplete() {
		transform.localPosition = ViewManager.HidePos;
		if (callback != null) {
			callback();	
			callback = null;
		}
	}
}
