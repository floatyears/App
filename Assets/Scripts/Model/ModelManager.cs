using System;
using System.Collections;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

public enum ModelEnum
{
    User			= 1000,
	UnitPartyInfo	= 1001,

	UIInsConfig		= 2000,
	MapConfig		= 2001,
}

public class ModelManager
{
    private static ModelManager instance;
    
    /// <summary>
    /// singleton 
    /// </summary>
    /// <value>The instance.</value>
    public static ModelManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new ModelManager();
            }
            return instance;
        }
    }

    

	//---------------------------------------------------------------------------------------------------------------//

	//---------------------------------------------------------------------------------------------------------------//

	private Dictionary<ModelEnum, IOriginModel> modelDataDic = new Dictionary<ModelEnum, IOriginModel>();
	
	/// <summary>
	/// Init this instance.
	/// </summary>
	public void Init () {
		InitConfigData ();
		InitData ();

		//new all server protocol handler
		InitNetworkHandler ();
	}

	void InitNetworkHandler () {
		AuthUser authUser = new AuthUser ();

	}

	//init config data
	void InitConfigData() {
		TextAsset obj = ResourceManager.Instance.LoadLocalAsset (UIConfig.UIInsConfigPath) as TextAsset;
		string info = obj.text;
		UIIns ins = new UIIns (info);
		AddData (ModelEnum.UIInsConfig, ins);
		MapConfig mc = new MapConfig ();
		AddData (ModelEnum.MapConfig, mc);
	}

	public void InitData () {
		ConfigUnitInfo cui = new ConfigUnitInfo ();
//		Debug.LogError ("InitData ConfigSkill");
		ConfigSkill cs = new ConfigSkill ();
		ConfigEnermy ce = new ConfigEnermy ();
		ConfigUnitBaseInfo cubi = new ConfigUnitBaseInfo ();

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
	public void AddData (ModelEnum modelType, IOriginModel model) {
		if (modelDataDic.ContainsKey (modelType)) {
			modelDataDic [modelType] = model;
		}
		else {
			modelDataDic.Add(modelType,model);	
		}
	}

	/// <summary>
	/// get the data to use
	/// </summary>
	/// <returns>The data.</returns>
	/// <param name="modelType">Model type.</param>
	/// <param name="erroMsg">Erro message.</param>
	public IOriginModel GetData(ModelEnum modelType, ErrorMsg erroMsg) {
		IOriginModel origin = null;

		if(!modelDataDic.TryGetValue(modelType,out origin)) {
			erroMsg.Code = ErrorCode.InvalidModelName;
			erroMsg.Msg = string.Format("required key {0}, but it not exist in ModelManager", modelType);
		}

		return origin;
	}
}

