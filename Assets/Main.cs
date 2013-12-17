using System.Collections;
using UnityEngine;
using ProtoBuf;

/// <summary>
/// main will always exist until the game close
/// </summary>

public class Main : MonoBehaviour 
{
	public GameObject uiRoot;

	private static Main mainScrpit;

	public static Main Instance
	{
		get
		{
			if(mainScrpit == null)
				mainScrpit = (Main)FindObjectOfType(typeof(Main));

			return mainScrpit;
		}
	}

	private GameInput gInput;

	public GameInput GInput
	{
		get{return gInput;}
	}

	private const float screenWidth = 640;

	private static float texScale = 0f;

	public static float TexScale
	{
		get{ return texScale; }
	}

	void Awake()
	{
		mainScrpit = this;

		gInput = gameObject.AddComponent<GameInput>();

		DontDestroyOnLoad(gameObject);


		texScale = screenWidth / Screen.width;
	}

	/// <summary>
	/// start game
	/// </summary>
	void OnEnable()
	{
		ViewManager.Instance.Init(uiRoot);
		ControllerManager.Instance.ChangeScene(SceneEnum.Fight);

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