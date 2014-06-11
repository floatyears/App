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

    public void Send() {
        IWWWPost http = new HttpNetBase();
        if (MakePacket()) { //make proto packet to Data
//			LogHelper.Log ("MakePacket => proto:{0} InstanceType:{1}",protoName, reqType);
            http.Send(this, protoName, Data);
            SetBlockMask(true);
        }
    }
	
    public void Receive(IWWWPost post) {
//		Debug.LogError ("receive : " + post.Url);
        instObj = ProtobufSerializer.ParseFormBytes(post.WwwInfo.bytes, rspType);
        if (instObj != null) {
            OnResponse(true);
        }
        else {
            OnResponse(false);
//            LogHelper.LogError("++++++proto.ParseFormBytes failed.++++++");
        }
		SetBlockMask(false);
        OnResponseEnd(this.instObj);
        
    }

    public void SetBlockMask(bool flag){
//        LogHelper.LogError("SetBlockMask(), {0}", flag);
        MsgCenter.Instance.Invoke(CommandEnum.SetBlocker, new BlockerMaskParams(BlockerReason.Connecting, flag));
        MsgCenter.Instance.Invoke(CommandEnum.WaitResponse, flag);
    }

    public virtual void OnResponse(bool success) {
        // implement in derived class
    }

    public virtual bool MakePacket() {
        //make packet to Data for send to server
        return true;
    }

    private DataListener netDoneCallback;

    public virtual void OnRequest(object data, DataListener callback) {
        OnRequestBefore(callback);
//		Debug.LogError ("OnReceiveCommand");
        OnReceiveCommand(data);

    }

    protected virtual void OnReceiveCommand(object data) {
        Send(); //send request to server
    }

    protected virtual void OnRequestBefore(DataListener callback) {
        netDoneCallback = callback;
    }

    protected virtual void OnResponseEnd(object data) {
        if (netDoneCallback != null) {
            netDoneCallback(data);
        }

    }
}

public abstract class NetDataBase {
    protected INetBase netBase;

}

