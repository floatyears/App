using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.IO;

public class ResourceUpdate : MonoBehaviour {

	//private static string serverResURL = "file://" + Application.dataPath +"/ServerTest/";
	public static string serverHost = ServerConfig.ResourceHost;

	public static string serverResURL =
#if UNITY_EDITOR || UNITY_STANDALONE
		serverHost+"/android/";
#elif UNITY_ANDROID
	serverHost+"/android/";
#elif UNITY_IOS
	serverHost+"/ios/";
#endif

	public static string serverVersionURL = serverResURL + "version.txt";
	//private static string serverVersionURL = serverResURL + "version.txt";

	public static string localResPath = 
#if UNITY_EDITOR
		"file:///Users/Resource/";
#elif UNITY_IOS
	"file://" + Application.persistentDataPath + "/";
#elif UNITY_ANDROID
	"file:///" + Application.persistentDataPath + "/assets/";
#else
	string.Empty;
#endif

	public static string localResFullPath = 

#if UNITY_EDITOR
		"/Users/Resource/";
#elif UNITY_IOS
	Application.persistentDataPath + "/";
#elif UNITY_ANDROID
	Application.persistentDataPath + "/assets/";
#else
	string.Empty;
#endif

//	private static string localVersionPath = localResPath + "version.txt";

//	private Dictionary<string, string[]> localVersionDic;

	private Dictionary<string, DownloadItemInfo> localVersionDic;
//	private Dictionary<string, string[]> serverVersionDic;
	private Dictionary<string, DownloadItemInfo> serverVersionDic;

//	static private char[] encryptKey = new char[]{'D','ß','˚','ø','∑','…','å','O','~','©'};

	private int total = 0;

	private float current = 0;

	private int alreadyDone = 0;

	private bool isLoginSent = false;

	private bool isShowRetry = false;

	private bool downloadLimit = true;

	private WWW www = null;

	private UIProgressBar pro;
	private UILabel tipText;
	private UILabel proText;

	private UISprite bg;
	private UISprite fg;

	private UILabel versionTxt;
	private string version;

	private WWW globalWWW;
	private bool startDown = false;

	private Queue<DownloadItemInfo> downLoadItemList = new Queue<DownloadItemInfo>();
	private Queue<DownloadItemInfo> retryItemList = new Queue<DownloadItemInfo>();


	private string appVersion = 
#if LANGUAGE_CN
	"版本：";
#elif LANGUAGE_EN
	"AppVersion:";
#else
	"";
#endif

	private string currentDownload = 
#if LANGUAGE_CN
	"已下载：";
#elif LANGUAGE_EN
	"Downloaded: ";
#else
	"";
#endif

	private string totalDownload = 
#if LANGUAGE_CN
	"总大小：";
#elif LANGUAGE_EN
	"Total: ";
#else
	"";
#endif

	// Use this for initialization
	void Start () {

		//get the gamemanager
//		Debug.Log ("start download=============");
		pro = GetComponent<UIProgressBar> ();
		proText = GameObject.Find("ProgressText").GetComponent<UILabel> ();
		tipText = GameObject.Find ("TipText").GetComponent<UILabel>();

		bg = GameObject.Find ("LoadProgress/Background").GetComponent<UISprite>();
		fg = GameObject.Find ("Foreground").GetComponent<UISprite>();

		if (GameObject.Find ("Version") != null) {
			versionTxt = GameObject.Find ("Version").GetComponent<UILabel> ();
			versionTxt.enabled = false;	
		}
			

		InvokeRepeating ("ShowTipText", 0, 5);

		pro.enabled = false;
		proText.enabled = false;
		tipText.enabled = false;
		bg.enabled = false;
		fg.enabled = false;


		localVersionDic = new Dictionary<string, DownloadItemInfo> ();
		serverVersionDic = new Dictionary<string, DownloadItemInfo> ();

//			List<TStageInfo> stages = DataCenter.Instance.GetCityInfo(1).Stages;
//			List<TQuestInfo> quests = stages[stages.Count - 1].QuestInfo;
//			Debug.Log("quest: " + DataCenter.Instance.QuestClearInfo.GetStoryCityState(2));
//			if (DataCenter.Instance.QuestClearInfo.GetStoryCityState(2) == StageState.CLEAR && (DataCenter.Instance.QuestClearInfo.GetStoryCityState(2) == StageState.CLEAR)) {
				//load the server version.txt
		StartDownload ();
		
		if (!Directory.Exists (localResFullPath)) {
			Directory.CreateDirectory (localResFullPath);
			Debug.Log ("create path: " + localResFullPath);
		}
				
//			} 
//	else {
//				SendMessageUpwards("CouldLogin",SendMessageOptions.DontRequireReceiver);	
//			}
//		}

	}

