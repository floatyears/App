using UnityEngine;
using System.Collections;

public class TipsLabelUI : MonoBehaviour {
	private UILabel showInfoLabel;
	private TweenScale tweenScale;
	private TweenAlpha tweenAlpha;
	private Transform panel;

	private Transform cacheParent;

	void Awake() {
		showInfoLabel = GetComponent<UILabel> ();
		tweenScale = GetComponent<TweenScale> ();
		tweenAlpha = GetComponent<TweenAlpha> ();
		panel = showInfoLabel.transform.parent.transform;
		cacheParent = panel.transform.parent;
	}

	void ChangeSceneComplete(object data) {
		AlphaEnd ();
	}

	public void ShowInfo(string text) {
		showInfoLabel.text = text;
		tweenScale.enabled = true;
		tweenScale.ResetToBeginning ();
	}

	public void ShowInfo(string text, GameObject target) {
		if(target != null)
			SetParent(target.transform);

		showInfoLabel.text = text;
		tweenScale.enabled = true;
		tweenScale.ResetToBeginning ();
	}

	public void ScaleEnd () {
		tweenScale.enabled = false;
		tweenAlpha.enabled = true;
		tweenAlpha.ResetToBeginning ();
	}

	public void AlphaEnd () {
		showInfoLabel.text = "";
		showInfoLabel.alpha = 1;
		showInfoLabel.transform.localScale = Vector3.zero;
		tweenAlpha.enabled = false;

		if (!cacheParent.Equals (panel.transform.parent)) {
			SetParent(cacheParent);
		}
	}
	void SetParent(Transform trans) {
		if (trans != null) {
			panel.transform.position = trans.position;		
		} else {
			panel.transform.localPosition = Vector3.zero;
		}
		panel.transform.localScale = Vector3.one;			
	}
}
