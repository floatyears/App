using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class GachaWindowView : UIComponentUnity {
    private UILabel chancesLabel;
    private UILabel titleLabel;
    private int originLayer;
    private bool displayingResult = false; // when 
    private int tryCount = 0;
    private GachaWindowInfo gachaInfo;
    private List<int> pickedBtnIdList = new List<int>();

    private Dictionary<GameObject, int> buttonDic = new Dictionary<GameObject, int>();

    public override void Init ( UIInsConfig config, IUICallback origin ) {
        base.Init (config, origin);
        InitUI();
    }
    
    public override void ShowUI () {
        base.ShowUI ();
        SetMenuBtnEnable(false);
        AddListener();
    }
    
    public override void HideUI () {
        base.HideUI ();
        Reset();
        SetMenuBtnEnable(true);
        RemoveListener();
    }
    
    public override void DestoryUI () {
        base.DestoryUI ();
    }

    public void AddListener(){
        MsgCenter.Instance.AddListener(CommandEnum.EnterGachaWindow, Enter);
    }

    public void RemoveListener(){
        MsgCenter.Instance.RemoveListener(CommandEnum.EnterGachaWindow, Enter);
    }

    public override void Callback(object data)
    {
        base.Callback(data);
        
        CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
        
        switch (cbdArgs.funcName)
        {
        case "SetTitleView": 
            CallBackDispatcherHelper.DispatchCallBack(SetTitleLabel, cbdArgs);
            break;
        default:
            break;
        }
    }

    private UIButton FindButtonById(int id){
        return FindChild<UIButton>("Board/Buttons/Button" + id);
    }

    private void InitUI() {
        titleLabel = FindChild<UILabel>("TitleBackground/TitleLabel");
        chancesLabel = FindChild<UILabel>("TitleBackground/ChancesLabel");

        UIButton[] buttons = FindChild("Board/Buttons").GetComponentsInChildren< UIButton >();
        for (int i = 0; i < buttons.Length; i++){
            UIButton button = FindButtonById(i);
            buttonDic.Add( button.gameObject, i);
            UIEventListener.Get( buttons[i].gameObject ).onClick = ClickButton;
        }

        originLayer = Main.Instance.NguiCamera.eventReceiverMask;
    }
    
    
    private void SetMenuBtnEnable(bool enable){
//        if (enable){
//            Main.Instance.NguiCamera.eventReceiverMask = LayerMask.NameToLayer("ScreenShelt") << 15;
//        }
//        else {
//            Main.Instance.NguiCamera.eventReceiverMask = originLayer;
//        }
    }

    private void SetTitleLabel(object args){
        string titleText = args as string;
        titleLabel.text = titleText;
    }

    private void Enter(object args){
        LogHelper.Log("Enter invoke SyncGachaInfos()");

        GachaWindowInfo gachaWindowInfo = args as GachaWindowInfo;
        if (gachaWindowInfo != null){
            gachaInfo = gachaWindowInfo;
            SyncGachaInfos();
        }
    }

    private void SyncGachaInfos(){
        chancesLabel.text = TextCenter.Instace.GetCurrentText("GachaChances", 0, gachaInfo.totalChances);
    }

    private void ClickButton(GameObject btn){
        int index = buttonDic[btn];
        if (pickedBtnIdList.Contains(index)){
            return;
        }
        LogHelper.Log("ClickButton() {0}", index);
        if (tryCount < gachaInfo.totalChances){
            showUnitByUserUnitID(btn, gachaInfo.unitList[index]);
        }
        else if (tryCount < DataCenter.maxGachaPerTime){
            int blankId = tryCount - gachaInfo.totalChances;
            if (blankId > gachaInfo.blankList.Count - 1){
                return;
            }
            showUnitByBlankId(btn, gachaInfo.blankList[blankId]);
        }
    }

    private void Reset(){
        displayingResult = false;
        tryCount = 0;
        pickedBtnIdList.Clear();
    }

    private void showUnitByUserUnitID(GameObject btn, uint uniqueId){
        LogHelper.Log("showUnitByUserUnit(), uniqueId {0}", uniqueId);
//        TUserUnit userUnit = DataCenter.Instance.MyUnitList.GetMyUnit(uniqueId);
        ShowUnitById(btn, userUnit.UnitInfo.ID, userUnit);
        DealAfterShowUnit(buttonDic[btn]);
    }

    private void showUnitByBlankId(GameObject btn, uint unitId){
//        LogHelper.Log("showUnitByUnitId(), unitId {0}", unitId);
        ShowUnitById(btn, unitId, null);
        DealAfterShowUnit(buttonDic[btn]);
    }

    private void ShowUnitById(GameObject btn, int unitId, TUserUnit userUnit){
        LogHelper.Log("ShowUnitById(), unitId {0}, userUnit not null {1}", unitId, userUnit != null);
    }

    private void DealAfterShowUnit(int index){
        LogHelper.Log("DealAfterShowUnit(), index {0}", index);
        if (tryCount < DataCenter.maxGachaPerTime){
            pickedBtnIdList.Add(index);
            tryCount += 1;
        }
        else {
            FinishShowGachaWindow();
        }
    }

    private void FinishShowGachaWindow(){
        LogHelper.Log("FinishShowGachaWindow()");
    }
}
