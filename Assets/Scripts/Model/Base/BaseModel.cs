using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

public abstract class BaseModel {

	/// <summary>
	/// origin data
	/// </summary>
	protected byte[] byteData;

	/// <summary>
	/// network if you will use network ,you must instance net
	/// </summary>
	protected NetBase net;
	
	private ModelManager mManager;
	/// <summary>
	/// Gets the Model Manager.
	/// </summary>
	/// <value>The M manager.</value>
	protected ModelManager MManager {
		get {
			return mManager;
		}
	}

	private MsgCenter msgCenter;

	protected ErrorMsg errMsg;
	
	public BaseModel(object instance) {
		Init(instance);
	}
	
	/// <summary>
	/// Load data from.
	/// </summary>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	protected T LoadProtobuf<T>(){
		return ProtobufSerializer.ParseFormBytes<T>(byteData);
	}
	
	/// <summary>
	/// Save this instance.
	/// </summary>
	protected ErrorMsg SaveWithProtobuf<T>(T protobufData){
		return Save(SerializeObject<T>(protobufData));
	}

	/// <summary>
	/// serialize class to byte array
	/// </summary>
	/// <returns>The object.</returns>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	protected byte[] SerializeObject<T>(T instance){
		return ProtobufSerializer.SerializeToBytes<T> (instance);
	}

	/// <summary>
	/// Save the specified newData and errorMsg.
	/// </summary>
	/// <param name="newData">New data.</param>
	protected ErrorMsg Save(byte[] newData) {
		// validate
		errMsg = Validate(newData);
		if (errMsg.Code == ErrorCode.Succeed) {
			byteData = newData;
		}
		return errMsg;
	}
	
	/// <summary>
	/// Validate the specified instance.
	/// </summary>
	/// <param name="instance">Instance.</param>
	protected ErrorMsg Validate(byte[] data) {
		Type type = this.GetType ();

		object oo = ProtobufSerializer.ParseFormBytes (data,type);
		if (oo == null) {
			errMsg.Code = ErrorCode.IllegalData;
			LogHelper.Log( " sorry ! " + type.Name +" is illegal instance !");
		} else {
			errMsg.Code = ErrorCode.Succeed;
			LogHelper.Log("congratulations ! " + type.Name +" instance  success !");
		}
		
		return errMsg;
	}
	
	/// <summary>
	/// Init this instance. Each subclass should do own Init
	/// </summary>
	protected virtual void Init(object instance) {
		mManager = ModelManager.Instance;
		LogHelper.Log (instance.GetType ());
		byteData = ProtobufSerializer.SerializeToBytes (instance);
	}



	/// <summary>
	/// Receives the net data.
	/// </summary>
	/// <param name="www">Www.</param>
	protected virtual void ReceiveNetData(WWW www) {

	}

	/// <summary>
	/// send request to server
	/// </summary>
	/// <returns><c>true</c>, if request was neted, <c>false</c> otherwise.</returns>
	public abstract bool NetRequest();
	
}