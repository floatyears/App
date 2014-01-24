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

	public static Main Instance {
		get {
			if(mainScrpit == null)
				mainScrpit = (Main)FindObjectOfType(typeof(Main));
			return mainScrpit;
		}
	}

	private GameInput gInput;

	public GameInput GInput {
		get{return gInput;}
	}

	private GameTimer gTimer;

	private const float screenWidth = 640;

	private static float texScale = 0f;

	public static float TexScale {
		get{ return texScale; }
	}

	private static byte globalDataSeed = 0;

	public static byte GlobalDataSeed {
		get {
			return globalDataSeed;
		}
	}

	private UILabel label;

	void Awake() {
		mainScrpit = this;
		globalDataSeed = (byte)Random.Range (0, 255);
		gInput = gameObject.AddComponent<GameInput>();
		gTimer = gameObject.AddComponent<GameTimer>();
		DontDestroyOnLoad(gameObject);
		texScale = screenWidth / Screen.width;
		// init manager class
		ViewManager.Instance.Init(uiRoot);
		ModelManager.Instance.Init ();
	}

	/// <summary>
	/// start game
	/// </summary>
	void OnEnable() {
		//UIManager.Instance.ChangeScene (SceneEnum.Start);
		ControllerManager.Instance.ChangeScene (SceneEnum.Fight);
	}
}
