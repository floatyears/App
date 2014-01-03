
using System.Collections;
using LitJson;


public class JsonOriginData : IOriginModel {
	
	protected string originData;
	protected JsonData jsonData;

	protected ErrorMsg error = new ErrorMsg ();

	public JsonOriginData (string info) {
		originData = info;
	}

	public virtual ErrorMsg SerializeData (object instance)
	{
		originData = JsonMapper.ToJson (instance);
		return error;
	}
	
	public virtual object DeserializeData ()
	{
		jsonData = JsonMapper.ToObject(originData);
		return jsonData;
	}
	
}