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
		TextCenter.Instance.Init (o=>{
			ModelManager.Instance.Init();

		});

       
		//NoviceGuideStepEntityManager.Instance ();
    }

    /// <summary>
    /// start game
    /// </summary>
    void OnEnable() {
		SetResolution();
		ResourceManager.Instance.Init (o => {
			AudioManager.Instance.PlayBackgroundAudio(AudioEnum.music_home);
			EffectManager em = EffectManager.Instance;
			UIManager.Instance.ChangeScene(SceneEnum.Loading);
			ConfigDragPanel dragPanelConfig = new ConfigDragPanel();
		});
    }

	public const float DefaultSize = 1.5f;
	public const int DefaultHeight = 960;
	void SetResolution() {
		float currentSize = Screen.height / (float)Screen.width;
		UIPanel rootPanel = uiRoot.GetComponent<UIPanel>();

		if (currentSize >= DefaultSize) {
			//the current screen is thinner than the default, keep the default.
			float sizePropotion = currentSize / DefaultSize;
			int height = System.Convert.ToInt32( DefaultHeight * sizePropotion);
			root.manualHeight = height;
			rootPanel.clipRange = new Vector4(0, 0, height / currentSize, height);
		}
		else{
			//the current screen is fatter than the default
			root.manualHeight = DefaultHeight;
			rootPanel.clipRange = new Vector4(0, 0, 640, root.manualHeight);
		}

//		TestScreenAdaption(Screen.width, Screen.height, root.manualHeight);
	}

	public UILabel test_screen_width;
	public UILabel test_screen_height;
	public UILabel test_screen_manual_height;
	void TestScreenAdaption(int w, int h, int mh){
		test_screen_width.text = "Screen_Width : " + w;
		test_screen_height.text = "Screen_Height : " + h;
		test_screen_manual_height.text = "Manual_Height : " + mh;
	}

    void OnDisable() {
//        sui.RemoveListener();
    }

//	void Update () {
//		if (Input.GetKeyDown (KeyCode.A)) {
//			MsgCenter.Instance.Invoke(CommandEnum.UserGuideAnim, null);	
//		}
//
//		if (Input.GetKeyDown (KeyCode.S)) {
//			MsgCenter.Instance.Invoke(CommandEnum.UserGuideAnim, true);	
//		}
//	}
}
