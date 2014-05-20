using UnityEngine;
using System.Collections;

public class TipText : MonoBehaviour {

	public UILabel text;

	public UISprite background;

	//public Camera uiCamera;

	private Vector2 mSize;

	private Vector2 mPos;

	private Transform mTrans;

	private static TipText instance;

	private TipText(){}

	void Awake(){
		instance = this;
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetText (string tooltipText)
	{
		if (text != null && !string.IsNullOrEmpty(tooltipText))
		{
			if (text != null) text.text = tooltipText;
			
			// Orthographic camera positioning is trivial
			
			if (background != null)
			{
				Transform textTrans = text.transform;
				Vector3 offset = textTrans.localPosition;
				Vector3 textScale = textTrans.localScale;
				
				// Calculate the dimensions of the printed text
				mSize = text.printedSize;
				
				// Scale by the transform and adjust by the padding offset
				mSize.x *= textScale.x;
				mSize.y *= textScale.y;
				
				Vector4 border = background.border;

				mSize.x += border.x + border.z + ( offset.x - border.x) * 2f;
				mSize.y += border.y + border.w + (-offset.y - border.y) * 2f;

				LogHelper.Log("--------border: "+border+ "mSize: "+mSize);

				background.width = Mathf.RoundToInt(mSize.x+40);
				background.height = Mathf.RoundToInt(mSize.y+25);
			}
		}
	}
}
