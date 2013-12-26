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

	private UILabel label;

	void Awake()
	{
		mainScrpit = this;

		gInput = gameObject.AddComponent<GameInput>();

		DontDestroyOnLoad(gameObject);

		Application.targetFrameRate = 1000;
		texScale = screenWidth / Screen.width;
	}

	/// <summary>
	/// start game
	/// </summary>
	void OnEnable()
	{
		ViewManager.Instance.Init(uiRoot);
		ControllerManager.Instance.ChangeScene(SceneEnum.Start);

		GameObject go = Resources.Load ("Prefabs/DragPanelItem") as GameObject;

		DragPanel dp = new DragPanel ("Test", go);
		dp.CreatUI ();
		dp.AddItem (5);

		//ControllerManager.Instance.ShowActor (1);
//		ControllerManager.Instance.HideActor ();
		//FileStream fs = new FileStream((Application.dataPath + "/Scripts/Protobuf/Person.proto"),FileMode.Open,FileAccess.Read);

		//ProtoReader pr=  new ProtoReader(fs,ProtoBuf.Meta.TypeModel.SerializeType,
//		msg.Person person = new msg.Person();
//
//		person.name = "aaa";
//		person.id = 11;
//
//		ProtoBuf.Serializers.

	}

//	float frame = 0f;
//	float interv = 0.5f;
//	int frameCount = 0;
//
//	void Update()
//	{
//		frameCount ++;
//		if (interv <= 0)
//		{
//			frame = frameCount / 0.5f;
//			frameCount = 0;
//			interv = 0.5f;
//		} 
//		else
//			interv -= Time.deltaTime;
//	}
//
//	void OnGUI()
//	{
//		GUILayout.Label (frame.ToString ());
//	}
}
