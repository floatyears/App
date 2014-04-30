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

    private GameInput gInput;

    public GameInput GInput {
        get{ return gInput;}
    }
    private GameTimer gTimer;
//    private ShowUnitInfo sui;

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
    public UICamera NguiCamera {
        get {
            if (nguiCamera == null) {
                nguiCamera = Camera.main.GetComponent<UICamera>();
            }
            return nguiCamera;
        }

    }

    void Awake() {

		mainScrpit = this;
        TrapInjuredInfo tii = TrapInjuredInfo.Instance;
        globalDataSeed = (byte)Random.Range(0, 255);

        gInput = gameObject.AddComponent<GameInput>();
        gTimer = gameObject.AddComponent<GameTimer>();
        DontDestroyOnLoad(gameObject);

        texScale = screenWidth / Screen.width;
		//sui = new ShowUnitInfo();
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
		AudioManager.Instance.PlayBackgroundAudio(AudioEnum.music_home);
        EffectManager em = EffectManager.Instance;
        UIManager.Instance.ChangeScene(SceneEnum.Loading);
    }
	
    void OnDisable() {
//        sui.RemoveListener();
    }
}
