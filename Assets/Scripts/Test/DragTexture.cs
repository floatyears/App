using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Collections;

public class DragTexture : MonoBehaviour {
	public enum EProbability {
		WHEqual,
		WidthBigger,
		HeightBigger,
	}

	private DragTexture instance;
	public DragTexture Instance {
		get {
			if(instance == null) {
				instance = this;
				if(instance == null) {
					instance = FindObjectOfType(typeof(DragTexture)) as DragTexture;
				}
			}

			return instance;
		}
	}

	public UITexture uiTexture;

	public float maxDelta = 2000f;

	public float scrollDelta = 20f;

	public float scaleBaseNumber = 0.02f;

	public float moveBaseNumber = 0.02f;

	private float probability = 1f;

	private string path = "";

	private EProbability probabilityState = EProbability.WHEqual;

	public Rect prevRect;

	private Vector2 scrollView = Vector2.zero;

	private List<string> filesName = new List<string>();

	private Dictionary<string, Texture2D> filesDic = new Dictionary<string, Texture2D>();

	private Dictionary<string, Rect> texConfig = new Dictionary<string, Rect>();

	private bool onlyDragState = true;

	private string switchState = "OnlyDrag";

	private float probabilityUITexture = 1f;
	
	void Awake() {
//		UIEventListener.Get (uiTexture.gameObject).onKey = OnKey;
		UIEventListener.Get (uiTexture.gameObject).onDrag = OnDragTexture;
		UIEventListener.Get (uiTexture.gameObject).onScroll = OnScrollTexture;
		prevRect = new Rect (0f, 0f, 1f, 1f);

		probabilityUITexture = (float)uiTexture.width / (float)uiTexture.height;
	}

