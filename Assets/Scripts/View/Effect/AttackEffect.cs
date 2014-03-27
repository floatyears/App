using UnityEngine;
using System.Collections.Generic;

public class AttackEffect : MonoBehaviour {
	private UITexture AvatarTexture;
	private UITexture backgroundTexture;
	private Vector3 moveEndPosition;
	private Vector3 dropEndPosition;

	public void RefreshItem (AttackInfo data) {
		if (backgroundTexture == null) {
			backgroundTexture = GetComponent<UITexture> ();
			AvatarTexture = transform.Find ("Avatar").GetComponent<UITexture> ();
			transform.localPosition = BattleCardArea.startPosition;
			moveEndPosition = new Vector3 (BattleCardArea.endPosition.x,  BattleCardArea.startPosition.y, BattleCardArea.endPosition.z - 10f);
			dropEndPosition = new Vector3 (moveEndPosition.x, BattleCardArea.endPosition.y, BattleCardArea.endPosition.z - 10f) ;
		}

		AttackInfo ai = data as AttackInfo;
		TUserUnit tuu = DataCenter.Instance.UserUnitList.Get (ai.UserUnitID);
//		Debug.LogError (tuu + "tuu : " + ai.UserUnitID);
		backgroundTexture.color = DGTools.TypeToColor (tuu.UnitInfo.Type);
		AvatarTexture.mainTexture =  tuu.UnitInfo.GetAsset (UnitAssetType.Avatar);
		Tween ();
	}

	private void Tween () {
//		Debug.LogError ("tween : " + Time.realtimeSinceStartup);
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
	}
}
