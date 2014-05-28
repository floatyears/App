using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtoBuf;
using bbproto;

/// <summary>
/// main will always exist until the game close
/// </summary>

public class Main : MonoBehaviour {
    public GameObject uiRoot;
    private static Main mainScrpit;

    public static Main Instance {
        get {
            if (mainScrpit == null)
                mainScrpit = (Main)FindObjectOfType(typeof(Main));

            return mainScrpit;
        }
    }

	public Camera bottomCamera;

	public Camera effectCamera;

    private GameInput gInput;

    public GameInput GInput {
        get{ return gInput;}
    }
    private GameTimer gTimer;
//    private ShowUnitInfo sui;

//    private const float screenWidth = 640;

//    private static float texScale = 0f;
//
//    public static float TexScale {
//        get{ return texScale; }
//    }

    private static byte globalDataSeed = 0;

    public static byte GlobalDataSeed {
        get {
            return globalDataSeed;
        }
    }

    private UICamera nguiCamera ;
    public UICamera NguiCamera {
        get {
            if (nguiCamera == null) {
                nguiCamera = Camera.main.GetComponent<UICamera>();
            }
            return nguiCamera;
        }

    }

	[HideInInspector]
	public UIRoot root;

    void Awake() {
		mainScrpit = this;
        TrapInjuredInfo tii = TrapInjuredInfo.Instance;
        globalDataSeed = (byte)Random.Range(0, 255);
		root = uiRoot.GetComponent<UIRoot> ();
        gInput = gameObject.AddComponent<GameInput>();
        gTimer = gameObject.AddComponent<GameTimer>();
        DontDestroyOnLoad(gameObject);

        // init manager class
        ViewManager.Instance.Init(uiRoot);
        ModelManager.Instance.Init();
		ConfigDragPanel dragPanelConfig = new ConfigDragPanel();

		//NoviceGuideStepEntityManager.Instance ();
    }

    /// <summary>
    /// start game
    /// </summary>
    void OnEnable() {
		SetResolution ();
		AudioManager.Instance.PlayBackgroundAudio(AudioEnum.music_home);
        EffectManager em = EffectManager.Instance;
        UIManager.Instance.ChangeScene(SceneEnum.Loading);

    }

	public const float DefaultSize = 1.5f;
	public const int DefaultHeight = 960;

	void SetResolution() {
		float currentSize = Screen.height / (float)Screen.width;
		UIPanel rootPanel = uiRoot.transform.Find("RootPanel").GetComponent<UIPanel>();
//		Debug.LogError (currentSize);
		if (currentSize >= DefaultSize) {
			float sizePropotion = currentSize / DefaultSize;
			int height = System.Convert.ToInt32( DefaultHeight * sizePropotion);
			root.manualHeight = height;
			rootPanel.clipRange = new Vector4(0, 0, Screen.width, height);
		}
	}
	
    void OnDisable() {
//        sui.RemoveListener();
    }
}
