using System;

public class ProtobufDataBase : IOriginModel {

	protected byte[] originData;

	private ErrorMsg errorMsgInfo = null;

	private Type type;

	public ProtobufDataBase(object instance) {
		Init ();
		SerializeData (instance);
	}

//	public ProtobufDataBase(byte[] data) {
//		Validate (data);
//		if (errorMsgInfo.Code == ErrorCode.Succeed) {
//			originData = data;
//		}
//	}

	void Init() {
		type = GetType ();
	}

	/// <summary>
	/// set data
	/// </summary>
	/// <returns>true</returns>
	/// <c>false</c>
	/// <param name="instance">Instance.</param>
	public ErrorMsg SerializeData (object instance) {

		originData = ProtobufSerializer.SerializeToBytes (instance);

		return Dipose (originData,instance.GetType().ToString());
	}

	/// <summary>
	/// get data
	/// </summary>
	/// <returns>The data.</returns>
	public object DeserializeData () {

		return ProtobufSerializer.ParseFormBytes(originData,type);
	}

	/// <summary>
	/// serialize class to byte array
	/// </summary>
	/// <returns>The object.</returns>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	protected byte[] SerializeObject<T>(T instance) {
		return ProtobufSerializer.SerializeToBytes<T> (instance);
	}

	/// <summary>
	/// check data vaild
	/// </summary>
	/// <param name="data">Data.</param>
	protected ErrorMsg Validate(byte[] data) {
		object obj = ProtobufSerializer.ParseFormBytes (data,type);

		return Dipose (obj,"");
	}


	ErrorMsg Dipose(object ins,string name) {
		if (ins == null) {
			errorMsgInfo.Code = ErrorCode.IllegalData;
			LogHelper.Log( " sorry ! " + name +" is illegal instance !");
		} else {
			errorMsgInfo.Code = ErrorCode.Succeed;
			LogHelper.Log("congratulations ! " + name +" instance  success !");
		}

		return errorMsgInfo;
	}
}
