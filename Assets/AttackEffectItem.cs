using UnityEngine;
using System.Collections;
using bbproto;

public class AttackEffectItem : MonoBehaviour {
	private UISprite backGroundSprite;
	private UISprite avatarTexture;
	private UILabel skillNameLabel;
	private UILabel ATKLabel;

	private Vector3 moveEndPosition = new Vector3(-317.5f,393.0f,0f);
	private Vector3 dropEndPosition = new Vector3(-317.5f,220.5f,0f);
	private Vector3 middlePosition = new Vector3(-314.0f,393.0f,0f);
	private Vector3 startPos = new Vector3 (377f, 393f, 0f);
	private Callback callback;

	public void RefreshInfo(string userUnitID, int skillID, Callback cb, int atk = 0, bool recoverHP = false) {
		if (backGroundSprite == null) {
			backGroundSprite = GetComponent<UISprite>();
			avatarTexture = transform.Find("Avatar").GetComponent<UISprite>();	
			skillNameLabel = transform.Find("SkillNameLabel").GetComponent<UILabel>();
			ATKLabel = transform.Find("ATKLabel").GetComponent<UILabel>();
			transform.localPosition = BattleManipulationView.startPosition;
//			moveEndPosition = new Vector3 (BattleManipulationView.endPosition.x - 10,  BattleManipulationView.startPosition.y, BattleManipulationView.endPosition.z - 10f);
//			dropEndPosition = new Vector3 (moveEndPosition.x, BattleManipulationView.endPosition.y, BattleManipulationView.endPosition.z - 10f);
//			middlePosition = new Vector3(BattleManipulationView.middlePosition.x - backGroundSprite.width * 0.5f, BattleManipulationView.middlePosition.y, BattleManipulationView.middlePosition.z);
		}

		callback = cb;

		UserUnit tuu = DataCenter.Instance.UnitData.UserUnitList.Get (userUnitID);
		if (tuu == null) {
//			Debug.LogError("userunit is null : " + userUnitID);	
			return;
		}
		backGroundSprite.spriteName = tuu.UnitType.ToString ();
		ResourceManager.Instance.GetAvatarAtlas (tuu.UnitInfo.id, avatarTexture, returnValue => {
//			BaseUnitItem.SetAvatarSprite (avatarTexture, returnValue, tuu.UnitInfo.ID);

			transform.localPosition = startPos;
			iTween.MoveTo(gameObject,iTween.Hash("position",moveEndPosition,"time",0.3f,"easetype",iTween.EaseType.easeInQuart,"islocal",true));
			iTween.RotateFrom (gameObject, iTween.Hash ("z", 10,"delay",0.3f, "time", 0.15f, "easetype", iTween.EaseType.easeOutBack,"oncomplete","DropComplete","oncompletetarget",gameObject));
//			iTween.MoveTo(gameObject,iTween.Hash("position",dropEndPosition,"delay",0.45f,"time", 0.2f,"easetype",iTween.EaseType.easeInOutQuart,));
				


			if (skillID >= 101 && skillID <= 104) { 						//General RecoverHP Skill
				SkillBase sbi = DataCenter.Instance.BattleData.Skill [skillID]; 	//(userUnitID, skillID, SkillType.NormalSkill);
				skillNameLabel.text =  TextCenter.GetText (SkillBase.SkillNamePrefix + skillID);//sbi.skillBase.name;
				ATKLabel.text = "HEAL " + atk;
				ATKLabel.gradientTop = new Color(0.51f,0.78f,0f);
				ATKLabel.gradientBottom = new Color(0.14f,0.64f,0.29f);
			} else {
				string id = DataCenter.Instance.BattleData.GetSkillID (userUnitID, skillID);
				SkillBase sbi = null;

				if (!DataCenter.Instance.BattleData.AllSkill.TryGetValue (id, out sbi)) {
						return;
				}
				skillNameLabel.text = TextCenter.GetText (SkillBase.SkillNamePrefix + skillID);
				ATKLabel.text = "ATK " + atk;
				ATKLabel.gradientTop = new Color(1f,0f,0f);
				ATKLabel.gradientBottom = new Color(0.6f,0f,0f);
			}
			if (atk == 0) {
				ATKLabel.text = "";
			}
		});
	}
	
	void DropComplete() {
//		transform.localPosition = ViewManager.HidePos;
		if (callback != null) {
			callback();	
			callback = null;
		}
	}
}