	void Start() {
//		Texture tex = uiTexture.mainTexture;
//		SetTexture (tex);
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.Mouse0)) {
			LeftMouseClick();
		}

		if (Input.GetKeyDown (KeyCode.Mouse1)) {
			RightMouseClick();
		}

		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			LeftArrow();
		}

		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			RightArrow();
		}

		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			UpArrow();
		}

		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			DownArrow();
		}
	}

	void OnGUI() {
		GUILayout.BeginHorizontal ();

		GUILayout.BeginVertical (GUILayout.Width(200f),GUILayout.Height(400f));
		path = GUILayout.TextArea (path, GUILayout.Width (200f), GUILayout.Height (50f));
		GUILayout.Space (20f);
		exportPath = GUILayout.TextArea (exportPath, GUILayout.Width (240f), GUILayout.Height (50f));
		GUILayout.Space (20f);
		if(GUILayout.Button("加载目录下所有文件",GUILayout.Width(200f),GUILayout.Height(80f))) {
			LoadFiles();
		}
		GUILayout.Space (20f);
		if (GUILayout.Button ("使用上一个卡牌数据", GUILayout.Width(200f), GUILayout.Height(80f))) {
			uiTexture.uvRect = prevRect;		
		}
		GUILayout.Space (20f);
		if (GUILayout.Button ("保存数据", GUILayout.Width (200f), GUILayout.Height (80f))) {
			SaveToFile();
		}
		GUILayout.EndVertical();

		if (GUILayout.Button (switchState, GUILayout.Width (80f), GUILayout.Height (80f))) {
			OnlyDrag();
		}

		GUILayout.EndHorizontal ();
		scrollView = GUILayout.BeginScrollView(scrollView,GUILayout.Width(350f),GUILayout.Height(500f));
		for (int i = 0; i < filesName.Count; i++) {
			string fileName = filesName[i];
			if(GUILayout.Button(fileName)) {
				LoadTextureFromPath(fileName);
			}
		}
		GUILayout.EndScrollView();
	}
	
	void OnlyDrag() {
		if (onlyDragState) {
			onlyDragState = false;
			switchState = "OnlyScale";
		} else {
			onlyDragState = true;
			switchState = "OnlyDrag";
		}
	}

	void LoadTextureFromPath(string fileName) {
		string[] s = fileName.Split('/');
		string name = s[s.Length - 1];
		name = name.Replace (".png", "");
//		Debug.LogError ("LoadTextureFromPath : " + name);
		if (filesDic.ContainsKey (name)) {
			Texture2D tex = filesDic[name];
			SetTexture(tex, name);
			return;
		}

		StartCoroutine (DownLoadTexture (fileName, name));
	}

	IEnumerator DownLoadTexture(string path, string name) {
		WWW www = new WWW ("file://" + path);
		yield return www;
		if (www.isDone) {
			Texture2D tex = www.texture;
			filesDic.Add(name, tex);
			SetTexture(tex, name);
		}
	}


	public void SetTexture(Texture tex,string name) {
		uiTexture.mainTexture = tex;
		Rect rect;
		if (!texConfig.TryGetValue (name, out rect)) {
			rect = uiTexture.uvRect;

			if (tex.width >  tex.height) {
				probability = (float)tex.width / (float)tex.height;
				probabilityState = EProbability.WidthBigger;
				rect.width = 1f; //1f;//1f;
				rect.height = probability * probabilityUITexture;
			} else if (tex.height > tex.width) {
				probability = (float)tex.height / (float)tex.width;
				probabilityState = EProbability.HeightBigger;
				rect.width = probability;
				rect.height = 1f;
			} else {
				probabilityState = EProbability.WHEqual;
				rect.width = 1f;
				rect.height = probabilityUITexture;
				probability = 1f;
			}	
			texConfig.Add(name, rect);
		}
		uiTexture.uvRect = rect;
	}

	void LoadFiles() {
//		Debug.LogError (File.Exists (path));
//		if (!File.Exists (path)) {
//			return;	
//		}

		string[] files = Directory.GetFiles (path);
		filesName.Clear ();
		for (int i = 0; i < files.Length; i++) {
			string filePath = files[i];
			string[] filePaths = filePath.Split('.');
			if(filePaths[filePaths.Length - 1] == "png") {
				filesName.Add(filePath);
			}
		}
	}

	void LeftArrow() {
		if (!onlyDragState) {
			return;
		}

		Rect rect = uiTexture.uvRect;
		rect.x += moveBaseNumber;
		SetRect (rect);
	}
	
	void RightArrow() {
		if (!onlyDragState) {
			return;
		}

		Rect rect = uiTexture.uvRect;
		rect.x -= moveBaseNumber;
		SetRect (rect);
	}
	
	void UpArrow() {
		if (!onlyDragState) {
			return;
		}

		Rect rect = uiTexture.uvRect;
		rect.y -= moveBaseNumber;
		SetRect (rect);
	}
	
	void DownArrow() {
		if (!onlyDragState) {
			return;
		}

		Rect rect = uiTexture.uvRect;
		rect.y += moveBaseNumber;
		SetRect (rect);
	}

	void OnDragTexture(GameObject obj, 	Vector2 delta) {
		if (!onlyDragState) {
			return;
		}

		float xUVRect = delta.x / maxDelta;
		float yUVRect = delta.y / maxDelta;

		Rect rect = uiTexture.uvRect;

		float xOffect = rect.x - xUVRect;
		float yOffect = rect.y - yUVRect;

		SetRect (new Rect (xOffect, yOffect, rect.width, rect.height));
	}

	void OnScrollTexture(GameObject obj, float scrollValue) {
		if (onlyDragState) {
			return;
		}

		float deltaValue = scrollValue / scrollDelta;
		SetClickScaleRect (deltaValue);
	}

	private string exportPath = "/Users/leiliang/Desktop/TextureUVConfig.csv";
	private const string endNumber = "0.00";

	void SaveToFile() {
		try {
			File.Delete (exportPath);
		} catch (System.Exception ex) { }

		try {
			FileStream fs = new FileStream(exportPath,FileMode.Create,FileAccess.ReadWrite);
			StreamWriter sr = new StreamWriter(fs);
		
			foreach (var item in texConfig) {
				System.Text.StringBuilder sb = new System.Text.StringBuilder();
				string name = item.Key;
				Rect rect = item.Value;
				sb.Append(name);
				sb.Append(",");
				sb.Append(rect.x.ToString(endNumber));
				sb.Append(",");
				sb.Append(rect.y.ToString(endNumber));
				sb.Append(",");
				sb.Append(rect.width.ToString(endNumber));
				sb.Append(",");
				sb.Append(rect.height.ToString(endNumber));
				sr.WriteLine(sb.ToString());
			}

			sr.Close();
			sr.Dispose();
		} catch (System.Exception ex) {
			Debug.LogError(ex.Message);
		}
	}

	void SetRect(Rect rect) {
		prevRect = rect;
		uiTexture.uvRect = rect;
		Texture tex = uiTexture.mainTexture;
		foreach (var item in filesDic) {
			if (item.Value.Equals (tex)) {
				texConfig [item.Key] = rect;
			}
		}
	}

	void LeftMouseClick() {
		if (onlyDragState) {
			return;
		}

		if(CheckClickArea())
			SetClickScaleRect (-scaleBaseNumber);
	}

	void RightMouseClick() {
		if (onlyDragState) {
			return;
		}

		if(CheckClickArea())
			SetClickScaleRect (scaleBaseNumber);
	}
	RaycastHit rch ;
	bool CheckClickArea(){
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		bool b = Physics.Raycast (ray, out rch);
		if (b) {
			if(rch.collider.name == "Texture") {
				return true;
			} else {
				return false;
			}
		}

		return false;
	}

	void SetClickScaleRect (float baseNumber) {
		Rect rect = uiTexture.uvRect;
		if (probabilityState == EProbability.WHEqual) {
			rect.width += baseNumber;
			rect.height += baseNumber;
		} else if (probabilityState == EProbability.WidthBigger) {
			rect.width += baseNumber;
			rect.height +=  probability * baseNumber;
		} else if (probabilityState == EProbability.HeightBigger) {
			rect.width += probability * baseNumber;
			rect.height += baseNumber;
		}

		SetRect (rect);
	}
}
