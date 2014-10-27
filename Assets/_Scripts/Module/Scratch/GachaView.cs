using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

//
//internal class GachaWindowInfo{
//	public GachaType gachaType;
//	public int totalChances = 1;
//	public List<uint> blankList = new List<uint>();
//	public List<uint> unitList = new List<uint>();
//	public List<uint> newUnitIdList = new List<uint>();
//}

public class GachaView : ViewBase {
	private const int rareAudioLevel = 4;
	
    private UILabel chancesLabel;
    private UILabel titleLabel;
    private bool displayingResult = false; //when
    private uint currentUid = 0;
    private int tryCount = 0;
//    private GachaWindowInfo gachaInfo;
    private List<int> pickedGridIdList = new List<int>();

    private Dictionary<GameObject, int> gridDict = new Dictionary<GameObject, int>();

	private List<uint> blankList;
	private GachaType gachaType;
	private List<uint> newUnitIdList;
	private int totalChances;
	private List<uint> unitList;
	
	int showIndex = 0;
	private GameObject currGrid;

	public override void Init ( UIConfigItem config, Dictionary<string, object> data = null ) {
        base.Init (config, data);

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
				go.name = i.ToString();
			}
		});
    }

	public override void ShowUI ()
	{
		base.ShowUI ();

		if (viewData != null) {
			if(viewData.ContainsKey("type")){
				Reset ();
				titleLabel.text = viewData["type"].ToString();
				
				blankList = viewData["blank"] as List<uint>;
				gachaType = (GachaType)viewData["type"];
				newUnitIdList = viewData["new"] as List<uint>;
				totalChances = (int)viewData["chances"];
				unitList = viewData["unit"] as List<uint>;
				
				chancesLabel.text = TextCenter.GetText("GachaChances", 0, totalChances);
				string title = "";
				switch (gachaType) {
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
				
				if (totalChances == 1) { //1 == user only can gacha ones
					foreach (var item in gridDict) {
						UIEventListenerCustom.Get (item.Key).onClick = ClickButton;
					}
				} else {
					AutoShowOneCard();
				}
				
//				NoviceGuideStepManager.Instance.StartStep (NoviceGuideStartType.SCRATCH);	
			}else if(viewData.ContainsKey("from")){
				AutoShowOneCard();
			}

		}
	}


	void AutoShowOneCard() {
		if (showIndex == totalChances || showIndex == unitList.Count) {
			showIndex = 0;
			ModuleManager.Instance.HideModule (ModuleEnum.GachaModule);
			ModuleManager.Instance.ShowModule (ModuleEnum.ScratchModule);
			return;
		}

		chancesLabel.text = (totalChances - showIndex - 1).ToString () + "/" + totalChances;
		uint uniqueID = unitList [showIndex];

		foreach (var item in gridDict) {
			if(item.Value == showIndex) {
				ShowInfo(item.Key, uniqueID);
				break;
			}
		}
	}

	void ShowInfo(GameObject grid, uint uniqueID) {
		showIndex += 1;
		currGrid = grid;
		currentUserUnit = DataCenter.Instance.UnitData.UserUnitList.GetMyUnit(uniqueID);
		sprite = currGrid.transform.FindChild("Cell/Texture").GetComponent<UISprite>();
		UnitInfo tui = currentUserUnit.UnitInfo;
		ResourceManager.Instance.GetAvatarAtlas(tui.id, sprite, o => {
			sprite.enabled = false;
		});

		iTween.ScaleTo (currGrid, iTween.Hash ("y", 0f, "time", 0.3f, "oncomplete", "RecoverScale", "oncompletetarget", gameObject));
	}

	UISprite sprite = null;
	UserUnit currentUserUnit = null;

	void RecoverScale() {
		currGrid.transform.FindChild ("Label").GetComponent<UILabel>().text = "";

		sprite.enabled = true;
		iTween.ScaleTo (currGrid, iTween.Hash ("y", 1f, "time", 0.3f, "oncomplete", "AnimEnd", "oncompletetarget", gameObject));
	}

	void AnimEnd() {
		AudioManager.Instance.PlayAudio (AudioEnum.sound_grid_turn);
		GameTimer.GetInstance ().AddCountDown (0.3f, () => {

			ModuleManager.Instance.ShowModule(ModuleEnum.ShowNewCardModule, "data",currentUserUnit);
			//			}
//			AutoShowOneCard ();
//			bool have = DataCenter.Instance.UnitData.CatalogInfo.IsHaveUnit (currentUserUnit.unitId);
//			if (have) {
//				AutoShowOneCard ();
//			} else {
//				DataCenter.Instance.UnitData.CatalogInfo.AddHaveUnit(currentUserUnit.unitId);
//				
		});
	}

    private void ClickButton(GameObject grid){
//		if (clickedGrids.Contains(grid)){
//			return;
//		}
//        int index = gridDict[grid];
//        if (pickedGridIdList.Contains(index)){
//            return;
//        }
//        AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
//        LogHelper.Log("ClickButton() {0}", index);
//		if (clickedGrids.Count >= totalChances){
//            return;
//        }
//		
//		clickedGrids.Add(grid);
//		currentUid = unitList[clickedGrids.Count-1];
//		StartCoroutine(ShowUnitRareById(grid, unitList[clickedGrids.Count-1]));
		if (showIndex > (unitList.Count - 1)) {
			return;
		}
		ShowInfo (grid,unitList[showIndex]);
    }

    private void Reset(){
//		Debug.LogError (" reset ");
        displayingResult = false;
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

}
