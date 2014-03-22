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
            SyncGachaInfosAtStart();
        }
    }

    private void SyncGachaInfosAtStart(){
        chancesLabel.text = TextCenter.Instace.GetCurrentText("GachaChances", 0, gachaInfo.totalChances);
    }

    private void SyncGachaInfos(){
        int nowChance = Mathf.Min(tryCount + 1, gachaInfo.totalChances);
        chancesLabel.text = TextCenter.Instace.GetCurrentText("GachaChances", nowChance, gachaInfo.totalChances);
    }

    
    private void ClickButton(GameObject btn){
        int index = buttonDic[btn];
        if (pickedBtnIdList.Contains(index)){
            return;
        }
        LogHelper.Log("ClickButton() {0}", index);
        if (tryCount < gachaInfo.totalChances){
            ShowUnitByUserUnitID(btn, gachaInfo.unitList[tryCount]);
        }
        else if (tryCount < DataCenter.maxGachaPerTime){
            int blankId = tryCount - gachaInfo.totalChances;
            if (blankId > gachaInfo.blankList.Count - 1){
                return;
            }
            ShowUnitByBlankId(btn, gachaInfo.blankList[blankId]);
        }
    }

    private void Reset(){
        displayingResult = false;
        tryCount = 0;
        pickedBtnIdList.Clear();

        foreach (var item in buttonDic) {
            ResetOneBtn(item.Key);
        }
    }

    private void ShowUnitByUserUnitID(GameObject btn, uint uniqueId){
//        LogHelper.Log("showUnitByUserUnit(), uniqueId {0}", uniqueId);
        TUserUnit userUnit = DataCenter.Instance.MyUnitList.GetMyUnit(uniqueId);
        ShowUnitById(btn, userUnit.UnitInfo.ID, userUnit);
        DealAfterShowUnit(buttonDic[btn]);
    }

    private void ShowUnitByBlankId(GameObject btn, uint unitId){
//        LogHelper.Log("showUnitByUnitId(), unitId {0}", unitId);
        ShowUnitById(btn, unitId, null);
        DealAfterShowUnit(buttonDic[btn]);
    }

    private void ShowUnitById(GameObject btn, uint unitId, TUserUnit userUnit){
        LogHelper.Log("ShowUnitById(), unitId {0}, userUnit not null {1} btn not null {2}", unitId, userUnit != null, btn != null);
        // 
        UITexture texture = btn.transform.FindChild("Texture").GetComponent<UITexture>();
        UISprite background = btn.transform.FindChild("Background").GetComponent<UISprite>();
        background.spriteName = string.Empty;

        UILabel label = btn.transform.FindChild("Label").GetComponent<UILabel>();
        label.text = string.Empty;

        texture.mainTexture = DataCenter.Instance.GetUnitInfo(unitId).GetAsset(UnitAssetType.Avatar);

        if (userUnit != null){
            SetAddInfo(btn, userUnit);
        }
        SyncGachaInfos();
    }

    private void ResetOneBtn(GameObject btn){
        UITexture texture = btn.transform.FindChild("Texture").GetComponent<UITexture>();
        UISprite background = btn.transform.FindChild("Background").GetComponent<UISprite>();
        background.spriteName = "playerInfoMsg";
        
        UILabel label = btn.transform.FindChild("Label").GetComponent<UILabel>();
        label.text = TextCenter.Instace.GetCurrentText("Open");
        
        texture.mainTexture = null;
    }

    private void SetAddInfo(GameObject btn, TUserUnit userUnit){
        //
        LogHelper.Log("SetAddInfo() ");
    }

    private void DealAfterShowUnit(int index){
        LogHelper.Log("DealAfterShowUnit(), index {0}", index);
        if (tryCount < DataCenter.maxGachaPerTime){
            pickedBtnIdList.Add(index);
            tryCount += 1;
        }
        LogHelper.Log("DealAfterShowUnit(), tryCount {0}", tryCount);
        if (tryCount == gachaInfo.totalChances){
        }
        else if (tryCount == DataCenter.maxGachaPerTime){
            FinishShowGachaWindow();
        }
        else {

        }
    }

    private void FinishShowGachaWindow(){
        LogHelper.Log("FinishShowGachaWindow()");
        UIManager.Instance.ChangeScene(SceneEnum.Scratch);
    }
}
