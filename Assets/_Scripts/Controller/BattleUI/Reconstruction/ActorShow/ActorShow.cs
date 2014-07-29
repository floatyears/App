using UnityEngine;
using System.Collections.Generic;

public class ActorShow : UIBaseUnity{

	private Vector3[] initPos = new Vector3[3] {new Vector3(-600f,0f,0f), new Vector3(0f,0f,0f), new Vector3(600f,0f,0f)};

	private UITexture showTexture;
	private UITexture freeTexture;

	private Transform prevButton;
	private Transform nextButton;

	private Dictionary<int,string> textureConfig = new Dictionary<int, string> ();

	private int currentID = -1;
	private static bool isInit =false;

	public override void Init (string name){
		if(isInit)
			return;
		isInit = true;
		base.Init (name);
		transform.parent = vManager.BottomPanel.transform;
		showTexture = FindChild<UITexture> ("Texture0");
		freeTexture = FindChild<UITexture> ("Texture1");
		showTexture.enabled = false;
		prevButton = FindChild<Transform>("PrevButton");
		nextButton = FindChild<Transform>("NextButton");

		UIEventListener.Get (prevButton.gameObject).onClick = PrevTexture;
		UIEventListener.Get (nextButton.gameObject).onClick = NextTexture;

		ResourceManager.Instance.LoadLocalAsset ("Config/Config", CallbackFunc);

	}

	private void CallbackFunc(object o){
		Object obj = o as Object;
		string info = (obj as TextAsset).text;
		string[] listInfo = info.Split ('#');
		
		for (int i = 1; i < listInfo.Length; i++) {
			if(string.IsNullOrEmpty(listInfo[i]))
				continue;
			
			string[] data = listInfo[i].Split('|');
			int id = System.Int32.Parse(data[0].Trim());
			textureConfig.Add(id,data[1].Trim());
		}}

	public override void ShowUI (){
		base.ShowUI ();
		gameObject.SetActive (true);

	}

	public override void CreatUI (){
		base.CreatUI ();
		Destroy (gameObject);
	}

	public override void HideUI (){
		base.HideUI ();
		gameObject.SetActive (false);
		currentID = -1;

	}

	public void ShowTextureID(int id = 1){
		if(currentID == -1)
			currentID = id;
		if (!showTexture.enabled) {
			showTexture.enabled = true;
		}

		GetTexture (id);
	}

	void GetTexture(int id)
	{
		string path = "Actor/" + textureConfig [id];
		ResourceManager.Instance.LoadLocalAsset (path,CallbackFunc1);
	}

	private void CallbackFunc1(object o1){
		Object obj = o1  as Object;
		Texture2D tex = obj as Texture2D;
		showTexture.mainTexture = tex;
		showTexture.width = tex.width;
		showTexture.height =tex.height;
	}

	UITexture temp;

	void PrevTexture(GameObject caller)
	{
		currentID --;

		if (currentID < 1) {
			currentID = textureConfig.Count;	
		}

		IMoveTo (showTexture,initPos[1], initPos[2]);

		IMoveTo (freeTexture, initPos[0],initPos[1]);
		ChangeTexture ();

		ShowTextureID (currentID);
	}

	void NextTexture(GameObject caller)
	{
		currentID ++;

		if (currentID > textureConfig.Count) {
			currentID = 1;	
		}
		IMoveTo (showTexture,initPos[1], initPos[0]);
		IMoveTo (freeTexture, initPos[2],initPos[1]);
		ChangeTexture ();

		ShowTextureID (currentID);
	}

	void ChangeTexture()
	{
		temp = showTexture;
		showTexture = freeTexture;
		freeTexture = temp;
	}

	void MoveTo(UITexture target,Vector3 from,Vector3 to)
	{
		TweenPosition tp = target.GetComponent<TweenPosition> ();
		tp.enabled = true;
		tp.ResetToBeginning();
		tp.transform.localPosition = from;
		tp.from = from;
		tp.to = to;

	}

	void IMoveTo(UITexture target,Vector3 from,Vector3 to)
	{
		target.transform.localPosition = from;
		iTween.MoveTo (target.gameObject,iTween.Hash("position",to,"time",0.5f));//,"easetype","easeOutBounce"));
	}
}





