using UnityEngine;
using System.Collections;
using bbproto;

public class AttackEffectItem : MonoBehaviour {
	private UISprite backGroundSprite;
	private UISprite avatarTexture;
	private UILabel skillNameLabel;
	private UILabel ATKLabel;

	private Vector3 moveEndPosition;
	private Vector3 dropEndPosition;
	private Vector3 middlePosition;
	private Callback callback;

	public void RefreshInfo(string userUnitID, int skillID, Callback cb, int atk = 0, bool recoverHP = false) {
		CheckComponentInit ();

		callback = cb;

		UserUnit tuu = DataCenter.Instance.UnitData.UserUnitList.Get (userUnitID);
		if (tuu == null) {
//			Debug.LogError("userunit is null : " + userUnitID);	
			return;
		}
		backGroundSprite.spriteName = tuu.UnitType.ToString ();
		ResourceManager.Instance.GetAvatarAtlas (tuu.UnitInfo.id, avatarTexture, returnValue => {
//			BaseUnitItem.SetAvatarSprite (avatarTexture, returnValue, tuu.UnitInfo.ID);

			transform.localPosition = BattleManipulationView.startPosition;
			iTween.MoveTo(gameObject,iTween.Hash("position",moveEndPosition,"time",0.3f,"easetype",iTween.EaseType.easeInQuart,"islocal",true));
			iTween.RotateFrom (gameObject, iTween.Hash ("z", 10,"delay",0.3f, "time", 0.15f, "easetype", iTween.EaseType.easeOutBack,"oncomplete","DropComplete","oncompletetarget",gameObject));
//			iTween.MoveTo(gameObject,iTween.Hash("position",dropEndPosition,"delay",0.45f,"time", 0.2f,"easetype",iTween.EaseType.easeInOutQuart,));
				
			if (atk == 0) {
				ATKLabel.text = "";
				return;
			}

			if (skillID >= 101 && skillID <= 104) { 						//General RecoverHP Skill
				SkillBase sbi = DataCenter.Instance.BattleData.Skill [skillID]; 	//(userUnitID, skillID, SkillType.NormalSkill);
				skillNameLabel.text =  TextCenter.GetText (SkillBase.SkillNamePrefix + skillID);//sbi.skillBase.name;
				ATKLabel.text = "HEAL " + atk;
			} else {
				string id = DataCenter.Instance.BattleData.GetSkillID (userUnitID, skillID);
				SkillBase sbi = null;

				if (!DataCenter.Instance.BattleData.AllSkill.TryGetValue (id, out sbi)) {
						return;
				}
				skillNameLabel.text = TextCenter.GetText (SkillBase.SkillNamePrefix + skillID);
				ATKLabel.text = "ATK " + atk;
			}
		});
	}

	public void ShowActiveSkill(string skillName, Callback cb) {
		callback = cb;
		CheckComponentInit ();
		skillNameLabel.text = skillName;
		ATKLabel.text = "";
//		avatarTexture.mainTexture = tex;
		transform.localPosition = middlePosition;
		iTween.ScaleFrom (gameObject, iTween.Hash ("scale", new Vector3 (3f, 3f, 3f), "time", 0.5f, "easetype", iTween.EaseType.easeInOutQuart, "oncomplete", "DropComplete", "oncompletetarget", gameObject));
	}

	void CheckComponentInit() {
		if (backGroundSprite == null) {
			backGroundSprite = GetComponent<UISprite>();
			avatarTexture = transform.Find("Avatar").GetComponent<UISprite>();	
			skillNameLabel = transform.Find("SkillNameLabel").GetComponent<UILabel>();
			ATKLabel = transform.Find("ATKLabel").GetComponent<UILabel>();
			transform.localPosition = BattleManipulationView.startPosition;
			moveEndPosition = new Vector3 (BattleManipulationView.endPosition.x,  BattleManipulationView.startPosition.y, BattleManipulationView.endPosition.z - 10f);
			dropEndPosition = new Vector3 (moveEndPosition.x, BattleManipulationView.endPosition.y, BattleManipulationView.endPosition.z - 10f);
			middlePosition = new Vector3(BattleManipulationView.middlePosition.x - backGroundSprite.width * 0.5f, BattleManipulationView.middlePosition.y, BattleManipulationView.middlePosition.z);
		}
	}
	
	void DropComplete() {
//		transform.localPosition = ViewManager.HidePos;
		if (callback != null) {
			callback();	
			callback = null;
		}
	}
}
