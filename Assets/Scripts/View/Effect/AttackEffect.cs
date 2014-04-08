using UnityEngine;
using System.Collections.Generic;
using bbproto;

public class AttackEffect : MonoBehaviour {
	private UITexture AvatarTexture;
	private UISprite backgroundTexture;
	private Vector3 moveEndPosition;
	private Vector3 dropEndPosition;

	private Dictionary<EUnitType, UITexture> attackBack = new Dictionary<EUnitType, UITexture> ();

	public void RefreshItem (AttackInfo data) {
//		Debug.LogWarning ("RefreshItem stopinput false " + Time.realtimeSinceStartup);
		MsgCenter.Instance.Invoke (CommandEnum.StopInput, false);
		if (backgroundTexture == null) {
			backgroundTexture = GetComponent<UISprite> ();
			AvatarTexture = transform.Find ("Avatar").GetComponent<UITexture> ();
			transform.localPosition = BattleCardArea.startPosition;
			moveEndPosition = new Vector3 (BattleCardArea.endPosition.x,  BattleCardArea.startPosition.y, BattleCardArea.endPosition.z - 10f);
			dropEndPosition = new Vector3 (moveEndPosition.x, BattleCardArea.endPosition.y, BattleCardArea.endPosition.z - 10f) ;
		}

		AttackInfo ai = data as AttackInfo;
		TUserUnit tuu = DataCenter.Instance.UserUnitList.Get (ai.UserUnitID);
		backgroundTexture.spriteName = tuu.UnitType.ToString ();
		AvatarTexture.mainTexture =  tuu.UnitInfo.GetAsset (UnitAssetType.Avatar);
		Tween ();
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
//		Debug.LogWarning ("RefreshItem stopinput true " + Time.realtimeSinceStartup);
		MsgCenter.Instance.Invoke (CommandEnum.StopInput, true);
	}
}
