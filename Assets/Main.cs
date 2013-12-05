using System.Collections;
using UnityEngine;
using ProtoBuf;

public class Main : MonoBehaviour 
{
	public GameObject uiRoot;
	
	/// <summary>
	/// start game
	/// </summary>
	void OnEnable()
	{
		ViewManager.Instance.Init(uiRoot);
		ControllerManager.Instance.ChangeScene(SceneEnum.Quest);

		//FileStream fs = new FileStream((Application.dataPath + "/Scripts/Protobuf/Person.proto"),FileMode.Open,FileAccess.Read);

		//ProtoReader pr=  new ProtoReader(fs,ProtoBuf.Meta.TypeModel.SerializeType,
//		msg.Person person = new msg.Person();
//
//		person.name = "aaa";
//		person.id = 11;
//
//		ProtoBuf.Serializers.

	}
}
