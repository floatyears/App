using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.IO;

public class ResourceUpdate : MonoBehaviour {

	//private static string serverResURL = "file://" + Application.dataPath +"/ServerTest/";
	public const string serverResURL = "http://107.170.243.127:6001/resource/";

	public const string serverVersionURL = serverResURL + "version.txt";
	//private static string serverVersionURL = serverResURL + "version.txt";

	public static string localResPath = 
#if UNITY_IPHONE
	"file://" + Application.dataPath + "/ResourceDownloadTest/LocalTest";//"/Raw";
#elif UNITY_ANDROID
	"file://" + "jar:file://" + Application.dataPath + "!/assets/";
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
	"file://" + Application.dataPath + "/StreamingAssets/";
#else
	string.Empty;
#endif
	private static string localVersionPath = localResPath + "/version.txt";

//	private Dictionary<string, string[]> localVersionDic;

	private Dictionary<string, DownloadItemInfo> localVersionDic;
//	private Dictionary<string, string[]> serverVersionDic;
	private Dictionary<string, DownloadItemInfo> serverVersionDic;

	static private char[] encryptKey = new char[]{'D','ß','˚','ø','∑','…','å','O','~','©'};

	private int total = 0;

	private float current = 0;

	private int alreadyDone = 0;

	private bool isLoginSent = false;

	private bool isShowRetry = false;

	private WWW www = null;

	private UIProgressBar pro;
	private UILabel tipText;
	private UILabel proText;
	private WWW globalWWW;
	private bool startDown = false;

	private List<DownloadItemInfo> downLoadItemList = new List<DownloadItemInfo>();
	private List<DownloadItemInfo> retryItemList = new List<DownloadItemInfo>();


	// Use this for initialization
	void Start () {

		//get the gamemanager

		pro = GetComponent<UIProgressBar> ();
//		proText = GameObject.Find("ProgressText").GetComponent<UILabel> ();
		tipText = GameObject.Find ("TipText").GetComponent<UILabel>();
		InvokeRepeating ("ShowTipText", 0, 5);


		localVersionDic = new Dictionary<string, DownloadItemInfo> ();
		serverVersionDic = new Dictionary<string, DownloadItemInfo> ();

		//load the server version.txt
		//StartDownload ();
	}

	void Update(){
		if (downLoadItemList.Count > 0) {
			current = 0;
			for (int i = 0; i < downLoadItemList.Count; i++) {
				DownloadItemInfo item = downLoadItemList[i];
				WWW www = item.www;

				if(www != null && string.IsNullOrEmpty(www.error)){
					current += item.size * www.progress;
				}
				if(!string.IsNullOrEmpty(www.error)) {
					//Debug.LogError("retryItemList : " + item.path + " error : " + www.error);
					//retryItemList.Add(item);
					if(item.retryCount >0)
					{
						item.StartDownload();
						item.retryCount--;
					}
					continue;
				}
				if(www.isDone) {
					//TODO download done.
					UpdateLocalRes(item);
					alreadyDone += item.size;

				}
			}
		}
//		Debug.Log (globalWWW);
//		pro.value = 1 -  (total >0 ? (current+alreadyDone)/ (float)total: 1);
//		proText.text = "current: " + (1-pro.value)*100 + "%(total " + (float)total / (float)(1024*1024) + "M)";

	}

	void LateUpdate() {
		for (int i = downLoadItemList.Count - 1; i >= 0; i--) {
			DownloadItemInfo item = downLoadItemList[i];
			if(!string.IsNullOrEmpty( item.www.error) && item.retryCount <=0){
				downLoadItemList.Remove(item);
				retryItemList.Add(item);
			}else if(item.www.isDone) {
				downLoadItemList.Remove(item);
				item.Dispose();
			}
		}
		//Debug.Log ("download list item: " + downLoadItemList.Count);
		if (downLoadItemList.Count <= 0 && !isLoginSent && startDown) {
			if(retryItemList.Count > 0){
				if(!isShowRetry){
					isShowRetry = true;

					MsgWindowParams mwp = new MsgWindowParams ();
					mwp.btnParams = new BtnParam[2];
					
					mwp.titleText = "Dowload Error";
					mwp.contentText = "Would you like to retry";
					
					BtnParam sure = new BtnParam ();
					sure.callback = DownloadAgain;
					sure.text = "OK";
					mwp.btnParams[0] = sure;
					
					sure = new BtnParam ();
					sure.callback = ExitGame;
					sure.text = "Cancel";
					mwp.btnParams[1] = sure;
					MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, mwp);
				}

			}else {
				isLoginSent = true;
				SendMessageUpwards("Login",SendMessageOptions.DontRequireReceiver);
			}

		}
			
