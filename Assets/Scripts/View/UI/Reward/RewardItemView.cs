using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class RewardItemView : MonoBehaviour {

	private GameObject mask;
	private GameObject btn;
	private UILabel text;

	private bool inited = false;

	private List<GameObject> itemList;

	private static GameObject prefab;
	public static GameObject Prefab{
		get{
			if(prefab == null){
				string sourcePath = "Prefabs/UI/Reward/RewardItem";
				prefab = Resources.Load(sourcePath) as GameObject;
			}
			return prefab;
		}
	}
	
	public static RewardItemView Inject(GameObject view){
		RewardItemView stageItemView = view.GetComponent<RewardItemView>();
		if(stageItemView == null) stageItemView = view.AddComponent<RewardItemView>();
		return stageItemView;
	}

	private BonusInfo data;
	public BonusInfo Data{
		get{return data;}
		set{
			data = value;

			if(!inited){
				itemList = new List<GameObject> ();
				itemList.Add (transform.FindChild ("Item1").gameObject);
				itemList.Add (transform.FindChild ("Item2").gameObject);
				itemList.Add (transform.FindChild ("Item3").gameObject);
				btn = transform.FindChild("OkBtn").gameObject;
				mask = transform.FindChild("Mask").gameObject;
				text = transform.FindChild("Label").GetComponent<UILabel>();
				//Debug.Log("scroll view: " + FindObjectOfType<UIScrollView>());
				btn.GetComponent<UIDragScrollView>().scrollView = FindObjectOfType<UIScrollView>();
				inited = true;
			}

			Debug.Log("id: " +data.id);
			if(data.enabled == 1){
				mask.SetActive(false);
				btn.GetComponent<BoxCollider>().enabled = true;
			}else{
				mask.SetActive(true);
				btn.GetComponent<BoxCollider>().enabled = false;
			}

			for(int i = 0; i < 3; i++){

				if(data.giftItem.Count > i){
					GiftItem gd = data.giftItem[i];
					//Debug.Log("count: " + itemList.Count);
					itemList[i].SetActive(true);
					SetItemData(itemList[i], gd);
					Debug.Log("is unit: " + (gd.content == (int)EGiftContent.UNIT)+" gd count:" + gd.count);
					SetUnitClick(itemList[i],gd.content == (int)EGiftContent.UNIT);
					          
				}else{
					itemList[i].SetActive(false);
				}
				
				
//				transform.
			}
			switch ((EBonusType)data.type) {
			case EBonusType.CHAIN_LOGIN:
				text.text = string.Format(TextCenter.GetText("ChainLogin"),data.matchValue);
				break;
			case EBonusType.TOTAL_LOGIN:
				text.text = string.Format(TextCenter.GetText("TotalLogin"),data.matchValue);
				break;
			case EBonusType.RANK_REACH:
				text.text = string.Format(TextCenter.GetText("RankReach"),data.matchValue);
				break;
			default:
				text.text = "";
			break;
			}

		}
	}

	private void SetItemData(GameObject obj, GiftItem gift){
		obj.transform.FindChild ("Num").GetComponent<UILabel>().text = "x" + gift.count;
		obj.transform.FindChild ("Img").GetComponent<UITexture>().mainTexture = Resources.Load("Texture/NoviceGuide/Gold") as Texture2D;
	}

	private void ClickUnit(GameObject obj){
		Debug.Log ("Click Item To Detail");
		int i; 
		int.TryParse(obj.name.Substring (4,1),out i);
		DGTools.ChangeToUnitDetail ((uint)data.giftItem [i - 1].value);
	}

	private void SetUnitClick(GameObject obj, bool IsUnit){
		if (IsUnit) {
			UIEventListenerCustom.Get (obj).onClick = ClickUnit;
			obj.GetComponent<BoxCollider>().enabled = true;// obj.GetComponent<BoxCollider>()
			obj.GetComponent<UIEventListenerCustom>().enabled = true;
			obj.GetComponent<UIDragScrollView>().enabled = true;

		} else {
			UIEventListenerCustom.Get (obj).onClick = null;
			obj.GetComponent<BoxCollider>().enabled = false;// obj.GetComponent<BoxCollider>()
			obj.GetComponent<UIEventListenerCustom>().enabled = false;
			obj.GetComponent<UIDragScrollView>().enabled = false;
		}
	}

	public void ClickTakeAward(){
		MsgCenter.Instance.Invoke(CommandEnum.TakeAward,data);
		RewardView.bonusIDs.Add (data.id);
	}

}
