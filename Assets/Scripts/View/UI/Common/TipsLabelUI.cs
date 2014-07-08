using UnityEngine;
using System.Collections;

public class TipsLabelUI : MonoBehaviour {
	private UILabel showInfoLabel;
	private TweenScale tweenScale;
	private TweenAlpha tweenAlpha;
	private UIPanel panel;

	private Transform cacheParent;

	void Awake() {
		showInfoLabel = GetComponent<UILabel> ();
		tweenScale = GetComponent<TweenScale> ();
		tweenAlpha = GetComponent<TweenAlpha> ();
		panel = showInfoLabel.transform.parent.GetComponent<UIPanel> ();
		cacheParent = panel.transform.parent;
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
		Debug.LogError ("SetParent : " + panel);
		panel.transform.parent = trans;		
		panel.transform.localPosition = Vector3.zero;
		panel.transform.localScale = Vector3.one;
	}
}
