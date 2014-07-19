using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class GachaWindowView : UIComponentUnity {
	private const int rareAudioLevel = 4;

    private UILabel chancesLabel;
    private UILabel titleLabel;
    private bool displayingResult = false; //when
    private uint currentUid = 0;
    private int tryCount = 0;
    private GachaWindowInfo gachaInfo;
    private List<int> pickedGridIdList = new List<int>();
    private List<GameObject> clickedGrids = new List<GameObject>();

    private Dictionary<GameObject, int> gridDict = new Dictionary<GameObject, int>();
    private Dictionary<GameObject, TUserUnit> gridUnitDict = new Dictionary<GameObject, TUserUnit>();

    public override void Init ( UIInsConfig config, IUICallback origin ) {
        base.Init (config, origin);
        InitUI();
    }

    public override void ResetUIState() {
        base.ResetUIState();
        SetActive(false);
      
    }
    
    public override void ShowUI () {
        base.ShowUI ();
        AddListener();
//		Debug.LogError ("show ui : " + UIManager.Instance.baseScene.PrevScene);
		if (UIManager.Instance.baseScene.PrevScene == SceneEnum.UnitDetail || UIManager.Instance.baseScene.PrevScene == SceneEnum.ShowCardEffect) {
			MsgCenter.Instance.Invoke(CommandEnum.BackSceneEnable, false);	
			ShowUnitGrid();
		}
	}
    
    public override void HideUI () {
        base.HideUI ();
        RemoveListener();
		CloseChooseGachaWindow ();
		SetMenuBtnEnable(true);
		Reset();
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

    public override void CallbackView(object data) {
        base.CallbackView(data);
        
        CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
        
        switch (cbdArgs.funcName) {
	        case "SetTitleView": 
	            CallBackDispatcherHelper.DispatchCallBack(SetTitleLabel, cbdArgs);
	            break;
	        default:
	            break;
        }
    }

    private void InitUI() {
        titleLabel = FindChild<UILabel>("TitleBackground/TitleLabel");
        chancesLabel = FindChild<UILabel>("TitleBackground/ChancesLabel");

        float width = 140.0f;
		float height = 150.0f;
        UISprite bg = FindChild<UISprite>("Board/Bg");
		ResourceManager.Instance.LoadLocalAsset ("Prefabs/UI/Scratch/GachaGrid", o => {
			GameObject gachaGrid = o as GameObject;
			for (int i = 0; i < DataCenter.maxGachaPerTime; i++) {
				GameObject go = NGUITools.AddChild (bg.gameObject, gachaGrid);
				go.transform.localPosition = new Vector3 (-width + (i % 3) * width, height - (i / 3) * height, 0);
				gridDict.Add (go, i);
				UIEventListener.Get (go).onClick = ClickButton;
			}
		});
    }

    private Texture2D GetChessStarTextureByRareLevel(int rare) {
		if (rare >= 4) {
			AudioManager.Instance.PlayAudio(AudioEnum.sound_card_4);
		}
        string path = string.Format("Texture/ChessStar{0}", rare);
        Texture2D texture = ResourceManager.Instance.LoadLocalAsset (path, null) as Texture2D;
        return texture;
    }
    
    private void SetMenuBtnEnable(bool enable) {
        MsgCenter.Instance.Invoke(CommandEnum.EnableMenuBtns, enable);
    }

    private void SetTitleLabel(object args) {
        string titleText = args as string;
        titleLabel.text = titleText;
    }

    private void CloseChooseGachaWindow(){
        MsgCenter.Instance.Invoke(CommandEnum.CloseMsgWindow);
    }

    private void Enter(object args){
		CloseChooseGachaWindow ();
		SetMenuBtnEnable(false);
        SetActive(true);
        MsgCenter.Instance.Invoke(CommandEnum.BackSceneEnable, false);
        SetMenuBtnEnable(false);
        GachaWindowInfo gachaWindowInfo = args as GachaWindowInfo;
        if (gachaWindowInfo != null){
            gachaInfo = gachaWindowInfo;
            SyncGachaInfosAtStart();
        }

		NoviceGuideStepEntityManager.Instance ().StartStep (NoviceGuideStartType.SCRATCH);
    }

    private void SyncGachaInfosAtStart(){
        chancesLabel.text = TextCenter.GetText("GachaChances", 0, gachaInfo.totalChances);
        string title = "";
        switch (gachaInfo.gachaType) {
        case GachaType.FriendGacha:
            title = TextCenter.GetText("FriendGachaTitle"); 
            break;
        case GachaType.RareGacha:
            title = TextCenter.GetText("RareGachaTitle"); 
            break;
        case GachaType.EventGacha:
            title = TextCenter.GetText("EventGachaTitle"); 
            break;
        default:
            break;
        }
        titleLabel.text = title;
    }

    private void SyncGachaInfos(){
        int nowChance = Mathf.Min(GetTryCount() + 1, gachaInfo.totalChances);
        chancesLabel.text = TextCenter.GetText("GachaChances", nowChance, gachaInfo.totalChances);
    }

    private int GetTryCount(){
        return clickedGrids.Count;
    }

    private bool GridAlreadyClicked(GameObject grid){
        return clickedGrids.Contains(grid);
    }

    private void ClickButton(GameObject grid){
        if (GetTryCount() >= gachaInfo.totalChances){
            return;
        }
        if (GridAlreadyClicked(grid)) {
            return;
        }
        int index = gridDict[grid];
        if (pickedGridIdList.Contains(index)){
            return;
        }
        AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
        LogHelper.Log("ClickButton() {0}", index);
        if (GetTryCount() >= gachaInfo.totalChances){
            return;
        }
        StartShowGachaGridResult(grid, gachaInfo.unitList[GetTryCount()]);
    }

    private void Reset(){
//		Debug.LogError (" reset ");
        displayingResult = false;
        clickedGrids.Clear();
        gridUnitDict.Clear();
        currentUid = 0;
        tryCount = 0;
        pickedGridIdList.Clear();
        foreach (var item in gridDict) {
            ResetOneGrid(item.Key as GameObject);
        }
    }

    private void ResetOneGrid(GameObject grid){
        UILabel label = grid.transform.FindChild("Label").GetComponent<UILabel>();
        label.text = TextCenter.GetText("Open");
        UISprite background = grid.transform.FindChild("Cell/Background").GetComponent<UISprite>();
        background.gameObject.SetActive(true);
        UISprite texture = grid.transform.FindChild("Cell/Texture").GetComponent<UISprite>();
        texture.spriteName = "";
        UILabel rightBottom = grid.transform.FindChild("Cell/Label_Right_Bottom").GetComponent<UILabel>();
        rightBottom.text = string.Empty;
    }

    private void StartShowGachaGridResult(GameObject grid, uint uniqueId){
        LogHelper.Log("StartShowGachaGridResult()");
        if (GridAlreadyClicked(grid)){
            return;
        }

        clickedGrids.Add(grid);
        currentUid = uniqueId;
        LogHelper.Log("StartCoroutine, ShowUnitRareById(), currentUid {0}, currenGrid", currentUid);
        StartCoroutine(ShowUnitRareById(grid));
    }

    private void EndShowGachaGridResult(){
        LogHelper.Log("EndShowGachaGridResult()");
    }

    private void ShowUnitByUserUnitID(GameObject btn, uint uniqueId){
        TUserUnit userUnit = DataCenter.Instance.UserUnitList.GetMyUnit(uniqueId);
        ShowUnitById(btn, userUnit.UnitInfo.ID, userUnit);
        DealAfterShowUnit(gridDict[btn]);
    }

    private void ShowUnitByBlankId(GameObject btn, uint unitId){
        ShowUnitById(btn, unitId, null);
        DealAfterShowUnit(gridDict[btn]);
    }

    IEnumerator ShowUnitRareById(GameObject grid){
        LogHelper.Log("ShowUnitRareById(), currentUid {0}", currentUid);
        yield return new WaitForSeconds(0.5f);
        TUserUnit userUnit = DataCenter.Instance.UserUnitList.GetMyUnit(currentUid);

        UILabel label = grid.transform.FindChild("Label").GetComponent<UILabel>();
        label.text = string.Empty;
        UISprite background = grid.transform.FindChild("Cell/Background").GetComponent<UISprite>();
        background.gameObject.SetActive(false);
        gridUnitDict.Add(grid, userUnit);
        EndShowGachaGridResult();
        DealAfterShowUnit(gridDict[grid]);
    }

    private void ShowUnitById(GameObject grid, uint unitId, TUserUnit userUnit = null){
        LogHelper.Log("ShowUnitById(), unitId {0}, userUnit not null= {1} btn not null= {2}", unitId, userUnit != null, grid != null);
        // 
        UILabel label = grid.transform.FindChild("Label").GetComponent<UILabel>();
        label.text = string.Empty;
        UISprite background = grid.transform.FindChild("Cell/Background").GetComponent<UISprite>();

		UISprite texture = grid.transform.FindChild("Cell/Texture").GetComponent<UISprite>();
        TUnitInfo currentUnitInfo;
        int level;

        if (userUnit != null){
            currentUnitInfo = userUnit.UnitInfo;
            level = userUnit.Level;
            SetAddInfo(grid, userUnit);
        }
        else {
            currentUnitInfo = DataCenter.Instance.GetUnitInfo(unitId);
            level = 1;
        }

		DataCenter.Instance.GetAvatarAtlas (currentUnitInfo.ID, texture);
//		currentUnitInfo.GetAsset(UnitAssetType.Avatar, o=>{
//			texture.mainTexture = o as Texture2D;
//		});
        UILabel rightBottom = grid.transform.FindChild("Cell/Label_Right_Bottom").GetComponent<UILabel>();
        rightBottom.text = TextCenter.GetText("Lv", level);

        SyncGachaInfos();
    }

    private void SetAddInfo(GameObject btn, TUserUnit userUnit){
        //
        LogHelper.Log("SetAddInfo() ");
    }

    private void DealAfterShowUnit(int index){
        LogHelper.Log("DealAfterShowUnit(), index {0}, GetTryCount() {1}", index, GetTryCount());
        if (GetTryCount() < DataCenter.maxGachaPerTime){
            pickedGridIdList.Add(index);
        }
        LogHelper.Log("DealAfterShowUnit(), GetTryCount() {0}, maxGachaPerTime {1}", GetTryCount(), DataCenter.maxGachaPerTime);

        tryCount += 1;

        if (tryCount >= DataCenter.maxGachaPerTime || tryCount == gachaInfo.totalChances){
            LogHelper.Log("DealAfterShowUnit(), GetTryCount() {0}", GetTryCount());
            StartAutoShowFinalResult();
//            FinishShowGachaWindow();
        }
//        else if (GetTryCount() == gachaInfo.totalChances){
//            AutoShowOpenBlankUnit();
//        }
        else {
            LogHelper.Log("DealAfterShowUnit() last case, GetTryCount() {0}, maxGachaPerTime {1}", GetTryCount(), DataCenter.maxGachaPerTime);
        }
    }

    GameObject GetFirstRestGrid(){
        GameObject obj = null;
        for (int i = 0; i < DataCenter.maxGachaPerTime; i++){
            if (!pickedGridIdList.Contains(i)){
                return GetGridById(i);
            }
        }
        return obj;
    }

    GameObject GetGridById(int index){
        foreach (var item in gridDict) {
            if ((int)item.Value == index) {
                return item.Key as GameObject;
            }
        }
        return null;
    }

    private void AutoShowOpenBlankUnit(){
        List<GameObject> lastGrids = new List<GameObject>();
        StartCoroutine(ShowOpenBlankUnit());
    }

    private void StartAutoShowFinalResult(){
		sortedGrids = GetSortedGrids();
		ShowUnitGrid ();
    }

	void ShowUnitGrid() {
		if (showIndex < gachaInfo.totalChances) {
			GameTimer.GetInstance ().AddCountDown (0.6f, ShowUnitByGrid);
		} else {
			FinishShowGachaWindow();		
		}
	}

	int showIndex = 0;
	List<GameObject> sortedGrids = null;

    void ShowUnitByGrid() {
		AudioManager.Instance.PlayAudio (AudioEnum.sound_grid_turn);

		GameObject grid = sortedGrids[showIndex];
    	ShowUnitById(grid, gridUnitDict[grid].UnitInfo.ID, gridUnitDict[grid]);
		showIndex += 1;
		TUserUnit currentUserunit = gridUnitDict [grid];
		bool have = DataCenter.Instance.CatalogInfo.IsHaveUnit (currentUserunit.UnitID);
		if (have) {
			ShowUnitGrid ();
		} else {
			DataCenter.Instance.CatalogInfo.AddHaveUnit(currentUserunit.Object.unitId);
			UIManager.Instance.ChangeScene(SceneEnum.ShowCardEffect);
			MsgCenter.Instance.Invoke(CommandEnum.ShowNewCard, currentUserunit);
		}
    }

    void ShowNewUnitDetail(int index){
        uint newUnitId = gachaInfo.newUnitIdList[index];
        if (newUnitId == 0){
            return;
        }
        UIManager.Instance.ChangeScene (SceneEnum.UnitDetail);
        TUserUnit unit = DataCenter.Instance.UserUnitList.GetMyUnit(newUnitId);
        MsgCenter.Instance.Invoke (CommandEnum.ShowUnitDetail, unit);
    }

    List<GameObject> GetSortedGrids(){
        List<GameObject> ret = new List<GameObject>();
        for (int i = 0; i < DataCenter.maxGachaPerTime; i++) {
            for (int j = 0; j < clickedGrids.Count; j++) {
                if (gridDict[clickedGrids[j]] == i){
                    ret.Add(clickedGrids[j]);
                    LogHelper.Log("GetSortedGrids(), i {0}", i);
                }
            }
        }
        return ret;
    }

    IEnumerator ShowOpenBlankUnit(){
        while(GetTryCount() < DataCenter.maxGachaPerTime){
            yield return new WaitForSeconds(0.6f);
            int blankId = GetTryCount() - gachaInfo.totalChances;
            if (blankId < gachaInfo.blankList.Count && blankId >= 0){
                Debug.Log("ShowOpenBlankUnit() yield blankId " + blankId);
                ShowUnitByBlankId(GetFirstRestGrid(), gachaInfo.blankList[blankId]);
            }
        }
        yield return null;
    }

    IEnumerator LastOperation(){
        yield return new WaitForSeconds(1f);
		showIndex = 0;
        UIManager.Instance.ChangeScene(SceneEnum.Scratch);
        yield return null;
    }
    
    private void FinishShowGachaWindow(){
//        LogHelper.Log("FinishShowGachaWindow()");
        StartCoroutine(LastOperation());
    }

    private void SetActive(bool active){
        this.gameObject.SetActive(active);
    }
}
