using UnityEngine;
using System.Collections;
using bbproto;

public class ProtoManager <T>: ProtobufDataBase,INetBase {
	public ProtoManager() {
		Send ();
	}
	
	public void Send () {
		IWWWPost http = new HttpNetBase ();

		MakePacket ();

		http.Send (this, "auth_user", Data);
	}
	
	public void Receive (IWWWPost post) {
		Debug.LogError ("ProtoManager Receive...");
		T instance = ProtobufSerializer.ParseFormBytes<T>(post.WwwInfo.bytes);
		// parse to current instance
		if (instance != null){
//			succeedFunc(instance, errorMsg, values);
		}

	}
	
	public bool MakePacket () {
		ReqAuthUser authUser = new ReqAuthUser ();
		authUser.header = new ProtoHeader ();
		authUser.header.apiVer = "1.0";
		authUser.terminal = new TerminalInfo ();
		authUser.terminal.uuid = "kory-abcdefg";

		SerializeData (authUser); // save to Data

		return true;
	}
}

