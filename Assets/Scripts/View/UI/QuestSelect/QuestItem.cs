using UnityEngine;
using System.Collections;
using bbproto;

public class QuestItem : MonoBehaviour {
	private UISprite bossAvatarSpr;
	private UISprite bossTypeSpr;
	private UISprite erotemeSpr;
	private UISprite maskSpr;
	private UISprite focusSpr;
	private UILabel clearFlagLabel;
	private UILabel questPosLabel;

	public static QuestItem Inject(GameObject prefab){
		QuestItem questItem = prefab.GetComponent<QuestItem>();
		if(questItem == null) prefab.AddComponent<QuestItem>();
		return questItem;
	}

	private void Awake(){
		bossAvatarSpr = transform.FindChild("Sprite_Avatar").GetComponent<UISprite>();
		bossTypeSpr = transform.FindChild("Sprite_Type").GetComponent<UISprite>();
		erotemeSpr = transform.FindChild("Sprite_Eroteme").GetComponent<UISprite>();
		maskSpr = transform.FindChild("Sprite_Mask").GetComponent<UISprite>();
		focusSpr = transform.FindChild("Sprite_Focus").GetComponent<UISprite>();
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

				uint bossID = data.BossID[ 0 ];
				bossAvatarSpr.atlas = DataCenter.Instance.GetAvatarAtlas(bossID);
				bossAvatarSpr.spriteName = bossID.ToString();

				switch (data.state) {
					case EQuestState.QS_NEW : 
						IsClear = false;
						break;
					case EQuestState.QS_CLEARED :
						IsClear = true;
						break;
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

	private bool isFocus;
	public bool IsFocus{
		get{
			return isFocus;
		}
		set{
			isFocus = value;
			if(isFocus){
				focusSpr.enabled = true;
			}
			else{
				focusSpr.enabled = false;
			}
		}
	}

	private int position;
	public int Position{
		get{
			return position;
		}
		set{
			position = value;
			if(data == null) {
				questPosLabel.text = string.Empty;
			}
			questPosLabel.text = string.Format("Quest : " + position);
		}
	}

	private  static GameObject itemPrefab;
	public static GameObject ItemPrefab{
		get{
			if(itemPrefab == null) {
				string sourcePath = "Prefabs/UI/Quest/QuestPrefab";
				itemPrefab = Resources.Load(sourcePath) as GameObject ;
			}
			return itemPrefab;
		}
	}

	public delegate void QuestItemCallback(QuestItem questItem);
	public QuestItemCallback callback;
	private void ClickItem(GameObject item){
		if(callback != null) callback(this);
	}
	
}
