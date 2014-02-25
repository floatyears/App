using UnityEngine;
using System.Collections;
using bbproto;


public class ProtoManager: ProtobufDataBase,INetBase {
	private string protoName;
	private System.Type instType;
	private object instObj;

	public ProtoManager() {
	}

	protected System.Type InstanceType {
		get { return instType; }
		set { instType = value; }
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
		LogHelper.Log ("manager.Send() => proto:{0} InstanceType:{1}",protoName, InstanceType);

		if( MakePacket () ) {
			LogHelper.Log ("MakePacket => proto:{0} InstanceType:{1}",protoName, InstanceType);
			http.Send (this, protoName, Data);
		}
	}
	
	public void Receive (IWWWPost post) {
		LogHelper.Log ("ProtoManager Receive...");
		instObj = ProtobufSerializer.ParseFormBytes(post.WwwInfo.bytes, instType);
		if (instObj != null) {
			OnReceiveFinish (true);
		} else {
			OnReceiveFinish (false);
			LogHelper.LogError("proto.ParseFormBytes failed.");
		}
	}

	public virtual void OnReceiveFinish (bool success) {
		// implement in derived class
	}

	public virtual bool MakePacket () {
		//make packet to Data for send to server
		LogHelper.Log("base.MakePacket()...");
		return true;
	}
}