	void Update(){
		if (downLoadItemList.Count > 0) {
			current = 0;
//			for (int i = 0; i < downLoadItemList.Count; i++) {
			DownloadItemInfo item = downLoadItemList.Peek();//downLoadItemList[i];
				WWW www = item.www;
				if(www != null && string.IsNullOrEmpty(www.error)){
					current += item.size * www.progress;
//					Debug.Log("download current: " + www.progress);
				}
//				if(!string.IsNullOrEmpty(www.error)) {
//					//Debug.LogError("retryItemList : " + item.path + " error : " + www.error);
//					//retryItemList.Add(item);
////					if(item.retryCount >0)
////					{
//						item.StartDownload();
////						item.retryCount--;
////					}
//					continue;
//				}
				if(www.isDone && string.IsNullOrEmpty(www.error)) {
					//TODO download done.
					Debug.LogWarning("downloadListItem: " + downLoadItemList.Count+"). www.isDone:"+www.url);
					UpdateLocalRes(item);
					alreadyDone += item.size;
				}
//			}
		}

//		Debug.Log (globalWWW);
//		Debug.Log ("============progress1: " + current + " already: " + alreadyDone);
		if (total > 0) {
			pro.value = (total >0 ? (current+alreadyDone)/ (float)total: 1);
			//		Debug.Log ("============progress2: " + pro.value);
			
			proText.text = currentDownload + (pro.value*100).ToString("F2") + "%(" + totalDownload + ((float)total / (float)(1024*1024)).ToString("F2") + "MB)";
		}

//		if (versionTxt != null) {
//			versionTxt.text = appVersion + version;
//			versionTxt.enabled = true;
//		}
			

		//#if INNER_TEST
		//		versionTxt.text = "download.count:"+downLoadItemList.Count+" retry.count:"+retryItemList.Count;
		//#endif
	}

