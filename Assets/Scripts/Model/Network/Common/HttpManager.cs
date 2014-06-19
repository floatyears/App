using UnityEngine;
using System.Collections.Generic;
using System;

public class HttpManager : INetSendPost {
    private static HttpManager instance;
    public static HttpManager Instance {
        get {
            if (instance == null) {
                instance = new HttpManager();
            }
            return instance;
        }
    }

//  public static string baseUrl = "http://127.0.0.1:6666/";
    public static string baseUrl = "http://107.170.243.127:6666/";

    private List<IWWWPost> wwwRequst = new List<IWWWPost>();

    private HttpManager() {
        GameInput.OnUpdate += HttpUpdate;
    }

    private string sessionId = "";
    /// <summary>
    /// network session id , init on game start, all network request must have this property
    /// </summary>
    public string SessionId {
        get { return sessionId; }
        set { sessionId = value; }
    }

    public ErrorMsg ValidateSessionId(object protobufModel) {
        ErrorMsg errMsg = new ErrorMsg();
        Type t = protobufModel.GetType();
        try {
            string protoSessionId = (string)t.GetProperty("sessionId").GetValue(protobufModel, null);
            if (protoSessionId != sessionId) {
                errMsg.Code = (int)ErrorCode.INVALID_SESSIONID;
            }
        }
        catch (Exception ex) {
            errMsg.Code = (int)ErrorCode.ILLEGAL_PARAM;
            errMsg.Msg = "request or response not has field sessionId";
        }
        return errMsg;
    }
	
    public void SendHttpPost(IWWWPost post) {
        if (post.WwwInfo != null) {
            wwwRequst.Add(post);	
        }
    }
	
    public void SendAssetPost(IWWWPost post) {
        post.WwwInfo = WWW.LoadFromCacheOrDownload(post.Url, post.Version);
        wwwRequst.Add(post);
    }

    void HttpUpdate() {
        int count = wwwRequst.Count;
        if (count == 0) {
            return;	
        }
        for (int i = wwwRequst.Count - 1; i >= 0; i--) {
            WWW www = wwwRequst[i].WwwInfo;
			if (www==null) {
				UnityEngine.Debug.LogError("remove null www["+i+"]. wwwRequst.Count="+wwwRequst.Count);	
				wwwRequst.RemoveAt(i);
				continue;
			}
            if (www.isDone && string.IsNullOrEmpty(www.error)) {
                RequestDone(wwwRequst[i]);
            }
            else if (!string.IsNullOrEmpty(www.error)) {
                IWWWPost post = wwwRequst[i];
                wwwRequst.RemoveAt(i);
                OpenMsgWindowByError(www.error, post);
                //LogHelper.Log("HttpUpdate(), received error, {0}", www.error);
            }
            else {
				//LogHelper.Log("HttpUpdate(), not done or done and error");
            }
        }
    }

    void RequestDone(IWWWPost wwwPost) {
//		Debug.LogError (wwwPost.WwwInfo.url);
        wwwRequst.Remove(wwwPost);
        wwwPost.ExcuteCallback();
    }

    void OpenMsgWindowByError(string text, IWWWPost post){
        LogHelper.Log("OpenMsgWindowByError(), received error, {0}", text);

        MsgWindowParams msgParams = null;

		if (text.StartsWith("Failed to connect to ") || text.StartsWith("couldn't connect to host") || text.StartsWith("Could not connect to the server")){
			Debug.LogError("OpenMsgWindowByError() error:"+text);
            msgParams = new MsgWindowParams();
            msgParams.btnParams = new BtnParam[2]{new BtnParam(), new BtnParam()};
            ErrorMsg errMsg = new ErrorMsg(ErrorCode.CONNECT_ERROR);
            msgParams.contentText = errMsg.Msg;

			msgParams.btnParam = new BtnParam();
			msgParams.btnParam.callback = CallbackRetry;
			msgParams.btnParam.args = post;
			msgParams.btnParam.text = TextCenter.GetText("retry");

//            msgParams.btnParams[0].text = TextCenter.GetText("retry");
//            msgParams.btnParams[0].callback = CallbackRetry;
//            msgParams.btnParams[0].args = post;
//            msgParams.btnParams[1].callback = CallbackCancelRequest;
        }
        else if (text.StartsWith("500 Internal Server Error")){
			Debug.LogError("OpenMsgWindowByError(), 500 Internal Server Error");
            msgParams = new MsgWindowParams();
            ErrorMsg errMsg = new ErrorMsg(ErrorCode.SERVER_500);
            msgParams.contentText = errMsg.Msg;
            
			msgParams.btnParam = new BtnParam();
			msgParams.btnParam.callback = CallbackRetry;
			msgParams.btnParam.args = post;
			msgParams.btnParam.text = TextCenter.GetText("retry");
		}
		else if (text.EndsWith("Operation timed out")){
			Debug.LogError("OpenMsgWindowByError():"+text);
			msgParams = new MsgWindowParams();
			ErrorMsg errMsg = new ErrorMsg(ErrorCode.TIMEOUT);
			msgParams.contentText = errMsg.Msg;
		
			msgParams.btnParam = new BtnParam();
			msgParams.btnParam.callback = CallbackRetry;
			msgParams.btnParam.args = post;
			msgParams.btnParam.text = TextCenter.GetText("retry");
		}
        else {
			Debug.LogError("OpenMsgWindowByError(), unknown Error: "+text);
            msgParams = new MsgWindowParams();
            ErrorMsg errMsg = new ErrorMsg(ErrorCode.NETWORK);
            msgParams.contentText = errMsg.Msg;

			msgParams.btnParam = new BtnParam();
			msgParams.btnParam.callback = CallbackRetry;
			msgParams.btnParam.args = post;
			msgParams.btnParam.text = TextCenter.GetText("retry");
        }

        if (msgParams != null){
            MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, msgParams);
			post.WwwInfo.Dispose();
			post.WwwInfo = null;
            return;
        }
    }

    void CallbackRetry(object args){
        Debug.LogError("CallbackRetry()");
        HttpNetBase networkBase = args as HttpNetBase;
        if (networkBase != null){
            networkBase.ReSend();
        }
    }

    void CallbackCancelRequest(object args){
        Debug.LogError("CallbackCancelRequest()");
        MsgCenter.Instance.Invoke(CommandEnum.SetBlocker, new BlockerMaskParams(BlockerReason.Connecting, false));
        MsgCenter.Instance.Invoke(CommandEnum.WaitResponse, false);
    }
}
