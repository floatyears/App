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
        sui = new ShowUnitInfo();
        // init manager class
        ViewManager.Instance.Init(uiRoot);
        ModelManager.Instance.Init();
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
    void OnEnable() {
        INetBase netBase = new AuthUser();
        Debug.Log("connect net to login : " + Time.realtimeSinceStartup);
        netBase.OnRequest(null, LoginSuccess);
//
        AudioManager.Instance.PlayAudio(AudioEnum.music_home);
        EffectManager em = EffectManager.Instance;
    }

    void LoginSuccess(object data) {
        Debug.Log("Login Success : " + Time.realtimeSinceStartup);
        if (data != null) {
            bbproto.RspAuthUser rspAuthUser = data as bbproto.RspAuthUser;
            if (rspAuthUser == null) {
                LogHelper.Log("authUser response rspAuthUser == null");
                return;
            }
			
            if (rspAuthUser.header.code != 0) {
                //TODO: showErrMsg()
                LogHelper.Log("rspAuthUser return error: {0} {1}", rspAuthUser.header.code, rspAuthUser.header.error);
                return;
            }

            uint userId = rspAuthUser.user.userId;

            if (rspAuthUser.isNewUser == 1) {
                LogHelper.Log("New user registeed, save userid:" + userId);
                GameDataStore.Instance.StoreData(GameDataStore.USER_ID, rspAuthUser.user.userId);
            }
			
            //TODO: update localtime with servertime
            //localTime = rspAuthUser.serverTime
			
            //save to GlobalData
            if (rspAuthUser.account != null) {
                DataCenter.Instance.AccountInfo = new TAccountInfo(rspAuthUser.account);
            }
			
            if (rspAuthUser.user != null) {
                DataCenter.Instance.UserInfo = new TUserInfo(rspAuthUser.user);
                if (rspAuthUser.evolveType != null) {
                    DataCenter.Instance.UserInfo.EvolveType = rspAuthUser.evolveType;
                }
				
                LogHelper.Log("authUser response userId:" + rspAuthUser.user.userId);
            }
            else {
                LogHelper.Log("authUser response rspAuthUser.user == null");
            }
			
            if (rspAuthUser.friends != null) {
                LogHelper.Log("rsp.friends have {0} friends.", rspAuthUser.friends.Count);
                DataCenter.Instance.SupportFriends = new List<TFriendInfo>();
//				Debug.LogError(rspAuthUser.friends.Count);
                foreach (FriendInfo fi in rspAuthUser.friends) {
                    TFriendInfo tfi = new TFriendInfo(fi);
                    DataCenter.Instance.SupportFriends.Add(tfi);
                }
            }
            else {
                LogHelper.Log("rsp.friends==null");
            }
			
            if (rspAuthUser.unitList != null) {
                foreach (UserUnit unit in rspAuthUser.unitList) {
                    DataCenter.Instance.MyUnitList.Add(userId, unit.uniqueId, new TUserUnit(unit));
                    DataCenter.Instance.UserUnitList.Add(userId, unit.uniqueId, new TUserUnit(unit));
                }
                LogHelper.Log("rspAuthUser add to myUserUnit.count: {0}", rspAuthUser.unitList.Count);
            }
			
            if (rspAuthUser.party != null && rspAuthUser.party.partyList != null) {
                DataCenter.Instance.PartyInfo = new TPartyInfo(rspAuthUser.party);
				
                //TODO: replace ModelManager.GetData(UnitPartyInfo) with DataCenter.Instance.PartyInfo.CurrentParty
                ModelManager.Instance.SetData(ModelEnum.UnitPartyInfo, DataCenter.Instance.PartyInfo.CurrentParty);
            }
            NetWorkTestHelper.Test();
        }
        Debug.Log("UIManager.Instance.ChangeScene before");
        UIManager.Instance.ChangeScene(SceneEnum.Start);
        TurnToReName();
       
    }

    void TurnToReName() {
        //		Debug.Log("PlayerInfoBar.TurnToReName() : Start");
        if (DataCenter.Instance.UserInfo == null) {
            Debug.LogError("DataCenter.Instance.UserInfo is null");
            return;
        }
		
        if (DataCenter.Instance.UserInfo.NickName == null) {
            Debug.LogError("DataCenter.Instance.UserInfo.NickName is null");
            return;
        }
		
        if (DataCenter.Instance.UserInfo.NickName.Length == 0) {
            UIManager.Instance.ChangeScene(SceneEnum.Others);
            Debug.Log("PlayerInfoBar.ChangeScene( Others ).");
        }

        Debug.Log("PlayerInfoBar.TurnToReName() : End. NickName is " + DataCenter.Instance.UserInfo.NickName);
    }

    void OnDisable() {
        sui.RemoveListener();
    }
}
