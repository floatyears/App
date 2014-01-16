using System;
using System.Collections;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

public enum ModelEnum
{
    User = 1000,
	UnitPartyInfo = 1001,

	UIInsConfig = 2000,

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

    private Dictionary<ModelEnum, BaseModel> modelDic = new Dictionary<ModelEnum, BaseModel>();

    public void Add (ModelEnum modelType, BaseModel model){
        modelDic.Add(modelType, model);
    }

    /// <summary>
    /// Gets the data.
    /// usage: ModelManager.Instance.GetaData<User>
    /// </summary>
    /// <returns>The data.</returns>
    /// <param name="key">Key.</param>
    /// <param name="errorMsg">Error message.</param>
    public BaseModel Get (ModelEnum modelType, ErrorMsg errorMsg) {

        BaseModel model = null;

        if (!modelDic.TryGetValue(modelType, out model)){
            errorMsg.Code = ErrorCode.InvalidModelName;
            errorMsg.Msg = String.Format("required key {0}, but it not exist in ModelManager", modelType);
        }
        return model;
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
	}

	//init config data
	void InitConfigData() {
		TextAsset obj = ResourceManager.Instance.LoadLocalAsset (UIConfig.UIInsConfigPath) as TextAsset;
		string info = obj.text;
		UIIns ins = new UIIns (info);
		AddData (ModelEnum.UIInsConfig, ins);


	}

	public void InitData () {
		ConfigUnitInfo cui = new ConfigUnitInfo ();
		ConfigSkill cs = new ConfigSkill ();
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

