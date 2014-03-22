using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class GachaWindowView : UIComponentUnity {
    private UILabel chancesLabel;
    private UILabel titleLabel;
    private bool displayingResult = false; // when 
    private int tryCount = 0;
    private GachaWindowInfo gachaInfo;
    private List<int> pickedGridIdList = new List<int>();

    private Dictionary<GameObject, int> gridDict = new Dictionary<GameObject, int>();

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

//    private UIButton FindButtonById(int id){
//        return FindChild<UIButton>("Board/Buttons/Button" + id);
//    }

    private void InitUI() {
        titleLabel = FindChild<UILabel>("TitleBackground/TitleLabel");
        chancesLabel = FindChild<UILabel>("TitleBackground/ChancesLabel");

        float side = 180.0f;
        UISprite bg = FindChild<UISprite>("Board/Bg");
        for (int i = 0; i < DataCenter.maxGachaPerTime; i++){
            GameObject gachaGrid = Resources.Load("Prefabs/UI/Scratch/GachaGrid") as GameObject;
            gachaGrid = NGUITools.AddChild(bg.gameObject, gachaGrid);
            UIButton button = gachaGrid.GetComponent<UIButton>();
            button.gameObject.transform.localPosition = new Vector3(-side + (i % 3) * side, side - (i / 3) * side, 0);
            gridDict.Add( button.gameObject, i);
            UIEventListener.Get( button.gameObject ).onClick = ClickButton;
        }
    }
    
//    private UITexture GetTextueByBtn(){
//
//    }

    private void SetMenuBtnEnable(bool enable){
        MsgCenter.Instance.Invoke(CommandEnum.EnableMenuBtns, enable);
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
        int index = gridDict[btn];
        if (pickedGridIdList.Contains(index)){
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
        pickedGridIdList.Clear();

//        foreach (var item in gridDict) {
//            ResetOneBtn(item.Key);
//        }
    }

    private void ShowUnitByUserUnitID(GameObject btn, uint uniqueId){
//        LogHelper.Log("showUnitByUserUnit(), uniqueId {0}", uniqueId);
        TUserUnit userUnit = DataCenter.Instance.MyUnitList.GetMyUnit(uniqueId);
        ShowUnitById(btn, userUnit.UnitInfo.ID, userUnit);
        DealAfterShowUnit(gridDict[btn]);
    }

    private void ShowUnitByBlankId(GameObject btn, uint unitId){
//        LogHelper.Log("showUnitByUnitId(), unitId {0}", unitId);
        ShowUnitById(btn, unitId, null);
        DealAfterShowUnit(gridDict[btn]);
    }

    private void ShowUnitById(GameObject btn, uint unitId, TUserUnit userUnit){
        LogHelper.Log("ShowUnitById(), unitId {0}, userUnit not null {1} btn not null {2}", unitId, userUnit != null, btn != null);
        // 
        UILabel label = btn.transform.FindChild("Label").GetComponent<UILabel>();
        label.text = string.Empty;
        UISprite background = btn.transform.FindChild("Background").GetComponent<UISprite>();
        background.spriteName = string.Empty;


        UITexture texture = btn.transform.FindChild("Cell/Texture").GetComponent<UITexture>();
        TUnitInfo currentUnitInfo;
        int level;

        if (userUnit != null){
            currentUnitInfo = userUnit.UnitInfo;
            level = userUnit.Level;
            SetAddInfo(btn, userUnit);
        }
        else {
            currentUnitInfo = DataCenter.Instance.GetUnitInfo(unitId);
            level = 1;
        }
        texture.mainTexture = currentUnitInfo.GetAsset(UnitAssetType.Avatar);
        UILabel rightBottom = btn.transform.FindChild("Cell/Label_Right_Bottom").GetComponent<UILabel>();
        rightBottom.text = TextCenter.Instace.GetCurrentText("Lv", level);

        SyncGachaInfos();
    }

    private void SetAddInfo(GameObject btn, TUserUnit userUnit){
        //
        LogHelper.Log("SetAddInfo() ");
    }

    private void DealAfterShowUnit(int index){
        LogHelper.Log("DealAfterShowUnit(), index {0}", index);
        if (tryCount < DataCenter.maxGachaPerTime){
            pickedGridIdList.Add(index);
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
