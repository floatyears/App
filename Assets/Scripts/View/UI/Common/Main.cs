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
	private GameTimer gTimer;
	private ShowUnitInfo sui;

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

	private UICamera nguiCamera ;
	public UICamera NguiCamera{
		get {
			if(nguiCamera == null) {
				nguiCamera = Camera.main.GetComponent<UICamera>();
			}
			return nguiCamera;
		}

	}

	void Awake()
	{
		mainScrpit = this;
		TrapInjuredInfo tii = TrapInjuredInfo.Instance;
		globalDataSeed = (byte)Random.Range (0, 255);

		gInput = gameObject.AddComponent<GameInput>();
		gTimer = gameObject.AddComponent<GameTimer>();
		DontDestroyOnLoad(gameObject);

		texScale = screenWidth / Screen.width;
		sui = new ShowUnitInfo ();
		// init manager class
		ViewManager.Instance.Init(uiRoot);
		ModelManager.Instance.Init ();
		TempConfig.InitStoryQuests();
		TempConfig.InitEventQuests();
		TempConfig.InitPlayerUnits();
		TempConfig.InitUnitAvatarSprite();
//		GameSingleDataStore.Instance.StoreSingleData ("aa", "bb");
//		Debug.LogError (System.Guid.NewGuid ().ToString ());
	}

	/// <summary>
	/// start game
	/// </summary>
	void OnEnable()
	{
		INetBase netBase = new AuthUser ();
		netBase.OnRequest (null, LoginSuccess);

		AudioManager.Instance.PlayAudio( AudioEnum.music_home );
		EffectManager em = EffectManager.Instance;
		//ProtoManager<bbproto.ReqAuthUser> authUser = new ProtoManager<bbproto.ReqAuthUser> ();
//		string info =  GameSingleDataStore.Instance.GetSingleData ("aa");
//		Debug.LogError (info);

	}

	void LoginSuccess(object data) {
		UIManager.Instance.ChangeScene( SceneEnum.Start);
	}

	void OnDisable () {
		sui.RemoveListener ();
	}
}