	void LateUpdate() {
//		for (int i = downLoadItemList.Count - 1; i >= 0; i--) {
		if (downLoadItemList.Count > 0){
		
			DownloadItemInfo item = downLoadItemList.Peek ();//downLoadItemList[i];
			if(!string.IsNullOrEmpty( item.www.error) /*&& item.retryCount <=0*/){
				Debug.Log("download.count: " + downLoadItemList.Count + " => download error: "+item.www.error + " url:"+item.www.url);

	//			downLoadItemList.Dequeue();//downLoadItemList.Remove(item);
				DownloadItemInfo item2 = downLoadItemList.Dequeue();//downLoadItemList.Remove(item);
				item2.Dispose();
				if(downLoadItemList.Count > 0){
					downLoadItemList.Peek().StartDownload();
				}
				retryItemList.Enqueue(item);//retryItemList.Add(item);
				Umeng.GA.Event("DownloadError",new Dictionary<string,string>(){{"downloaded" , alreadyDone + "bytes"},{"err", item.www.error},{"count","count: " + downLoadItemList.Count+")"},{"device",SystemInfo.deviceUniqueIdentifier}});

			}else if(item.www.isDone) {
				DownloadItemInfo item1 = downLoadItemList.Dequeue();//downLoadItemList.Remove(item);
				item1.Dispose();
	//			DownloadItemInfo i = ;
				if(downLoadItemList.Count > 0){
					downLoadItemList.Peek().StartDownload();
				}
			}else {
	#if INNER_TEST
//				Debug.LogWarning(i+"/"+downLoadItemList.Count+") url:"+item.www.url+" www.isDone=false progress:"+item.www.progress+" www.err:"+item.www.error);
	#endif
			}
		}
//		}
		//Debug.Log ("download list item: " + downLoadItemList.Count);
		if (!isLoginSent) {
			if (downLoadItemList.Count <= 0 && startDown) {
				if (retryItemList.Count > 0) {
					if (!isShowRetry) {
						isShowRetry = true;

					    string titleText = 
#if LANGUAGE_EN
						"Download Error";
#else
						"下载错误";
#endif
						//TextCenter.GetText("DownloadErrorTitle");
						string contentText = //TextCenter.GetText("DownloadErrorContent");
#if LANGUAGE_EN
						"There is an error occured when dowloading resources, please connect to the Internet and Retry!";
#else
						"下载资源过程中出现错误，请连接到服务器并重试！";
#endif
						
						string btntext = //TextCenter.GetText("Retry");
#if LANGUAGE_EN
						"Retry";
#else
						"重试";
#endif
						
//						sure = new BtnParam ();
//						sure.callback = ExitGame;
//						sure.text = TextCenter.GetText("Cancel");
//						mwp.btnParams[1] = sure;
//						MsgCenter.Instance.Invoke (CommandEnum.OpenMsgWindow, mwp);
						TipsManager.Instance.ShowMsgWindow(titleText,contentText,btntext);
					}
	
				} else {
						isLoginSent = true;
						if(!downloadLimit)
							GameDataPersistence.Instance.StoreData("ResourceComplete","true");
							
						if (this.transform.parent.name == "ResourceDownloadWindow(Clone)") {
								
								
//								StartCoroutine(CallLater());
								ModuleManger.Instance.ShowModule (ModuleEnum.HomeModule);
								MsgCenter.Instance.Invoke (CommandEnum.ResourceDownloadComplete);
								
								Umeng.GA.FinishLevel ("NewUserDownload");
								Umeng.GA.EventEnd ("NewUserDownloadTime");
								GameDataAnalysis.Event (GameDataAnalysisEventType.DownloadEnd);
						} else {
								
								SendMessageUpwards ("CouldLogin", SendMessageOptions.DontRequireReceiver);
//								Umeng.GA.FinishLevel ("NewUserDownload");
//								Umeng.GA.EventEnd ("NewUserDownloadTime");
//								GameDataAnalysis.Event (GameDataAnalysisEventType.DownloadEnd);
						}

//					if (string.IsNullOrEmpty(GameDataStore.Instance.GetData (GameDataStore.UUID))) {
//
//
//						Umeng.GA.EventEnd("NewUserDownloadTime");
//						GameDataAnalysis.Event(GameDataAnalysisEventType.DownloadEnd);//,new Dictionary<string,string>(){{"DeviceInfo",SystemInfo.deviceUniqueIdentifier}});
//					}
					}	
				}
			} 
//		else {
//			if (this.transform.parent.name == "ResourceDownload(Clone)") {
//				ModuleManger.Instance.ShowModule (ModuleEnum.Home);
//				MsgCenter.Instance.Invoke (CommandEnum.ResourceDownloadComplete);
////				Umeng.GA.FinishLevel ("NewUserDownload");
////				Umeng.GA.EventEnd ("NewUserDownloadTime");
////				GameDataAnalysis.Event (GameDataAnalysisEventType.DownloadEnd);
//			} else {
//				SendMessageUpwards ("CouldLogin", SendMessageOptions.DontRequireReceiver);
////				Umeng.GA.FinishLevel ("NewUserDownload");
////				Umeng.GA.EventEnd ("NewUserDownloadTime");
////				GameDataAnalysis.Event (GameDataAnalysisEventType.DownloadEnd);
//			}
//				}
			
		//Debug.LogError ("LateUpdate : " + retryItemList.Count);
//		for (int i = 0; i < retryItemList.Count; i++) {
//			retryItemList[i].StartDownload();
//			downLoadItemList.Add(retryItemList[i]);
//		}
	}
//
//	IEnumerator CallLater(){
//		yield return new WaitForSeconds(1f);
//		ModuleManger.Instance.ShowModule (ModuleEnum.Home);
//		MsgCenter.Instance.Invoke (CommandEnum.ResourceDownloadComplete);
//	}

