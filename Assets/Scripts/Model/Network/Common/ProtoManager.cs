using UnityEngine;
using System.Collections;
using bbproto;


public class ProtoManager: ProtobufDataBase, INetBase {
	private string protoName;
	private object instObj;
	protected System.Type reqType;
	protected System.Type rspType;
	
	public ProtoManager() {
	}
	
	protected object InstanceObj {
		get { return instObj; }
		set { instObj = value; }
	}

	protected string Proto {
		set { protoName = value; }
	}

	public void Send () {
		IWWWPost http = new HttpNetBase ();

		if( MakePacket () ) { //make proto packet to Data
//			LogHelper.Log ("MakePacket => proto:{0} InstanceType:{1}",protoName, reqType);
			http.Send (this, protoName, Data);
		}
	}
	
	public void Receive (IWWWPost post) {
		instObj = ProtobufSerializer.ParseFormBytes(post.WwwInfo.bytes, rspType);
		if (instObj != null) {
			OnResponse (true);
		} else {
			OnResponse (false);
			LogHelper.LogError("++++++proto.ParseFormBytes failed.++++++");
		}
	}

	public virtual void OnResponse (bool success) {
		// implement in derived class
	}

	public virtual bool MakePacket () {
		//make packet to Data for send to server
		return true;
	}

	private DataListener netDoneCallback;

	public virtual void OnRequest (object data, DataListener callback) {

	}

	protected void OnRequestBefoure (DataListener callback) {
		netDoneCallback = callback;
	}

	protected void OnResposeEnd(object data) {
		if (netDoneCallback != null) {
			netDoneCallback(data);
		}
	}
}

public abstract class NetDataBase {
	public INetBase netBase;
	public abstract void ParseNetData(object data);
}

