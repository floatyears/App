using UnityEngine;
using System.Collections;
using bbproto;


public class ProtoManager: ProtobufDataBase, INetBase {
    private string protoName;
    private object instObj;
    protected System.Type reqType;
    protected System.Type rspType;
    protected ErrorMsg errMsg = new ErrorMsg();
	
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
        }
    }
	
    public void Receive(IWWWPost post) {
//        this.instObj = null;
        instObj = ProtobufSerializer.ParseFormBytes(post.WwwInfo.bytes, rspType);
        if (instObj != null) {
            OnResponse(true);
        }
        else {
            OnResponse(false);
            LogHelper.LogError("++++++proto.ParseFormBytes failed.++++++");
        }

        errMsg = new ErrorMsg();
        OnResponseEnd(this.instObj);
        onResponseCallback();
    }

    public virtual void OnResponse(bool success) {
        // implement in derived class
    }

    public virtual bool MakePacket() {
        //make packet to Data for send to server
        return true;
    }

    private ResponseCallback netDoneCallback;

    public virtual void OnRequest(object data, ResponseCallback callback) {
        OnRequestBefore(callback);
//		Debug.LogError ("OnReceiveCommand");
        OnReceiveCommand(data);
    }

    public virtual void OnRequest(ResponseCallback callback) {
        OnRequestBefore(callback);
        //      Debug.LogError ("OnReceiveCommand");
//        OnReceiveCommand(data);
    }

    protected virtual void OnReceiveCommand(object data) {
        Send(); //send request to server
    }

    protected virtual void OnRequestBefore(ResponseCallback callback) {
        netDoneCallback = callback;
    }

    protected virtual void OnResponseEnd(object data) {
    }

    protected virtual void onResponseCallback() {
        if (netDoneCallback != null) {
            netDoneCallback(errMsg);
        }
    }
}

public abstract class NetDataBase {
    protected INetBase netBase;

}

