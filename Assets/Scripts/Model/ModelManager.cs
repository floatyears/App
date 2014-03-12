using System;
using System.Collections;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

public class ModelManager {
    private static ModelManager instance;
    
    /// <summary>
    /// singleton 
    /// </summary>
    /// <value>The instance.</value>
    public static ModelManager Instance {
        get {
            if (instance == null) {
                instance = new ModelManager();
            }
            return instance;
        }
    }

    

    //---------------------------------------------------------------------------------------------------------------//

    //---------------------------------------------------------------------------------------------------------------//

    private Dictionary<ModelEnum, object> modelDataDic = new Dictionary<ModelEnum, object>();
	
    /// <summary>
    /// Init this instance.
    /// </summary>
    public void Init() {
        InitConfigData();
        InitData();

        //new all server protocol handler
        InitNetworkHandler();
    }

    void InitNetworkHandler() {
//		AuthUser authUser = new AuthUser ();
//		RenameNick rename = new RenameNick ();
//		StartQuest startquest = new StartQuest ();
//		ClearQuest clearquest = new ClearQuest ();
//		ChangeParty changeparty = new ChangeParty();
//		LevelUp
    }

    //init config data
    void InitConfigData() {
        TextAsset obj = ResourceManager.Instance.LoadLocalAsset(UIConfig.UIInsConfigPath) as TextAsset;
        string info = obj.text;
        UIIns ins = new UIIns(info);
        SetData(ModelEnum.UIInsConfig, ins);
//		MapConfig mc = new MapConfig ();
//		SetData (ModelEnum.MapConfig, mc);
    }

    public void InitData() {
        ConfigUnitInfo cui = new ConfigUnitInfo();
//		Debug.LogError ("InitData ConfigSkill");
        ConfigSkill cs = new ConfigSkill();
        ConfigEnermy ce = new ConfigEnermy();
        ConfigUnitBaseInfo cubi = new ConfigUnitBaseInfo();
        ConfigTrap ct = new ConfigTrap();

        ConfigFriendList configFriendList = new ConfigFriendList();
        ConfigAudio audioConfig = new ConfigAudio();
        ConfigStage stage = new ConfigStage();
        ConfigViewData tempViewData = new ConfigViewData();
    }
	
    /// <summary>
    /// Adds the data.
    /// </summary>
    /// <param name="modelType">Model type.</param>
    /// <param name="model">Model.</param>
    public void SetData(ModelEnum modelType, object model) {
        if (modelDataDic.ContainsKey(modelType)) {
            modelDataDic[modelType] = model;
        }
        else {
            modelDataDic.Add(modelType, model);	
        }
    }

    /// <summary>
    /// get the data to use
    /// </summary>
    /// <returns>The data.</returns>
    /// <param name="modelType">Model type.</param>
    /// <param name="erroMsg">Erro message.</param>
    public object GetData(ModelEnum modelType, ErrorMsg erroMsg) {
        object origin = null;

        if (!modelDataDic.TryGetValue(modelType, out origin)) {
            erroMsg.Code = ErrorCode.INVALID_MODEL_NAME;
            erroMsg.Msg = string.Format("required key {0}, but it not exist in ModelManager", modelType);
        }

        return origin;
    }
}

