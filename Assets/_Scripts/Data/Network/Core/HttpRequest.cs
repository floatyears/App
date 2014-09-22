using UnityEngine;
using System.Collections;

public class HttpRequest {
//	private WWW www;

	private ProtocolNameEnum protoName = ProtocolNameEnum.NONE;

	private ProtocolNameEnum failProtoName = ProtocolNameEnum.NONE;
	/// <summary>
	/// callback when success
	/// </summary>
	private NetCallback callback;

	/// <summary>
	/// callback when fail
	/// </summary>
	private NetCallback failCallback;

//	private WWWForm requestForm;

	private ProtoBuf.IExtensible msg;

	private int timeout;

	/// <summary>
	/// Initializes a new instance of the <see cref="HttpRequest"/> class.
	/// </summary>
	/// <param name="callback">Callback.</param>
	/// <param name="protoName">Proto name.</param>
	public HttpRequest(){
	}
	

	public void OnRequestSuccessHandler (object msg) {
		if (callback != null) {
			callback(msg);		
		}
	}

	public void OnRequestFailHandler(object msg){
		if (failCallback != null) {
			failCallback(msg);
		}
	}

	public ProtocolNameEnum ProtoName{
		get{
			return protoName;
		}
		set{
			protoName = value;
		}
	}

	public ProtoBuf.IExtensible Msg{
		get{
			return msg;
		}
		set{
			msg = value;
		}
	}

	public NetCallback SuccessCallback{
		get{
			return callback;
		}
		set{
			callback = value;
		}
	}

	public ProtocolNameEnum FailProtoName{
		get{
			return failProtoName;
		}
		set{
			failProtoName = value;
		}
	}

	public NetCallback FailCallback{
		get{
			return failCallback;
		}
		set{
			failCallback = value;
		}
	}
//
//    public void ReSend(){
//		www = new WWW (Url, data);
//		HttpRequestManager.Instance.SendHttpPost (this);
//    }
//	
//	public void ExcuteCallback () {
//		if (callback != null) {
//			callback(this);
//		}
//	}
//
//	public void Send() {
//		//        IWWWPost http = new HttpNetBase();
//		if (MakePacket()) { //make proto packet to Data
//			//			LogHelper.Log ("MakePacket => proto:{0} InstanceType:{1}",protoName, reqType);
//			//            http.Send(this, protoName, Data);
//			
//			SetBlockMask(true);
//		}
//	}
//	
//	public void Receive(IWWWPost post) {
//		//		Debug.LogError ("receive : " + post.Url);
//		instObj = ProtobufSerializer.ParseFormBytes(post.WwwInfo.bytes, rspType);
//		if (instObj != null) {
//			OnResponse(true);
//		}
//		else {
//			OnResponse(false);
//			//            LogHelper.LogError("++++++proto.ParseFormBytes failed.++++++");
//		}
//		SetBlockMask(false);
//		OnResponseEnd(this.instObj);
//		
//	}
//	
//	public virtual void SetBlockMask(bool flag){
//		//		Debug.LogError("SetBlockMask(), " + flag);
//		ModuleManager.SendMessage(ModuleEnum.MaskModule, "block", new BlockerMaskParams(BlockerReason.Connecting, flag));
//		ModuleManager.SendMessage(ModuleEnum.MaskModule, "wait", flag);
//	}
//	
//	public virtual void OnResponse(bool success) {
//		// implement in derived class
//	}
//	
//	public virtual bool MakePacket() {
//		//make packet to Data for send to server
//		return true;
//	}
//	
//	private DataListener netDoneCallback;
//	
//	public virtual void OnRequest(object data, DataListener callback) {
//		OnRequestBefore(callback);
//		//		Debug.LogError ("OnReceiveCommand");
//		OnReceiveCommand(data);
//		
//	}
//	
//	protected virtual void OnReceiveCommand(object data) {
//		Send(); //send request to server
//	}
//	
//	protected virtual void OnRequestBefore(DataListener callback) {
//		netDoneCallback = callback;
//	}
//	
//	protected virtual void OnResponseEnd(object data) {
//		if (netDoneCallback != null) {
//			
//			netDoneCallback(data);
//		}
//		
//	}
}