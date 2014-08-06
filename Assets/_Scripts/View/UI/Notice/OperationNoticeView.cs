using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OperationNoticeView : UIComponentUnity {

	private bool firstShow = true;

	private List<GameObject> noticeList;

	private string sourcePath = "Prefabs/UI/Notice/OperationNoticeQuest";

	private string contentLabel = "Content/Text";
	private string titleLabel = "Title";

	private GameObject contentItem;

//	private Dictionary<string,string> contents;

	private GameObject content;

//	private GameObject okBtn;

	public override void Init(UIInsConfig config, IUICallback origin) {
		base.Init(config, origin);
		InitUI();
	}

	public override void ShowUI() {

		base.ShowUI();
//		Debug.Log ("show operation notice: " + config.localPosition.y);
		ShowUIAnimation ();
	}
	
	public override void HideUI() {
		//show Login Bonus

		base.HideUI();
//		Debug.Log ("hide operation notice: " + config.localPosition.y);
//		iTween.Stop (gameObject);
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}
	
	private void InitUI(){
		content = this.FindChild ("Content/Table");

		FindChild<UILabel> ("Title").text = TextCenter.GetText ("Text_Notice");
		FindChild<UILabel> ("OkBtn/Label").text = TextCenter.GetText ("OK");
//		okBtn = this.FindChild ("OkBtn");
//		//contents = new Dictionary<string, string> ();
//		UIokBtn

		ResourceManager.Instance.LoadLocalAsset (sourcePath,o =>{

			GameObject prefab = o as GameObject;
			if (DataCenter.Instance.NoticeInfo != null && DataCenter.Instance.NoticeInfo.NoticeList != null) {
//				Debug.Log("operation notice");
//				DataCenter.Instance.NoticeInfo.NoticeList.Reverse();
				foreach (var nItem in DataCenter.Instance.NoticeInfo.NoticeList) {
					GameObject item = NGUITools.AddChild(content,prefab);
					
					LogHelper.Log("------operation notice transform:" + item);

					SetItemContent(item,nItem.title,nItem.message);
				}


				
//				LogHelper.Log("------operation notice transform:" + item);
				bbproto.StatHelperCount data = DataCenter.Instance.HelperCount;
				if(data != null){
					GameObject item1 = NGUITools.AddChild(content,prefab);
					SetItemContent(item1,TextCenter.GetText("Notice_HelperTitle"),string.Format(TextCenter.GetText("Notice_HelperContent"),DataCenter.Instance.LoginInfo.LoginDayTotal, data.helpFriendCount,data.helpHelperCount,data.friendPointGet));
//					item1.transform.FindChild(titleLabel).GetComponent<UILabel>().text = ;//data.title;
//					item1.transform.FindChild(contentLabel).GetComponent<UILabel>().text = ;//nItem.message;
				}
			}
		});
		

//		contents.Add ("notice1", "content1\ndsf \ndsafsd \nasdfs");
//		contents.Add ("notice2", "content2\n sdf as \nsdfas\nadsfs\nasasdf");
//		contents.Add ("notice3", "content2\n sdf as \nsdfas\nadsfs\nasasdf");
//		contents.Add ("notice4", "content2\n sdf as \nsdfas\nadsfs\nasasdf");
//		contents.Add ("notice5", "content2\n sdf as \nsdfas\nadsfs\nasasdf");

//		foreach (var i in contents) {
//			GameObject item = NGUITools.AddChild(content,prefab);
//
//			LogHelper.Log("------operation notice transform:" + item);
//
//			item.transform.FindChild(titleLabel).GetComponent<UILabel>().text = i.Key;
//			item.transform.FindChild(contentLabel).GetComponent<UILabel>().text = i.Value;
//
//			//item.transform.parent = content.transform;
//		}


		//		sortBtn = transform.FindChild("Button_Sort").GetComponent<UIButton>();
		//		UIEventListener.Get(sortBtn.gameObject).onClick = ClickSortBtn;
		//sortRuleLabel = transform.FindChild("Label_Sort_Rule").GetComponent<UILabel>();
		
		//curSortRule = SortUnitTool.DEFAULT_SORT_RULE;
		//sortRuleLabel.text = curSortRule.ToString();
	}

	private void SetItemContent(GameObject obj, string titleS, string contentS){
		GameObject title = obj.transform.FindChild("Title").gameObject;
		GameObject contentL = obj.transform.FindChild("Content/Text").gameObject;
		GameObject content = obj.transform.FindChild("Content").gameObject;
		
		title.GetComponent<UILabel>().text = titleS;//nItem.title;
		contentL.GetComponent<UILabel>().text = contentS + "                                                                                  ";//nItem.message;
		
		obj.transform.FindChild("TitleBg").GetComponent<UIDragScrollView>().scrollView = content.GetComponent<UIDragScrollView>().scrollView = FindChild<UIScrollView>("Content");

		Vector3 size = contentL.GetComponent<UILabel> ().localSize;
		Vector3 tempS = content.GetComponent<BoxCollider> ().size;

		tempS.y = size.y;
		content.GetComponent<BoxCollider> ().size = tempS;
		Vector3 tempS2 = content.GetComponent<BoxCollider> ().center;
		tempS2.y = -size.y/2;

		content.GetComponent<BoxCollider> ().center = tempS2;
	}

	void ShowUIAnimation(){
		gameObject.transform.localPosition = new Vector3(-1000, config.localPosition.y, 0);
		iTween.MoveTo(gameObject, iTween.Hash("x", config.localPosition.x, "time", 0.4f, "islocal", true));
	}

	public void ClickOK(){
		AudioManager.Instance.PlayAudio( AudioEnum.sound_click );

		bool backHome = false;
		if (UIManager.Instance.baseScene.PrevScene == SceneEnum.Others)
			backHome = true;
		if (!backHome) {
			UIManager.Instance.ChangeScene (SceneEnum.Home);
			if (DataCenter.Instance.LoginInfo.Bonus != null && DataCenter.Instance.LoginInfo.Bonus != null
			    && DataCenter.Instance.LoginInfo.Bonus.Count > 0 && firstShow) {
				//			Debug.LogError ("show Reward scene... ");
//				HideUI();
				firstShow = false;
				UIManager.Instance.ChangeScene (SceneEnum.Reward);
//				MsgCenter.Instance.Invoke(CommandEnum.GotoRewardMonthCardTab,4);
//				HideUI();
			}	
		}else{
			UIManager.Instance.ChangeScene(SceneEnum.Others);
//			HideUI();
		}


//		HideUI ();
	}
}
