using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Reflection;
using bbproto;
using System.Text;

public class HttpRequestManager : MonoBehaviour{
    private static HttpRequestManager instance;
    public static HttpRequestManager Instance {
        get {
            if (instance == null) {
				instance = FindObjectOfType<HttpRequestManager>();;
            }
            return instance;
        }
    }

	private Queue<HttpRequest> wwwRequestQueue = new Queue<HttpRequest>();
	private Dictionary<ProtocolNameEnum, NetCallback> protoListeners = new Dictionary<ProtocolNameEnum, NetCallback> ();
	private List<HttpRequest> requestList = new List<HttpRequest> ();
//	private Dictionary<string, ProtoBuf.IExtensible> tempMsgDic = new Dictionary<string, ProtoBuf.IExtensible>();
	private Queue<HttpRequest> requestPool = new Queue<HttpRequest> ();
	
	/// <summary>
	/// adds the http request. 
	/// </summary>
	/// <param name="post">Post.</param>
	/// <param name="callback">Callback.</param>
	/// <param name="protoName">Proto name.protoName means the proto from server you want to listen. </param>
	/// <param name="isSimultanuous">Is simultanuous.simultanuous means send the messages simultanuosly.</param>
	private void AddHttpRequest(HttpRequest request, bool isSimultanuous = false) {
		if (request == null) {
			return;
		}
		requestList.Add (request);
		if (isSimultanuous) {
			StartCoroutine (SendMsg(request));
		}else{
			wwwRequestQueue.Enqueue(request);
			if(wwwRequestQueue.Count < 2){
				StartCoroutine (SendMsg());
			}
		}
    }

	/// <summary>
	/// Sends the http request.
	/// </summary>
	/// <param name="msg">Message.</param>
	/// <param name="callback">Callback.</param>
	/// <param name="protoName">Proto name.</param>
	/// <param name="isSimultanuous">Is simultanuous.</param>
	/// <param name="failCallback">Fail callback.</param>
	/// <param name="failProtoName">Fail proto name.</param>
	public void SendHttpRequest(ProtoBuf.IExtensible msg, NetCallback callback, ProtocolNameEnum protoName, bool isSimultanuous = false, NetCallback failCallback = null, ProtocolNameEnum failProtoName = ProtocolNameEnum.NONE){
		HttpRequest req = null;
		if(requestPool.Count > 0)
			req = requestPool.Dequeue ();
		if(req == null)
			req = new HttpRequest();
		req.Msg = msg;
		req.ProtoName = protoName;
		req.SuccessCallback = callback;
		req.FailProtoName = failProtoName;
		req.FailCallback = failCallback;
		AddHttpRequest (req, isSimultanuous);
	}

	/// <summary>
	/// Add proto listener even though we didn't actully send the proto.
	/// </summary>
	/// <param name="protoName">Proto name.</param>
	/// <param name="callback">Callback.</param>
	public void AddProtoListener(ProtocolNameEnum protoName, NetCallback callback){
		if(protoListeners.ContainsKey(protoName)){
			protoListeners[protoName] += callback;
		}else{
			protoListeners.Add(protoName,callback);
		}
	}

	/// <summary>
	/// removes the proto listener.
	/// </summary>
	/// <param name="protoName">Proto name.</param>
	public void RemoveProtoListener(ProtocolNameEnum protoName,NetCallback callback){
		if(protoListeners.ContainsKey(protoName)){
			protoListeners[protoName] -= callback;
		}
	}

	IEnumerator SendMsg() {
		if (wwwRequestQueue.Count > 0) {
			HttpRequest request = wwwRequestQueue.Dequeue();	
			
			if (request != null) {
				Debug.Log ("Proto Send: [[[---" + request.Msg.GetType().Name + "---]]]");
				ModuleManager.SendMessage(ModuleEnum.MaskModule,"connect",true);
				WWW www = new WWW (ServerConfig.ServerHost + "/" + GetUrlByType(request.Msg.GetType()), ProtobufSerializer.SerializeToBytes(request.Msg));
				yield return www;
				ModuleManager.SendMessage(ModuleEnum.MaskModule,"connect",false);
				RequestDone (www, request);
				StartCoroutine(SendMsg ());		
			}
		}
		yield return null;
    }

