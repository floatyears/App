using UnityEngine;
using System.Collections;

public class TopUI : UIBaseUnity
{
	private UILabel labTextRank;
	private UILabel labVauleRank;

	private UILabel labVaulePlayerName;

	private UILabel labValueMoney;

	private UILabel labVauleCurStamina;
	private UILabel labVauleLine;
	private UILabel labVauleTotalStamina;

	private UILabel labVauleChipNum;

	public static UILabel labVauleUIName;

	public static GameObject infoBar;

	void Start()
	{
		Init("Top");
	}
	public override void Init (string name)
	{
		base.Init (name);

		//Find out Labels
		labTextRank = FindChild<UILabel>("Player_Info_Bar/Lab_T_Rank");
		labVauleRank = FindChild<UILabel>("Player_Info_Bar/Lab_V_Rank");

		labVaulePlayerName = FindChild<UILabel>("Player_Info_Bar/Lab_V_PlayerName");

		labValueMoney = FindChild<UILabel>("Player_Info_Bar/Lab_V_Icon");

		labVauleCurStamina = FindChild<UILabel>("Player_Info_Bar/Lab_V_CurStamina");
		labVauleLine = FindChild<UILabel>("Player_Info_Bar/Lab_T_Line");
		labVauleTotalStamina = FindChild<UILabel>("Player_Info_Bar/Lab_V_TotalStamina");

		labVauleChipNum = FindChild<UILabel>("Player_Info_Bar/Lab_V_ChipNum");

		labVauleUIName = FindChild<UILabel>("Scene_Info_Bar/Lab_UI_Name");
		LogHelper.Log(labVauleUIName);
		infoBar = transform.Find("Scene_Info_Bar").gameObject;

		//Give Vaules from Config
		labTextRank.text = UIConfig.Lab_T_Rank;
		labVauleRank.text = UIConfig.Lab_V_Rank;

		labVaulePlayerName.text = UIConfig.Lab_V_PlayerName;

		labValueMoney.text = UIConfig.Lab_V_Money;

		labVauleCurStamina.text = UIConfig.Lab_V_CurStamina;
		labVauleLine.text = UIConfig.Lab_T_Line;
		labVauleTotalStamina.text = UIConfig.Lab_V_TotalStamina;

		labVauleChipNum.text = UIConfig.Lab_V_ChipNum;
	}
}
