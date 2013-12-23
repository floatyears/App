using UnityEngine;
using System.Collections;

public class PlayerInfoBar : UIBaseUnity
{
	private UILabel labTextRank;
	private UILabel labVauleRank;

	private UILabel labVaulePlayerName;

	private UILabel labValueMoney;

	private UILabel labVauleCurStamina;
	private UILabel labVauleLine;
	private UILabel labVauleTotalStamina;

	private UILabel labVauleChipNum;
	
	void Start()
	{
		Init("PlayerInfoBar");
	}
	public override void Init (string name)
	{
		base.Init (name);

		//Find out Labels
		labTextRank = FindChild<UILabel>("Lab_T_Rank");
		labVauleRank = FindChild<UILabel>("Lab_V_Rank");

		labVaulePlayerName = FindChild<UILabel>("Lab_V_PlayerName");

		labValueMoney = FindChild<UILabel>("Lab_V_Icon");

		labVauleCurStamina = FindChild<UILabel>("Lab_V_CurStamina");
		labVauleLine = FindChild<UILabel>("Lab_T_Line");
		labVauleTotalStamina = FindChild<UILabel>("Lab_V_TotalStamina");

		labVauleChipNum = FindChild<UILabel>("Lab_V_ChipNum");

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
