using UnityEngine;
using System;
using System.Collections.Generic;

public class NetBase : INetwork {
	/// <summary>
	/// Universal Url
	/// </summary>
	public static string baseUrl = "";
	
	private string reallyUrl = "";

	/// <summary>
	/// subclass's url. init on construct function
	/// </summary>
	public string ReallyUrl {
		get {
			return reallyUrl;
		}
		set {
			reallyUrl = value;
		}
	}

	private static string sessionId = "";
	/// <summary>
	/// network session id , init on game start, all network request must have this property
	/// </summary>
	public static string SessionId {
		get { return sessionId; }
		set { sessionId = value; }
	}

	private List<NetworkBase> httpRequest;

	public NetBase(HttpCallback cb,string url) {

		ReallyUrl = baseUrl + url;
	}

	~NetBase()
	{

	}

	void AddEvent(){
		GameInput.OnUpdate += HandleOnUpdate;
	}

	void RemoveEvent(){
		GameInput.OnUpdate -= HandleOnUpdate;
	}

	bool VerificationUrl() {

		if (string.IsNullOrEmpty (ReallyUrl))
			return false;

		if (baseUrl == ReallyUrl)
			return false;

		if (!ReallyUrl.Contains (baseUrl))
			return false;

		return true;
	}

	void HandleOnUpdate ()
	{
		int count = httpRequest.Count;
		if (count == 0)
			return;

		for (int i = 0; i < count; i++) {
			NetworkBase networkBase = httpRequest[i];
			if(CheckError(networkBase)){
				if(networkBase.wwwRequest.isDone) {
					networkBase.ExcuteCallback();
					httpRequest.Remove(networkBase);
				}
				else
					continue;
			}
			else{
				httpRequest.Remove(networkBase);
			}
		}

		if(httpRequest.Count == 0)
			RemoveEvent ();

	}

	bool CheckError(NetworkBase net) {
		if (!string.IsNullOrEmpty (net.wwwRequest.error)) {
			net.wwwRequest = null;
			LogHelper.Log ("url : " + net.wwwRequest.url + "http request " + net.wwwRequest.error);
			return false;		
		}
		else
			return true;
	}


	public ErrorMsg ValidateSessionId(object protobufModel){
		ErrorMsg errMsg = new ErrorMsg();
		Type t = protobufModel.GetType();
		try {
			string protoSessionId = (string)t.GetProperty("sessionId").GetValue(protobufModel, null);
			if (protoSessionId != sessionId){
				errMsg.Code = ErrorCode.INVALID_SESSIONID;
			}
		} catch (Exception ex) {
			errMsg.Code = ErrorCode.ILLEGAL_PARAM;
			errMsg.Msg = "request or response not has field sessionId";
		}
		return errMsg;
	}



	public void SendAssetRequest () {
		throw new NotImplementedException ();
	}



	public void SendRequest(byte[] data = null)
	{
		NetworkBase network = new NetworkBase ();
		if (data == null) {
			network.wwwRequest = new WWW (reallyUrl);
		} 
		else {
			network.wwwRequest = new WWW(reallyUrl,data);
		}

		if(httpRequest.Count == 0)
			AddEvent ();

		httpRequest.Add (network);
	}

	public void ClearDownload ()
	{
		if (httpRequest.Count > 0) {
			httpRequest.Clear ();
			RemoveEvent ();
		}

	}
}
