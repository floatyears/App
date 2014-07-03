using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OperationNoticeView : UIComponentUnity {

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
	}
	
	public override void HideUI() {
		//show Login Bonus

		base.HideUI();
//		Debug.Log ("hide operation notice: " + config.localPosition.y);
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}
	
	private void InitUI(){
		content = this.FindChild ("Content/Table");

		FindChild<UILabel> ("Title").text = TextCenter.GetText ("Text_Notice");
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
					
					item.transform.FindChild(titleLabel).GetComponent<UILabel>().text = nItem.title;
					item.transform.FindChild(contentLabel).GetComponent<UILabel>().text = nItem.message;
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

	public void ClickOK(){
		if (DataCenter.Instance.LoginInfo.Bonus != null && DataCenter.Instance.LoginInfo.Bonus != null
			&& DataCenter.Instance.LoginInfo.Bonus.Count > 0) {
//			Debug.LogError ("show Reward scene... ");
			UIManager.Instance.ChangeScene (SceneEnum.Reward);	
		} else {
			UIManager.Instance.ChangeScene (SceneEnum.Home);
		}

//		HideUI ();
	}
}