	IEnumerator SendMsg(HttpRequest rq) {
		HttpRequest request = rq;
		Debug.Log ("Proto Send: [[[---" + rq.Msg.GetType().Name + "---]]]");
		ModuleManager.SendMessage(ModuleEnum.MaskModule,"connect",true);
		WWW www = new WWW (ServerConfig.ServerHost + "/" + GetUrlByType(request.Msg.GetType()), ProtobufSerializer.SerializeToBytes(request.Msg));
		yield return www;
		ModuleManager.SendMessage(ModuleEnum.MaskModule,"connect",false);
		RequestDone(www,request);
	}


	private string GetUrlByType(Type type){
		string url = type.Name.Substring (3);
		StringBuilder returnVal = new StringBuilder();
		for (int i = 0; i < url.Length; i ++) {
			if(char.IsUpper(url[i])){
				if(i > 0){
					returnVal.Append('_');
					returnVal.Append(char.ToLower(url[i]));
				}else{
					returnVal.Append(char.ToLower(url[i]));
				}
			}else{
				returnVal.Append(url[i]);
			}
		}
		return returnVal.ToString ();
	}

	void RequestDone(WWW www, HttpRequest request) {

		if (www.isDone && string.IsNullOrEmpty (www.error)) {
			GeneralProtoRsp returnVal = ProtobufSerializer.ParseFormBytes(www.bytes,typeof(GeneralProtoRsp)) as GeneralProtoRsp;
//			Type.get
			PropertyInfo[] properties = typeof(GeneralProtoRsp).GetProperties();
			object msg;
			ProtocolNameEnum name;
			foreach (var item in properties) {
				msg = item.GetValue(returnVal,null);
				
				if(msg != null){
					Debug.Log("Proto Recv: name-> [[[---" + item.Name  + "---]]]" + " value->" + msg ); 
					name = (ProtocolNameEnum)Enum.Parse(typeof(ProtocolNameEnum), msg.GetType().Name);
					if(protoListeners.ContainsKey(name)){
						protoListeners[name](msg);
					}
					HttpRequest req;
					for (int i = requestList.Count - 1; i >= 0; i--) {
						req = requestList[i];
						if(name == req.ProtoName){
							req.OnRequestSuccessHandler(msg);
							requestList.RemoveAt(i);
							requestPool.Enqueue(req);

						}else if(name == req.FailProtoName){
							req.OnRequestFailHandler(msg);
							requestList.RemoveAt(i);
							requestPool.Enqueue(req);
						}
					}
				}
			}

		} else if(!string.IsNullOrEmpty(www.error)) {
			string text = www.error;
			ErrorMsg errMsg = null;
			if (text.StartsWith("Failed to connect to ") || text.StartsWith("couldn't connect to host") || text.StartsWith("Could not connect to the server")){
				errMsg = new ErrorMsg(ErrorCode.CONNECT_ERROR);
			}
			else if (text.StartsWith("500 Internal Server Error")){
				errMsg = new ErrorMsg(ErrorCode.SERVER_500);
			}
			else if (text.EndsWith("Operation timed out")){
				errMsg = new ErrorMsg(ErrorCode.TIMEOUT);
			}
			else {
				errMsg = new ErrorMsg(ErrorCode.NETWORK);
			}
			if (errMsg != null){
				//            MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, msgParams);
				TipsManager.Instance.ShowMsgWindow(TextCenter.GetText("Error"),errMsg.Msg , TextCenter.GetText("Retry"),CallbackRetry,request);
			}
		}
		www.Dispose ();
    }

    void CallbackRetry(object args){
		SendMsg (args as HttpRequest);
	}
	
}