	private void ShowTipText(){
		if (DataCenter.Instance.LoginInfo != null && DataCenter.Instance.LoginInfo.Data != null) {
			if (DataCenter.Instance.LoginInfo.Data.Rank < 5) {
				tipText.text = TextCenter.GetText ("Tips_A_" + Utility.MathHelper.RandomToInt (1, 13));
			} else if (DataCenter.Instance.LoginInfo.Data.Rank < 10) {
				tipText.text = TextCenter.GetText ("Tips_B_" + Utility.MathHelper.RandomToInt (1, 10));
			} else if (DataCenter.Instance.LoginInfo.Data.Rank < 20) {
				tipText.text = TextCenter.GetText ("Tips_C_" + Utility.MathHelper.RandomToInt (1, 18));
			} else if (DataCenter.Instance.LoginInfo.Data.Rank < 30) {
				tipText.text = TextCenter.GetText ("Tips_D_" + Utility.MathHelper.RandomToInt (1, 18));
			} else {
				tipText.text = TextCenter.GetText ("Tips_E_" + Utility.MathHelper.RandomToInt (1, 24));
			}	
		} else {
			tipText.text = TextCenter.GetText ("Tips_A_" + Utility.MathHelper.RandomToInt (1, 13));
		}  

		if (tipText.text.Equals(string.Empty)) {
			tipText.text = //TextCenter.GetText ("DefaultTips");
#if LANGUAGE_EN
				"It will take a few seconds to download for the first time, \nplease wait a moment";
#else
			"首次进入游戏需要一定时间进行加载，请稍等.";
#endif
		}

	}

	private void DownloadAgain(object data){
		Umeng.GA.Event ("DownloadAgain",alreadyDone+"");
		downLoadItemList.Clear ();
		total = 0;
		alreadyDone =  0;
//		foreach (var item in retryItemList) {
////			item.retryCount = 3;
//			downLoadItemList.Add(item);
//			total += item.size;
//			item.StartDownload();
//
//		}
		while (retryItemList.Count > 0) {
			DownloadItemInfo item = retryItemList.Dequeue();
			downLoadItemList.Enqueue(item);
			total += item.size;
//			item.StartDownload();
		}
		downLoadItemList.Peek().StartDownload ();
		retryItemList.Clear ();
		isShowRetry = false;

	}


	private void ExitGame(object data){

	}
	public void StartDownload(){
		if (string.IsNullOrEmpty(GameDataPersistence.Instance.GetData (GameDataPersistence.UUID))) {
			Umeng.GA.StartLevel("NewUserDownload");
			Umeng.GA.EventBegin("NewUserDownloadTime");
//			GameDataAnalysis.Event(GameDataAnalysisEventType.NewUser,"NewUserDownloadStart");
			GameDataAnalysis.Event(GameDataAnalysisEventType.GetVersion,new Dictionary<string,string>(){{"DeviceInfo",SystemInfo.deviceUniqueIdentifier}});
		}
		
		StartCoroutine (Download (serverVersionURL + "?t=" + Random.Range(1000,1000000), delegate(WWW serverVersion) {
//			Debug.Log ("download serverVersion from " + serverVersionURL + ", version text:"+serverVersion.text);
			if(string.IsNullOrEmpty(serverVersion.error)){
				LoadVersionConfig(serverVersion.text,serverVersionDic);
			}else{
				StartDownload();
				return;
			}

			if (versionTxt != null) {
				versionTxt.text = appVersion + version;
				versionTxt.enabled = true;
			}
			//load the local version.txt. if not exists, jump through the init.

			StartCoroutine(Download(localResPath + "version.txt",delegate(WWW localVersion) {
				Debug.Log("local version err: " + localVersion.error);

//				Debug.Log("local version txt: " + File.ReadAllText(localResFullPath + "version.txt"));

				if(string.IsNullOrEmpty(localVersion.error))
				{
					LoadVersionConfig(localVersion.text,localVersionDic);
				}

				CompareVersion();
			},true));
		}));
	}

