using UnityEngine;
using System.Collections;
using bbproto;

public class QuestItem : MonoBehaviour {
	private UISprite bossAvatarSpr;
	private UISprite erotemeSpr;
	private UISprite maskSpr;
	private UILabel clearFlagLabel;
	private UILabel questPosLabel;

	/// <summary>
	/// The panel which use to show the info that current picked.
	/// </summary>
	private QuestSelectView infoPanel;

	public static QuestItem Inject(GameObject prefab){
		QuestItem questItem = prefab.GetComponent<QuestItem>();
		if(questItem == null) prefab.AddComponent<QuestItem>();
		return questItem;
	}

	private void Awake(){
		bossAvatarSpr = transform.FindChild("Sprite_Avatar").GetComponent<UISprite>();
		erotemeSpr = transform.FindChild("Sprite_ErotemeSpr").GetComponent<UISprite>();
		maskSpr = transform.FindChild("Sprite_Mask").GetComponent<UISprite>();
		clearFlagLabel = transform.FindChild("Label_Clear").GetComponent<UILabel>();
		questPosLabel = transform.FindChild("Label_Pos").GetComponent<UILabel>();
	}

	private TQuestInfo data;
	public TQuestInfo Data{
		get{
			return data;
		}
		set{
			data = value;
			//binding data with view
			if(data == null){
				//do some work about clear
			}
			else{
				//do some view show by QuestClear state
				switch (data.state) {
					case EQuestState.QS_NEW : 
						IsClear = false;
						break;
					case EQuestState.QS_CLEARED :
						IsClear = true;
					default:
						break;
				}
			}
		}
	}

	private bool isClear;
	public bool IsClear{
		get{
			return isClear;
		}
		set{
			isClear = value;
			if(isClear){
				clearFlagLabel.text = "Clear";
				clearFlagLabel.color = Color.yellow;
				erotemeSpr.enabled = false;
				maskSpr.enabled = false;
				UIEventListener.Get(gameObject).onClick = null;
			}
			else{
				clearFlagLabel.text = "New";
				clearFlagLabel.color = Color.green;
				erotemeSpr.enabled = true;
				maskSpr.enabled = true;
				UIEventListener.Get(gameObject).onClick = ClickItem;
			}
		}
	}

	private  static GameObject itemPrefab;
	public static GameObject ItemPrefab{
		get{
			if(itemPrefab == null) {
				string sourcePath = "Prefabs/UI/UnitItem/QuestPrefab";
				itemPrefab = Resources.Load(sourcePath) as GameObject ;
			}
			return itemPrefab;
		}
	}
	
	private void ClickItem(GameObject item){
		Debug.Log("Click Quest Item : " + item.name);
		infoPanel.ShowInfoPanelContent(data);
	}
	
}
