using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class ExportResource : EditorWindow {

	private enum PLATFORM_OP
		{
			iOS = 0,
			Android = 1
		}

	private enum PACK_TYPE
		{
			FOLDER = 0,
			FILE = 1
		} 

	private enum FILE_TYPE
		{
			//ASSET = "asset",
			UNITY3D = 0
			//UNITY = "unity"
		}

	static private string savePath = "";

	static private string fileName = "*";

	static private bool toggleMD5 = true;

	static private bool collectDependency = true;

	static private System.Enum op = PLATFORM_OP.iOS;

	static private System.Enum pt = PACK_TYPE.FOLDER;

	static private System.Enum ft = FILE_TYPE.UNITY3D;

	static private BuildTarget tgtPlatform = BuildTarget.iPhone;

	static private string fileEx = "unity3d";

	static private string exportConfigPath = "exportResourceConfig.txt";

	static private string exportConfigContent = "";

	static private string versionConfigPath = "/version.txt";

	//static private string versionConfigContent = "";

	static private string projPath;

	static private Dictionary<string,string> versionConfigDic;

	static private string[] sepStr = new string[]{"|"};
	static private string sepString = "|";

	static private string objStr = "";

	static private GUIStyle guiStyle = new GUIStyle();

	static private char[] encryptKey = new char[]{'D','ß','˚','ø','∑','…','å','O','~','©'};

	//static private char[] sepStrChars = {'#','*','#'};

	void OnGUI()
	{

		//InitExportConfig ();
		op = EditorGUILayout.EnumPopup ("Target Platform:", op);

	//	pt = EditorGUILayout.EnumPopup ("Resource Type:", pt);

		GUI.Label (new Rect (8, 30, 80, 30), "objects: ");


		if (Selection.activeObject != null && Selection.activeObject.name != null) {
			guiStyle.normal.textColor = Color.green;
			objStr = Selection.activeObject.name;		
		}else{
			guiStyle.normal.textColor = Color.red;
			objStr = "no object was selected!";
		}
		GUI.Label (new Rect (100, 30, 200, 30),objStr ,guiStyle);

		toggleMD5 = GUI.Toggle (new Rect (8, 50, 100, 20), toggleMD5, "need MD5");

		GUI.Label (new Rect (10, 70, 70, 30), "Save Path:");
		GUI.TextField (new Rect (70, 70, 120, 20), savePath);

		if (GUI.Button(new Rect(190, 70, 55, 20),"change"))
		{
			//Debug.Log("text field clicked");
			SelectFolder();

		}

		GUI.Label (new Rect (10, 95, 70, 20), "Asset Name:");

		fileName = GUI.TextField (new Rect (90, 95, 80, 20), fileName);

		GUI.Label (new Rect (170, 100, 10, 20), ".");

		ft = EditorGUI.EnumPopup (new Rect (180, 98, 65, 30), ft);

		collectDependency = GUI.Toggle (new Rect (100, 50, 110, 30),collectDependency,"Collect Dependency");

		if (GUI.Button (new Rect (10, 120, 120, 30), "Export Resource")) 
		{
			ExportResrouce();
		}

		//EditorGUIUtility.LookLikeControls ();
	}

	[MenuItem("Asserts/ShowWindow")]
	static void Init()
	{
		//Debug.Log ((int)'a');
		savePath = Application.dataPath;

		projPath = Application.dataPath.Substring(0,Application.dataPath.LastIndexOf ('/') + 1);

		//read export config file .if not exits, create one.
		InitExportConfig ();

		//get the versionConfig data from file.
		InitVersionConfig ();

		EditorWindow window = EditorWindow.GetWindowWithRect (typeof(ExportResource),new Rect(100,100,250,160),true,"Export Resource");
		window.Show ();
	}

	//[MenuItem("Asserts/Build AssertBundle From Selection - Track dependencies")]
	void ExportResrouce()
	{
		//save config
		SaveExportConfig ();

		//select platform
		if (pt.Equals(PLATFORM_OP.iOS))
			tgtPlatform = BuildTarget.iPhone;
		else if (pt.Equals(PLATFORM_OP.Android))
			tgtPlatform = BuildTarget.Android;

		//file extension...temporarily only unity3d..maybe there will be others later.
		if (ft.Equals(FILE_TYPE.UNITY3D)) {
			fileEx = "unity3d";
		}

		if (string.IsNullOrEmpty (savePath)) {
			Debug.Log("the savePath is not valid, please reopon the window!");
			return;
		}

		//assemble path //AA;   
	//	Application.dataPath + "/AssetBundles/" + path;
		string path = savePath + "/"+ fileName + "." + fileEx;

		//Debug.Log ("asset path:"+path);

		if (path.Length != 0) {

			//check if any objects was selected
			if(!Selection.activeObject)
			{
				Debug.Log("no object was selected, please select a object!");
				return;
			}

			Debug.Log("collect dependency:" + collectDependency);
			if(collectDependency)
			{
				Object[] selection = Selection.GetFiltered(typeof(Object),SelectionMode.DeepAssets);
				BuildPipeline.BuildAssetBundle(Selection.activeObject,selection,path,BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets ,tgtPlatform);
				Selection.objects = selection;
			}
			else{
				BuildPipeline.BuildAssetBundle(Selection.activeObject,Selection.objects,path,BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets ,tgtPlatform);
			}

			FileStream fs = new FileStream(path,FileMode.Open,FileAccess.Read);
			BinaryReader br = new BinaryReader(fs);

			byte[] tmpall = br.ReadBytes((int)fs.Length);
			br.Close();
			fs.Close();
			string hash = "";

			//encryoted
//			for (int i = 0; i < 10; i++) {
//				tmpall[i] = (byte)(tmpall[i] ^ (int)encryptKey[i]);
//			}

			using(MD5 md5Hash = MD5.Create())
			{
				hash = GetMD5Hash(md5Hash,tmpall);
			}

			File.WriteAllBytes(path,tmpall);


			Debug.Log("btye length: " + tmpall.Length);
			//if the record exists, increate the version
			if(versionConfigDic.ContainsKey(fileName))
			{
				string version = GetIncreamentVersion(versionConfigDic[fileName]);
				versionConfigDic[fileName] = fileName + sepString + path + sepString + hash + sepString + version + sepString + tmpall.Length;
			}else
			{
				versionConfigDic[fileName] = fileName + sepString + path + sepString + hash + sepString + "0" + sepString + tmpall.Length;
			}

			SaveVersionConfig();


		}
	}

	//[MenuItem("Asserts/BuildScene")]
	static void BuildScene()
	{

	}

	void SelectFolder()
	{
		string fullPath = EditorUtility.SaveFolderPanel ("Save Path", "", "");
		if (string.IsNullOrEmpty (fullPath)) {
			Debug.Log("the select folder is not valid.");
			return;		
		}
		int index = fullPath.IndexOf (projPath);
		if (index < 0) {
			Debug.Log("the path is valid, please reselect it!");
		} else {
			savePath = fullPath.Substring(projPath.Length);
			Debug.Log(index + savePath);
		}

	}

	static void InitExportConfig()
	{
		if (!File.Exists (exportConfigPath)) {
			exportConfigContent = "SavePath:" + savePath + ";Filename:"+fileName+";Extension:"+ fileEx;
			using(StreamWriter sw = File.CreateText( projPath + exportConfigPath))
			{
				Debug.Log(exportConfigContent);
				sw.WriteLine(exportConfigContent);
			}
		}
		
		using(StreamReader sr = File.OpenText(projPath + exportConfigPath))
		{
			string content = sr.ReadLine();
			//Debug.Log(content);
			if(content == null)
			{
				File.Delete(projPath + exportConfigPath);
				InitExportConfig(); 
			}else
			{
				string[] cc = content.Split(new char[]{';'},System.StringSplitOptions.RemoveEmptyEntries);	
				
				savePath = cc[0].Split(':')[1];
				fileName = cc[1].Split(':')[1];
			}

		}
	}

	static void SaveExportConfig()
	{
		if(string.IsNullOrEmpty(savePath)){
			Debug.Log("the path is not valid!please reselect it or reopen the window!");
			return;
		}

		exportConfigContent = "SavePath:"+savePath+";Filename:"+fileName+";Extension:"+fileEx;

		FileStream fs = new FileStream (projPath + exportConfigPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
		BinaryWriter bw = new BinaryWriter (fs); 
		bw.Write (exportConfigContent);
		bw.Flush ();
		fs.Close ();
	}

	static void InitVersionConfig()
	{
		if (versionConfigDic == null) {
			versionConfigDic = new Dictionary<string, string>();		
		}
		versionConfigDic.Clear ();
		if (!File.Exists (savePath + versionConfigPath)) {
			if(savePath.IndexOf(Application.dataPath) < 0)
			{
				Debug.Log("save path is not valid");
				return;
			}
				
			try{
				File.Create(savePath + versionConfigPath);
			}catch(IOException e)
			{
				Debug.Log(e.Message); 
			}

			return;
		}
		using (StreamReader sr = File.OpenText(savePath + versionConfigPath)) {


			while(true)
			{
				string record = sr.ReadLine();
				//Debug.Log("record:" +record);
				if(record == null)
					break;
				versionConfigDic[GetPath(record)] = record;

			}
		}
	}

	static void SaveVersionConfig()
	{
		Debug.Log ("save config");
		if (File.Exists (savePath + versionConfigPath)) {
			File.Delete(savePath + versionConfigPath);
		}

		using (StreamWriter sw = File.CreateText(savePath + versionConfigPath)) {
			foreach (string record in versionConfigDic.Values) 
			{
				sw.WriteLine(record);
			}
		}
	}

	static string GetMD5Hash(MD5 md5Hash, byte[] bytes)
	{
		byte[] data = md5Hash.ComputeHash (bytes);
		StringBuilder sb = new StringBuilder ();
		// Loop through each byte of the hashed data  
		// and format each one as a hexadecimal string. 
		for (int i = 0; i < data.Length; i++)
			sb.Append (data [i].ToString ("x2"));
		return sb.ToString ();
	}

	static string GetIncreamentVersion(string record)
	{
		int version = 0;

		string[] strs = record.Split (sepStr,System.StringSplitOptions.RemoveEmptyEntries);
		for (int i = 0; i < strs.Length; i++) {
			Debug.Log("version index:" + i);
			Debug.Log ("version string:" + strs[i]);	
		}

		int.TryParse(record.Split (sepStr,System.StringSplitOptions.RemoveEmptyEntries)[3],out version);
		Debug.Log ("version:" + version);
		version = version + 1;
		return version.ToString();
	}

	static string GetPath(string record)
	{
		string fulleName = record.Split (sepStr,System.StringSplitOptions.RemoveEmptyEntries)[0];
		//fulleName.Substring(0,fulleName.LastIndexOf("."));
		return fulleName;
	}

}