		//Debug.LogError ("LateUpdate : " + retryItemList.Count);
//		for (int i = 0; i < retryItemList.Count; i++) {
//			retryItemList[i].StartDownload();
//			downLoadItemList.Add(retryItemList[i]);
//		}
	}

	private void ShowTipText(){
		if (DataCenter.Instance.LoginInfo != null && DataCenter.Instance.LoginInfo.Data != null) {
			if (DataCenter.Instance.LoginInfo.Data.Rank < 5) {
				tipText.text = TextCenter.GetText ("Tips_A_" + MathHelper.RandomToInt (1, 13));
			} else if (DataCenter.Instance.LoginInfo.Data.Rank < 10) {
				tipText.text = TextCenter.GetText ("Tips_B_" + MathHelper.RandomToInt (1, 10));
			} else if (DataCenter.Instance.LoginInfo.Data.Rank < 20) {
				tipText.text = TextCenter.GetText ("Tips_C_" + MathHelper.RandomToInt (1, 18));
			} else if (DataCenter.Instance.LoginInfo.Data.Rank < 30) {
				tipText.text = TextCenter.GetText ("Tips_D_" + MathHelper.RandomToInt (1, 18));
			} else {
				tipText.text = TextCenter.GetText ("Tips_E_" + MathHelper.RandomToInt (1, 24));
			}	
		} else {
			tipText.text = TextCenter.GetText ("Tips_A_" + MathHelper.RandomToInt (1, 13));
		}     
	}

	private void DownloadAgain(object data){
		downLoadItemList.Clear ();
		total = 0;
		alreadyDone =  0;
		foreach (var item in retryItemList) {
			item.retryCount = 3;
			downLoadItemList.Add(item);
			total += item.size;
			item.StartDownload();

		}
		retryItemList.Clear ();
		isShowRetry = false;
	}


	private void ExitGame(object data){

	}
	public void StartDownload(){
		StartCoroutine (Download (serverVersionURL, delegate(WWW serverVersion) {
			Debug.Log("download serverVersion:"+serverVersion.text);
			LoadVersionConfig(serverVersion.text,serverVersionDic);
			
			//load the local version.txt. if not exists, jump through the init.
			StartCoroutine(Download(localVersionPath,delegate(WWW localVersion) {
				if(string.IsNullOrEmpty(localVersion.error))
				{
					LoadVersionConfig(localVersion.text,localVersionDic);
				}
				CompareVersion();
			}));
		}));
	}

	void LoadVersionConfig(string content,Dictionary<string,DownloadItemInfo> versionDic)
	{
		if (content == null || content.Length == 0) {
			return;
		}

		string[] records = content.Split (new string[]{"\n"},System.StringSplitOptions.RemoveEmptyEntries);
			foreach (string record in records) {
//				string[] items = record.Split('|');
			DownloadItemInfo item = DownloadItemInfo.ParseSting (record);
			versionDic.Add (item.name, item);
		}
//		
	}

	//generate the localVersionConfig.txt base the localVersionDic.
	void UpdateLocalVersionConfig(string name,bool wirteToFile)
	{
		Debug.Log ("version config save before");
		localVersionDic[name] = serverVersionDic[name];
		if (!wirteToFile)
			return;
		Debug.Log ("version config save");
		string verStr = "";
		foreach (var value in localVersionDic.Values) {
//			foreach(string item in value)
//			{
//				verStr += item + "|";
//			}
//			verStr = verStr.Substring(0,verStr.Length - 1);
//			verStr +="\n";
			verStr += value.ToString();
		}

		//File.WriteAllText (localVersionPath, verStr);

		//only for test
		Debug.Log ("local version path: " + localVersionPath);
		File.WriteAllText ("Assets/ResourceDownloadTest/LocalTest/version.txt", verStr);
	}

	void UpdateLocalRes(DownloadItemInfo downloadItem)
	{
		string hash = "";
		using(MD5 md5Hash = MD5.Create())
		{
			hash = GetMD5Hash(md5Hash,downloadItem.www.bytes);
		}

		//check the MD5, if not mamtch ,reload the file
		if (serverVersionDic [downloadItem.name].md5 == hash) {
			try{
				//Debug.Log(localResPath + "/" + serverVersionDic [name] [0] + ".unity3d");
				//File.WriteAllBytes (localResPath + "/" + serverVersionDic [name] [0] + ".unity3d", resBytes);
				//only for test
				File.WriteAllBytes ("Assets/ResourceDownloadTest/LocalTest/" + downloadItem.name + ".unity3d", downloadItem.www.bytes);
				UpdateLocalVersionConfig(downloadItem.name,true);

				current += serverVersionDic [downloadItem.name].size;
//				int.TryParse(serverVersionDic [name].size,out current);

				//StartCoroutine(LoadAssetToScene(resBytes));
			}catch(IOException e)
			{
				Debug.Log(e.Message+ ";"+e.StackTrace);
			}

		} else {
			Debug.Log("load res again");
//			LoadRes(serverVersionDic[name][1],name);
//			retryItemList.Add(downloadItem);
			if(downloadItem.retryCount >0)
			{
				downloadItem.StartDownload();
				downloadItem.retryCount--;
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

	void CompareVersion()
	{
		Debug.Log ("compare version");
		total = 0;
		downLoadItemList.Clear ();
		startDown = false;
		foreach (string name in serverVersionDic.Keys) {
			if(!localVersionDic.ContainsKey(name))
			{
				DownloadItemInfo serverItem = serverVersionDic[name];
				downLoadItemList.Add(serverItem);
				total += serverItem.size;
				serverItem.StartDownload();
				//load the resource
//				LoadRes(serverVersionDic[name][1],name);
//				int.TryParse(serverVersionDic[name][4],out total);
			}else
			{
				DownloadItemInfo localItem = localVersionDic[name];
				DownloadItemInfo serverItem = serverVersionDic[name];
				if(localItem.version < serverItem.version) {
					downLoadItemList.Add(serverItem);
					total += serverItem.size;
					serverItem.StartDownload();
				}
				else if(localItem.version == serverItem.version) {
					//skip
				}
				else{
					//throw data error;
				}

//				int oldVer = 0, newVer = 0;
//				int.TryParse(localVersionDic[name][3],out oldVer);
//				int.TryParse(serverVersionDic[name][3],out newVer);
//				if(oldVer < newVer)
//				{
//					LoadRes(serverVersionDic[name][1],name);
//					int.TryParse(serverVersionDic[name][4],out total);
//				}else if(oldVer == newVer)
//				{
//					Debug.Log("the " + name + " is updated!");
//				}else{
//					Debug.Log("something wrong is with the version.txt");
//				}
			}
		}
		startDown = true;

	}

	IEnumerator Download(string url, CompleteDownloadCallback callback)
	{

		WWW www = new WWW (url);
		globalWWW = www;
		Debug.Log (globalWWW);
		yield return www;


		if (!string.IsNullOrEmpty (www.error)) {
			Debug.Log(www.error+":" + url);
		}
		//call the callback nomatter what errors.
		if (callback != null) {
			callback(www);		
		}
		www.Dispose ();
		globalWWW = null;
	}

	void LoadRes(string url,string name){
		//Debug.Log ("load res: " + name + " url:" + serverResURL);
		StartCoroutine(Download(serverResURL + name + ".unity3d",delegate(WWW serverRes) {
			//StartCoroutine(Download(serverResURL + url,delegate(WWW serverRes) {

			if(!string.IsNullOrEmpty(serverRes.error))
			{
				Debug.Log("the res download has some err: " + serverRes.error);
				return;
			}
//			UpdateLocalRes(serverRes.bytes,name);
		}));
	}

	public delegate void CompleteDownloadCallback(WWW www);

	IEnumerator LoadAssetToScene(byte[] resBytes)
	{
		Debug.Log ("load asset to scene");
//		for (int i = 0; i < 10; i++) {
//			resBytes[i] = (byte)(resBytes[i] ^ (int)encryptKey[i]);
//		}
		Debug.Log ("before decryption:" + Time.realtimeSinceStartup);
		AssetBundleCreateRequest acr = AssetBundle.CreateFromMemory (resBytes);
		yield return acr;
		Debug.Log ("after decryption: " + Time.realtimeSinceStartup);
		AssetBundle bundle = acr.assetBundle;
		bundle.LoadAll ();
		Debug.Log (Time.realtimeSinceStartup);
		Debug.Log ("init instance");
		Instantiate (bundle.mainAsset);
	}
}

public class DownloadItemInfo {
//	public string serverPrePath = "";
	/// <summary>
	/// 0
	/// </summary>
	public string name;
	/// <summary>
	/// 1
	/// </summary>
	public string path;
	/// <summary>
	/// 2
	/// </summary>
	public string md5;
	/// <summary>
	/// 3
	/// </summary>
	public int version;
	/// <summary>
	/// 4
	/// </summary>
	public int size;

	public int retryCount = 3;

	public WWW www;

	public void StartDownload() {
		if (www != null) {
			www.Dispose();		
		}
		www = new WWW (ResourceUpdate.serverResURL + name + ".unity3d");
		Debug.Log ("url: " +ResourceUpdate.serverResURL + name + ".unity3d");
	}

	public static DownloadItemInfo ParseSting(string info) {
		DownloadItemInfo item = new DownloadItemInfo ();
//		string[] records = info.Split (new string[]{"\n"},System.StringSplitOptions.RemoveEmptyEntries);
//		foreach (string record in records) {
		string[] items = info.Split('|');
		item.name = items[0];
		item.path = items[1];
		item.md5 = items[2];
		item.version = System.Int32.Parse (items [3]);
		item.size = System.Int32.Parse(items[4]);
		return item;
//		}
	} 

	public override string ToString () {
		StringBuilder sb = new StringBuilder ();
		sb.Append (name);
		sb.Append("|");
		sb.Append (path);
		sb.Append("|");
		sb.Append (md5);
		sb.Append("|");
		sb.Append (version);
		sb.Append("|");
		sb.Append (size);
		sb.Append("\n");
		return sb.ToString ();
	}

	public void Dispose(){
		www.Dispose ();
		www = null;
	}
}
