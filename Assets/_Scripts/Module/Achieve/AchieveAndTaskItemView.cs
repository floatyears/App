using UnityEngine;
using System.Collections;
using bbproto;

public class AchieveAndTaskItemView : DragPanelItemBase {

	private UISprite bigIcon;
	private UILabel name;
	private UILabel progressLabel;
	private GameObject Icon;

	private TaskConf data;

	private void Init(){
		bigIcon = transform.FindChild ("Item/Img").GetComponent<UISprite> ();
		name = transform.FindChild ("Name").GetComponent<UILabel>();
		progressLabel = transform.FindChild ("Label_Progress").GetComponent<UILabel> ();

		transform.FindChild ("Label_Award").GetComponent<UILabel> ().text = TextCenter.GetText ("Text_Award");
		transform.FindChild ("Go/Label").GetComponent<UILabel>().text = TextCenter.GetText("Text_Take");
	}

	public override void SetData<T> (T d, params object[] args)
	{
		if (name == null)
			Init ();
		data = d as TaskConf;
	}

	public override void ItemCallback (params object[] args)
	{

	}
}
