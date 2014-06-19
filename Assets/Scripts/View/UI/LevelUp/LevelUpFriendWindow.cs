using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class LevelUpFriendWindow : UIComponentUnity {
    protected DragPanel friendDragPanel;
	protected Dictionary<GameObject,TFriendInfo> friendUnitInfoDic = new Dictionary<GameObject, TFriendInfo>();
	protected Dictionary<string, object> dragPanelArgs = new Dictionary<string, object>();
	protected List<TFriendInfo> friendInfoList = new List<TFriendInfo>();
	protected List<UnitInfoStruct> unitInfoStruct = new List<UnitInfoStruct>();

	private Dictionary<int, UILabel> infoLabel = new Dictionary<int, UILabel> ();
	private UnitItemInfo baseItemInfo = null;
	private TUserUnit friendUnit = null;

    public override void Init(UIInsConfig config, IUICallback origin) {
        base.Init(config, origin);
        InitUI();
    }

    public override void ShowUI() {
        base.ShowUI();
		MsgCenter.Instance.AddListener (CommandEnum.BaseAlreadySelect, BaseAlreadySelect);
        MsgCenter.Instance.AddListener(CommandEnum.PanelFocus, FocusOnPanel);
    }

    public override void HideUI() {
        base.HideUI();
		MsgCenter.Instance.RemoveListener (CommandEnum.BaseAlreadySelect, BaseAlreadySelect);
        MsgCenter.Instance.RemoveListener(CommandEnum.PanelFocus, FocusOnPanel);
    }

    void InitUI() {
        InitDragPanel();
		for (int i = 0; i < 3; i++) {
			UILabel label = FindChild<UILabel>("Info_Panel/VauleLabel/" + i);
			infoLabel.Add(i,label);
		}
        this.gameObject.SetActive(false);
    }

    public override void ResetUIState() {
        base.ResetUIState();
        friendUnit = null;
        ShowInfo();
    }

    void InitDragPanel() {
        if (DataCenter.Instance.SupportFriends != null)
            friendInfoList.AddRange(DataCenter.Instance.SupportFriends);
        string name = "FriendDragPanel";
        int count = friendInfoList.Count;//DataCenter.Instance.Friends.Count;
        string itemSourcePath = "Prefabs/UI/Friend/AvailFriendItem";
		ResourceManager.Instance.LoadLocalAsset(itemSourcePath , o =>{
			GameObject itemGo = o as GameObject;
			//InitDragPanelArgs();
			friendDragPanel = CreateDrag(name, count, itemGo);
			FillDragPanel(friendDragPanel);
			friendDragPanel.DragPanelView.SetScrollView(ConfigDragPanel.LevelUpDragPanelArgs, transform);
		});

    }

    private DragPanel CreateDrag(string name, int count, GameObject item) {
        DragPanel panel = new DragPanel(name, item);
        panel.CreatUI();
        panel.AddItem(count);
        return panel;
    }


    private void AddEventListener(GameObject item) {
        UIEventListenerCustom.Get(item).onClick = ClickFriendItem;
        UIEventListenerCustom.Get(item).LongPress = PressItem;
    }

    private void FillDragPanel(DragPanel panel) {
        if (panel == null)
            return;
        for (int i = 0; i < panel.ScrollItem.Count; i++) {
            GameObject scrollItem = panel.ScrollItem[i];
            TFriendInfo tfiItem = friendInfoList[i];	
            friendUnitInfoDic.Add(scrollItem, tfiItem);
            StoreLabelInfo(scrollItem, tfiItem.UserUnit);
            ShowItem(scrollItem);
            AddEventListener(scrollItem);
        }
    }

    private void ShowItem(GameObject item) {
        GameObject avatarGo = item.transform.FindChild("Texture_Avatar").gameObject;
        UITexture avatarTex = avatarGo.GetComponent< UITexture >();
        TFriendInfo tfriendInfo;
        if (!friendUnitInfoDic.TryGetValue(item, out tfriendInfo)) {
            Debug.Log("ShowItem(),Not Exist vaule");
            return;
        }

        if (friendUnitInfoDic[item].UserUnit == null) {
            Debug.LogError("LevelUpFriendWindow, friendUnitInfoDic[item].UserUnit is Null, Return");
            return;
        }
        //
        uint curUnitId = friendUnitInfoDic[item].UserUnit.UnitID;
//		Debug.LogError("Base Show Avatar : curUnitId is : " + curUnitId);
		DataCenter.Instance.GetUnitInfo (curUnitId).GetAsset (UnitAssetType.Avatar, o=>{
			avatarTex.mainTexture = o as Texture2D;
		});//UnitInfo[curUnitId].GetAsset(UnitAssetType.Avatar);

        int addAttack = tfriendInfo.UserUnit.AddAttack;
        int addHp = tfriendInfo.UserUnit.AddHP;

        int level = tfriendInfo.UserUnit.Level;

        UILabel nickNameLabel = item.transform.FindChild("Label_Name").GetComponent<UILabel>();
        if (tfriendInfo.NickName == string.Empty)
            nickNameLabel.text = "NoName";
        else
            nickNameLabel.text = tfriendInfo.NickName;

        UILabel friendTypeLabel = item.transform.FindChild("Label_Friend_Type").GetComponent<UILabel>();
        int friendType = (int)tfriendInfo.FriendState;
//		Debug.Log("friendType : " + friendType);
        switch (friendType) {
        case 1:
            friendTypeLabel.text = "Friend";
            friendTypeLabel.color = Color.yellow;
            break;
        case 4:
            friendTypeLabel.text = "Support";
            friendTypeLabel.color = Color.green;
            break;
        default:
            break;
        }

        if (friendUnitInfoDic[item].FriendPoint == null) {
            Debug.Log("friendUnitInfoDic[ item ].FriendPoint is Null  ");
            return;
        }
        UILabel friendPointLabel = item.transform.FindChild("Label_Friend_Point").GetComponent<UILabel>();
        friendPointLabel.text = string.Format("{0}pt", tfriendInfo.FriendPoint);
    }


    protected virtual void ClickFriendItem(GameObject item) {
        AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		TUserUnit temp = friendUnitInfoDic[item].UserUnit;
		if (temp.Equals (friendUnit)) {
			friendUnit = null;
		} else {
			friendUnit = temp;
		}

		ShowInfo();
		MsgCenter.Instance.Invoke(CommandEnum.PickFriendUnitInfo, temp);
    }

    void PressItem(GameObject item) {
		TUserUnit temp = friendUnitInfoDic [item].UserUnit;
        UIManager.Instance.ChangeScene(SceneEnum.UnitDetail);//before
		MsgCenter.Instance.Invoke(CommandEnum.ShowUnitDetail, temp);//after
    }

	void BaseAlreadySelect(object data) {
		baseItemInfo = data as UnitItemInfo;
	}

    void FocusOnPanel(object data) {
        string msg = (string)data;
//		Debug.Log("Friend Window receive : " + msg);
        if (msg == "Tab_Friend") {
            this.gameObject.SetActive(true);
            if (IsInvoking("CrossShow")) {
                CancelInvoke("CrossShow");
            }
            InvokeRepeating("CrossShow", 0.1f, 1f);
			ShowInfo();
        }
        else {
            this.gameObject.SetActive(false);
        }
    }

	void ShowInfo () {
//        LogHelper.Log("friend support level showInfo, baseItemInfo {0}", baseItemInfo);
		if (friendUnit == null || baseItemInfo == null) {
			infoLabel [0].text = "x 1.00";
			infoLabel [1].text = "x 1.00";
			infoLabel [2].text = "x 1.00";
			return;	
		}
		float tValue = DGTools.TypeMultiple (baseItemInfo.userUnitItem, friendUnit);
		float rValue = DGTools.RaceMultiple (baseItemInfo.userUnitItem, friendUnit);
		infoLabel [0].text = "x " + tValue.ToString("0.00");
		infoLabel [1].text = "x " + rValue.ToString("0.00");
		infoLabel [2].text = "x " +  DGTools.AllMultiple (tValue, rValue).ToString("0.00");
	}

    bool exchange = false;
    void CrossShow() {
        if (exchange) {
            for (int i = 0; i< unitInfoStruct.Count; i++) {
                unitInfoStruct[i].targetLabel.text = string.Format("+{0}", unitInfoStruct[i].text2);
            }
            exchange = false;
        }
        else {
            //                                target.text = text1;
            for (int i = 0; i< unitInfoStruct.Count; i++) {
                unitInfoStruct[i].targetLabel.text = string.Format("+{0}", unitInfoStruct[i].text1);
                //Debug.LogError("Text2 : " + item.text2);
            }
            exchange = true;
        }
    }//End
        
    void StoreLabelInfo(GameObject item,TUserUnit tuu) {

		if (tuu == null) {
			return;	
		}
        UnitInfoStruct infoStruct = new UnitInfoStruct();
        infoStruct.text1 = tuu.Level.ToString();
        infoStruct.text2 = (tuu.AddHP + tuu.AddAttack).ToString();
        infoStruct.targetLabel = item.transform.FindChild("Label_Info").GetComponent<UILabel>();
        unitInfoStruct.Add(infoStruct);
    }
}








