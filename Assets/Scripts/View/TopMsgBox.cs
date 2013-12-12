using UnityEngine;
using System.Collections;

public class TopMsgBox : UIBaseUnity
{
	private UILabel labTextCurTotalExp;
	private UILabel labTextNextExp;
	private UILabel labTextRank;
	private UILabel labVauleCurTotalExp;
	private UILabel labVauleNextExp;
	private UILabel labVauleRank;

	void Start()
	{
		Init("aaa");
	}

	public override void Init (string name)
	{
		base.Init (name);

		labTextCurTotalExp = FindChild<UILabel>("Lab_T_CurTotalExp");
		labTextNextExp = FindChild<UILabel>("Lab_T_NextExp");
		labTextRank = FindChild<UILabel>("Lab_T_Rank");
		Debug.Log(labTextRank);
		labVauleCurTotalExp = FindChild<UILabel>("Lab_V_CurTotalExp");
		labVauleNextExp = FindChild<UILabel>("Lab_V_NextExp");
		labVauleRank = FindChild<UILabel>("Lab_V_Rank");

		labTextCurTotalExp.text = UIConfig.Lab_T_CurTotalExp;
		labTextNextExp.text = UIConfig.Lab_T_NextExp;
		labTextRank.text = UIConfig.Lab_T_Rank;

		labVauleCurTotalExp.text = UIConfig.Lab_V_CurTotalExp;
		labVauleNextExp.text = UIConfig.Lab_V_NextExp;
		labVauleRank.text = UIConfig.Lab_V_Rank;

	}
}