	void LoadVersionConfig(string content,Dictionary<string,DownloadItemInfo> versionDic)
	{
		if (content == null || content.Length == 0) {
			return;
		}

		string[] records = content.Split (new string[]{"\n"},System.StringSplitOptions.RemoveEmptyEntries);
		int i = 0;
		Debug.Log ("serverHost:"+ServerConfig.ServerHost+" version content: " + content);
		if (records [0].IndexOf ("version") >= 0) {
			version = records[0].Split(':')[1];
			i = 1;
		}
		for(; i < records.Length; i++){
			DownloadItemInfo item = DownloadItemInfo.ParseSting (records[i]);
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
		Debug.Log ("local version path: " + localResFullPath);
		File.WriteAllText (localResFullPath + "version.txt",verStr);
//		WriteStringToFile (verStr, localResFullPath + "version.txt");
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
				Debug.Log("md5 is ok.  local res path: " + localResFullPath +" downloadItem.name:"+downloadItem.name);

				//File.WriteAllBytes (localResPath + "/" + serverVersionDic [name] [0] + ".unity3d", resBytes);
				//only for test
				File.WriteAllBytes ( localResFullPath + downloadItem.name + ".unity3d",downloadItem.www.bytes);
//				WriteToFile(downloadItem.www.bytes,localResFullPath + downloadItem.name + ".unity3d");
				UpdateLocalVersionConfig(downloadItem.name,true);

				current += serverVersionDic [downloadItem.name].size;
//				int.TryParse(serverVersionDic [name].size,out current);

				//StartCoroutine(LoadAssetToScene(resBytes));
//				LoadAssetToScene()
			}catch(IOException e)
			{
				Debug.Log(e.Message+ ";"+e.StackTrace);
			}

		} else {
			Debug.Log("load res again: server-version.txt-md5:"+serverVersionDic [downloadItem.name].md5+" != downloaded filehash:"+hash);
//			LoadRes(serverVersionDic[name][1],name);
//			retryItemList.Add(downloadItem);
//			if(downloadItem.retryCount >0)
//			{
				downloadItem.StartDownload();
//				downloadItem.retryCount--;
//			}
		}
	}

	void DeleteAndWrite(string fileName){
		if (File.Exists (fileName)) {
			File.Delete(fileName);
		}
	}
	
	void WriteToFile(byte[] data, string path){
		DeleteAndWrite (path);
		try {
			FileStream fs = new FileStream(path, FileMode.CreateNew, FileAccess.Write);
			fs.Write(data, 0, data.Length);
			fs.Close();
			fs.Dispose();
			//			Debug.LogError("write to file success : " + fileName);
		} catch (System.Exception ex) {
//			Debug.LogError("WriteToFile exception : " + ex.Message);
		}
	}

	void WriteStringToFile(string content, string path){
		DeleteAndWrite (path);
		try {
			using (StreamWriter outfile = new StreamWriter(path))
			{
				outfile.Write(content);
			}
			//			Debug.LogError("write to file success : " + fileName);
		} catch (System.Exception ex) {
//			Debug.LogError("WriteToFile exception : " + ex.Message);
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
		total = 0;
		downLoadItemList.Clear ();
		startDown = false;

		if (ServerConfig.Channel != "AndroidTest") {

			if (this.transform.parent.name == "Loading(Clone)") {
				if (GameDataPersistence.Instance.GetData ("ResrouceDownload") == "Start" || (GameDataPersistence.Instance.GetData ("ResourceComplete") == "true")) {
					//					SendMessageUpwards ("CouldLogin", SendMessageOptions.DontRequireReceiver);	
					//					return;
				downloadLimit = false;
				}
			} else {
					downloadLimit = false;
			}		
		} else {
			downloadLimit = false;		
		}

		Debug.Log("chanel-------"+downloadLimit);
		foreach (string name in serverVersionDic.Keys) {

			DownloadItemInfo serverItem = serverVersionDic[name];
			if(downloadLimit){
				if(serverItem.name.IndexOf("Protobuf") < 0 && serverItem.name.IndexOf("Language") < 0 ){
//					Debug.Log("log: " + serverItem.name);
					continue;
				}
					
			} 
			if(!localVersionDic.ContainsKey(name))
			{

				downLoadItemList.Enqueue(serverItem);
				total += serverItem.size;
//				serverItem.StartDownload();
				//load the resource
//				LoadRes(serverVersionDic[name][1],name);
//				int.TryParse(serverVersionDic[name][4],out total);
			}else
			{
				DownloadItemInfo localItem = localVersionDic[name];
				if(localItem.version != serverItem.version) {
					Debug.Log("server version: " + serverItem.version + "  local version: " + localItem.version);
					downLoadItemList.Enqueue(serverItem);
					total += serverItem.size;
//					serverItem.StartDownload();
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
		Debug.Log ("count: " + downLoadItemList.Count );
		if (downLoadItemList.Count > 0) {
			downLoadItemList.Peek ().StartDownload ();	
		}
		if(string.IsNullOrEmpty(GameDataPersistence.Instance.GetData (GameDataPersistence.UUID)))
			GameDataAnalysis.Event(GameDataAnalysisEventType.DownloadStart,new Dictionary<string,string>(){{"DeviceInfo",SystemInfo.deviceUniqueIdentifier}});
		startDown = true;
		if (total > 0) {
			pro.enabled = true;
			proText.enabled = true;
			tipText.enabled = true;
			fg.enabled = true;
			bg.enabled = true;
		}
	}

	IEnumerator Download(string url, CompleteDownloadCallback callback,bool ignoreErr = false)
	{
		WWW www = new WWW (url);
		globalWWW = www;
		//Debug.Log (globalWWW);
		yield return www;


//		Debug.Log ("error: " + www.error);
		if (!string.IsNullOrEmpty (www.error) && !ignoreErr) {
			Debug.Log (www.error + " : " + url);

			Umeng.GA.Event("DownloadError","FileErr:" + url);

			MsgWindowParams mwp = new MsgWindowParams ();
			mwp.btnParam = new BtnParam();

			mwp.titleText = 
#if LANGUAGE_CN
			"下载错误";
#else
			"File Download Error";
#endif
			mwp.contentText = 
#if LANGUAGE_CN
	"此文件下载错误：" + url;
#else
			"Network error.Please check your network connection and try again later.";
#endif
			
			BtnParam sure = new BtnParam ();
			sure.callback = o=>{
				if (callback != null) {
					callback(www);		
				}
				www.Dispose ();
				globalWWW = null;
			};
			sure.text = 
			#if LANGUAGE_CN
				"确定";
			#else
				"OK";
			#endif
//			mwp.btnParam = null;
			
//			sure = new BtnParam ();
//			sure.callback = ExitGame;
//			sure.text = 
//			#if LANGUAGE_CN
//				"重试";
//			#else
//				"Retry";
//			#endif
			mwp.btnParam = sure;
			MsgCenter.Instance.Invoke (CommandEnum.OpenMsgWindow, mwp);

		} else {
			//call the callback nomatter what errors.
			if (callback != null) {
				callback(www);		
			}
			www.Dispose ();
			globalWWW = null;
		}

	}

//	void LoadRes(string url,string name){
//		//Debug.Log ("load res: " + name + " url:" + serverResURL);
//		StartCoroutine(Download(serverResURL + name + ".unity3d",delegate(WWW serverRes) {
//			//StartCoroutine(Download(serverResURL + url,delegate(WWW serverRes) {
//
//			if(!string.IsNullOrEmpty(serverRes.error))
//			{
//				Debug.Log("the res download has some err: " + serverRes.error);
//				return;
//			}
////			UpdateLocalRes(serverRes.bytes,name);
//		}));
//	}

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

//	public int retryCount = 3;

	public WWW www;

	public void StartDownload() {
		if (www != null) {
			www.Dispose();		
		}
		www = new WWW (ResourceUpdate.serverResURL + name + ".unity3d?"+ Random.Range(10000,1000000));
		Debug.Log ("url: " +ResourceUpdate.serverResURL + name + ".unity3d");

//		if (!string.IsNullOrEmpty (www.error)) {
//			Debug.Log (www.error + " : " + www.url);
//			
//			Umeng.GA.Event("DownloadError","FileErr:" + www.url);
//			
//			MsgWindowParams mwp = new MsgWindowParams ();
//			mwp.btnParam = new BtnParam();
//			
//			mwp.titleText = 
//				#if LANGUAGE_CN
//				"下载错误";
//			#else
//			"File Download Error";
//			#endif
//			mwp.contentText = 
//				#if LANGUAGE_CN
//				"此文件下载错误：" + url;
//			#else
//			"Network error.Please check your network connection and try again later.";
//			#endif
//			
//			BtnParam sure = new BtnParam ();
//			sure.callback = null;
//			sure.text = 
//			#if LANGUAGE_CN
//				"确定";
//			#else
//				"OK";
//			#endif
//			//			mwp.btnParam = null;
//			
//			//			sure = new BtnParam ();
//			//			sure.callback = ExitGame;
//			//			sure.text = 
//			//			#if LANGUAGE_CN
//			//				"重试";
//			//			#else
//			//				"Retry";
//			//			#endif
//			mwp.btnParam = sure;
//			MsgCenter.Instance.Invoke (CommandEnum.OpenMsgWindow, mwp);
//			
//		}
	}

	public static DownloadItemInfo ParseSting(string info) {
		DownloadItemInfo item = new DownloadItemInfo ();
//		string[] records = info.Split (new string[]{"\n"},System.StringSplitOptions.RemoveEmptyEntries);
//		foreach (string record in records) {
//		Debug.Log ("dowload item: " + info);

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
