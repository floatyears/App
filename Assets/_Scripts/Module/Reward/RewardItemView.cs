﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class RewardItemView : MonoBehaviour {

//	private GameObject mask;
	private GameObject btn;
	private UILabel text;

	private bool inited = false;

	private List<GameObject> itemList;

	private static GameObject prefab;

	private UIAtlas atlas;
	public static GameObject Prefab{
		get{
			if(prefab == null){
				string sourcePath = "Prefabs/UI/Reward/RewardItem";
				prefab = ResourceManager.Instance.LoadLocalAsset(sourcePath, null) as GameObject;
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

				atlas = transform.FindChild ("Item3/Img").gameObject.GetComponent<UISprite>().atlas;

				btn = transform.FindChild("OkBtn").gameObject;
//				mask = transform.FindChild("Mask").gameObject;
				text = transform.FindChild("Label").GetComponent<UILabel>();



				transform.FindChild("OkBtn/Label").GetComponent<UILabel>().text = TextCenter.GetText("Reward_Take");
				//Debug.Log("scroll view: " + FindObjectOfType<UIScrollView>());
				btn.GetComponent<UIDragScrollView>().scrollView = FindObjectOfType<UIScrollView>();

//				atlas = ResourceManager.Instance.LoadLocalAsset("Atlas/Atlas_Login",
				inited = true;
			}

//			Debug.Log("id: " +data.id);
			if(data.enabled == 1){
//				mask.SetActive(false);
//				btn.GetComponent<BoxCollider>().enabled = true;
				btn.GetComponent<UIButton>().isEnabled = true;
			}else{
//				mask.SetActive(true);
//				btn.GetComponent<BoxCollider>().enabled = false;
				btn.GetComponent<UIButton>().isEnabled = false;
			}


			int index = 0;
			foreach (var item in data.giftItem) {
				if(item.count > 0){
//					GiftItem gd = item//data.giftItem[index];
					itemList[index].SetActive(true);
					SetItemData(itemList[index], item);
					index++;
//					Debug.Log("foreach item: " + item);
				}
					
			}
			for (int i = index; i < 3; i++) {
				itemList[i].SetActive(false);
			}

			switch (data.type) {
			case 2://EBonusType.CHAIN_LOGIN:
				text.text = string.Format(TextCenter.GetText("ChainLogin"),data.matchValue);
				break;
			case 1://EBonusType.TOTAL_LOGIN:
				text.text = string.Format(TextCenter.GetText("TotalLogin"),data.matchValue);
				break;
			case 3://EBonusType.RANK_REACH:
				text.text = string.Format(TextCenter.GetText("RankReach"),data.matchValue);
				break;
			case 6://EBonusType.COMPENSATION:
			case 5:
				text.text = string.Format(TextCenter.GetText("RewardMonthCard"),data.matchValue);
				break;
			default:
				text.text = string.Format(TextCenter.GetText("OtherRewards"),data.matchValue);
				break;
			break;
			}

//			Debug.Log("reward item: " + data.id + " gift: " + data.giftItem.Count);
		}
	}

	private void SetItemData(GameObject obj, GiftItem gift){
//		Debug.Log ("gift count: " + gift.count);
		if (gift.count <= 0) {
			obj.SetActive(false);
			return;
		}
		if (gift.content == (int)EGiftContent.UNIT) {
			UIEventListenerCustom.Get (obj).LongPress = ClickUnit;
			obj.GetComponent<BoxCollider>().enabled = true;// obj.GetComponent<BoxCollider>()
			obj.GetComponent<UIEventListenerCustom>().enabled = true;
			obj.GetComponent<UIDragScrollView>().enabled = true;

			ResourceManager.Instance.GetAvatarAtlas((uint)gift.value, obj.transform.FindChild("Img").GetComponent<UISprite>());
			int type = (int)DataCenter.Instance.GetUnitInfo((uint)gift.value).Type;
			obj.transform.FindChild("Bg").GetComponent<UISprite>().spriteName = GetAvatarBgSpriteName(type);
			obj.transform.FindChild("Border").GetComponent<UISprite>().spriteName = GetBorderSpriteName(type);


		} else {
			UIEventListenerCustom.Get (obj).LongPress = null;
			obj.GetComponent<BoxCollider>().enabled = false;// obj.GetComponent<BoxCollider>()
			obj.GetComponent<UIEventListenerCustom>().enabled = false;
			obj.GetComponent<UIDragScrollView>().enabled = false;

			UISprite sp = obj.transform.FindChild("Img").GetComponent<UISprite>();
			sp.atlas = atlas;
			sp.spriteName = "icon_" + gift.content;

			obj.transform.FindChild("Bg").GetComponent<UISprite>().spriteName = "";
			obj.transform.FindChild("Border").GetComponent<UISprite>().spriteName = "";
		}

		obj.transform.FindChild ("Num").GetComponent<UILabel>().text = "x" + gift.count;
//		ResourceManager.Instance.LoadLocalAsset("Texture/NoviceGuide/Gold", o =>{
//			obj.transform.FindChild ("Img").GetComponent<UITexture>().mainTexture = o as Texture2D; 
//		});
	}

	string GetBorderSpriteName (int unitType) {
		switch (unitType) {
		case 1:
			return "avatar_border_fire";
		case 2:
			return "avatar_border_water";
		case 3:
			return "avatar_border_wind";
		case 4:
			return "avatar_border_light";
		case 5:
			return "avatar_border_dark";
		case 6:
			return "avatar_border_none";
		default:
			return "avatar_border_none";
			break;
		}
	}

	string GetAvatarBgSpriteName(int unitType) {
		switch (unitType) {
		case 1:
			return "avatar_bg_fire";
		case 2:
			return "avatar_bg_water";
		case 3:
			return "avatar_bg_wind";
		case 4:
			return "avatar_bg_light";
		case 5:
			return "avatar_bg_dark";
		case 6:
			return "avatar_bg_none";
		default:
			return "avatar_bg_none";
			break;
		}
	}


	private void ClickUnit(GameObject obj){

		Debug.Log ("Click Item To Detail");
		uint i = 0; 
		uint.TryParse(obj.transform.FindChild("Img").GetComponent<UISprite>().spriteName,out i);
		DGTools.ChangeToUnitDetail (i);


	}
//
//	private void SetUnitClick(GameObject obj, bool IsUnit){
//
//	}

	public void ClickTakeAward(){
		AudioManager.Instance.PlayAudio( AudioEnum.sound_click );

		RewardView.bonusIDs.Add (data.id);
		MsgCenter.Instance.Invoke(CommandEnum.TakeAward,data);
	}

}