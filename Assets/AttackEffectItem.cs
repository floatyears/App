using UnityEngine;
using System.Collections;

public class AttackEffectItem : MonoBehaviour {
	private UISprite backGroundSprite;
	private UITexture avatarTexture;
	private Vector3 moveEndPosition;
	private Vector3 dropEndPosition;

	private Callback callback;

	public void RefreshInfo(AttackInfo ai, Callback cb) {
		if (backGroundSprite == null) {
			backGroundSprite = GetComponent<UISprite>();
			avatarTexture = transform.Find("Avatar").GetComponent<UITexture>();		
			transform.localPosition = BattleCardArea.startPosition;
			moveEndPosition = new Vector3 (BattleCardArea.endPosition.x,  BattleCardArea.startPosition.y, BattleCardArea.endPosition.z - 10f);
			dropEndPosition = new Vector3 (moveEndPosition.x, BattleCardArea.endPosition.y, BattleCardArea.endPosition.z - 10f) ;
		}
		callback = cb;
		TUserUnit tuu = DataCenter.Instance.UserUnitList.Get (ai.UserUnitID);
		backGroundSprite.spriteName = tuu.UnitType.ToString ();
		avatarTexture.mainTexture =  tuu.UnitInfo.GetAsset (UnitAssetType.Avatar);
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
		if (callback != null) {
			callback();	
			callback = null;
		}
	}
}
